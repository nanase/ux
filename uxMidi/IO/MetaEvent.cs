/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

/* 規格出展: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace uxMidi.IO
{
    /// <summary>
    /// MIDI の演奏制御、メタデータに関するイベントを提供します。
    /// </summary>
    public class MetaEvent : Event
    {
        #region Property
        /// <summary>
        /// 格納された可変長のバイトデータを取得します。
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// このメタイベントのタイプを取得します。
        /// </summary>
        public MetaType MetaType { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// パラメータを指定して新しい MetaEvent クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="deltaTime">デルタタイム。</param>
        /// <param name="tick">ティック位置。</param>
        /// <param name="type">メタイベントのタイプ。</param>
        /// <param name="br">読み込まれるバイトリーダ。</param>
        internal MetaEvent(int deltaTime, long tick, EventType type, BinaryReader br)
            : base(deltaTime, tick)
        {
            this.Type = type;

            this.Load(br);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// このメタイベントのバイトデータをテンポデータと解釈し、テンポを取得します。
        /// </summary>
        /// <returns>テンポ値。</returns>
        public double GetTempo()
        {
            return 60.0 * 1e6 / (double)(this.Data[0] << 16 | this.Data[1] << 8 | this.Data[2]);
        }

        /// <summary>
        /// このインスタンスを表す文字列を取得します。
        /// </summary>
        /// <returns>このインスタンスを表す文字列。</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Type, this.MetaType);
        }
        #endregion

        #region Private Method
        private void Load(BinaryReader br)
        {
            this.MetaType = (MetaType)br.ReadByte();

            int length = br.ReadByte();

            this.Data = new byte[length];
            br.Read(this.Data, 0, length);
        }
        #endregion
    }

    /// <summary>
    /// メタイベントの種類を表す列挙体です。
    /// </summary>
    public enum MetaType
    {
        /// <summary>
        /// シーケンスナンバー。
        /// </summary>
        SequenceNumber = 0x00,

        /// <summary>
        /// テキスト。
        /// </summary>
        Text = 0x01,

        /// <summary>
        /// 著作権表示。
        /// </summary>
        Copyrights = 0x02,

        /// <summary>
        /// トラック名、またはタイトル。
        /// </summary>
        TrackName = 0x03,

        /// <summary>
        /// 楽器名。
        /// </summary>
        InstrumentName = 0x04,
        
        /// <summary>
        /// 歌詞。
        /// </summary>
        Lyrics = 0x05,

        /// <summary>
        /// マーカ。
        /// </summary>
        Marker = 0x06,

        /// <summary>
        /// キューポイント。
        /// </summary>
        QuePoint = 0x07,

        /// <summary>
        /// プログラム名。
        /// </summary>
        ProgramName = 0x08,

        /// <summary>
        /// デバイス名。
        /// </summary>
        DeviceName = 0x09,

        /// <summary>
        /// MIDI チャネルプレフィクス。
        /// </summary>
        MidiChannelPrefix = 0x20,

        /// <summary>
        /// ポート指定。
        /// </summary>
        Port = 0x21,

        /// <summary>
        /// トラック終端。
        /// </summary>
        EndOfTrack = 0x2F,

        /// <summary>
        /// テンポ。
        /// </summary>
        Tempo = 0x51,

        /// <summary>
        /// SMPTEオフセット。
        /// </summary>
        SmpteOffset = 0x54,

        /// <summary>
        /// 拍子。
        /// </summary>
        Time = 0x58,

        /// <summary>
        /// 調。
        /// </summary>
        Key = 0x59,

        /// <summary>
        /// シーケンサ特定メタイベント。
        /// </summary>
        Sequencer = 0x7F,
    }
}
