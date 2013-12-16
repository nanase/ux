/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using ux;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi
{
    /// <summary>
    /// ux と MIDI を接続するための抽象クラスです。
    /// </summary>
    public abstract class MidiConnector : IDisposable
    {
        #region -- Private Fields --
        private readonly Selector selector;
        #endregion

        #region -- Protected Fields --
        /// <summary>
        /// ux のマスターオブジェクトです。
        /// </summary>
        protected readonly Master uxMaster;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ux のマスターオブジェクトを取得します。
        /// </summary>
        public Master Master { get { return this.uxMaster; } }

        /// <summary>
        /// 通過した MIDI イベントの総数を取得します。
        /// </summary>
        public long EventCount { get; protected set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定して新しい MidiConnector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        public MidiConnector(float samplingRate)
        {
            this.selector = new PolyphonicSelector(samplingRate);
            this.uxMaster = this.selector.Master;
        }

        /// <summary>
        /// セレクタとサンプリング周波数を指定して新しい MidiConnector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="selector">セレクタ。</param>
        public MidiConnector(float samplingRate, Selector selector)
        {
            this.uxMaster = new Master(samplingRate, selector.PartCount);
            this.selector = selector;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ファイル名を指定してプリセットを追加します。
        /// </summary>
        /// <param name="filename">追加されるプリセットが記述された XML ファイル名。</param>
        public void AddPreset(string filename)
        {
            this.selector.Preset.Load(filename);
        }

        /// <summary>
        /// 読み込まれたプリセットをすべてクリアします。
        /// </summary>
        public void ClearPreset()
        {
            this.selector.Preset.Clear();
        }

        /// <summary>
        /// プリセットをリロードします。現在設定されている音源の更新はされません。
        /// </summary>
        public void ReloadPreset()
        {
            this.selector.Preset.Reload();
        }

        /// <summary>
        /// MIDI とのコネクションを開始します。実際の動作は継承クラスによって異なります。
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// MIDI とのコネクションを停止します。実際の動作は継承クラスによって異なります。
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// ux にリセット命令を送ります。ドラムの初期化が発生します。
        /// </summary>
        public void Reset()
        {
            this.selector.Reset();
        }

        /// <summary>
        /// このオブジェクトに割り当てられたリソースを解放します。
        /// </summary>
        public abstract void Dispose();
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        protected void ProcessMidiEvent(IEnumerable<Event> events)
        {
            this.selector.ProcessMidiEvent(events);
        }
        #endregion
    }
}
