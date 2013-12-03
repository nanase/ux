/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using ux.Component;

namespace ux.Waveform
{
    /// <summary>
    /// 線形帰還シフトレジスタを用いた長周期擬似ノイズジェネレータです。
    /// </summary>
    class LongNoise : StepWaveform
    {
        #region -- Constructors --
        /// <summary>
        /// 新しい LongNoise クラスのインスタンスを初期化します。
        /// </summary>
        public LongNoise()
            : base()
        {
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            this.freqFactor = 0.001;
            this.value = new float[32767];
            this.length = 32767;

            for (int i = 0; i < 32767; i++)
            {
                reg += (ushort)(reg + (((reg >> 14) ^ (reg >> 13)) & 1));
                this.value[i] = (output ^= (ushort)(reg & 1)) * 2.0f - 1.0f;
            }
        }
        #endregion
    }

    /// <summary>
    /// 線形帰還シフトレジスタを用いた短周期擬似ノイズジェネレータです。
    /// </summary>
    class ShortNoise : StepWaveform
    {
        #region -- Constructors --
        /// <summary>
        /// 新しい ShortNoise クラスのインスタンスを初期化します。
        /// </summary>
        public ShortNoise()
            : base()
        {
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            this.freqFactor = 0.001;
            this.value = new float[127];
            this.length = 127;

            for (int i = 0; i < 127; i++)
            {
                reg += (ushort)(reg + (((reg >> 6) ^ (reg >> 5)) & 1));
                this.value[i] = (output ^= (ushort)(reg & 1)) * 2.0f - 1.0f;
            }
        }
        #endregion
    }

    /// <summary>
    /// 周期とシード値を元にした擬似乱数によるノイズジェネレータです。
    /// </summary>
    class RandomNoise : StepWaveform
    {
        #region -- Private Fields --
        private int seed = 0;
        private int array_length = 1024;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 新しい RandomNoise クラスのインスタンスを初期化します。
        /// </summary>
        public RandomNoise()
            : base()
        {
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            this.freqFactor = 1.0;
            this.value = new float[this.array_length];
            this.length = this.array_length;

            Random r = new Random(this.seed);

            for (int i = 0; i < this.array_length; i++)
                this.value[i] = (float)(r.NextDouble() * 2.0 - 1.0);
        }

        public override void SetParameter(int data1, float data2)
        {
            switch ((RandomNoiseOperate)data1)
            {
                case RandomNoiseOperate.Seed:
                    this.seed = (int)data2;
                    this.Reset();
                    break;

                case RandomNoiseOperate.Length:
                    int leng = (int)data2;
                    if (leng > 0 && leng < 65536)
                        this.array_length = leng;

                    this.Reset();
                    break;

                default:
                    base.SetParameter(data1, data2);
                    break;
            }
        }
        #endregion
    }
}
