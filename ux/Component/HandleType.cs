/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;

namespace ux.Component
{
	/// <summary>
	/// 内部で扱われるハンドルのタイプを表す列挙体です。
	/// </summary>
	public enum HandleType
	{
        ZeroGate = 0,
		Reset = 1,
		Release = 2,
		Silence = 3,
		NoteOff = 4,
		Volume = 5,	
		Panpot = 6,
		Vibrate = 7,
		Waveform = 8,
		EditWaveform = 9,
		Envelope = 10,
		FineTune = 11,
		KeyShift = 12,
		Portament = 13,
		NoteOn = 14
	}
}
