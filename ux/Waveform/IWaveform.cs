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
