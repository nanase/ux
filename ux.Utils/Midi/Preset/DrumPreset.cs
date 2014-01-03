/* ux.Utils / Software Synthesizer Library

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

using System.Collections.Generic;
using ux.Component;

namespace ux.Utils.Midi
{
    /// <summary>
    /// ドラムまたはノイズのプリセットを表します。
    /// </summary>
    public class DrumPreset
    {
        #region -- Private Fields --
        private readonly List<Handle> initHandles;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// プログラムナンバーを取得します。
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// ドラムのノートナンバーを取得します。
        /// </summary>
        public int Note { get; private set; }

        /// <summary>
        /// 初期化時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> InitHandles { get { return this.initHandles; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい DrumPreset クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="number">プログラムナンバー。</param>
        /// <param name="note">ドラムのノートナンバー。</param>
        /// <param name="initHandles">初期化時に実行されるハンドルの列挙子。</param>
        public DrumPreset(int number, int note, IEnumerable<Handle> initHandles)
        {
            this.Number = number;
            this.Note = note;
            this.initHandles = new List<Handle>(initHandles);
        }
        #endregion
    }
}
