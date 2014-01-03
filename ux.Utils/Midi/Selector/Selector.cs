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
using System.IO;
using ux;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi
{
    /// <summary>
    /// 転送された MIDI イベントを選択し適切なパートに送信するための抽象クラスです。
    /// </summary>
    public abstract class Selector
    {
        #region -- Protected Fields --
        protected readonly Preset preset;        
        protected readonly Master master;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ux のマスターオブジェクトを取得します。
        /// </summary>
        public Master Master { get { return this.master; } }

        /// <summary>
        /// セレクタに割り当てられたプリセットオブジェクトを取得します。
        /// </summary>
        public Preset Preset { get { return this.preset; } }

        /// <summary>
        /// このセレクタがマスターオブジェクトに要求するパート数を取得します。
        /// </summary>
        public abstract int PartCount { get; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// マスターオブジェクトを指定して新しい Selector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="master">マスターオブジェクト。</param>
        public Selector(Master master)
        {
            this.master = master;
            this.preset = new Preset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ux にリセット命令を送ります。ドラムの初期化が発生します。
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public abstract void ProcessMidiEvent(IEnumerable<Event> events);
        #endregion
    }
}
