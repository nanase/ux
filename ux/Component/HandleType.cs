/* ux - Micro Xylph / Software Synthesizer Core Library

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
        Edit = EditWaveform,

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
