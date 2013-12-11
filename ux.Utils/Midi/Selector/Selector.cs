/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
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
