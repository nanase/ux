/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Component
{
    /// <summary>
    /// 周波数と位相から波形を生成するウェーブジェネレータのインタフェースです。
    /// </summary>
	interface IWaveform
	{
		#region Methods
        /// <summary>
        /// 与えられた周波数と位相から波形を生成します。
        /// </summary>
        /// <param name="data">生成された波形データが代入される配列。</param>
        /// <param name="frequency">生成に使用される周波数の配列。</param>
        /// <param name="phase">生成に使用される位相の配列。</param>
        /// <param name="count">配列に代入されるデータの数。</param>
		void GetWaveforms(float[] data, double[] frequency, double[] phase, int count);

        /// <summary>
        /// パラメータを指定して波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
		void SetParameter (PValue parameter);
		#endregion
	}
}
