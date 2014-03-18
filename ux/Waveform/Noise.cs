/* ux - Micro Xylph / Software Synthesizer Core Library

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
        private const int DataSize = 32767;
        #endregion

        #region -- Constructors --
        static LongNoise()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            LongNoise.data = new float[LongNoise.DataSize];

            for (int i = 0; i < LongNoise.DataSize; i++)
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
            this.length = LongNoise.DataSize;
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
        private const int DataSize = 127;
        #endregion

        #region -- Constructors --
        static ShortNoise()
        {
            ushort reg = 0xffff;
            ushort output = 1;

            ShortNoise.data = new float[ShortNoise.DataSize];

            for (int i = 0; i < ShortNoise.DataSize; i++)
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
            this.length = ShortNoise.DataSize;
        }
        #endregion
    }

    struct RandomNoiseCache : CacheObject<RandomNoiseCache>
    {
        public float[] DataValue { get; set; }
        public int Seed { get; private set; }
        public int Length { get; private set; }

        public RandomNoiseCache(int seed, int length)
            : this()
        {
            this.Seed = seed;
            this.Length = length;
        }

        public bool Equals(RandomNoiseCache other)
        {
            return this.Seed == other.Seed && this.Length == other.Length;
        }

        public bool CanResize(RandomNoiseCache other)
        {
            return this.Seed == other.Seed && this.Length >= other.Length;
        }
    }

    /// <summary>
    /// 周期とシード値を元にした擬似乱数によるノイズジェネレータです。
    /// </summary>
    class RandomNoise : CachedWaveform<RandomNoiseCache>
    {
        #region -- Private Fields --
        private RandomNoiseCache param;
        #endregion

        #region -- Public Properties --
        protected override bool CanResizeData { get { return true; } }

        protected override bool GeneratingFloat { get { return true; } }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            this.freqFactor = 1.0;
            this.param = new RandomNoiseCache(0, 1024);
            this.GenerateStep();
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
                    this.param = new RandomNoiseCache((int)data2, this.param.Length);
                    break;

                case RandomNoiseOperate.Length:
                    int length = (int)data2;
                    if (length > 0 && length <= StepWaveform.MaxDataSize)
                        this.param = new RandomNoiseCache(this.param.Seed, length);
                    break;

                default:
                    base.SetParameter(data1, data2);
                    break;
            }

            this.GenerateStep();
        }
        #endregion

        #region -- Protected Methods --
        protected override float[] GenerateFloat(RandomNoiseCache parameter)
        {
            float[] value = new float[parameter.Length];

            Random r = new Random(parameter.Seed);

            for (int i = 0; i < parameter.Length; i++)
                value[i] = (float)(r.NextDouble() * 2.0 - 1.0);

            return value;
        }
        #endregion

        #region -- Private Methods --
        private void GenerateStep()
        {
            this.Cache(this.param);
        }
        #endregion
    }
}
