/* uxConsole / Software Synthesizer Library

LICENSE - The MIT License (MIT)

Copyright (c) 2013-2014 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Linq;
using ALSharp;
using ux.Utils.Midi;

namespace uxConsole
{
    class Program
    {
        static void Main(string[] args)
        #region -- Public Static Methods --
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
        #endregion
    }
}
