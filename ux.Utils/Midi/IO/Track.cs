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

using System.Collections.Generic;
using System.IO;

namespace ux.Utils.Midi.IO
{
    /// <summary>
    /// イベントをグループ化したトラックを提供します。
    /// </summary>
    public class Track
    {
        #region -- Private Fields --
        private readonly List<Event> events;
        private int dataLength;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// トラックの番号を取得します。
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// 格納されているイベントの列挙子を取得します。
        /// </summary>
        public IEnumerable<Event> Events { get { return this.events; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい Track クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="number">トラックの番号。</param>
        /// <param name="br">読み込まれるバイトリーダ。</param>
        internal Track(int number, BinaryReader br)
        {
            this.events = new List<Event>();
            this.Number = number;
            this.LoadFromStream(br);
        }
        #endregion

        #region -- Public Methods --
        private void LoadFromStream(BinaryReader br)
        {
            // トラックのデータ長
            this.dataLength = br.ReadInt32().ToLittleEndian();

            long completePosition = br.BaseStream.Position + this.dataLength;
            
            EventType type = EventType.Unknown;
            byte status;
            int channel = 0;

            long tick = 0L;

            // イベントが入る。イベントに処理移行
            while (br.BaseStream.Position < completePosition)
            {
                int dt = Event.ParseDeltaTime(br);

                tick += dt;

                if (((status = br.ReadByte()) & 0x80) == 0x80)
                {
                    switch (status & 0xf0)
                    {
                        case 0xf0:
                            type = (EventType)status;
                            break;

                        default:
                            type = (EventType)(status & 0xf0);
                            channel = status & 0x0f;
                            break;
                    }
                }
                else
                {
                    br.BaseStream.Seek(-1L, SeekOrigin.Current);
                }

                switch (type)
                {
                    case EventType.NoteOff:
                    case EventType.NoteOn:
                    case EventType.PolyphonicKeyPressure:
                    case EventType.ControlChange:
                    case EventType.ProgramChange:
                    case EventType.ChannelPressure:
                    case EventType.Pitchbend:
                        events.Add(new MidiEvent(dt, tick, type, channel, br));
                        break;

                    case EventType.SystemExclusiveF0:
                    case EventType.SystemExclusiveF7:
                        events.Add(new SystemExclusiveEvent(dt, tick, type, br));
                        break;

                    case EventType.MetaEvent:
                        events.Add(new MetaEvent(dt, tick, type, br));
                        break;

                    default:
                        throw new InvalidDataException();
                }
            }

            if(br.BaseStream.Position > completePosition)
                throw new InvalidDataException();
        }
        #endregion
    }
}
