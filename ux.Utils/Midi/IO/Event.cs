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

/* 規格出典: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System.IO;

namespace ux.Utils.Midi.IO
{
    /// <summary>
    /// MIDI の命令となるイベントを抽象化したクラスです。
    /// </summary>
    public abstract class Event
    {
        #region -- Public Properties --
        /// <summary>
        /// 以前のイベントとの差分時間 (デルタタイム) を取得します。
        /// </summary>
        public int DeltaTime { get; private set; }

        /// <summary>
        /// このイベントが送出されるティック位置を取得します。
        /// </summary>
        public long Tick { get; private set; }

        /// <summary>
        /// このイベントの種類を取得します。
        /// </summary>
        public EventType Type { get; protected set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// デルタタイムとティック位置を指定して新しい Event クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="tick"></param>
        public Event(int deltaTime, long tick)
        {
            this.DeltaTime = deltaTime;
            this.Tick = tick;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// デルタタイムを解析します。
        /// </summary>
        /// <param name="br">解析元となるバイトリーダ。</param>
        /// <returns>解析され求められたデルタタイム。</returns>
        public static int ParseDeltaTime(BinaryReader br)
        {
            // 先頭ビットが 1 ==> 前の結果を左に7つビットシフトして、次のバイトとOR、繰り返し
            //             0 ==> 1 と同じだがそれで終わり

            int deltaTime = 0;
            byte current;
            
            do
            {
                current = br.ReadByte();

                deltaTime <<= 7;
                deltaTime |= current & 0x7f;

            } while ((current & 0x80) == 0x80);

            return deltaTime;
        }

        /// <summary>
        /// このインスタンスを表す文字列を取得します。
        /// </summary>
        /// <returns>このインスタンスを表す文字列。</returns>
        public override string ToString()
        {
            return string.Format("{0}", this.Type);
        }
        #endregion
    }

    /// <summary>
    /// イベントの種類を表す列挙体です。
    /// </summary>
    public enum EventType
	{
        /// <summary>
        /// 不明。イベントの種類を特定できません。
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// ノートオフ。data1 がノートオフされるノートナンバー、data2 がそのベロシティです。
        /// </summary>
        NoteOff = 0x80,

        /// <summary>
        /// ノートオン。data1 がノートオンされるノートナンバー、data2 がそのベロシティです。
        /// </summary>
        NoteOn = 0x90,

        /// <summary>
        /// ポリフォニックキープレッシャー。
        /// </summary>
        PolyphonicKeyPressure = 0xa0,

        /// <summary>
        /// コントロールチェンジ。data1 がコントロール値、data2 が値となります。
        /// </summary>
        ControlChange = 0xb0,

        /// <summary>
        /// プログラムチェンジ。
        /// </summary>
        ProgramChange = 0xc0,

        /// <summary>
        /// チャネルプレッシャー。
        /// </summary>
        ChannelPressure = 0xd0,

        /// <summary>
        /// ピッチベンド。data1 と data2 がベンド値となります。
        /// </summary>
        Pitchbend = 0xe0,

        /// <summary>
        /// システムエクスクルーシブ。F0 から F7 までの可変長バイト列です。
        /// </summary>
        SystemExclusiveF0 = 0xf0,

        /// <summary>
        /// システムエクスクルーシブ。F7 まで続く可変長バイト列です。
        /// </summary>
        SystemExclusiveF7 = 0xf7,

        /// <summary>
        /// メタイベント。演奏自体の制御やメタデータの格納に用います。
        /// </summary>
        MetaEvent = 0xff,
	}
}
