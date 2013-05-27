/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Waveform
{
	class LongNoise : StepWaveform
	{
		#region Constructors
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

    class ShortNoise : StepWaveform
    {
        #region Constructors
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