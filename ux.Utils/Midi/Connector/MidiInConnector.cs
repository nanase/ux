/* ux.Utils / Software Synthesizer Library

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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ux.Utils.Midi.IO;

/* 参考:
 * http://stackoverflow.com/questions/1991159/getting-signals-from-a-midi-port-in-c-sharp
 */

namespace ux.Utils.Midi
{
    /// <summary>
    /// MIDI-IN デバイスと ux を接続します。
    /// </summary>
    public class MidiInConnector : MidiConnector
    {
        #region -- Private Fields --
        private static readonly byte[] GmReset = { 0xf0, 0x7e, 0x7f, 0x09, 0x01, 0xf7 };

        private const int BufferSize = 256;

        private NativeMethods.MidiInProc midiInProc;
        private NativeMethods.MIDIHDR midiHeader;
        private IntPtr ptrHeader, handle;
        private byte[] buffer = new byte[BufferSize];
        private readonly uint headerSize;
        private bool playing, closing;
        private Queue<byte> exclusiveQueue;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// MIDI-IN デバイスの数を取得します。
        /// </summary>
        public static int InputCount
        {
            get { return NativeMethods.midiInGetNumDevs(); }
        }

        /// <summary>
        /// MIDI-IN デバイス名を格納した配列を取得します。
        /// </summary>
        public static string[] InputDeviceNames
        {
            get
            {
                int count = MidiInConnector.InputCount;
                NativeMethods.MIDIINCAPS result = new NativeMethods.MIDIINCAPS();
                string[] names = new string[count];

                for (int i = 0; i < count; i++)
                {
                    NativeMethods.midiInGetDevCaps((UIntPtr)i,
                                                   ref result,
                                                   (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIINCAPS)));
                    names[i] = result.szPname;
                }

                return names;
            }
        }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数とデバイス ID を指定して新しい MidiInConnector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="id">オープンされる MIDI-IN デバイスの ID。</param>
        public MidiInConnector(float samplingRate, int id)
            : base(samplingRate)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new PlatformNotSupportedException();

            this.midiInProc = new NativeMethods.MidiInProc(MidiProc);
            this.handle = IntPtr.Zero;
            this.exclusiveQueue = new Queue<byte>();

            if (!this.Open(id))
                throw new InvalidOperationException();

            this.headerSize = (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIHDR));
            this.midiHeader = new NativeMethods.MIDIHDR();
            this.midiHeader.data = Marshal.AllocHGlobal(BufferSize);
            this.midiHeader.bufferLength = (uint)BufferSize;
            Marshal.Copy(this.buffer, 0, this.midiHeader.data, BufferSize);

            this.ptrHeader = Marshal.AllocHGlobal((int)this.headerSize);
            Marshal.StructureToPtr(this.midiHeader, this.ptrHeader, true);

            NativeMethods.midiInPrepareHeader(this.handle, this.ptrHeader, this.headerSize);
            NativeMethods.midiInAddBuffer(this.handle, this.ptrHeader, this.headerSize);

            this.Reset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// MIDI-IN からの入力を開始します。
        /// </summary>
        public override void Play()
        {
            if (!this.playing)
            {
                NativeMethods.midiInStart(handle);
                this.playing = true;
            }
        }

        /// <summary>
        /// MIDI-IN からの入力を停止します。
        /// </summary>
        public override void Stop()
        {
            if (this.playing)
            {
                NativeMethods.midiInStop(handle);
                this.playing = false;
            }
        }

        /// <summary>
        /// MIDI-IN デバイスとの接続を閉じ、リソースを解放します。
        /// </summary>
        public override void Dispose()
        {
            this.Stop();
            this.Close();
        }
        #endregion

        #region -- Private Methods --
        private bool Close()
        {
            this.closing = true;
            NativeMethods.midiInReset(this.handle);
            NativeMethods.midiInUnprepareHeader(this.handle, this.ptrHeader, this.headerSize);
            bool result = (NativeMethods.midiInClose(this.handle) == NativeMethods.MMSYSERR_NOERROR);

            Marshal.FreeHGlobal(this.midiHeader.data);
            this.midiHeader.data = IntPtr.Zero;

            Marshal.DestroyStructure(this.ptrHeader, typeof(NativeMethods.MIDIHDR));
            Marshal.FreeHGlobal(this.ptrHeader);
            this.ptrHeader = IntPtr.Zero;

            this.handle = IntPtr.Zero;
            return result;
        }

        private bool Open(int id)
        {
            return NativeMethods.midiInOpen(out this.handle,
                                            id,
                                            this.midiInProc,
                                            IntPtr.Zero,
                                            NativeMethods.CALLBACK_FUNCTION) == NativeMethods.MMSYSERR_NOERROR;
        }

        private void MidiProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, int dwParam1, int dwParam2)
        {
            switch (wMsg)
            {
                case NativeMethods.MIM_DATA:
                    this.ProcessMidiEvent(new[] { new MidiEvent((EventType)(dwParam1 & 0xf0),
                                                                dwParam1 & 0x0f,
                                                                dwParam1 >> 8 & 0xff,
                                                                dwParam1 >> 16 & 0xff) });
                    this.EventCount++;
                    break;

                case NativeMethods.MIM_LONGDATA:
                    this.midiHeader = (NativeMethods.MIDIHDR)Marshal.PtrToStructure(this.ptrHeader,
                                                                                    typeof(NativeMethods.MIDIHDR));
                    Marshal.Copy(this.midiHeader.data, this.buffer, 0, BufferSize);

                    for (int i = 0; i < BufferSize; i++)
                    {
                        byte value = this.buffer[i];
                        this.exclusiveQueue.Enqueue(this.buffer[i]);

                        if (value == 0xf7)
                        {
                            this.ProcessExclusiveMessage();
                            break;
                        }
                    }

                    if (!this.closing)
                        NativeMethods.midiInAddBuffer(this.handle, this.ptrHeader, this.headerSize);
                    break;

                default:
                    break;
            }
        }

        private void ProcessExclusiveMessage()
        {
            byte[] message = this.exclusiveQueue.ToArray();
            this.exclusiveQueue.Clear();

            if (GmReset.SequenceEqual(message))
            {
                this.ProcessMidiEvent(Enumerable.Range(0, 16).Select(i => new MidiEvent(EventType.ControlChange,
                                                                                        i,
                                                                                        121,
                                                                                        0)));
            }
        }
        #endregion

        static class NativeMethods
        {
            internal const int MMSYSERR_NOERROR = 0;
            internal const int CALLBACK_FUNCTION = 0x00030000;

            internal const int MIM_OPEN = 961;
            internal const int MIM_CLOSE = 962;
            internal const int MIM_DATA = 963;
            internal const int MIM_LONGDATA = 964;

            internal delegate void MidiInProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, int dwParam1, int dwParam2);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInGetNumDevs();

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInClose(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInOpen(out IntPtr lphMidiIn,
                                                  int uDeviceID,
                                                  MidiInProc dwCallback,
                                                  IntPtr dwCallbackInstance,
                                                  int dwFlags);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInStart(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInStop(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS caps, uint cbMidiInCaps);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInPrepareHeader(IntPtr hMidiIn, IntPtr lpMidiInHdr, uint cbMidiInHdr);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInAddBuffer(IntPtr hMidiIn, IntPtr lpMidiInHdr, uint cbMidiInHdr);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInUnprepareHeader(IntPtr hMidiIn, IntPtr lpMidiInHdr, uint cbMidiInHdr);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInReset(IntPtr hMidiIn);

            [StructLayout(LayoutKind.Sequential)]
            public struct MIDIINCAPS
            {
                public ushort wMid;
                public ushort wPid;
                public uint vDriverVersion;     // MMVERSION
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public string szPname;
                public uint dwSupport;
            }

            internal enum MMRESULT : uint
            {
                MMSYSERR_NOERROR = 0,
                MMSYSERR_ERROR = 1,
                MMSYSERR_BADDEVICEID = 2,
                MMSYSERR_NOTENABLED = 3,
                MMSYSERR_ALLOCATED = 4,
                MMSYSERR_INVALHANDLE = 5,
                MMSYSERR_NODRIVER = 6,
                MMSYSERR_NOMEM = 7,
                MMSYSERR_NOTSUPPORTED = 8,
                MMSYSERR_BADERRNUM = 9,
                MMSYSERR_INVALFLAG = 10,
                MMSYSERR_INVALPARAM = 11,
                MMSYSERR_HANDLEBUSY = 12,
                MMSYSERR_INVALIDALIAS = 13,
                MMSYSERR_BADDB = 14,
                MMSYSERR_KEYNOTFOUND = 15,
                MMSYSERR_READERROR = 16,
                MMSYSERR_WRITEERROR = 17,
                MMSYSERR_DELETEERROR = 18,
                MMSYSERR_VALNOTFOUND = 19,
                MMSYSERR_NODRIVERCB = 20,
                WAVERR_BADFORMAT = 32,
                WAVERR_STILLPLAYING = 33,
                WAVERR_UNPREPARED = 34
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIDIHDR
            {
                public IntPtr data;
                public uint bufferLength;
                public uint bytesRecorded;
                public IntPtr user;
                public uint flags;
                public IntPtr next;
                public IntPtr reserved;
                public uint offset;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public IntPtr[] reservedArray;
            }
        }
    }
}
