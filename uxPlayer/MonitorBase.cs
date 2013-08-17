/* uxPlayer / Software Synthesizer
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace uxPlayer
{
    abstract class MonitorBase : IDisposable
    {
        protected readonly Color backgroundColor;
        protected readonly Graphics graphics;
        protected readonly Bitmap bitmap;
        protected readonly Size size;

        public Bitmap Bitmap { get { return this.bitmap; } }

        public MonitorBase(Color backgroundColor, Size size)
        {
            this.backgroundColor = backgroundColor;
            this.size = size;

            this.bitmap = new Bitmap(size.Width, size.Height);
            this.graphics = Graphics.FromImage(this.bitmap);
        }

        public abstract void Draw(float[] data);

        #region IDisposable メンバー

        public void Dispose()
        {
            this.graphics.Dispose();
            this.bitmap.Dispose();
        }

        #endregion
    }

    class WaveformMonitor : MonitorBase
    {
        private PointF[] buffer = new PointF[0];
        private Pen p = Pens.Black;

        public WaveformMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
        }

        public override void Draw(float[] data)
        {
            this.graphics.Clear(this.backgroundColor);

            float dx = (float)this.size.Width / (float)data.Length * 2f;
            float dy = (float)this.size.Height / 4f;

            int length = data.Length / 2;

            if (this.buffer.Length != data.Length / 2)
                this.buffer = new PointF[data.Length / 2];

            for (int i = 0, k = 0; k < length; i += 2, k++)
                this.buffer[k] = new PointF(i * dx, dy - data[i] * dy);

            this.graphics.DrawLines(this.p, this.buffer);

            for (int i = 1, k = 0; k < length; i += 2, k++)
                this.buffer[k] = new PointF(i * dx, dy * 3 - data[i] * dy);

            this.graphics.DrawLines(this.p, this.buffer);

            this.graphics.Flush();
        }
    }

    class FrequencyMonitor : MonitorBase
    {
        private PointF[] buffer = new PointF[0];
        private Pen p = Pens.Black;

        private double[] re = new double[0];
        private double[] im = new double[0];

        public FrequencyMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
        }

        public override void Draw(float[] data)
        {
            float dx = (float)this.size.Width / (float)(data.Length / 4);
            float y_offset = +20.0f;

            int fftSize = data.Length / 2;
            int height = this.size.Height / 2;

            if (this.buffer.Length != fftSize)
            {
                this.buffer = new PointF[fftSize];
                this.re = new double[fftSize];
                this.im = new double[fftSize];
            }

            this.graphics.Clear(this.backgroundColor);

            // L channel
            {
                for (int i = 0, j = 0; i < fftSize; i++, j += 2)
                    this.re[i] = data[j];

                Array.Clear(this.im, 0, fftSize);
                Utils.WindowingAsHanning(re);
                Utils.FFT(fftSize, false, re, im);

                for (int i = 0; i < fftSize; i++)
                {
                    double d = 20.0 * Math.Log10(Math.Sqrt(re[i] * re[i] + im[i] * im[i]) / fftSize) + y_offset;

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
                for (int i = 0, j = 1; i < fftSize; i++, j += 2)
                    this.re[i] = data[j];

                Array.Clear(this.im, 0, fftSize);
                Utils.WindowingAsHanning(re);
                Utils.FFT(fftSize, false, re, im);

                for (int i = 0; i < fftSize; i++)
                {
                    double d = 20.0 * Math.Log10(Math.Sqrt(re[i] * re[i] + im[i] * im[i]) / fftSize) + y_offset;

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
    }

    class FrequencySpectrumMonitor : MonitorBase
    {
        private double[] re = new double[0];
        private double[] im = new double[0];
        private Bitmap spectrum = null;

        public FrequencySpectrumMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
            this.graphics.Clear(Color.Black);
        }

        unsafe public override void Draw(float[] data)
        {
            float dx = (float)this.size.Width / (float)(data.Length / 4);
            const float y_offset = +210.0f;
            const float y_amplifier = 100.0f;

            int fftSize = data.Length;

            if (fftSize != this.re.Length)
            {
                this.re = new double[fftSize];
                this.im = new double[fftSize];
                this.spectrum = new Bitmap(1, fftSize / 4);
            }
            else
                Array.Clear(this.im, 0, fftSize);

            data.CopyTo(re, 0);
            Utils.WindowingAsHanning(re);
            Utils.FFT(fftSize, false, re, im);

            for (int i = 0; i < fftSize / 4; i++)
            {
                re[i] = y_amplifier * Math.Log10(Math.Sqrt(re[i] * re[i] + im[i] * im[i]) / (fftSize * 4)) + y_offset;

                if (re[i] < -255.0)
                    re[i] = -255.0;
                else if (re[i] > 0.0)
                    re[i] = 0.0;
            }

            var bdata = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            {
                for (byte* i = (byte*)bdata.Scan0, j = i + 4, l = i + bdata.Width * bdata.Height * 4; j < l; i++, j++)
                    *i = *j;
            }
            this.bitmap.UnlockBits(bdata);

            bdata = this.spectrum.LockBits(new Rectangle(0, 0, 1, fftSize / 4), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
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
    }

    class VolumeMonitor : MonitorBase
    {
        private float[] l = new float[0];
        private float[] r = new float[0];
        private float max_l, max_r, ave_l, ave_r;

        private Brush max = new SolidBrush(Color.FromArgb(64, 64, 64));
        private Brush ave = new SolidBrush(Color.FromArgb(128, 128, 128));

        public VolumeMonitor(Color backgroundColor, Size size)
            : base(backgroundColor, size)
        {
        }

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
    }

    static class Utils
    {
        public static void WindowingAsHanning(double[] array)
        {
            double k = 2.0 * Math.PI / (double)array.Length;

            if (array.Length % 2 == 0)
                for (int i = 0, length = array.Length; i < length; i++)
                    array[i] *= 0.5 - 0.5 * Math.Cos(k * i);
            else
                for (int i = 0, length = array.Length; i < length; i++)
                    array[i] *= 0.5 - 0.5 * Math.Cos(k * (i + 0.5));
        }

        public static double Sinc(double x)
        {
            return x == 0.0 ? 1.0 : Math.Sin(x) / x;
        }

        public static void FFT(int n, bool invert, double[] ar, double[] ai)
        {
            int m, mh, i, j, k;
            double wr, wi, xr, xi,
                   theta = (invert ? 8.0 : -8.0) * Math.Atan(1.0) / n;

            for (m = n; (mh = m >> 1) >= 1; m = mh, theta *= 2.0)
                for (i = 0; i < mh; i++)
                    for (wr = Math.Cos(theta * i), wi = Math.Sin(theta * i), j = i; j < n; j += m)
                    {
                        k = j + mh;
                        xr = ar[j] - ar[k];
                        xi = ai[j] - ai[k];
                        ar[j] += ar[k];
                        ai[j] += ai[k];
                        ar[k] = wr * xr - wi * xi;
                        ai[k] = wr * xi + wi * xr;
                    }

            for (i = 0, j = 1; j < n - 1; j++)
            {
                for (k = n >> 1; k > (i ^= k); k >>= 1)
                    ;
                if (j < i)
                {
                    xr = ar[j];
                    xi = ai[j];
                    ar[j] = ar[i];
                    ai[j] = ai[i];
                    ar[i] = xr;
                    ai[i] = xi;
                }
            }
        }
    }
}
