/* uxBenchmark / Software Synthesizer Library

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
using System.Diagnostics;
using System.Linq;
using ux;
using ux.Component;

namespace uxBenchmark
{
    class Program
    {
        #region -- Private Fields --
        private const int samplingRate = 44100;
        private const int bufferSize = 1024;
        private const int iterate = 3000;        
        #endregion

        #region -- Public Static Methods --
        public static void Main(string[] args)
        {
            Console.WriteLine("ux Benchmark :: Copyright (c) 2013-2014 Tomona Nanase");
            Console.Write("Press any key to start benchmark...");
            Console.ReadLine();

            var sw = Stopwatch.StartNew();

            MuteTest();
            DefaultSquareTest();
            DefaultFMTest();
            FullFMTest();

            sw.Stop();

            Console.WriteLine();
            Console.WriteLine("Elapsed Time: {0:mm\\:ss}", sw.Elapsed);
            Console.WriteLine("Complete!");
            Console.ReadLine();
        }
        #endregion

        #region -- Private Static Methods --
        #region MuteTest
        static void MuteTest()
        {
            TimeSpan result;

            Console.WriteLine();
            Console.WriteLine("[MuteTest] for {0}", iterate * 10);

            Console.WriteLine("Part  Total  PerCall      PerSecond");

            foreach (int partCount in new[] { 16, 64, 128, 256 })
            {
                Console.Write("{0,-4}  ", partCount);
                result = MuteTest(partCount);
                Console.WriteLine("{0:mm\\:ss}  {1:ss\\.fffffff}s  {2:f3}", result, result.Divide(iterate), 1.0 / result.Divide(iterate).TotalSeconds);
            }
        }

        static TimeSpan MuteTest(int partCount)
        {
            Master master = new Master(samplingRate, partCount);
            float[] buffer = new float[bufferSize];

            master.Play();
            master.Read(buffer, 0, bufferSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            sw.Restart();
            sw.Restart();

            for (int i = 0; i < iterate * 10; i++)
            {
                master.Read(buffer, 0, bufferSize);
            }

            sw.Stop();

            return sw.Elapsed;
        }
        #endregion

        #region DefaultFMTest
        static void DefaultFMTest()
        {
            TimeSpan result;

            Console.WriteLine();
            Console.WriteLine("[DefaultFMTest] for {0}", iterate);

            Console.WriteLine("Part  Total  PerCall      PerSecond");

            foreach (int partCount in new[] { 16, 64, 128, 256 })
            {
                Console.Write("{0,-4}  ", partCount);
                result = DefaultFMTest(partCount);
                Console.WriteLine("{0:mm\\:ss}  {1:ss\\.fffffff}s  {2:f3}", result, result.Divide(iterate), 1.0 / result.Divide(iterate).TotalSeconds);
            }
        }

        static TimeSpan DefaultFMTest(int partCount)
        {
            Master master = new Master(samplingRate, partCount);
            float[] buffer = new float[bufferSize];

            master.PushHandle(new Handle(0, HandleType.Waveform, (int)WaveformType.FM), Enumerable.Range(1, partCount));
            master.PushHandle(new Handle(0, HandleType.NoteOn, 60, 1.0f), Enumerable.Range(1, partCount));

            master.Play();
            master.Read(buffer, 0, bufferSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            sw.Restart();
            sw.Restart();

            for (int i = 0; i < iterate; i++)
            {
                master.Read(buffer, 0, bufferSize);
            }

            sw.Stop();

            return sw.Elapsed;
        }
        #endregion

        #region DefaultSquareTest
        static void DefaultSquareTest()
        {
            TimeSpan result;

            Console.WriteLine();
            Console.WriteLine("[DefaultSquareTest] for {0}", iterate);

            Console.WriteLine("Part  Total  PerCall      PerSecond");

            foreach (int partCount in new[] { 16, 64, 128, 256 })
            {
                Console.Write("{0,-4}  ", partCount);
                result = DefaultSquareTest(partCount);
                Console.WriteLine("{0:mm\\:ss}  {1:ss\\.fffffff}s  {2:f3}", result, result.Divide(iterate), 1.0 / result.Divide(iterate).TotalSeconds);
            }
        }

        static TimeSpan DefaultSquareTest(int partCount)
        {
            Master master = new Master(samplingRate, partCount);
            float[] buffer = new float[bufferSize];

            master.PushHandle(new Handle(0, HandleType.Waveform, (int)WaveformType.Square), Enumerable.Range(1, partCount));
            master.PushHandle(new Handle(0, HandleType.NoteOn, 60, 1.0f), Enumerable.Range(1, partCount));

            master.Play();
            master.Read(buffer, 0, bufferSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            sw.Restart();
            sw.Restart();

            for (int i = 0; i < iterate; i++)
            {
                master.Read(buffer, 0, bufferSize);
            }

            sw.Stop();

            return sw.Elapsed;
        }
        #endregion

        #region FullFMTest
        static void FullFMTest()
        {
            TimeSpan result;

            Console.WriteLine();
            Console.WriteLine("[FullFMTest] for {0}", iterate);

            Console.WriteLine("Part  Total  PerCall      PerSecond");

            foreach (int partCount in new[] { 16, 64, 128, 256 })
            {
                Console.Write("{0,-4}  ", partCount);
                result = FullFMTest(partCount);
                Console.WriteLine("{0:mm\\:ss}  {1:ss\\.fffffff}s  {2:f3}", result, result.Divide(iterate), 1.0 / result.Divide(iterate).TotalSeconds);
            }
        }

        static TimeSpan FullFMTest(int partCount)
        {
            Master master = new Master(samplingRate, partCount);
            float[] buffer = new float[bufferSize];

            var range = Enumerable.Range(1, partCount).ToArray();

            master.PushHandle(new Handle(0, HandleType.Waveform, (int)WaveformType.FM), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)FMOperate.Send0, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)FMOperate.Send0, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)FMOperate.Send0, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)FMOperate.Send0, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)FMOperate.Send1, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)FMOperate.Send1, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)FMOperate.Send1, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)FMOperate.Send1, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)FMOperate.Send2, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)FMOperate.Send2, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)FMOperate.Send2, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)FMOperate.Send2, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)FMOperate.Send3, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)FMOperate.Send3, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)FMOperate.Send3, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)FMOperate.Send3, 0.1f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)FMOperate.Out, 0.25f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)FMOperate.Out, 0.25f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)FMOperate.Out, 0.25f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)FMOperate.Out, 0.25f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)EnvelopeOperate.A, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)EnvelopeOperate.A, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)EnvelopeOperate.A, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)EnvelopeOperate.A, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)EnvelopeOperate.P, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)EnvelopeOperate.P, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)EnvelopeOperate.P, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)EnvelopeOperate.P, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)EnvelopeOperate.D, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)EnvelopeOperate.D, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)EnvelopeOperate.D, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)EnvelopeOperate.D, 5), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op0 | (int)EnvelopeOperate.S, 0.5f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op1 | (int)EnvelopeOperate.S, 0.5f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op2 | (int)EnvelopeOperate.S, 0.5f), range);
            master.PushHandle(new Handle(0, HandleType.Edit, (int)FMOperate.Op3 | (int)EnvelopeOperate.S, 0.5f), range);
            master.PushHandle(new Handle(0, HandleType.NoteOn, 60, 1.0f), range);

            master.Play();
            master.Read(buffer, 0, bufferSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            sw.Restart();
            sw.Restart();

            for (int i = 0; i < iterate; i++)
            {
                master.Read(buffer, 0, bufferSize);
            }

            sw.Stop();

            return sw.Elapsed;
        }
        #endregion
        #endregion
    }

    public static class Extension
    {
        #region -- Public Static Methods --
        public static TimeSpan Divide(this TimeSpan a, double b)
        {
            return new TimeSpan((long)Math.Round(a.Ticks / b));
        }
        #endregion
    }
}
