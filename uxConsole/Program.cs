using ALSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ux;
using ux.Component;
using uxMidi.IO;
using uxMidi.Sequencer;
using ALSharp.Utils;

namespace uxConsole
{
    class Program
    {
        static long loopTick = 0L;

        static void Main (string[] args)
        {
            const int frequencty = 44100;

            if (args.Length == 0)
            {
                Console.WriteLine ("MIDI ファイルを指定してください。");
                return;
            }

            if (!File.Exists (args [0]))
            {
                Console.WriteLine ("ファイル {0} は存在しません。", args [0]);
                return;
            }

            Sequence sequence = new Sequence (args [0]);
            Sequencer sequencer = new Sequencer (sequence);

            Console.WriteLine ("Loaded: {0}", Path.GetFileName (args [0]));
            Encoding sjis = Encoding.GetEncoding (932);

            foreach (MetaEvent item in sequence.Tracks.Where(t => t.Number == 0).SelectMany(t => t.Events).Where(e => e is MetaEvent))
            {
                switch (item.MetaType)
                {
                    case MetaType.Text:
                        Console.WriteLine ("\tText: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.Copyrights:
                        Console.WriteLine ("\tCopyrights: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.TrackName:
                        Console.WriteLine ("\tTitle: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.InstrumentName:
                        Console.WriteLine ("\tInstrument: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.Lyrics:
                        Console.WriteLine ("\tLyrics: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.ProgramName:
                        Console.WriteLine ("\tProgram: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    case MetaType.DeviceName:
                        Console.WriteLine ("\tDevice: {0}", sjis.GetString (item.Data).Trim ());
                        break;
                    default:
                        break;
                }

            }

            Console.WriteLine ("\tFormat: {0}, Resolution: {1}", sequence.Format, sequence.Resolution);
            Console.WriteLine ("\tTracks: {0}, Events: {1}, MaxTick: {2}", sequence.Tracks.Count (), sequence.EventCount, sequence.MaxTick);
            Console.WriteLine ("Frequency: {0:n0} Hz", Stopwatch.Frequency);

            //
            Master master = new Master (frequencty, 16);

            Reset (master);

            master.Play ();

            sequencer.OnTrackEvent += (sender, e) => ProcessMessage (master, e.Events);
            sequencer.SequenceStarted += (sender, e) => Console.WriteLine ("Sequencer Start");
            sequencer.SequenceEnd += (sender, e) => { 
                Console.WriteLine ("Sequencer End");

                if (loopTick <= sequence.MaxTick - sequence.Resolution * 4)
                {
                    master.Release ();
                    sequencer.Tick = loopTick;
                    sequencer.Start ();
                }
            };
            sequencer.SequenceStopped += (sender, e) => Console.WriteLine ("Sequencer Stop");
            sequencer.TempoChanged += (sender, e) => Console.WriteLine ("Tempo {0:f2} => {1:f2}", e.OldTempo, e.NewTempo);

            Func<float[], int, int, int> process = (buffer, offset, count) => master.Read (buffer, offset, count);

            var setting = new PlayerSettings () { BufferSize = 512, BufferCount = 64, BitPerSample = 16, SamplingFrequency = frequencty };
            using (var l = new SinglePlayer(process, setting))
            {
                l.Play ();
                sequencer.Start ();
                Console.ReadLine ();
                Console.CursorTop--;
            }

            sequencer.Stop ();
        }

        static void ProcessMessage (Master master, IEnumerable<Event> messages)
        {
            foreach (MidiEvent message in messages.Where(e => e is MidiEvent))
            {
                int part = message.Channel + 1;

                switch (message.Type)
                {
                    case EventType.NoteOff:
                        if (part != 10)
                            master.PushHandle (new Handle (part, HandleType.NoteOff, message.Data1));
                        break;

                    case EventType.NoteOn:
                        if (part == 10)
                        {
                            if (message.Data1 == 38 || message.Data1 == 40)
                                master.PushHandle (new Handle (part, HandleType.NoteOn, 50, message.Data2 / 127f));
                            else if (message.Data1 == 35 || message.Data1 == 36)
                                master.PushHandle (new Handle (part, HandleType.NoteOn, 29, message.Data2 / 127f));
                        }
                        else
                            master.PushHandle (new Handle (part, HandleType.NoteOn, message.Data1, message.Data2 / 127f));
                        break;

                    case EventType.ControlChange:
                        switch (message.Data1)
                        {
                            case 1:
                                if (message.Data2 > 0)
                                {
                                    master.PushHandle (new Handle (part, HandleType.Vibrate, (int)VibrateOperate.On));
                                    master.PushHandle (new Handle (part, HandleType.Vibrate, (int)VibrateOperate.Depth, message.Data2 * 0.125f));
                                }
                                else
                                    master.PushHandle (new Handle (part, HandleType.Vibrate, (int)VibrateOperate.Off));
                                break;

                            case 7:
                                master.PushHandle (new Handle (part, HandleType.Volume, message.Data2 / 127f));
                                break;

                            case 10:
                                master.PushHandle (new Handle (part, HandleType.Panpot, message.Data2 / 64f - 1f));
                                break;

                            case 11:
                                master.PushHandle (new Handle (part, HandleType.Volume, (int)VolumeOperate.Expression, message.Data2 / 127f));
                                break;

                            case 120:
                                master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Silence)));
                                break;

                            case 121:
                                master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Reset)));
                                Reset (master);
                                break;

                            case 123:
                                master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.NoteOff)));
                                break;

                            case 111:
                                Console.WriteLine ("Tkool MIDI loop Message in {0} tick", message.Tick);
                                loopTick = message.Tick;
                                break;

                            default:
                                break;
                        }

                        break;

                    case EventType.Pitchbend:
                        master.PushHandle (new Handle (part, HandleType.FineTune, ((((message.Data2 << 7) | message.Data1) - 8192) / 8192f) * 1.12246f / 8f + 1f));
                        break;

                    default:
                        break;
                }
            }
        }

        static void Reset (Master master)
        {
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Reset)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Waveform, (int)WaveformType.FM)));

            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator0 | FMOperate.Send0), 1.0f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator1 | FMOperate.Send1), 0.8f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator1 | FMOperate.Send0), 0.8f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator2 | FMOperate.Send0), 1.8f)));

            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator0 | FMOperate.Frequency), 1.002f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator1 | FMOperate.Frequency), 2.008f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator2 | FMOperate.Frequency), 4.006f)));

            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator0 | FMOperate.Send0) | (int)(EnvelopeOperate.Decay), 0.5f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator0 | FMOperate.Send0) | (int)(EnvelopeOperate.Sustain), 0.0f)));

            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator2 | FMOperate.Send0) | (int)(EnvelopeOperate.Decay), 0.1f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.EditWaveform, (int)(FMOperate.Operator2 | FMOperate.Send0) | (int)(EnvelopeOperate.Sustain), 0.0f)));

            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Envelope, (int)EnvelopeOperate.Attack, 0.0f)));
            master.PushHandle (Enumerable.Range (0, 16).Select (i => new Handle (i, HandleType.Envelope, (int)EnvelopeOperate.Release, 0.5f)));

            master.PushHandle (new[]{
                                            new Handle (10, HandleType.Waveform, (int)WaveformType.LongNoise),
                                            new Handle (10, HandleType.EditWaveform, (int)StepWaveformOperate.FreqFactor, 1f),
                                            new Handle (10, HandleType.Envelope, (int)EnvelopeOperate.Attack, 0f),
                                            new Handle (10, HandleType.Envelope, (int)EnvelopeOperate.Peak, 0.01f),
                                            new Handle (10, HandleType.Envelope, (int)EnvelopeOperate.Decay, 0.25f),
                                            new Handle (10, HandleType.Envelope, (int)EnvelopeOperate.Sustain, 0.0f),
                                            new Handle (10, HandleType.Envelope, (int)EnvelopeOperate.Release, 0.0f),
                                        }
            );
        }
    }
}
