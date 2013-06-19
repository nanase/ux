/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using ux.Component;

namespace ux.Waveform
{
    /// <summary>
    /// ステップ (階段状) 波形を生成できるジェネレータクラスです。
    /// </summary>
    class StepWaveform : IWaveform
    {
        #region Protected Field
        /// <summary>
        /// 円周率 Math.PI を Single 型にキャストした定数値です。
        /// </summary>
        protected const float PI = (float)Math.PI;

        /// <summary>
        /// 円周率 Math.PI を 2 倍し Single 型にキャストした定数値です。
        /// </summary>
        protected const float PI2 = (float)(Math.PI * 2.0);

        /// <summary>
        /// 円周率 Math.PI を 0.5 倍し Single 型にキャストした定数値です。
        /// </summary>
        protected const float PI_2 = (float)(Math.PI * 0.5);

        /// <summary>
        /// 波形生成に用いられる生データの配列です。
        /// </summary>
        protected float[] value;

        /// <summary>
        /// 波形生成に用いられるデータ長の長さです。
        /// </summary>
        protected float length;

        /// <summary>
        /// 波形生成に用いられる周波数補正係数です。
        /// </summary>
        protected double freqFactor = 1.0;
        #endregion

        #region Private Field
        private Queue<byte> queue = null;
        #endregion

        #region Constructors
        /// <summary>
        /// 空の波形データを使って新しい StepWaveform クラスのインスタンスを初期化します。
        /// </summary>
        public StepWaveform()
        {
            this.SetStep(new byte[] { 0 });
        }
        #endregion

        #region IWaveform implementation
        /// <summary>
        /// 与えられた周波数と位相からステップ波形を生成します。
        /// </summary>
        /// <param name="data">生成された波形データが代入される配列。</param>
        /// <param name="frequency">生成に使用される周波数の配列。</param>
        /// <param name="phase">生成に使用される位相の配列。</param>
        /// <param name="count">配列に代入されるデータの数。</param>
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

        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
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

        /// <summary>
        /// 指定されたステップデータから波形生成用のデータを作成します。
        /// </summary>
        /// <param name="data">波形生成のベースとなるステップデータ。</param>
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
