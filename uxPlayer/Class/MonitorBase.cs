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
using System.Drawing;
using System.Drawing.Imaging;
using SoundUtils;

namespace uxPlayer
{
    abstract class MonitorBase : IDisposable
    {
        #region -- Protected Fields --
        protected readonly Color backgroundColor;
        protected readonly Graphics graphics;
        protected readonly Bitmap bitmap;
        protected readonly Size size;
        #endregion

        #region -- Private Fields --
        private bool disposed;
        #endregion

        #region -- Public Properties --
        public Bitmap Bitmap { get { return this.bitmap; } }
        #endregion

        #region -- Constructors --
        public MonitorBase(Color backgroundColor, Size size)
        {
            this.backgroundColor = backgroundColor;
            this.size = size;

            this.bitmap = new Bitmap(size.Width, size.Height);
            this.graphics = Graphics.FromImage(this.bitmap);
        }
        #endregion

        #region -- Public Methods --
        public abstract void Draw(float[] data);

        #region IDisposable メンバー
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合は true。
        /// アンマネージリソースだけを解放する場合は false。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.graphics.Dispose();
                    this.bitmap.Dispose();
                }

                this.disposed = true;
            }

            //base.Dispose(disposing);
        }
        #endregion
    }

    class WaveformMonitor : MonitorBase
    {
        #region -- Private Fields --
        private PointF[] buffer = new PointF[0];
        private Pen p = Pens.Black;
        #endregion

        #region -- Constructors --
        public WaveformMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
            this.graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }
        #endregion

        #region -- Public Methods --
        public override void Draw(float[] data)
        {
            this.graphics.Clear(this.backgroundColor);

            float dx = (float)this.size.Width / (float)data.Length * 2f;
            float dy = (float)this.size.Height / 4f - 1;

            int length = data.Length / 2;

            if (this.buffer.Length != data.Length / 2)
                this.buffer = new PointF[data.Length / 2];

            for (int i = 0, k = 0; k < length; i++, k++)
                this.buffer[k] = new PointF(i * dx, dy - data[i] * dy);

            this.graphics.DrawLines(this.p, this.buffer);

            for (int i = 1, k = 0; k < length; i++, k++)
                this.buffer[k] = new PointF(i * dx, dy * 3 - data[i] * dy - 1);

            this.graphics.DrawLines(this.p, this.buffer);

            this.graphics.Flush();
        }
        #endregion
    }

    class FrequencyMonitor : MonitorBase
    {
        #region -- Private Fields --
        private PointF[] buffer = new PointF[0];
        private Pen p = Pens.Black;
        private FastFourier fft;

        private double[] fft_buffer = new double[0];
        private double[] lchannel = new double[0];
        private double[] rchannel = new double[0];
        #endregion

        #region -- Constructors --
        public FrequencyMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
        }
        #endregion

        #region -- Public Methods --
        public override void Draw(float[] data)
        {
            float dx = (float)this.size.Width / (float)(data.Length / 8);
            float y_offset = +20.0f;

            int fftSize = data.Length / 2;
            int height = this.size.Height / 2;

            if (this.buffer.Length != fftSize)
            {
                this.buffer = new PointF[fftSize / 4];
                this.lchannel = new double[fftSize];
                this.rchannel = new double[fftSize];
                this.fft_buffer = new double[fftSize * 2];
                this.fft = new FastFourier(fftSize);
            }

            this.graphics.Clear(this.backgroundColor);

            for (int i = 0, j = 0, k = 1; i < fftSize; i++, j += 2, k += 2)
            {
                this.lchannel[i] = data[j];
                this.rchannel[i] = data[k];
            }

            // L channel
            {
                Window.Hanning(lchannel);
                Channel.Interleave(lchannel, this.fft_buffer, fftSize);
                this.fft.TransformComplex(false, this.fft_buffer);

                for (int i = 0, j = 0, k = 1; i < fftSize / 4; i++, j += 2, k += 2)
                {
                    double d = 20.0 * Math.Log10(Math.Sqrt(this.fft_buffer[j] * this.fft_buffer[j] + this.fft_buffer[k] * this.fft_buffer[k]) / fftSize) + y_offset;

                    if (d <= -height)
                        d = -height;
                    else if (d > 0.0)
                        d = 0.0;

                    this.buffer[i] = new PointF(i * dx, -(float)d);
                }

                this.graphics.DrawLines(this.p, this.buffer);
            }

            // R channel
            {
                Window.Hanning(rchannel);
                Channel.Interleave(rchannel, this.fft_buffer, fftSize);
                this.fft.TransformComplex(false, this.fft_buffer);

                for (int i = 0, j = 0, k = 1; i < fftSize / 4; i++, j += 2, k += 2)
                {
                    double d = 20.0 * Math.Log10(Math.Sqrt(this.fft_buffer[j] * this.fft_buffer[j] + this.fft_buffer[k] * this.fft_buffer[k]) / fftSize) + y_offset;

                    if (d <= -height)
                        d = -height;
                    else if (d > 0.0)
                        d = 0.0;

                    this.buffer[i] = new PointF(i * dx, this.size.Height / 2 - (float)d);
                }

                this.graphics.DrawLines(this.p, this.buffer);
            }

            this.graphics.DrawLine(this.p, 0, height, this.size.Width, height);

            this.graphics.Flush();
        }
        #endregion
    }

    class FrequencySpectrumMonitor : MonitorBase
    {
        #region -- Private Fields --
        private double[] fft_buffer = new double[0];
        private double[] re = new double[0];
        private Bitmap spectrum = null;
        private FastFourier fft;

        private bool disposed;
        #endregion

        #region -- Constructors --
        public FrequencySpectrumMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
            this.graphics.Clear(Color.Black);
        }
        #endregion

        #region -- Public Methods --
        unsafe public override void Draw(float[] data)
        {
            float dx = (float)this.size.Width / (float)(data.Length / 4);
            const float y_offset = +210.0f;
            const float y_amplifier = 100.0f;

            int fftSize = data.Length;

            if (fftSize != this.fft_buffer.Length)
            {
                this.fft = new FastFourier(fftSize);
                this.re = new double[fftSize];
                this.fft_buffer = new double[fftSize * 2];
                this.spectrum = new Bitmap(1, fftSize / 8);
            }

            data.CopyTo(re, 0);
            Window.Hanning(this.re);
            Channel.Interleave(this.re, this.fft_buffer, fftSize);
            this.fft.TransformComplex(false, this.fft_buffer);

            for (int i = 0, j = 0, k = 1; i < fftSize / 8; i++, j += 2, k += 2)
            {
                re[i] = y_amplifier * Math.Log10(Math.Sqrt(this.fft_buffer[j] * this.fft_buffer[j] + this.fft_buffer[k] * this.fft_buffer[k]) / (fftSize * 4)) + y_offset;

                if (re[i] < -255.0)
                    re[i] = -255.0;
                else if (re[i] > 0.0)
                    re[i] = 0.0;
            }

            var bdata = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height),
                                             ImageLockMode.ReadWrite,
                                             PixelFormat.Format32bppArgb);
            {
                for (byte* i = (byte*)bdata.Scan0, j = i + 4, l = i + bdata.Width * bdata.Height * 4; j < l; i++, j++)
                    *i = *j;
            }
            this.bitmap.UnlockBits(bdata);

            bdata = this.spectrum.LockBits(new Rectangle(0, 0, 1, fftSize / 8),
                                           ImageLockMode.ReadWrite,
                                           PixelFormat.Format32bppArgb);
            {
                int j = 0;
                for (byte* i = (byte*)bdata.Scan0, l = i + bdata.Height * 4; i < l; j++)
                {
                    byte b = (byte)(255.0 + re[j]);
                    *i++ = b;
                    *i++ = b;
                    *i++ = b;
                    *i++ = 255;
                }
            }
            this.spectrum.UnlockBits(bdata);

            this.graphics.DrawImage(this.spectrum, this.size.Width - 1, this.size.Height - 1, 1, -this.size.Height);
            this.graphics.Flush();
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合は true。
        /// アンマネージリソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.spectrum.Dispose();
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion
    }

    class VolumeMonitor : MonitorBase
    {
        #region -- Private Fields --
        private float[] l = new float[0];
        private float[] r = new float[0];
        private float max_l, max_r, ave_l, ave_r;

        private Brush max = new SolidBrush(Color.FromArgb(64, 64, 64));
        private Brush ave = new SolidBrush(Color.FromArgb(128, 128, 128));

        private bool disposed;
        #endregion

        #region -- Constructors --
        public VolumeMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
        }
        #endregion

        #region -- Public Methods --
        public override void Draw(float[] data)
        {
            int length = data.Length / 2;
            float center = this.size.Width / 2.0f;
            float height = this.size.Height * 0.66f;

            if (length != l.Length)
            {
                this.l = new float[length];
                this.r = new float[length];
            }

            for (int i = 0, k = 0; k < length; i += 2, k++)
                this.l[k] = data[i];

            for (int i = 1, k = 0; k < length; i += 2, k++)
                this.r[k] = data[i];

            this.graphics.Clear(this.backgroundColor);

            this.max_l += (VolumeMonitor.GetMax(this.l) * center - this.max_l) * 0.25f;
            this.max_r += (VolumeMonitor.GetMax(this.r) * center - this.max_r) * 0.25f;

            this.graphics.FillRectangle(this.max, center - 1.0f - max_l, 0f, max_l, height);
            this.graphics.FillRectangle(this.max, center, 0f, max_r, height);

            this.ave_l += (VolumeMonitor.GetAverage(this.l) * center - this.ave_l) * 0.25f;
            this.ave_r += (VolumeMonitor.GetAverage(this.r) * center - this.ave_r) * 0.25f;

            this.graphics.FillRectangle(this.ave, center - 1.0f - ave_l, height, ave_l, this.size.Height - height);
            this.graphics.FillRectangle(this.ave, center, height, ave_r, this.size.Height - height);

            this.graphics.Flush();
        }
        #endregion

        #region -- Private Static Methods --
        private static float GetMax(float[] data)
        {
            float max = 0.0f;

            for (int i = 0; i < data.Length; i++)
            {
                if (/*data[i] > 0.0f &&*/ data[i] > max)
                    max = data[i];
                else if (data[i] < 0.0f && data[i] < -max)
                    max = -data[i];
            }

            return max;
        }

        private static float GetAverage(float[] data)
        {
            float total = 0.0f;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 0.0f)
                    total += data[i];
                else
                    total -= data[i];
            }

            return total / data.Length;
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合は true。
        /// アンマネージリソースだけを解放する場合は false。</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.max.Dispose();
                    this.ave.Dispose();
                }

                this.disposed = true;
            }

            base.Dispose(disposing);
        }
        #endregion
    }
}
