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
using System.IO;

namespace uxPlayer
{
    class WaveFormatWriter : IDisposable
    {
        #region -- Private Fields --
        private bool isDisposed;
        #endregion

        #region -- Public Properties --
        public Stream BaseStream { get; private set; }

        public int SamplingFreq { get; private set; }

        public int BitPerSample { get; private set; }

        public int ChannelCount { get; private set; }

        public long WrittenBytes { get; private set; }

        public long WrittenSamples { get; private set; }
        #endregion

        #region -- Constructor --
        public WaveFormatWriter(Stream stream, int samplingFreq, int bitPerSample, int channelCount)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanSeek || !stream.CanWrite)
                throw new InvalidOperationException();

            if (samplingFreq <= 0)
                throw new ArgumentOutOfRangeException("samplingFreq");

            if (bitPerSample != 8 && bitPerSample != 16)
                throw new ArgumentOutOfRangeException("bitPerSample");

            if (channelCount < 1 || channelCount > 2)
                throw new ArgumentOutOfRangeException("channelCount");

            this.BaseStream = stream;
            this.SamplingFreq = samplingFreq;
            this.BitPerSample = bitPerSample;
            this.ChannelCount = channelCount;

            this.BaseStream.Seek(44L, SeekOrigin.Begin);
        }
        #endregion

        #region -- Public Methods --
        public void Write(byte[] buffer, int offset, int count)
        {
            this.BaseStream.Write(buffer, offset, count);
            this.WrittenBytes += count;
        }

        unsafe public void Write(short[] buffer, int offset, int count)
        {
            byte[] buf;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                    buf[j] = (byte)Math.Round((buffer[i] / 65536.0 + 0.5) * 255);

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        tmp = buffer[i];
                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        tmp = buffer[i];
                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        unsafe public void Write(double[] buffer, int offset, int count)
        {
            byte[] buf;
            double dtmp;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                {
                    dtmp = buffer[i];
                    if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        continue;
                    else if (dtmp > 1.0)
                        buf[j] = 255;
                    else if (dtmp < -1.0)
                        buf[j] = 0;
                    else
                        buf[j] = (byte)Math.Round((dtmp + 1.0) * 127.5);
                }

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5);

                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5);

                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        unsafe public void Write(float[] buffer, int offset, int count)
        {
            byte[] buf;
            float dtmp;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                {
                    dtmp = buffer[i];
                    if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        continue;
                    else if (dtmp > 1.0f)
                        buf[j] = 255;
                    else if (dtmp < -1.0f)
                        buf[j] = 0;
                    else
                        buf[j] = (byte)Math.Round((dtmp + 1.0f) * 127.5f);
                }

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0f)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0f)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5f);

                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5f);

                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        public void Dispose()
        {
            if (this.isDisposed)
                return;

            this.Flush();
            this.BaseStream.Dispose();

            this.isDisposed = true;
        }
        #endregion

        #region -- Private Methods --
        private void Flush()
        {
            bool little = BitConverter.IsLittleEndian;
            bool big = !little;

            this.BaseStream.Seek(0L, SeekOrigin.Begin);
            using (BinaryWriter bw = new BinaryWriter(this.BaseStream))
            {
                // 4 bytes, offset 4
                bw.Write(ReverseBit((int)0x52494646, little));

                // 4 bytes, offset 8
                bw.Write(ReverseBit((int)(this.WrittenBytes + 36), big));

                // 8 bytes, offset 16
                bw.Write(ReverseBit((long)0x57415645666D7420, little));

                // 4 bytes, offset 20
                bw.Write(ReverseBit((int)16, big));

                // 2 bytes, offset 22
                bw.Write(ReverseBit((short)1, big));

                // 2 bytes, offset 24
                bw.Write(ReverseBit((short)this.ChannelCount, big));

                // 4 bytes, offset 28
                bw.Write(ReverseBit((int)this.SamplingFreq, big));

                // 4 bytes, offset 32
                bw.Write(ReverseBit((int)(this.SamplingFreq * this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 34
                bw.Write(ReverseBit((short)(this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 36
                bw.Write(ReverseBit((short)this.BitPerSample, big));

                // 4 bytes, offset 40
                bw.Write(ReverseBit((int)0x64617461, little));

                // 4 bytes, offset 44
                bw.Write(ReverseBit((int)this.WrittenBytes, big));
            }
        }
        #endregion

        #region -- Private Static Methods --
        unsafe private static long ReverseBit(long value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1, b2 = b0 + 2, b3 = b0 + 3,
                b4 = b0 + 4, b5 = b0 + 5, b6 = b0 + 6, b7 = b0 + 7;
                return *b0 * 72057594037927936 + *b1 * 281474976710656 +
                    *b2 * 1099511627776 + *b3 * 4294967296 +
                    *b4 * 16777216 + *b5 * 65536 +
                    *b6 * 256 + *b7;
            }
            else
                return value;
        }

        unsafe private static int ReverseBit(int value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1, b2 = b0 + 2, b3 = b0 + 3;
                return *b0 * 16777216 + *b1 * 65536 + *b2 * 256 + *b3;
            }
            else
                return value;
        }

        unsafe private static short ReverseBit(short value, bool reverse)
        {
            if (reverse)
            {
                byte* b0 = (byte*)&value, b1 = b0 + 1;
                return (short)(*b0 * 256 + *b1);
            }
            else
                return value;
        }
        #endregion
    }
}