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
    /// 音源プログラムのプリセットを表します。
    /// </summary>
    public class ProgramPreset
    {
        #region -- Private Fields --
        private readonly List<Handle> initHandles;
        private readonly List<Handle> finalHandles;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// プログラムナンバーを取得します。
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// プログラムナンバーの MSB 値を取得します。
        /// </summary>
        public int MSB { get; private set; }

        /// <summary>
        /// プログラムナンバーの LSB 値を取得します。
        /// </summary>
        public int LSB { get; private set; }

        /// <summary>
        /// 初期化時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> InitHandles { get { return this.initHandles; } }

        /// <summary>
        /// 破棄時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> FinalHandles { get { return this.finalHandles; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい ProgramPreset クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="number">プログラムナンバー。</param>
        /// <param name="msb">プログラムナンバーの MSB 値。</param>
        /// <param name="lsb">プログラムナンバーの LSB 値。</param>
        /// <param name="initHandles">初期化時に実行されるハンドルの列挙子。</param>
        /// <param name="finalHandles">破棄時に実行されるハンドルの列挙子。</param>
        public ProgramPreset(int number,
                             int msb,
                             int lsb,
                             IEnumerable<Handle> initHandles,
                             IEnumerable<Handle> finalHandles)
        {
            this.Number = number;
            this.MSB = msb;
            this.LSB = lsb;
            this.initHandles = new List<Handle>(initHandles);

            if (finalHandles != null)
                this.finalHandles = new List<Handle>(finalHandles);
        }
        #endregion
    }
}
