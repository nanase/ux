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
        #region -- Methods --
        /// <summary>
        /// 与えられた周波数と位相から波形を生成します。
        /// </summary>
        /// <param name="data">生成された波形データが代入される配列。</param>
        /// <param name="frequency">生成に使用される周波数の配列。</param>
        /// <param name="phase">生成に使用される位相の配列。</param>
        /// <param name="sampleTime">波形が開始されるサンプル時間。</param>
        /// <param name="count">配列に代入されるデータの数。</param>
        void GetWaveforms(float[] data, double[] frequency, double[] phase, int sampleTime, int count);

        /// <summary>
        /// パラメータを指定して波形の設定値を変更します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        void SetParameter(int data1, float data2);

        /// <summary>
        /// エンベロープをアタック状態に遷移させます。
        /// </summary>
        void Attack();

        /// <summary>
        /// エンベロープをリリース状態に遷移させます。
        /// </summary>
        /// <param name="time">リリースされたサンプル時間。</param>
        void Release(int time);

        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        void Reset();
        #endregion
    }
}
