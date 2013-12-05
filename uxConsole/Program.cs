/* uxConsole / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Linq;
using ALSharp;
using uxMidi;

namespace uxConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const int frequencty = 44100;

            MidiConnector connector;

            if (args.Any(s => !s.StartsWith("-")))
            {
                connector = new SmfConnector(frequencty);
                ((SmfConnector)connector).Load(args.First(s => !s.StartsWith("-")));
            }
            else
            {
                connector = new MidiInConnector(frequencty, 0);
            }

            foreach (var preset in args.Where(a => a.StartsWith("-p:")).Select(a => a.Substring(3)))
            {
                Console.Write("Preset: " + preset);
                connector.AddPreset(preset);
                Console.WriteLine(" ... [OK]");
            }

            Console.WriteLine("[q] Quit, [r] Reload Presets");

            Func<float[], int, int, int> process = (buffer, offset, count) => connector.Master.Read(buffer, offset, count);

            var setting = new PlayerSettings() { BufferSize = 512, BufferCount = 64, BitPerSample = 16, SamplingFrequency = frequencty };
            using (var l = new SinglePlayer(process, setting))
            {
                Console.WriteLine("Playing!");
                l.Play();
                connector.Play();

                var end = false;

                do
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Q:
                            end = true;
                            break;

                        case ConsoleKey.R:
                            connector.ReloadPreset();
                            break;

                        default:
                            break;
                    }
                } while (!end);
            }

            connector.Stop();
        }
    }
}
