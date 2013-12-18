/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Component
{
	/// <summary>
	/// エンベロープの状態を表す列挙体です。
	/// </summary>
	public enum EnvelopeState
	{
		/// <summary>
		/// 無音状態。
		/// </summary>
		Silence,
		/// <summary>
		/// アタック(立ち上がり)状態。
		/// </summary>
		Attack,
		/// <summary>
		/// リリース(余韻)状態。
		/// </summary>
		Release,
	}
}

