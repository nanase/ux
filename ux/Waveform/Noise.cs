/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using ux.Component;

namespace ux.Waveform
{
    /// <summary>
    /// 線形帰還シフトレジスタを用いた長周期擬似ノイズジェネレータです。
    /// </summary>
    class LongNoise : StepWaveform
    {
        #region -- Private Fields --
        private static readonly float[] data;
        #endregion

        #region -- Constructors --
        static LongNoise()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            LongNoise.data = new float[32767];

            for (int i = 0; i < 32767; i++)
            {
                reg += (ushort)(reg + (((reg >> 14) ^ (reg >> 13)) & 1));
                LongNoise.data[i] = (output ^= (ushort)(reg & 1)) * 2.0f - 1.0f;
            }
        }

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
            this.freqFactor = 0.001;
            this.value = LongNoise.data;
            this.length = 32767;
        }
        #endregion
    }

    /// <summary>
    /// 線形帰還シフトレジスタを用いた短周期擬似ノイズジェネレータです。
    /// </summary>
    class ShortNoise : StepWaveform
    {
        #region -- Private Fields --
        private static readonly float[] data;
        #endregion

        #region -- Constructors --
        static ShortNoise()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            ShortNoise.data = new float[127];

            for (int i = 0; i < 127; i++)
            {
                reg += (ushort)(reg + (((reg >> 6) ^ (reg >> 5)) & 1));
                ShortNoise.data[i] = (output ^= (ushort)(reg & 1)) * 2.0f - 1.0f;
            }
        }

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
            this.freqFactor = 0.001;
            this.value = ShortNoise.data;
            this.length = 127;
        }
        #endregion
    }

    /// <summary>
    /// 周期とシード値を元にした擬似乱数によるノイズジェネレータです。
    /// </summary>
    class RandomNoise : StepWaveform
    {
        #region -- Private Fields --
        private static readonly LinkedList<NoiseCache> cache = new LinkedList<NoiseCache>();

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
            this.Generate();
        }

        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
        public override void SetParameter(int data1, float data2)
        {
            switch ((RandomNoiseOperate)data1)
            {
                case RandomNoiseOperate.Seed:
                    this.seed = (int)data2;
                    break;

                case RandomNoiseOperate.Length:
                    int leng = (int)data2;
                    if (leng > 0 && leng < 65536)
                        this.array_length = leng;
                    break;

                default:
                    base.SetParameter(data1, data2);
                    break;
            }

            this.Generate();
        }
        #endregion

        #region -- Private Methods --
        private void Generate()
        {
            NoiseCache nc = new NoiseCache();

            for (var now = cache.First; now != null; now = now.Next)
            {
                if (now.Value.seed == this.seed && now.Value.array_length >= this.array_length)
                {
                    nc = now.Value;
                    break;
                }
            }

            if (nc.array_length == 0)
            {
                this.value = new float[this.array_length];
                this.length = this.array_length;

                Random r = new Random(this.seed);

                for (int i = 0; i < this.array_length; i++)
                    this.value[i] = (float)(r.NextDouble() * 2.0 - 1.0);
            }

            else if (nc.array_length == this.array_length)
            {
                this.value = nc.data;
                this.length = this.array_length;
                return;
            }
            else
            {
                this.value = new float[this.array_length];
                this.length = this.array_length;
                Array.Copy(nc.data, this.value, this.array_length);
            }

            cache.AddFirst(new NoiseCache() { array_length = this.array_length, data = this.value, seed = this.seed });

            if (cache.Count > 32)
                cache.RemoveLast();
        }
        #endregion

        struct NoiseCache
        {
            public int seed;
            public int array_length;
            public float[] data;
        }
    }
}
