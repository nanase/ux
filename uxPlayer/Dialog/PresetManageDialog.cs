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
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace uxPlayer
{
    public partial class PresetManageDialog : Form
    {
        public IEnumerable<string> FileNames
        {
            get { return this.listView_presets.Items.OfType<ListViewItem>().Select(i => i.Text); }
        }

        public PresetManageDialog(IEnumerable<string> fileNames)
        {
            InitializeComponent();

            this.listView_presets.Items.AddRange(fileNames.Select(f => new ListViewItem(f, 0)).ToArray());
            this.UpdateListItemStatus();
        }

        private void UpdateListItemStatus()
        {
            foreach (ListViewItem item in this.listView_presets.Items)
            {
                item.ImageIndex = File.Exists(item.Text) ? 0 : 1;
                item.ToolTipText = String.Format("フルパス: {0}", new FileInfo(item.Text).FullName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!this.listView_presets.Items.ContainsKey(this.openFileDialog.FileName))
                    this.listView_presets.Items.Add(this.openFileDialog.FileName, 0);
            }

            this.UpdateListItemStatus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView_presets.SelectedItems)
            {
                this.listView_presets.Items.Remove(item);
            }

            this.UpdateListItemStatus();
        }
    }
}
