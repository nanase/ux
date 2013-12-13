using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ux;
using ux.Component;

namespace uxBenchmark
{
    public class Program
    {
        const int samplingRate = 44100;
        const int bufferSize = 1024;
        const int iterate = 3000;

        static void Main(string[] args)
        {
            Console.WriteLine("ux Benchmark");
            Console.WriteLine("Copyright (c) 2013 Tomona Nanase");

            Console.WriteLine();
            Console.Write("Press any key to start benchmark...");
            Console.ReadLine();

            var sw = Stopwatch.StartNew();
            MuteTest();
            DefaultFMTest();
            DefaultSquareTest();

            sw.Stop();

            Console.WriteLine();
            Console.WriteLine("Complete in {0:mm\\:ss}", sw.Elapsed);
            Console.ReadLine();
        }

        static void MuteTest()
        {
            TimeSpan result;

            Console.WriteLine();
            Console.WriteLine("[MuteTest] for {0}", iterate);

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

            for (int i = 0; i < iterate; i++)
            {
                master.Read(buffer, 0, bufferSize);
            }

            sw.Stop();

            return sw.Elapsed;
        }

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

    }

    public static class Extension
    {
        public static TimeSpan Divide(this TimeSpan a, double b)
        {
            return new TimeSpan((long)Math.Round(a.Ticks / b));
        }
    }
}
