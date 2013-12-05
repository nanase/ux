/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ux;
using ux.Component;
using uxMidi.IO;

namespace uxMidi
{
    /// <summary>
    /// ux と MIDI を接続するための抽象クラスです。
    /// </summary>
    public abstract class MidiConnector : IDisposable
    {
        private readonly List<ProgramPreset> presets;
        private readonly List<DrumPreset> drumset;
        private readonly List<string> presetFiles;
        private readonly int[] partLsb, partMsb, partProgram;
        private readonly ProgramPreset[] nowPresets;
        private readonly int[] drumTargets = new[] { 10, 17, 18, 19, 20, 21, 22, 23 };
        #region -- Private Fields --
        #endregion

        #region -- Protected Fields --
        /// <summary>
        /// ux のマスターオブジェクトです。
        /// </summary>
        protected readonly Master uxMaster;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ux のマスターオブジェクトを取得します。
        /// </summary>
        public Master Master { get { return this.uxMaster; } }

        /// <summary>
        /// 通過した MIDI イベントの総数を取得します。
        /// </summary>
        public long EventCount { get; protected set; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定して新しい MidiConnector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
        public MidiConnector(float samplingFreq)
        {
            const int Part = 15 + 8;

            this.presets = new List<ProgramPreset>();
            this.drumset = new List<DrumPreset>();
            this.presetFiles = new List<string>();
            this.uxMaster = new Master(samplingFreq, Part);

            this.partLsb = new int[Part];
            this.partMsb = new int[Part];
            this.partProgram = new int[Part];
            this.nowPresets = new ProgramPreset[Part];
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ファイル名を指定してプリセットを追加します。
        /// </summary>
        /// <param name="filename">追加されるプリセットが記述された XML ファイル名。</param>
        public void AddPreset(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            this.presets.AddRange(PresetReader.Load(filename));
            this.drumset.AddRange(PresetReader.DrumLoad(filename));
            this.presetFiles.Add(filename);
        }

        /// <summary>
        /// 読み込まれたプリセットをすべてクリアします。
        /// </summary>
        public void ClearPreset()
        {
            this.presets.Clear();
            this.drumset.Clear();
            this.presetFiles.Clear();
        }

        /// <summary>
        /// プリセットをリロードします。現在設定されている音源の更新はされません。
        /// </summary>
        public void ReloadPreset()
        {
            this.presets.Clear();
            this.drumset.Clear();

            foreach (var filename in this.presetFiles)
            {
                if (!File.Exists(filename))
                    continue;

                this.presets.AddRange(PresetReader.Load(filename));
                this.drumset.AddRange(PresetReader.DrumLoad(filename));
            }
        }

        /// <summary>
        /// MIDI とのコネクションを開始します。実際の動作は継承クラスによって異なります。
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// MIDI とのコネクションを停止します。実際の動作は継承クラスによって異なります。
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// ux にリセット命令を送ります。ドラムの初期化が発生します。
        /// </summary>
        public void Reset()
        {
            this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
            this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Reset)));
            this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.FM)));

            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Waveform, (int)WaveformType.LongNoise)));
            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Envelope, (int)EnvelopeOperate.Sustain, 0.0f)));
            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Envelope, (int)EnvelopeOperate.Release, 0.0f)));
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        protected void ProcessMidiEvent(IEnumerable<Event> events)
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
                        this.uxMaster.PushHandle(new Handle(part, HandleType.NoteOff, message.Data1));
                        break;

                    case EventType.NoteOn:
                        if (message.Data2 > 0)
                            this.uxMaster.PushHandle(new Handle(part, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                        else
                            this.uxMaster.PushHandle(new Handle(part, HandleType.NoteOff, message.Data1));
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
                                    this.uxMaster.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.On));
                                    this.uxMaster.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f));
                                }
                                else
                                    this.uxMaster.PushHandle(new Handle(part, HandleType.Vibrate, (int)VibrateOperate.Off));
                                break;

                            case 7:
                                this.uxMaster.PushHandle(new Handle(part, HandleType.Volume, message.Data2 / 127f));
                                break;

                            case 10:
                                this.uxMaster.PushHandle(new Handle(part, HandleType.Panpot, message.Data2 / 64f - 1f));
                                break;

                            case 11:
                                this.uxMaster.PushHandle(new Handle(part, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f));
                                break;

                            case 32:
                                this.partLsb[message.Channel] = message.Data2;
                                break;

                            case 120:
                                this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
                                break;

                            case 121:
                                this.Reset();
                                break;

                            case 123:
                                this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.NoteOff)));
                                break;

                            default:
                                break;
                        }

                        break;

                    case EventType.Pitchbend:
                        this.uxMaster.PushHandle(new Handle(part, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f));
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

        #region Private Method
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
                        this.uxMaster.PushHandle(preset.InitHandles, target);
                        this.uxMaster.PushHandle(new Handle(target, HandleType.NoteOn, preset.Note, @event.Data2));
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
                                this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.On)));
                                this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.Depth, @event.Data2 * 0.125f)));
                            }
                            else
                                this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Vibrate, (int)VibrateOperate.Off)));
                            break;

                        case 7:
                            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Volume, @event.Data2 / 127f)));
                            break;

                        case 10:
                            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Panpot, @event.Data2 / 64f - 1f)));
                            break;

                        case 11:
                            this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.Volume, (int)VolumeOperate.Expression, @event.Data2 / 127f)));
                            break;

                        case 32:
                            this.partLsb[@event.Channel] = @event.Data2;
                            break;

                        case 120:
                            this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.Silence)));
                            break;

                        case 121:
                            this.Reset();
                            break;

                        case 123:
                            this.uxMaster.PushHandle(Enumerable.Range(1, 23).Select(i => new Handle(i, HandleType.NoteOff)));
                            break;

                        default:
                            break;
                    }

                    break;

                case EventType.Pitchbend:
                    this.uxMaster.PushHandle(this.drumTargets.Select(i => new Handle(i, HandleType.FineTune, ((((@event.Data2 << 7) | @event.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f)));
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
                this.uxMaster.PushHandle(this.nowPresets[channel].FinalHandles, part);

            ProgramPreset preset = this.presets.Find(p => p.Number == @event.Data1 && p.MSB == this.partMsb[channel] && p.LSB == this.partLsb[channel]) ??
                                   this.presets.Find(p => p.Number == @event.Data1);

            if (preset != null)
            {
                this.uxMaster.PushHandle(preset.InitHandles, part);
                Console.WriteLine("Matching Program: {0}", @event.Data1);
            }
            else
            {
                this.uxMaster.PushHandle(new Handle(part, HandleType.Waveform, (int)WaveformType.FM));
                Console.WriteLine("Matching no Program: {0}", @event.Data1);
            }

            this.nowPresets[channel] = preset;
        }
        #endregion

        #region IDisposable メンバー
        /// <summary>
        /// このオブジェクトに割り当てられたリソースを解放します。
        /// </summary>
        public abstract void Dispose();
        #endregion
    }
}
