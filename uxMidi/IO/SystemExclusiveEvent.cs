/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

/* 規格出展: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System.IO;

namespace uxMidi.IO
{
    /// <summary>
    /// 可変用のデータを持ったイベントを提供します。
    /// </summary>
    public class SystemExclusiveEvent : Event
    {
        #region Property
        /// <summary>
        /// 可変長のバイトデータを取得します。
        /// </summary>
        public byte[] Data { get; private set; }
        #endregion

        #region Constructor
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

        #region Private Method
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
