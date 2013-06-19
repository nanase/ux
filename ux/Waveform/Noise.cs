/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Waveform
{
    /// <summary>
    /// 線形帰還シフトレジスタを用いた長周期擬似ノイズジェネレータです。
    /// </summary>
	class LongNoise : StepWaveform
	{
		#region Constructors
        /// <summary>
        /// 新しい LongNoise クラスのインスタンスを初期化します。
        /// </summary>
		public LongNoise ()
			: base()
		{
			ushort reg = 0xffff;
			ushort output = 1;

			this.freqFactor = 0.001;
			this.value = new float[32767];
			this.length = 32767;

			for (int i = 0; i < 32767; i++) {
				reg += (ushort)(reg + (((reg >> 14) ^ (reg >> 13)) & 1));
				this.value [i] = (output ^= (ushort)(reg & 1)) * 2.0f - 1.0f;
			}
		}
		#endregion
	}

    /// <summary>
    /// 線形帰還シフトレジスタを用いた短周期擬似ノイズジェネレータです。
    /// </summary>
    class ShortNoise : StepWaveform
    {
        #region Constructors
        /// <summary>
        /// 新しい ShortNoise クラスのインスタンスを初期化します。
        /// </summary>
        public ShortNoise()
            : base()
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
}