/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Runtime.InteropServices;
using ux.Utils.Midi.IO;

/* 参考 : http://stackoverflow.com/questions/1991159/getting-signals-from-a-midi-port-in-c-sharp
 */

namespace ux.Utils.Midi
{
    /// <summary>
    /// MIDI-IN デバイスと ux を接続します。
    /// </summary>
    public class MidiInConnector : MidiConnector
    {
        #region -- Private Fields --
        private NativeMethods.MidiInProc midiInProc;
        private IntPtr handle;
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
                    NativeMethods.midiInGetDevCaps((UIntPtr)i, ref result, (uint)Marshal.SizeOf(typeof(NativeMethods.MIDIINCAPS)));
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
        /// <param name="samplingFreq">サンプリング周波数。</param>
        /// <param name="id">オープンされる MIDI-IN デバイスの ID。</param>
        public MidiInConnector(float samplingFreq, int id)
            : base(samplingFreq)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new PlatformNotSupportedException();

            this.midiInProc = new NativeMethods.MidiInProc(MidiProc);
            this.handle = IntPtr.Zero;

            if (!this.Open(id))
                throw new InvalidOperationException();

            this.Reset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// MIDI-IN からの入力を開始します。
        /// </summary>
        public override void Play()
        {
            NativeMethods.midiInStart(handle);
        }

        /// <summary>
        /// MIDI-IN からの入力を停止します。
        /// </summary>
        public override void Stop()
        {
            NativeMethods.midiInStop(handle);
        }

        /// <summary>
        /// MIDI-IN デバイスとの接続を閉じ、リソースを解放します。
        /// </summary>
        public override void Dispose()
        {
            this.Close();
            this.Stop();
        }
        #endregion

        #region -- Private Methods --
        private bool Close()
        {
            bool result = (NativeMethods.midiInClose(this.handle) == NativeMethods.MMSYSERR_NOERROR);
            this.handle = IntPtr.Zero;
            return result;
        }

        private bool Open(int id)
        {
            return NativeMethods.midiInOpen(out this.handle, id, this.midiInProc, IntPtr.Zero, NativeMethods.CALLBACK_FUNCTION) == NativeMethods.MMSYSERR_NOERROR;
        }

        

        private void MidiProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, int dwParam1, int dwParam2)
        {
            if (wMsg == 0x000003c3)
            {
                this.ProcessMidiEvent(new[] { new MidiEvent((EventType)(dwParam1 & 0xf0), dwParam1 & 0x0f, dwParam1 >> 8 & 0xff, dwParam1 >> 16 & 0xff) });
                this.EventCount++;
            }
        }
        #endregion

        static class NativeMethods
        {
            internal const int MMSYSERR_NOERROR = 0;
            internal const int CALLBACK_FUNCTION = 0x00030000;

            internal delegate void MidiInProc(IntPtr hMidiIn, int wMsg, IntPtr dwInstance, int dwParam1, int dwParam2);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInGetNumDevs();

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInClose(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInOpen(out IntPtr lphMidiIn, int uDeviceID, MidiInProc dwCallback, IntPtr dwCallbackInstance, int dwFlags);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInStart(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern int midiInStop(IntPtr hMidiIn);

            [DllImport("winmm.dll", SetLastError = true)]
            internal static extern MMRESULT midiInGetDevCaps(UIntPtr uDeviceID, ref MIDIINCAPS caps, uint cbMidiInCaps);

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
        }
    }
}
