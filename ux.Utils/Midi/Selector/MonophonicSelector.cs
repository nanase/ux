/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ux;
using ux.Component;
using ux.Utils.Midi.IO;

namespace ux.Utils.Midi
{
    /// <summary>
    /// モノフォニックとして MIDI イベントを選択するセレクタを提供します。
    /// </summary>
    public class MonophonicSelector : Selector
    {
        #region -- Private Fields --
        private ProgramPreset[] nowPresets;

        private int[] partLsb, partMsb, partProgram;
        private int[] drumTargets = new[] { 10, 17, 18, 19, 20, 21, 22, 23 };
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// このセレクタがマスターオブジェクトに要求するパート数を取得します。
        /// </summary>
        public override int PartCount
        {
            get { return 15 + 8; }
        }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定してマスターオブジェクトを生成し、
        /// 新しい MonophonicSelector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
        public MonophonicSelector(float samplingFreq)
            : base(new Master(samplingFreq, 15 + 8))
        {
            this.Initalize();
        }

        /// <summary>
        /// マスターオブジェクトを指定して
        /// 新しい MonophonicSelector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="master">マスターオブジェクト。</param>
        public MonophonicSelector(Master master)
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
            this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
            this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Reset)));
            this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.FM)));

            this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.LongNoise)));
            this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Envelope, (int)EnvelopeOperate.Sustain, 0.0f)));
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

                if (part == 10)
                {
                    this.ProcessDrumEvent(message);
                    continue;
                }

                switch (message.Type)
                {
                    case EventType.NoteOff:
                        this.master.PushHandle(new Handle(part, HandleType.NoteOff, message.Data1));
                        break;

                    case EventType.NoteOn:
                        if (message.Data2 > 0)
                            this.master.PushHandle(new Handle(part, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                        else
                            this.master.PushHandle(new Handle(part, HandleType.NoteOff, message.Data1));
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
                                    this.master.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.On));
                                    this.master.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f));
                                }
                                else
                                    this.master.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.Off));
                                break;

                            case 7:
                                this.master.PushHandle(new Handle(part, HandleType.Volume, message.Data2 / 127f));
                                break;

                            case 10:
                                this.master.PushHandle(new Handle(part, HandleType.Panpot, message.Data2 / 64f - 1f));
                                break;

                            case 11:
                                this.master.PushHandle(new Handle(part, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f));
                                break;

                            case 32:
                                this.partLsb[message.Channel] = message.Data2;
                                break;

                            case 120:
                                this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
                                break;

                            case 121:
                                this.Reset();
                                break;

                            case 123:
                                this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.NoteOff)));
                                break;

                            default:
                                break;
                        }

                        break;

                    case EventType.Pitchbend:
                        this.master.PushHandle(new Handle(part, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f));
                        break;

                    case EventType.ProgramChange:
                        this.ChangeProgram(message);
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

            this.partLsb = new int[this.PartCount];
            this.partMsb = new int[this.PartCount];
            this.partProgram = new int[this.PartCount];
            this.nowPresets = new ProgramPreset[this.PartCount];
        }

        private void ProcessDrumEvent(MidiEvent @event)
        {
            switch (@event.Type)
            {
                case EventType.NoteOn:

                    int target = @event.Data1 % 8;

                    if (target == 0)
                        target = 10;
                    else
                        target += 16;

                    var preset = this.drumset.Find(p => p.Number == @event.Data1);

                    if (preset != null)
                    {
                        this.master.PushHandle(preset.InitHandles, target);
                        this.master.PushHandle(new Handle(target, HandleType.NoteOn, preset.Note, @event.Data2));
                    }
                    break;

                case EventType.ControlChange:
                    switch (@event.Data1)
                    {
                        case 0:
                            this.partMsb[@event.Channel] = @event.Data2;
                            break;

                        case 1:
                            if (@event.Data2 > 0)
                            {
                                this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.On)));
                                this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.Depth, @event.Data2 * 0.125f)));
                            }
                            else
                                this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.Off)));
                            break;

                        case 7:
                            this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Volume, @event.Data2 / 127f)));
                            break;

                        case 10:
                            this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Panpot, @event.Data2 / 64f - 1f)));
                            break;

                        case 11:
                            this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Volume, (int)VolumeOperate.Expression, @event.Data2 / 127f)));
                            break;

                        case 32:
                            this.partLsb[@event.Channel] = @event.Data2;
                            break;

                        case 120:
                            this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
                            break;

                        case 121:
                            this.Reset();
                            break;

                        case 123:
                            this.master.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.NoteOff)));
                            break;

                        default:
                            break;
                    }

                    break;

                case EventType.Pitchbend:
                    this.master.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.FineTune, ((((@event.Data2 << 7) | @event.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f)));
                    break;

                default:
                    break;
            }
        }

        private void ChangeProgram(MidiEvent @event)
        {
            int channel = @event.Channel;
            int part = channel + 1;

            if (part == 10)
                return;

            if (this.nowPresets[channel] != null)
                this.master.PushHandle(this.nowPresets[channel].FinalHandles, part);

            ProgramPreset preset = this.presets.Find(p => p.Number == @event.Data1 && p.MSB == this.partMsb[channel] && p.LSB == this.partLsb[channel]) ??
                                   this.presets.Find(p => p.Number == @event.Data1);

            if (preset != null)
            {
                this.master.PushHandle(preset.InitHandles, part);
                Console.WriteLine("Matching Program: {0}", @event.Data1);
            }
            else
            {
                this.master.PushHandle(new Handle(part, HandleType.Waveform, (int)WaveformType.FM));
                Console.WriteLine("Matching no Program: {0}", @event.Data1);
            }

            this.nowPresets[channel] = preset;
        }
        #endregion
    }
}
