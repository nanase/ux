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

    /// <summary>
    /// 擬似乱数ジェネレータに作用するオプションを表した列挙体です。
    /// </summary>
    public enum RandomNoiseOperate
    {
        /// <summary>
        ///擬似乱数ジェネレータのシード値。
        /// </summary>
        Seed = 0x0100,
        /// <summary>
        /// 擬似乱数の周期。
        /// </summary>
        Length,
    }

    /// <summary>
    /// 基本波形クラスに作用するオプションを表した列挙体です。
    /// </summary>
    public enum BasicWaveformOperate
    {
        /// <summary>
        /// デューティ比。
        /// </summary>
        Duty = 0x0100,

        /// <summary>
        /// 波形タイプ。
        /// </summary>
        Type,
    }

    /// <summary>
    /// ステップ波形クラスに作用するオプションを表した列挙体です。
    /// </summary>
    public enum StepWaveformOperate
    {
        /// <summary>
        /// 周波数係数。指定された値が出力周波数に乗算されます。
        /// </summary>
        FreqFactor = 0x0000,

        /// <summary>
        /// ユーザ波形の開始。このパラメータの実数値からユーザ波形として登録します。
        /// </summary>
        Begin,

        /// <summary>
        /// ユーザ波形の終了。このパラメータの実数値までユーザ波形として登録します。
        /// </summary>
        End,

        /// <summary>
        /// ユーザ波形のキューイング。
        /// </summary>
        Queue,
    }

    /// <summary>
    /// FM音源クラスに作用するオプションを表した列挙体です。
    /// </summary>
    public enum FMOperate
    {
        /// <summary>
        /// オペレータ 0 に対する変調指数。
        /// </summary>
        Send0 = 0x0000,

        /// <summary>
        /// オペレータ 1 に対する変調指数。
        /// </summary>
        Send1 = 0x0100,

        /// <summary>
        /// オペレータ 2 に対する変調指数。
        /// </summary>
        Send2 = 0x0200,

        /// <summary>
        /// オペレータ 3 に対する変調指数。
        /// </summary>
        Send3 = 0x0300,

        /// <summary>
        /// 出力キャリア振幅。
        /// </summary>
        Output = 0x0400,
        Out = Output,

        /// <summary>
        /// キャリア周波数。
        /// </summary>
        Frequency = 0x0500,
        Freq = Frequency,
        //

        /// <summary>
        /// オペレータ 0。
        /// </summary>
        Operator0 = 0x0000,
        Op0 = Operator0,

        /// <summary>
        /// オペレータ 1。
        /// </summary>
        Operator1 = 0x1000,
        Op1 = Operator1,

        /// <summary>
        /// オペレータ 2。
        /// </summary>
        Operator2 = 0x2000,
        Op2 = Operator2,

        /// <summary>
        /// オペレータ 3。
        /// </summary>
        Operator3 = 0x3000,
        Op3 = Operator3,
    }

    /// <summary>
    /// エンベロープに作用するオプションを表した列挙体です。
    /// </summary>
    public enum EnvelopeOperate
    {
        /// <summary>
        /// オプションなし。これは音源に対するオプションと組み合わせるために用意されています。
        /// </summary>
        None = 0x00,

        /// <summary>
        /// アタック時間。
        /// </summary>
        Attack = 0x01,

        /// <summary>
        /// アタック時間。
        /// </summary>
        A = Attack,

        /// <summary>
        /// ピーク時間。
        /// </summary>
        Peak = 0x02,

        /// <summary>
        /// ピーク時間。
        /// </summary>
        P = Peak,

        /// <summary>
        /// ディケイ時間。
        /// </summary>
        Decay = 0x03,

        /// <summary>
        /// ディケイ時間。
        /// </summary>
        D = Decay,

        /// <summary>
        /// サスティンレベル。
        /// </summary>
        Sustain = 0x04,

        /// <summary>
        /// サスティンレベル。
        /// </summary>
        S = Sustain,

        /// <summary>
        /// リリース時間。
        /// </summary>
        Release = 0x05,

        /// <summary>
        /// リリース時間。
        /// </summary>
        R = Release,
    }

    /// <summary>
    /// ボリュームに作用するオプションを表した列挙体です。
    /// </summary>
    public enum VolumeOperate
    {
        /// <summary>
        /// 変化を伴わない音量。ボリューム。
        /// </summary>
        Volume,

        /// <summary>
        /// 変化を伴う音量。抑揚。
        /// </summary>
        Expression,

        /// <summary>
        /// 発音の強さ。ベロシティ。
        /// </summary>
        Velocity,

        /// <summary>
        /// 発音の増幅度。ゲイン。
        /// </summary>
        Gain,
    }

    /// <summary>
    /// ビブラートに作用するオプションを表した列挙体です。
    /// </summary>
    public enum VibrateOperate
    {
        /// <summary>
        /// ビブラート無効。
        /// </summary>
        Off,

        /// <summary>
        /// ビブラート有効。
        /// </summary>
        On,

        /// <summary>
        /// ビブラートが開始される遅延時間。
        /// </summary>
        Delay,

        /// <summary>
        /// ビブラートの深さ。
        /// </summary>
        Depth,

        /// <summary>
        /// ビブラートの周波数。
        /// </summary>
        Freq,
    }

    /// <summary>
    /// 音源の種類を表した列挙体です。
    /// </summary>
    public enum WaveformType
    {
        /// <summary>
        /// 矩形波。
        /// </summary>
        Square,

        /// <summary>
        /// 三角波。
        /// </summary>
        Triangle,

        /// <summary>
        /// 線形帰還シフトレジスタによる短周期ノイズ。
        /// </summary>
        ShortNoise,

        /// <summary>
        /// 線形帰還シフトレジスタによる長周期ノイズ。
        /// </summary>
        LongNoise,

        /// <summary>
        /// 擬似乱数ジェネレータによるノイズ。
        /// </summary>
        RandomNoise,

        /// <summary>
        /// FM 音源。
        /// </summary>
        FM,
    }

    /// <summary>
    /// ポルタメントに作用するオプションを表した列挙体です。
    /// </summary>
    public enum PortamentOperate
    {
        /// <summary>
        /// ポルタメント無効。
        /// </summary>
        Off,

        /// <summary>
        /// ポルタメント有効。
        /// </summary>
        On,

        /// <summary>
        /// ポルタメントの速さ。
        /// </summary>
        Speed,
    }
}
