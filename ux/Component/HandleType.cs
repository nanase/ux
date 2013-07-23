/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Component
{
	/// <summary>
	/// 内部で扱われるハンドルのタイプを表す列挙体です。
	/// </summary>
	public enum HandleType
	{
        /// <summary>
        /// ゼロのゲートを持ち、発音されないノートを表します。
        /// </summary>
        ZeroGate,

        /// <summary>
        /// パートまたはマスターの各パラメータをリセットします。
        /// </summary>
		Reset,

        /// <summary>
        /// ノートのエンベロープをサイレンス状態に移行させ、無音状態にします。
        /// </summary>
		Silence,

        /// <summary>
        /// ノートのエンベロープをリリース状態に移行させます。
        /// </summary>
		NoteOff,

        /// <summary>
        /// ボリューム (音量) を変更します。
        /// </summary>
		Volume,

        /// <summary>
        /// パンポット (定位) を変更します。
        /// </summary>
		Panpot,

        /// <summary>
        /// ビブラートに関するパラメータを設定します。
        /// </summary>
		Vibrate,

        /// <summary>
        /// パートに波形を割り当てます。
        /// </summary>
		Waveform,

        /// <summary>
        /// 波形のパラメータを編集します。
        /// </summary>
		EditWaveform,

        /// <summary>
        /// パートの音量エンベロープを変更します。
        /// </summary>
		Envelope,

        /// <summary>
        /// パートのファインチューン値を変更します。
        /// </summary>
		FineTune,

        /// <summary>
        /// パートの発音ノートキーをシフトします。
        /// </summary>
		KeyShift,

        /// <summary>
        /// ポルタメント効果に関するパラメータを設定します。
        /// </summary>
		Portament,

        /// <summary>
        /// パートを指定されたノートまたは周波数でアタック状態にします。
        /// </summary>
		NoteOn
	}
}
