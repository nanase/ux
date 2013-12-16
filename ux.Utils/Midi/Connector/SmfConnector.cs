/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ux.Utils.Midi.IO;
using ux.Utils.Midi.Sequencer;

namespace ux.Utils.Midi
{
    /// <summary>
    /// SMF ファイルを読み込み、ux と接続します。
    /// </summary>
    public class SmfConnector : MidiConnector
    {
        #region -- Private Fields --
        private Sequence sequence;
        private Sequencer.Sequencer sequencer;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// MIDI イベントが格納されたシーケンスオブジェクトを取得します。
        /// </summary>
        public Sequence Sequence { get { return this.sequence; } }

        /// <summary>
        /// 演奏に用いるシーケンサオブジェクトを取得します。
        /// </summary>
        public Sequencer.Sequencer Sequencer { get { return this.sequencer; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定して新しい SmfConnector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        public SmfConnector(float samplingRate)
            : base(samplingRate)
        {
            this.Reset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// SMF ファイルを指定してシーケンサを初期化します。
        /// </summary>
        /// <param name="filename">読み込まれる SMF ファイル。</param>
        public void Load(string filename)
        {
            if (this.sequencer != null)
                this.sequencer.Stop();

            this.sequence = new Sequence(filename);
            this.sequencer = new Sequencer.Sequencer(sequence);

            Console.WriteLine("Loaded: {0}", Path.GetFileName(filename));
            Encoding sjis = Encoding.GetEncoding(932);

            foreach (MetaEvent item in sequence.Tracks.Where(t => t.Number == 0).SelectMany(t => t.Events).Where(e => e is MetaEvent))
            {
                switch (item.MetaType)
                {
                    case MetaType.Text:
                        Console.WriteLine("\tText: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.Copyrights:
                        Console.WriteLine("\tCopyrights: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.TrackName:
                        Console.WriteLine("\tTitle: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.InstrumentName:
                        Console.WriteLine("\tInstrument: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.Lyrics:
                        Console.WriteLine("\tLyrics: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.ProgramName:
                        Console.WriteLine("\tProgram: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    case MetaType.DeviceName:
                        Console.WriteLine("\tDevice: {0}", sjis.GetString(item.Data).Trim());
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine("\tFormat: {0}, Resolution: {1}", sequence.Format, sequence.Resolution);
            Console.WriteLine("\tTracks: {0}, Events: {1}, MaxTick: {2}", sequence.Tracks.Count(), sequence.EventCount, sequence.MaxTick);
            Console.WriteLine("Frequency: {0:n0} Hz", Stopwatch.Frequency);

            sequencer.OnTrackEvent += (sender, e) =>
            {
                var events = e.Events.ToArray();
                this.EventCount += events.Length;
                this.ProcessMidiEvent(events);
            };
            sequencer.SequenceStarted += (sender, e) => Console.WriteLine("Sequencer Start");
            sequencer.SequenceEnd += (sender, e) => Console.WriteLine("Sequencer End");
            sequencer.SequenceStopped += (sender, e) => Console.WriteLine("Sequencer Stop");
            sequencer.TempoChanged += (sender, e) => Console.WriteLine("Tempo {0:f2} => {1:f2}", e.OldTempo, e.NewTempo);

        }

        /// <summary>
        /// シーケンサを開始します。
        /// </summary>
        public override void Play()
        {
            if (this.sequencer != null)
                this.sequencer.Start();
        }

        /// <summary>
        /// シーケンサを停止します。
        /// </summary>
        public override void Stop()
        {
            if (this.sequencer != null)
                this.sequencer.Stop();
        }

        /// <summary>
        /// シーケンサを停止し、リソースを解放します。
        /// </summary>
        public override void Dispose()
        {
            this.Stop();
        }
        #endregion
    }
}
