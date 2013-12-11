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
    /// ポリフォニックとして MIDI イベントを選択するセレクタを提供します。
    /// </summary>
    public class PolyphonicSelector : Selector
    {
        #region -- Private Fields --
        private const int PartParChannel = 8;
        private ProgramPreset[] nowPresets;
        private ProgramPreset[] nowDrumPresets;
        private int[] partLsb, partMsb, partProgram;

        private int[] allParts, drumParts;
        private int[][] partSequences;

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
            this.master.PushHandle(this.handles_reset);
        }

        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public override void ProcessMidiEvent(IEnumerable<Event> events)
        {
            foreach (MidiEvent message in events.Where(e => e is MidiEvent))
            {
                int channel = message.Channel;
                int targetPart = channel + 1;
                int targetStart = 1 + PolyphonicSelector.PartParChannel * channel;
                var targetParts = this.partSequences[channel];

                switch (message.Type)
                {
                    case EventType.NoteOff:
                        targetStart += (message.Data1 % PolyphonicSelector.PartParChannel);
                        this.master.PushHandle(new Handle(targetStart, HandleType.NoteOff, message.Data1));
                        break;

                    case EventType.NoteOn:
                        targetStart += (message.Data1 % PolyphonicSelector.PartParChannel);

                        if (targetPart == 10)
                        {
                            if (message.Data2 > 0)
                            {
                                var preset = this.drumset.Find(p => p.Number == message.Data1);

                                if (preset != null)
                                {
                                    this.master.PushHandle(preset.InitHandles, targetStart);
                                    this.master.PushHandle(new Handle(targetStart, HandleType.NoteOn, preset.Note, message.Data2 / 127f));
                                }
                            }
                        }
                        else
                        {
                            if (message.Data2 > 0)
                                this.master.PushHandle(new Handle(targetStart, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                            else
                                this.master.PushHandle(new Handle(targetStart, HandleType.NoteOff, message.Data1));
                        }
                        break;

                    case EventType.ControlChange:
                        switch (message.Data1)
                        {
                            case 0:
                                this.partMsb[channel] = message.Data2;
                                break;

                            case 1:
                                if (message.Data2 > 0)
                                {
                                    this.master.PushHandle(PolyphonicSelector.handle_vibrateOn, targetParts);
                                    this.master.PushHandle(new Handle(0, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f), targetParts);
                                }
                                else
                                    this.master.PushHandle(PolyphonicSelector.handle_vibrateOff, targetParts);
                                break;

                            case 7:
                                this.master.PushHandle(new Handle(0, HandleType.Volume, message.Data2 / 127f), targetParts);
                                break;

                            case 10:
                                this.master.PushHandle(new Handle(0, HandleType.Panpot, message.Data2 / 64f - 1f), targetParts);
                                break;

                            case 11:
                                this.master.PushHandle(new Handle(0, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f), targetParts);
                                break;

                            case 32:
                                this.partLsb[channel] = message.Data2;
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
                        this.master.PushHandle(new Handle(0, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f), targetParts);
                        break;

                    case EventType.ProgramChange:
                        if (targetPart != 10)
                            this.ChangeProgram(message, targetParts);
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

            this.allParts = Enumerable.Range(1, this.PartCount).ToArray();
            this.drumParts = Enumerable.Range(1 + 9 * PolyphonicSelector.PartParChannel, PolyphonicSelector.PartParChannel).ToArray();

            this.partSequences = new int[16][];

            for (int i = 0; i < 16; i++)
                this.partSequences[i] = Enumerable.Range(1 + i * PolyphonicSelector.PartParChannel, PolyphonicSelector.PartParChannel).ToArray();

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

        private void ChangeProgram(MidiEvent @event, int[] targets)
        {
            int channel = @event.Channel;

            if (this.nowPresets[channel] != null)
            {
                foreach (var item in this.nowPresets[channel].FinalHandles)
                    this.master.PushHandle(item, targets);
            }

            ProgramPreset preset = this.presets.Find(p => p.Number == @event.Data1 && p.MSB == this.partMsb[channel] && p.LSB == this.partLsb[channel]) ??
                                   this.presets.Find(p => p.Number == @event.Data1);

            if (preset != null)
            {
                foreach (var item in preset.InitHandles)
                    this.master.PushHandle(item, targets);

                Console.WriteLine("Matching Program: {0}", @event.Data1);
            }
            else
            {
                this.master.PushHandle(PolyphonicSelector.handle_waveform_no_match, targets);
                Console.WriteLine("Matching no Program: {0}", @event.Data1);
            }

            this.nowPresets[channel] = preset;
        }
        #endregion
    }
}
