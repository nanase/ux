/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Component
{
	interface IWaveform
	{
		#region Methods
		void GetWaveforms(float[] data, double[] frequency, double[] phase, int count);
		void SetParameter (PValue parameter);
		#endregion
	}
}
