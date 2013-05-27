/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using ux.Component;

namespace ux.Waveform
{
    class StepWaveform : IWaveform
    {
        #region Protected Members
        protected const float PI = (float)Math.PI;
        protected const float PI2 = PI * 2.0f;
        protected const float PI_2 = PI / 2.0f;
        #endregion

        #region Private Members
        protected float[] value;
        protected float length;
        protected double freqFactor = 1.0;
        private Queue<byte> queue = null;
        #endregion

        #region Constructors
        public StepWaveform()
        {
            this.SetStep(new byte[] { 0 });
        }
        #endregion

        #region IWaveform implementation
        public virtual void GetWaveforms(float[] data, double[] frequency, double[] phase, int count)
        {
            float tmp;
            for (int i = 0; i < count; i++)
            {
                tmp = (float)(phase[i] * frequency[i] * freqFactor);
                if (float.IsInfinity(tmp) || float.IsNaN(tmp) || tmp < 0.0f)
                    data[i] = 0.0f;
                else
                    data[i] = this.value[(int)(tmp * this.length) % this.value.Length];
            }
        }

        public virtual void SetParameter(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "freqfactor":
                    this.freqFactor = parameter.Value * 0.001;
                    break;

                case "begin":
                    this.queue = new Queue<byte>();
                    this.queue.Enqueue((byte)parameter.Value);
                    break;

                case "end":
                    if (this.queue != null)
                    {
                        this.queue.Enqueue((byte)parameter.Value);
                        if (this.queue.Count <= 32767)
                            this.SetStep(this.queue.ToArray());
                    }
                    this.queue = null;
                    break;

                case "queue":
                    if (this.queue != null)
                        this.queue.Enqueue((byte)parameter.Value);
                    break;

                default:
                    break;
            }
        }

        public void SetStep(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException();
            if (data.Length > 32767)
                throw new ArgumentException("ステップデータは 32767 バイト以下でなければなりません。");

            float max = data.Max(),
            min = data.Min(),
            a = 2.0f / (max - min);
            this.length = data.Length;
            this.value = new float[data.Length];

            if (max == min)
                return;

            for (int i = 0; i < data.Length; i++)
                this.value[i] = (data[i] - min) * a - 1.0f;
        }
        #endregion
    }
}
