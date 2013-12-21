/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

/* 規格出展: about Standard MIDI format
 *          http://www2s.biglobe.ne.jp/~yyagi/material/smfspec.html
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi.Sequencer
{
    /// <summary>
    /// MIDI に関する一連のイベントを格納したシーケンスを提供します。
    /// </summary>
    public class Sequence
    {
        #region -- Private Fields --
        private readonly List<Track> tracks;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 読み込まれた SMF のフォーマットを取得します。このプロパティは 0 または 1 のみの値となります。
        /// </summary>
        public int Format { get; private set; }

        /// <summary>
        /// シーケンスに関連付けられた分解能値を取得します。
        /// </summary>
        public int Resolution { get; private set; }

        /// <summary>
        /// 格納されたイベントの中で最大のティック数を取得します。
        /// </summary>
        public long MaxTick { get; private set; }

        /// <summary>
        /// 格納されたイベントの総数を取得します。
        /// </summary>
        public int EventCount { get; private set; }

        /// <summary>
        /// シーケンスが持つトラックの列挙子を取得します。
        /// </summary>
        public IEnumerable<Track> Tracks { get { return this.tracks; } }

        /// <summary>
        /// ループが開始されると判定されたティックを取得します。
        /// </summary>
        public long LoopBeginTick { get; private set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// ファイルを指定して新しい Sequence クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="filename">読み込まれる SMF (MIDI) ファイル名。</param>
        public Sequence(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException();

            if (!File.Exists(filename))
                throw new FileNotFoundException();

            this.tracks = new List<Track>();
            Console.WriteLine("Loading: " + filename);

            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                this.LoadFile(stream);
        }

        /// <summary>
        /// ストリームを指定して新しい Sequence クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">読み込み可能なストリーム。</param>
        public Sequence(Stream stream)
        {
            this.tracks = new List<Track>();
            this.LoadFile(stream);
        }
        #endregion

        #region -- Private Methods --
        private void LoadFile(Stream stream)
        {
            using (BinaryReader br = new BinaryReader(stream))
            {
                // ヘッダ読み取り
                // マシンはリトルエンディアンなのでバイト列を適宜反転
                // (SMFは ビッグエンディアン で記述されている)

                uint magic = br.ReadUInt32().ToLittleEndian();
                long endOfStream = stream.Length - 1;

                // マジックナンバー: 4D 54 68 64 (MThd)
                if (magic == 0x52494646)
                    endOfStream = this.SeekForRiff(br);
                else if (magic != 0x4d546864)
                    throw new InvalidDataException();

                // ヘッダ長 (6バイトで固定)
                if (br.ReadInt32().ToLittleEndian() != 6)
                    throw new InvalidDataException();

                // フォーマット (0 or 1. 2 についてはサポート対象外)
                this.Format = br.ReadInt16().ToLittleEndian();
                if (this.Format != 0 && this.Format != 1)
                    throw new InvalidDataException();

                // トラック数
                int trackCount = br.ReadInt16().ToLittleEndian();

                // 時間単位 (正数、つまり分解能のみサポート)
                this.Resolution = br.ReadInt16().ToLittleEndian();
                if (Resolution < 1)
                    throw new InvalidDataException();

                // トラックの追加
                int trackNumber = 0;
                while (stream.Position < endOfStream)
                {
                    // マジックナンバー: 4d 54 72 6b (MTrk)
                    if (br.ReadUInt32().ToLittleEndian() == 0x4d54726b)
                    {
                        // Track クラスに処理を移す
                        this.tracks.Add(new Track(trackNumber, br));
                    }
                    else
                    {
                        // トラックチャンクでないなら、長さ分だけスキップ
                        uint length = br.ReadUInt32().ToLittleEndian();
                        stream.Seek(length, SeekOrigin.Current);
                    }

                    trackNumber++;
                }

                if (!this.tracks.Any(t =>
                {
                    if (!t.Events.Any())
                        return true;
                    Event e = t.Events.Last();
                    return (e.Type == EventType.MetaEvent && ((MetaEvent)e).MetaType == MetaType.EndOfTrack);
                }))
                    throw new InvalidDataException();

                this.EventCount = this.Tracks.SelectMany(t => t.Events).Count();
                this.MaxTick = this.Tracks.SelectMany(t => t.Events).Max(e => e.Tick);
                this.LoopBeginTick = this.DetectLoopBegin();
            }
        }

        private long SeekForRiff(BinaryReader br)
        {
            // RMI フォーマットに対応するためのメソッド
            // RIFF がリトルエンディアンで、SMF がビッグエンディアン
            // エンディアンネスが混在している
            // 混在している。

            uint riffLength = br.ReadUInt32();
            uint length;

            if (riffLength > br.BaseStream.Length)
                throw new InvalidDataException();

            // マジックナンバー: 52 4d 49 44 (RMID)
            if (br.ReadUInt32() == 0x524d4944)
                throw new InvalidDataException();

            // マジックナンバー: 64 61 74 61 (data)
            while (br.ReadUInt32().ToLittleEndian() != 0x64617461)
            {
                // データチャンクでないなら、長さ分だけスキップ
                length = br.ReadUInt32();
                br.BaseStream.Seek(length, SeekOrigin.Current);
            }

            length = br.ReadUInt32();

            // マジックナンバー: 4D 54 68 64 (MThd)
            if (br.ReadUInt32() == 0x4d546864)
                throw new InvalidDataException();

            return br.BaseStream.Position + length;
        }

        private long DetectLoopBegin()
        {
            var k = this.Tracks.SelectMany(t => t.Events).OfType<MidiEvent>().Where(e => e.Type == EventType.ControlChange && e.Data1 == 111).LastOrDefault();
            return (k == null) ? 0 : k.Tick;
        }
        #endregion
    }
}
