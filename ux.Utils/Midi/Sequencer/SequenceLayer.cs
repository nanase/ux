/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi.Sequencer
{
    /// <summary>
    /// 複数のシーケンサが同時に演奏するためのレイヤを定義します。
    /// </summary>
    public class SequenceLayer
    {
        #region -- Private Field --
        private Sequencer sequencer = null;
        private readonly int[] targetChannels;
        private readonly Preset preset;
        private readonly Master master;
        private readonly Selector selector;
        #endregion

        #region -- Public Properties --
        public Sequencer Sequencer { get { return this.sequencer; } }
        #endregion

        #region -- Constructor --
        /// <summary>
        /// パラメータを指定して新しい SequenceLayer クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="preset">適用されるプリセット。</param>
        /// <param name="master">シーケンサの送出先となるマスターオブジェクト。</param>
        /// <param name="selector">シーケンサが使用するセレクタオブジェクト。</param>
        /// <param name="targetChannels">このレイヤーが通過させるハンドルのターゲットチャネル。</param>
        public SequenceLayer(Preset preset, Master master, Selector selector, IEnumerable<int> targetChannels = null)
        {
            if (preset == null)
                throw new ArgumentNullException("preset");

            if (master == null)
                throw new ArgumentNullException("master");

            this.preset = preset;
            this.master = master;
            this.selector = selector;
            this.targetChannels = (targetChannels == null) ? Enumerable.Range(0, selector.PartCount).ToArray() : targetChannels.ToArray();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// SMF ファイルを読み込み、シーケンスを開始します。
        /// </summary>
        /// <param name="file">読み込まれる SMF ファイル。</param>
        public void Load(string file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            using (FileStream fs = new FileStream(file, FileMode.Open))
                this.Load(fs);
        }

        /// <summary>
        /// SMF データを格納したストリームを読み込み、シーケンスを開始します。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        public void Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanRead)
                throw new NotSupportedException();

            Sequence sequence = new Sequence(stream);

            if (this.sequencer != null)
                this.sequencer.Stop();

            this.sequencer = new Sequencer(sequence);
            this.sequencer.Start();
            this.sequencer.OnTrackEvent += this.OnTrackEvent;
        }

        /// <summary>
        /// シーケンスを開始し、演奏を再生します。
        /// </summary>
        public void Play()
        {
            this.sequencer.Start();
        }

        /// <summary>
        /// シーケンスを停止し、演奏を停止します。
        /// </summary>
        public void Stop()
        {
            this.sequencer.Stop();
        }
        #endregion

        #region -- Private Methods --
        private void OnTrackEvent(object sender, TrackEventArgs e)
        {
            this.selector.ProcessMidiEvent(e.Events.OfType<MidiEvent>().Where(m => this.targetChannels.Contains(m.Channel)));
        }
        #endregion
    }
}
