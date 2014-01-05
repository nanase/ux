/* uxPlayer / Software Synthesizer

LICENSE - The MIT License (MIT)

Copyright (c) 2013-2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ux;
using System.Threading.Tasks;
using System.IO;
using ux.Component;
using System.Threading;
using System.Security.Permissions;
using ux.Utils.Midi;

namespace uxPlayer
{
    public partial class ExportDialog : Form
    {
        private string inputFile;
        private IEnumerable<string> presetFiles;
        private volatile bool reqEnd;

        public ExportDialog(string inputFile, IEnumerable<string> presetFiles)
        {
            this.inputFile = inputFile;
            this.presetFiles = presetFiles;
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int CS_NOCLOSE = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;

                return cp;
            }
        }

        private void ExportDialog_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 7;
            this.radioButton3_CheckedChanged(null, null);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDown2.Enabled = this.numericUpDown3.Enabled = this.label3.Enabled = this.label4.Enabled = this.radioButton3.Checked;
            this.numericUpDown1.Enabled = this.label2.Enabled = this.radioButton4.Checked;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (this.trackBar1.Value == 0)
                this.label1.Text = "x1 (無効)";
            else
                this.label1.Text = String.Format("x{0:f0}", Math.Pow(2, this.trackBar1.Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog()
                == System.Windows.Forms.DialogResult.OK)
                this.textBox1.Text = this.saveFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled
                = this.groupBox4.Enabled = this.label8.Enabled = this.textBox1.Enabled
                = this.button1.Enabled = this.button2.Enabled = this.button3.Enabled
                = false;
            this.button4.Enabled = true;

            int bit = this.radioButton2.Checked ? 2 : 1;
            int samplingRate = int.Parse(this.comboBox2.Text.Substring(0, this.comboBox2.Text.IndexOf(' ')));
            int oversampling = (int)Math.Pow(2, this.trackBar1.Value);

            long filesize = (this.radioButton5.Checked) ?
                (long)Int32.MaxValue :
                    (this.radioButton3.Checked) ?
                    (long)((double)(this.numericUpDown2.Value * 60m + this.numericUpDown3.Value) * samplingRate * 2.0 * bit) :
                    (long)(this.numericUpDown1.Value * 1024.0m * 1024.0m);
            long output = 0;

            bool sequenceEnded = false;

            SmfConnector connector = new SmfConnector(samplingRate * oversampling);
            {
                foreach (var presetFile in this.presetFiles)
                    connector.AddPreset(presetFile);

                connector.Load(this.inputFile);
                connector.Sequencer.SequenceEnd += (s2, e2) => { sequenceEnded = true; connector.Master.Release(); };
            }

            if (!this.CheckFileCreate(this.textBox1.Text))
            {
                button4_Click(null, null);
                return;
            }

            this.reqEnd = false;

            Task.Factory.StartNew(() =>
            {
                using (FileStream fs = new FileStream(this.textBox1.Text, FileMode.Create))
                using (WaveFormatWriter wfw = new WaveFormatWriter(fs, samplingRate, bit * 8, 2))
                {
                    const int bufferSize = WaveFilter.DefaultFFTSize * 2;
                    int size;

                    float[] buffer = new float[bufferSize];
                    double[] buffer_double = new double[bufferSize];
                    double[] bufferOut = new double[bufferSize];
                    WaveFilter filter = new WaveFilter(true, samplingRate * oversampling, bufferSize);
                    double bufferTime = (buffer.Length / 2.0) / (samplingRate * oversampling);

                    filter.SetFilter(FilterType.LowPass,
                                     samplingRate / 2 - FFTFiltering.GetDelta(samplingRate * oversampling,
                                                                              WaveFilter.DefaultFilterSize + 1),
                                     0);

                    while (!this.reqEnd && filesize > output)
                    {
                        size = buffer.Length;

                        connector.Sequencer.Progress(bufferTime);
                        connector.Master.Read(buffer, 0, size);

                        if (oversampling > 1)
                        {
                            for (int i = 0; i < size; i++)
                                buffer_double[i] = buffer[i];

                            filter.Filtering(buffer_double, bufferOut);

                            for (int i = 0, j = 0; i < size; i += oversampling * 2)
                            {
                                buffer_double[j++] = bufferOut[i];
                                buffer_double[j++] = bufferOut[i + 1];
                            }

                            wfw.Write(buffer_double, 0, (int)Math.Min(size / oversampling, filesize - output));
                        }
                        else
                            wfw.Write(buffer, 0, size);

                        output = wfw.WrittenBytes;

                        if (sequenceEnded && connector.Master.ToneCount == 0)
                            this.reqEnd = true;
                    }

                    this.reqEnd = true;
                    this.Invoke(new Action(() => button4_Click(null, null)));
                }
            }, TaskCreationOptions.LongRunning);

            Task.Factory.StartNew(() =>
            {
                TimeSpan ts;

                while (!this.reqEnd)
                {
                    ts = TimeSpan.FromSeconds(output / 2.0 / bit / (double)samplingRate);

                    this.Invoke(new Action(() =>
                    {
                        long position = connector.Sequencer.Tick;
                        this.label7.Text = String.Format("{0:p0}", (double)output / (double)filesize);
                        this.progressBar1.Value = (int)((double)output / (double)filesize * 100);
                        this.label5.Text = String.Format("出力: {0:f0} KB", output / 1024.0);
                        this.label6.Text = String.Format("位置: {0}", position < 0 ? 0 : position);
                        this.label9.Text = String.Format("時間: {0}:{1:d2}", (int)ts.TotalMinutes, ts.Seconds);
                    }));

                    Thread.Sleep(30);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Sequencer_SequenceEnd(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.reqEnd = true;

            this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled
                    = this.groupBox4.Enabled = this.label8.Enabled = this.textBox1.Enabled
                    = this.button1.Enabled = this.button2.Enabled = this.button3.Enabled
                    = true;
            this.button4.Enabled = false;
            this.progressBar1.Value = 100;
            this.label7.Text = "100%";
        }

        private bool CheckFileCreate(string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create)) { }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
