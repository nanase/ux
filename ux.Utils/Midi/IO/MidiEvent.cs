/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

/* 規格出展: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System.IO;

namespace ux.Utils.Midi.IO
{
    /// <summary>
    /// MIDI の演奏に関わるイベントを提供します。
    /// </summary>
    public class MidiEvent : Event
    {
        #region -- Public Properties --
        /// <summary>
        /// 1つ目のパラメータを取得します。
        /// </summary>
        public int Data1 { get; private set; }

        /// <summary>
        /// 2つ目のパラメータを取得します。
        /// </summary>
        public int Data2 { get; private set; }

        /// <summary>
        /// 対象となるチャネル番号を取得します。
        /// </summary>
        public int Channel { get; private set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい MidiEvent クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="deltaTime">デルタタイム。</param>
        /// <param name="tick">ティック位置。</param>
        /// <param name="type">イベントのタイプ。</param>
        /// <param name="channel">チャネル番号。</param>
        /// <param name="br">読み込まれるバイトリーダ。</param>
        internal MidiEvent(int deltaTime, long tick, EventType type, int channel, BinaryReader br)
            : base(deltaTime, tick)
        {
            this.Type = type;
            this.Channel = channel;

            this.Load(br);
        }

        public MidiEvent(EventType type, int channel, int data1, int data2)
            : base(0, 0)
        {
            this.Type = type;
            this.Channel = channel;
            this.Data1 = data1;
            this.Data2 = data2;
        }

        /// <summary>
        /// このインスタンスを表す文字列を取得します。
        /// </summary>
        /// <returns>このインスタンスを表す文字列。</returns>
        public override string ToString()
        {
            return string.Format("{0}, Channel={1}, Control={2}", this.Type, this.Channel, this.Data1);
        }
        #endregion

        #region -- Private Methods --
        private void Load(BinaryReader br)
        {
            this.Data1 = br.ReadByte();

            if (this.Type != EventType.ProgramChange && this.Type != EventType.ChannelPressure)
                this.Data2 = br.ReadByte();
        }
        #endregion
    }
}
