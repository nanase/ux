/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ux;
using ux.Component;
using uxMidi.IO;

namespace uxMidi
{
    /// <summary>
    /// ポリフォニックとして MIDI イベントを選択するセレクタを提供します。
    /// </summary>
    public class PolyphonicSelector : Selector
    {
        #region -- Private Fields --
        private const int PartParChannel = 8;
        private ProgramPreset[] nowPresets;
        private ProgramPreset[] nowDrumPresets;
        private int[] partLsb, partMsb, partProgram;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ux のマスターオブジェクトを取得します。
        /// </summary>
        public override int PartCount
        {
            get { return PolyphonicSelector.PartParChannel * 16; }
        }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定してマスターオブジェクトを生成し、
        /// 新しい PolyphonicSelector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
        public PolyphonicSelector(float samplingFreq)
            : base(new Master(samplingFreq, PolyphonicSelector.PartParChannel * 16))
        {
            this.Initalize();
        }

        /// <summary>
        /// マスターオブジェクトを指定して
        /// 新しい PolyphonicSelector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="master">マスターオブジェクト。</param>
        public PolyphonicSelector(Master master)
            : base(master)
        {
            this.Initalize();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ux にリセット命令を送ります。ドラムの初期化が発生します。
        /// </summary>
        public override void Reset()
        {
            var seq = Enumerable.Range(1, this.PartCount);
            var drumseq = Enumerable.Range(1 + 9 * PolyphonicSelector.PartParChannel, PolyphonicSelector.PartParChannel);

            this.master.PushHandle(seq.Select(i => new Handle(i, HandleType.Silence)));
            this.master.PushHandle(seq.Select(i => new Handle(i, HandleType.Reset)));
            this.master.PushHandle(seq.Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.FM)));

            this.master.PushHandle(drumseq.Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.LongNoise)));
            this.master.PushHandle(drumseq.Select(i => new Handle(i, HandleType.Envelope, (int)EnvelopeOperate.Sustain, 0.0f)));
            this.master.PushHandle(drumseq.Select(i => new Handle(i, HandleType.Envelope, (int)EnvelopeOperate.Release, 0.0f)));
        }

        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public override void ProcessMidiEvent(IEnumerable<Event> events)
        {
            foreach (MidiEvent message in events.Where(e => e is MidiEvent))
            {
                int part = message.Channel + 1;
                int target = 1 + PolyphonicSelector.PartParChannel * message.Channel;
                var seq = Enumerable.Range(target, PolyphonicSelector.PartParChannel);

                switch (message.Type)
                {
                    case EventType.NoteOff:
                        if (part != 10)
                        {
                            target += (message.Data1 % PolyphonicSelector.PartParChannel);
                            this.master.PushHandle(new Handle(target, HandleType.NoteOff, message.Data1));
                        }
                        break;

                    case EventType.NoteOn:
                        target += (message.Data1 % PolyphonicSelector.PartParChannel);

                        if (part == 10)
                        {
                            if (message.Data2 > 0)
                            {
                                var preset = this.drumset.Find(p => p.Number == message.Data1);

                                if (preset != null)
                                {
                                    this.master.PushHandle(preset.InitHandles, target);
                                    this.master.PushHandle(new Handle(target, HandleType.NoteOn, preset.Note, message.Data2 / 127f));
                                }
                            }
                        }
                        else
                        {
                            if (message.Data2 > 0)
                                this.master.PushHandle(new Handle(target, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                            else
                                this.master.PushHandle(new Handle(target, HandleType.NoteOff, message.Data1));
                        }
                        break;

                    case EventType.ControlChange:
                        switch (message.Data1)
                        {
                            case 0:
                                this.partMsb[message.Channel] = message.Data2;
                                break;

                            case 1:
                                if (message.Data2 > 0)
                                {
                                    this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Vibrate, (int)VibrateOperate.On)));
                                    this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f)));
                                }
                                else
                                    this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Vibrate, (int)VibrateOperate.Off)));
                                break;

                            case 7:
                                this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Volume, message.Data2 / 127f)));
                                break;

                            case 10:
                                this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Panpot, message.Data2 / 64f - 1f)));
                                break;

                            case 11:
                                this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f)));
                                break;

                            case 32:
                                this.partLsb[message.Channel] = message.Data2;
                                break;

                            case 120:
                                this.master.PushHandle(Enumerable.Range(1, this.PartCount).Select(i => new Handle(i, HandleType.Silence)));
                                break;

                            case 121:
                                this.Reset();
                                break;

                            case 123:
                                this.master.PushHandle(Enumerable.Range(1, this.PartCount).Select(i => new Handle(i, HandleType.NoteOff)));
                                break;

                            default:
                                break;
                        }

                        break;

                    case EventType.Pitchbend:
                        this.master.PushHandle(seq.Select(p => new Handle(p, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f)));
                        break;

                    case EventType.ProgramChange:
                        this.ChangeProgram(message, seq);
                        break;

                    default:
                        break;
                }
            }
        }
        #endregion

        #region -- Private Methods --
        private void Initalize()
        {
            this.partLsb = new int[16];
            this.partMsb = new int[16];
            this.partProgram = new int[16];
            this.nowPresets = new ProgramPreset[16];
            this.nowDrumPresets = new ProgramPreset[PolyphonicSelector.PartParChannel];
        }

        private void ChangeProgram(MidiEvent @event, IEnumerable<int> targets)
        {
            int channel = @event.Channel;
            int part = channel + 1;

            if (part == 10)
                return;

            if (this.nowPresets[channel] != null)
            {
                foreach (var item in this.nowPresets[channel].FinalHandles)
                    this.master.PushHandle(targets.Select(i => new Handle(item, i)));
            }

            ProgramPreset preset = this.presets.Find(p => p.Number == @event.Data1 && p.MSB == this.partMsb[channel] && p.LSB == this.partLsb[channel]) ??
                                   this.presets.Find(p => p.Number == @event.Data1);

            if (preset != null)
            {
                foreach (var item in preset.InitHandles)
                    this.master.PushHandle(targets.Select(i => new Handle(item, i)));

                Console.WriteLine("Matching Program: {0}", @event.Data1);
            }
            else
            {
                this.master.PushHandle(targets.Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.FM)));
                Console.WriteLine("Matching no Program: {0}", @event.Data1);
            }

            this.nowPresets[channel] = preset;
        }
        #endregion
    }
}
