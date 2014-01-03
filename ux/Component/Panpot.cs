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
using ux.Utils;

namespace ux.Component
{
    /// <summary>
    /// 音の定位 (左右チャネルのバランス) を表す実数値を格納する構造体です。
    /// </summary>
    struct Panpot
    {
        #region -- Private Fields --
        private readonly float l, r;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 左チャネルのレベルを取得または設定します。
        /// </summary>
        public float L { get { return this.l; } }

        /// <summary>
        /// 右チャネルのレベルを取得または設定します。
        /// </summary>
        public float R { get { return this.r; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 左右チャネルのレベルを指定して新しい Panpot 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="lChannel">左チャネルのレベル。</param>
        /// <param name="rChannel">右チャネルのレベル。</param>
        public Panpot(float lChannel, float rChannel)
            : this()
        {
            this.l = lChannel.Clamp(1.0f, 0.0f);
            this.r = rChannel.Clamp(1.0f, 0.0f);
        }

        /// <summary>
        /// 左右チャネルのレベルを制御するパンポット値を指定して新しい Panpot 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="value">パンポット値。</param>
        public Panpot(float value)
            : this()
        {
            this.l = value >= 0.0f ? (float)Math.Sin((value + 1f) * Math.PI / 2.0) : 1.0f;
            this.r = value <= 0.0f ? (float)Math.Sin((-value + 1f) * Math.PI / 2.0) : 1.0f;
        }
        #endregion
    }
}
