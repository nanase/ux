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

namespace ux.Utils
{
	/// <summary>
	/// 拡張メソッドを集めたクラスです。
	/// </summary>
	public static class ValueTypeEx
    {
        #region -- Public Static Methods --
        #region Clamp
        /// <summary>
        /// Double 値を 0.0 から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <returns>クランプされた値。</returns>
        public static double Clamp(this double value, double max)
        {
            return value < 0.0 ? 0.0 : value > max ? max : value;
        }

        /// <summary>
        /// Double 値を最小値 min から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <param name="min">範囲の最小値。</param>
        /// <returns>クランプされた値。</returns>
        public static double Clamp(this double value, double max, double min)
        {
            return value < min ? min : value > max ? max : value;
        }

        /// <summary>
        /// Single 値を 0.0 から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <returns>クランプされた値。</returns>
        public static float Clamp(this float value, float max)
        {
            return value < 0.0f ? 0.0f : value > max ? max : value;
        }

        /// <summary>
        /// Single 値を最小値 min から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <param name="min">範囲の最小値。</param>
        /// <returns>クランプされた値。</returns>
        public static float Clamp(this float value, float max, float min)
        {
            return value < min ? min : value > max ? max : value;
        }

        /// <summary>
        /// Int32 値を 0 から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <returns>クランプされた値。</returns>
        public static int Clamp(this int value, int max)
        {
            return value < 0 ? 0 : value > max ? max : value;
        }

        /// <summary>
        /// Int32 値を最小値 min から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <param name="min">範囲の最小値。</param>
        /// <returns>クランプされた値。</returns>
        public static int Clamp(this int value, int max, int min)
        {
            return value < min ? min : value > max ? max : value;
        }

        /// <summary>
        /// Int16 値を 0 から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <returns>クランプされた値。</returns>
        public static short Clamp(this short value, short max)
        {
            return value < (short)0 ? (short)0 : value > max ? max : value;
        }

        /// <summary>
        /// Int16 値を最小値 min から最大値 max の範囲でクランプします。
        /// </summary>
        /// <param name="value">クランプされる値。</param>
        /// <param name="max">範囲の最大値。</param>
        /// <param name="min">範囲の最小値。</param>
        /// <returns>クランプされた値。</returns>
        public static short Clamp(this short value, short max, short min)
        {
            return value < min ? min : value > max ? max : value;
        }
        #endregion
        #endregion
	}
}
