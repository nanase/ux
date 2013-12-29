/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ux.Component;
using ux.Utils;

namespace ux.Waveform
{
    /// <summary>
    /// ステップ (階段状) 波形を生成できるジェネレータクラスです。
    /// </summary>
    class StepWaveform : IWaveform
    {
        #region -- Protected Fields --
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
        /// データとして保持できるステップ数を表した定数値です。
        /// </summary>
        protected const int MaxDataSize = 65536;

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

        #region -- Private Fields --
        private Queue<byte> queue = null;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 空の波形データを使って新しい StepWaveform クラスのインスタンスを初期化します。
        /// </summary>
        public StepWaveform()
        {
            this.Reset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 与えられた周波数と位相からステップ波形を生成します。
        /// </summary>
        /// <param name="data">生成された波形データが代入される配列。</param>
        /// <param name="frequency">生成に使用される周波数の配列。</param>
        /// <param name="phase">生成に使用される位相の配列。</param>
        /// <param name="sampleTime">波形が開始されるサンプル時間。</param>
        /// <param name="count">配列に代入されるデータの数。</param>
        public virtual void GetWaveforms(float[] data, double[] frequency, double[] phase, int sampleTime, int count)
        {
            int length = this.value.Length;

            for (int i = 0; i < count; i++)
            {
                float tmp = (float)(phase[i] * frequency[i] * this.freqFactor);
                if (tmp < 0.0f)
                    data[i] = 0.0f;
                else
                    data[i] = this.value[(int)(tmp * this.length) % length];
            }
        }

        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        public virtual void SetParameter(int data1, float data2)
        {
            switch ((StepWaveformOperate)data1)
            {
                case StepWaveformOperate.FreqFactor:
                    this.freqFactor = data2.Clamp(float.MaxValue, 0.0f) * 0.001;
                    break;

                case StepWaveformOperate.Begin:
                    this.queue = new Queue<byte>();
                    this.queue.Enqueue((byte)data2.Clamp(255.0f, 0.0f));
                    break;

                case StepWaveformOperate.End:
                    if (this.queue != null)
                    {
                        this.queue.Enqueue((byte)data2.Clamp(255.0f, 0.0f));
                        if (this.queue.Count <= 32767)
                            this.SetStep(this.queue.ToArray());

                        this.queue = null;
                    }                    
                    break;

                case StepWaveformOperate.Queue:
                    if (this.queue != null)
                        this.queue.Enqueue((byte)data2.Clamp(255.0f, 0.0f));
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

            if (data.Length > StepWaveform.MaxDataSize)
                throw new ArgumentException(String.Format("ステップデータは {0} 個以下でなければなりません。", StepWaveform.MaxDataSize));

            float max = data.Max();
            float min = data.Min();
            float a = 2.0f / (max - min);

            this.length = data.Length;
            this.value = new float[data.Length];

            if (max == min)
                return;

            for (int i = 0; i < data.Length; i++)
                this.value[i] = (data[i] - min) * a - 1.0f;
        }

        /// <summary>
        /// エンベロープをアタック状態に遷移させます。
        /// </summary>
        public virtual void Attack()
        {
        }

        /// <summary>
        /// エンベロープをリリース状態に遷移させます。
        /// </summary>
        /// <param name="time">リリースされたサンプル時間。</param>
        public virtual void Release(int time)
        {
        }

        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public virtual void Reset()
        {
            this.SetStep(new byte[] { 0 });
        }
        #endregion
    }
}
