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

/* 規格出展: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System.IO;

namespace ux.Utils.Midi.IO
{
    /// <summary>
    /// 可変用のデータを持ったイベントを提供します。
    /// </summary>
    public class SystemExclusiveEvent : Event
    {
        #region -- Public Properties --
        /// <summary>
        /// 可変長のバイトデータを取得します。
        /// </summary>
        public byte[] Data { get; private set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい SystemExclusiveEvent クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="deltaTime">デルタタイム。</param>
        /// <param name="tick">ティック位置。</param>
        /// <param name="type">イベントのタイプ。</param>
        /// <param name="br">読み込まれるバイトリーダ。</param>
        internal SystemExclusiveEvent(int deltaTime, long tick, EventType type, BinaryReader br)
            : base(deltaTime, tick)
        {
            this.Type = type;

            this.Load(br);
        }

        /// <summary>
        /// このインスタンスを表す文字列を取得します。
        /// </summary>
        /// <returns>このインスタンスを表す文字列。</returns>
        public override string ToString()
        {
            return string.Format("{0}, Length={1}", this.Type, this.Data.Length);
        }
        #endregion

        #region -- Private Methods --
        private void Load(BinaryReader br)
        {
            int length = br.ReadByte();

            if (this.Type == EventType.SystemExclusiveF0)
            {
                this.Data = new byte[length + 1];
                this.Data[0] = 0xf0;
                br.Read(this.Data, 1, length);
            }
            else
            {
                this.Data = new byte[length];
                br.Read(this.Data, 0, length);
            }
        }
        #endregion
    }
}
