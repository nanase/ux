/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi.Sequencer
{
    /// <summary>
    /// イベントをスケジューリングし、決められた時間に送出するシーケンサを提供します。
    /// </summary>
    public class Sequencer
    {
        #region -- Private Fields --
        private long tick;
        private double tempo = 120.0;
        private int interval = 5;
        private long endOfTick;
        private double tempoFactor = 1.0;
        private double tickTime;
        private long loopBeginTick = 0L;

        private int eventIndex = 0;

        private Task sequenceTask = null;
        private readonly List<Event> events;
        private readonly object syncObject = new object();

        private volatile bool reqEnd;
        private volatile bool reqRewind;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 一連のイベントを格納したシーケンスを取得します。
        /// </summary>
        public Sequence Sequence { get; private set; }

        /// <summary>
        /// シーケンサの現在のテンポ (BPM) を取得します。
        /// </summary>
        public double Tempo { get { return this.tempo; } }

        /// <summary>
        /// シーケンサの現在のティックを取得または設定します。
        /// </summary>
        public long Tick
        {
            get { return this.tick; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();

                this.eventIndex = 0;

                if (this.sequenceTask == null)
                {
                    this.tick = value;
                }
                else
                {
                    lock (this.syncObject)
                    {
                        this.tick = value;
                        this.reqRewind = true;
                    }
                }
            }
        }

        /// <summary>
        /// シーケンサがスケジューリングのために割り込む間隔をミリ秒単位で取得または設定します。
        /// </summary>
        public int Interval
        {
            get { return this.interval; }
            set
            {
                if (value < 1)
                    throw new ArgumentException();

                this.interval = value;
            }
        }

        /// <summary>
        /// シーケンサのテンポに応じて乗算される係数を取得または設定します。
        /// </summary>
        public double TempoFactor
        {
            get { return this.tempoFactor; }
            set
            {
                if (value <= 0.0)
                    throw new ArgumentException();

                this.tempoFactor = value;
                this.RecalcTickTime();
            }
        }

        /// <summary>
        /// シーケンサのループが開始されるティック位置を取得または設定します。
        /// </summary>
        public long LoopBeginTick
        {
            get { return this.loopBeginTick; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                this.loopBeginTick = value;
            }
        }

        /// <summary>
        /// シーケンサが指定位置でループされるかの真偽値を取得または設定します。
        /// </summary>
        public bool Looping { get; set; }
        #endregion

        #region -- Public Events --
        /// <summary>
        /// シーケンサによってスケジュールされたイベントが送出される時に発生します。
        /// </summary>
        public event EventHandler<TrackEventArgs> OnTrackEvent;

        /// <summary>
        /// イベントによってテンポが変更された時に発生します。このイベントは TempoFactor の影響を受けません。
        /// </summary>
        public event EventHandler<TempoChangedEventArgs> TempoChanged;

        /// <summary>
        /// シーケンサが開始された時に発生します。
        /// </summary>
        public event EventHandler SequenceStarted;

        /// <summary>
        /// シーケンサが停止した時に発生します。
        /// </summary>
        public event EventHandler SequenceStopped;

        /// <summary>
        /// シーケンサがスケジュールされたイベントの最後を処理し、シーケンスの最後に達した時に発生します。
        /// </summary>
        public event EventHandler SequenceEnd;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// シーケンスを指定して新しい Sequencer クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="sequence">一連のイベントが格納されたシーケンス。</param>
        public Sequencer(Sequence sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException();

            this.Sequence = sequence;
            this.events = new List<Event>(sequence.Tracks.SelectMany(t => t.Events).OrderBy(e => e.Tick));
            this.endOfTick = sequence.MaxTick;

            this.tick = -(long)(sequence.Resolution * 1.0);
            this.loopBeginTick = sequence.LoopBeginTick;

            this.RecalcTickTime();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// シーケンサを開始します。
        /// </summary>
        public void Start()
        {
            if (this.sequenceTask != null && !this.sequenceTask.IsCompleted)
                return;

            if (this.SequenceStarted != null)
                this.SequenceStarted(this, new EventArgs());

            this.reqEnd = false;
            this.sequenceTask = Task.Factory.StartNew(this.Update);
        }

        /// <summary>
        /// シーケンサを停止します。
        /// </summary>
        public void Stop()
        {
            if (this.sequenceTask == null)
                return;

            if (this.SequenceStopped != null)
                this.SequenceStopped(this, new EventArgs());

            this.reqEnd = true;

            if (Task.CurrentId.HasValue && Task.CurrentId.Value == this.sequenceTask.Id)
                return;

            this.sequenceTask.Wait();
            this.sequenceTask.Dispose();
            this.sequenceTask = null;
        }
        #endregion

        #region -- Private Methods --
        private void Update()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            long oldTick = 0L;
            long nowTick, startTick, endTick;
            long processTick;
            double progressTick = 0.0;

            while (!this.reqEnd)
            {
                Thread.Sleep(this.interval);

                if (this.reqEnd)
                    break;

                lock (this.syncObject)
                {
                    if (this.reqRewind)
                    {
                        nowTick = oldTick = 0L;
                        this.reqRewind = false;
                        stopwatch.Restart();
                        continue;
                    }

                    nowTick = stopwatch.ElapsedTicks;
                    progressTick += ((double)(nowTick - oldTick) * this.tickTime);

                    if (progressTick == 0.0)
                        continue;

                    processTick = (long)progressTick;
                    progressTick -= processTick;

                    startTick = this.tick;
                    endTick = startTick + processTick;

                    this.OutputEvents(this.SelectEvents(startTick, endTick).ToList());

                    oldTick = nowTick;
                    this.tick += processTick;

                    if (this.tick >= this.endOfTick)
                    {
                        if (this.Looping)
                            this.Tick = this.loopBeginTick;
                        else if (this.SequenceEnd != null)
                            this.SequenceEnd(this, new EventArgs());
                    }
                }
            }

            this.tick = -(long)(this.Sequence.Resolution * 1.0);

            stopwatch.Stop();
        }

        private IEnumerable<Event> SelectEvents(long start, long end)
        {
            Event @event;
            MetaEvent tempoEvent;

            if (this.eventIndex < 0)
                this.eventIndex = 0;

            this.eventIndex = this.events.FindIndex(this.eventIndex, e => e.Tick >= start);

            while (this.eventIndex >= 0 && this.eventIndex < this.events.Count && this.events[this.eventIndex].Tick < end)
            {
                @event = this.events[this.eventIndex++];

                if (@event is MetaEvent)
                {
                    tempoEvent = (MetaEvent)@event;
                    if (tempoEvent.MetaType == MetaType.Tempo)
                        this.ChangeTempo(tempoEvent.GetTempo());
                }

                yield return @event;
            }
        }

        private void ChangeTempo(double newTempo)
        {
            if (this.tempo == newTempo)
                return;

            double oldTempo = this.tempo;

            if (this.TempoChanged != null)
                this.TempoChanged(this, new TempoChangedEventArgs(oldTempo, newTempo));

            this.tempo = newTempo;
            this.RecalcTickTime();
        }

        private void OutputEvents(IEnumerable<Event> events)
        {
            if (this.OnTrackEvent != null)
                this.OnTrackEvent(this, new TrackEventArgs(events));
        }

        private void RecalcTickTime()
        {
            this.tickTime = 1.0 / ((double)Stopwatch.Frequency * ((60.0 / (this.tempo * this.tempoFactor)) / (double)this.Sequence.Resolution));
        }
        #endregion
    }

    /// <summary>
    /// イベントの送出イベントに用いられるデータを格納したクラスです。
    /// </summary>
    public class TrackEventArgs : EventArgs
    {
        #region -- Public Properties --
        /// <summary>
        /// イベントの列挙子を取得します。
        /// </summary>
        public IEnumerable<Event> Events { get; private set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// イベントの列挙子を指定して新しい TrackEventArgs クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public TrackEventArgs(IEnumerable<Event> events)
        {
            this.Events = events;
        }
        #endregion
    }

    /// <summary>
    /// テンポ変更イベントに用いられるデータを格納したクラスです。
    /// </summary>
    public class TempoChangedEventArgs : EventArgs
    {
        #region -- Public Properties --
        /// <summary>
        /// 変更前のテンポを取得します。
        /// </summary>
        public double OldTempo { get; private set; }

        /// <summary>
        /// 変更後のテンポを取得します。
        /// </summary>
        public double NewTempo { get; private set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 引数を指定して新しい TempoChangedEventArgs クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="oldTempo">変更前のテンポ。</param>
        /// <param name="newTempo">変更後のテンポ。</param>
        public TempoChangedEventArgs(double oldTempo, double newTempo)
        {
            this.OldTempo = oldTempo;
            this.NewTempo = newTempo;
        }
        #endregion
    }
}
