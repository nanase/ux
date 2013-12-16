/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
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
        private int[] drumParts = new[] { 10, 17, 18, 19, 20, 21, 22, 23 };
        private int[] allParts;

        #region Handles
        #region Static
        private static Handle handle_vibrateOn = new Handle(0, HandleType.Vibrate, (int)VibrateOperate.On);
        private static Handle handle_vibrateOff = new Handle(0, HandleType.Vibrate, (int)VibrateOperate.Off);
        private static Handle handle_waveform_no_match = new Handle(0, HandleType.Waveform, (int)WaveformType.FM);
        #endregion

        #region Dynamic
        private Handle[] handles_reset;
        private Handle[] handle_silence;
        private Handle[] handle_release;
        #endregion
        #endregion
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
        /// <param name="samplingRate">サンプリング周波数。</param>
        public MonophonicSelector(float samplingRate)
            : base(new Master(samplingRate, 15 + 8))
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
            this.master.PushHandle(this.handles_reset);
        }

        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public override void ProcessMidiEvent(IEnumerable<Event> events)
        {
            foreach (MidiEvent message in events.OfType<MidiEvent>())
            {
                int channel = message.Channel;
                int targetPart = channel + 1;

                switch (message.Type)
                {
                    case EventType.NoteOff:
                        this.master.PushHandle(new Handle(targetPart, HandleType.NoteOff, message.Data1));
                        break;

                    case EventType.NoteOn:
                        if (targetPart == 10)
                        {
                            int target = message.Data1 % 8;

                            if (target == 0)
                                target = 10;
                            else
                                target += 16;

                            var preset = this.preset.FindDrum(p => p.Number == message.Data1);

                            if (preset != null)
                            {
                                this.master.PushHandle(preset.InitHandles, target);
                                this.master.PushHandle(new Handle(target, HandleType.NoteOn, preset.Note, message.Data2));
                            }
                        }
                        else
                        {
                            if (message.Data2 > 0)
                                this.master.PushHandle(new Handle(targetPart, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                            else
                                this.master.PushHandle(new Handle(targetPart, HandleType.NoteOff, message.Data1));
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
                                    this.master.PushHandle(MonophonicSelector.handle_vibrateOn);
                                    this.master.PushHandle(new Handle(targetPart, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f));
                                }
                                else
                                    this.master.PushHandle(MonophonicSelector.handle_vibrateOff);
                                break;

                            case 7:
                                this.master.PushHandle(new Handle(targetPart, HandleType.Volume, message.Data2 / 127f));
                                break;

                            case 10:
                                this.master.PushHandle(new Handle(targetPart, HandleType.Panpot, message.Data2 / 64f - 1f));
                                break;

                            case 11:
                                this.master.PushHandle(new Handle(targetPart, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f));
                                break;

                            case 32:
                                this.partLsb[message.Channel] = message.Data2;
                                break;

                            case 120:
                                this.master.PushHandle(this.handle_silence);
                                break;

                            case 121:
                                this.Reset();
                                break;

                            case 123:
                                this.master.PushHandle(this.handle_release);
                                break;

                            default:
                                break;
                        }

                        break;

                    case EventType.Pitchbend:
                        this.master.PushHandle(new Handle(targetPart, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f));
                        break;

                    case EventType.ProgramChange:
                        if (targetPart != 10)
                            this.ChangeProgram(message, targetPart);
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

            this.allParts = Enumerable.Range(1, this.PartCount).ToArray();

            #region Handles
            this.handle_silence = this.allParts.Select(p => new Handle(p, HandleType.Silence)).ToArray();
            this.handle_release = this.allParts.Select(p => new Handle(p, HandleType.NoteOff)).ToArray();
            this.handles_reset = this.handle_silence
                                     .Concat(this.allParts.Select(p => new Handle(p, HandleType.Reset)))
                                     .Concat(this.allParts.Select(p => new Handle(p, HandleType.Waveform, (int)WaveformType.FM)))
                                     .Concat(this.drumParts.Select(p => new Handle(p, HandleType.Waveform, (int)WaveformType.LongNoise)))
                                     .Concat(this.drumParts.Select(p => new Handle(p, HandleType.Envelope, (int)EnvelopeOperate.Sustain, 0.0f)))
                                     .ToArray();
            #endregion
        }

        private void ChangeProgram(MidiEvent @event, int part)
        {
            int channel = @event.Channel;

            if (this.nowPresets[channel] != null)
                this.master.PushHandle(this.nowPresets[channel].FinalHandles, part);

            ProgramPreset preset = this.preset.FindProgram(p => p.Number == @event.Data1 && p.MSB == this.partMsb[channel] && p.LSB == this.partLsb[channel]) ??
                                   this.preset.FindProgram(p => p.Number == @event.Data1);

            if (preset != null)
            {
                this.master.PushHandle(preset.InitHandles, part);
                Console.WriteLine("Matching Program: {0}", @event.Data1);
            }
            else
            {
                this.master.PushHandle(new Handle(MonophonicSelector.handle_waveform_no_match, part));
                Console.WriteLine("Matching no Program: {0}", @event.Data1);
            }

            this.nowPresets[channel] = preset;
        }
        #endregion
    }
}
