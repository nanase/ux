/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using ux.Utils;
using ux.Waveform;

namespace ux.Component
{
    /// <summary>
    /// 波形生成の単位となるパートクラスです。
    /// </summary>
    class Part
    {
        #region -- Private Fields --
        private static readonly double[] NoteFactor;
        private const float Amplifier = 0.5f;
        private const double A = 2.0;

        private readonly double SampleDeltaTime;
        private readonly Envelope envelope;
        private readonly float samplingRate;
        private IWaveform waveform;

        private float volume, expression, velocity, gain;
        private double finetune, noteFreq, notePhase, noteFreqOld;
        private double vibrateDepth, vibrateFreq, vibrateDelay, vibratePhase;
        private double portamentSpeed;
        private bool portament, vibrate;
        private float[] smplBuffer, envlBuffer;
        private double[] phasBuffer, freqBuffer;
        private int sampleTime;
        private Panpot panpot;
        private int keyShift;
        private float[] buffer;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 生成された波形のバッファ配列を取得します。
        /// </summary>
        public float[] Buffer { get { return this.buffer; } }

        /// <summary>
        /// このパートが発音状態にあるかを表す真偽値を取得します。
        /// </summary>
        public bool IsSounding { get { return this.envelope.State != EnvelopeState.Silence; } }
        #endregion

        #region -- Constructors --
        static Part()
        {
            Part.NoteFactor = new double[128];
            for (int i = 0; i < 128; i++)
                Part.NoteFactor[i] = (Math.Pow(2.0, (i - 69) / 12.0)) * 440.0;
        }

        /// <summary>
        /// 新しい Part クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">マスタークラスでのサンプリング周波数。</param>
        public Part(float samplingRate)
        {
            this.envelope = new Envelope(samplingRate);
            this.buffer = new float[0];
            this.Reset();

            this.ExtendBuffers(0);
            this.SampleDeltaTime = 1.0 / samplingRate;
            this.samplingRate = samplingRate;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 波形を生成します。
        /// </summary>
        /// <param name="sampleCount">生成される波形のサンプル数。</param>
        public void Generate(int sampleCount)
        {
            // 未発音は除外
            if (!this.IsSounding)
            {
                Array.Clear(this.buffer, 0, this.buffer.Length);
                return;
            }

            // サンプルバッファ更新
            if (this.smplBuffer.Length < sampleCount)
                this.ExtendBuffers(sampleCount);

            // 出力バッファ更新
            if (this.buffer.Length < sampleCount * 2)
                this.buffer = new float[(int)(sampleCount * 2.5)];
            else
                Array.Clear(this.buffer, 0, this.buffer.Length);

            #region Generate Parameter
            for (int i = 0; i < sampleCount; i++)
            {
                // 目標周波数 - ビブラートを考慮
                double target_freq = this.noteFreq * this.finetune +
                            ((this.vibrate && this.notePhase > this.vibrateDelay) ?
                            this.vibrateDepth * Math.Sin(2.0 * Math.PI * this.vibrateFreq * this.vibratePhase) : 0.0);

                // 発音周波数 - ポルタメントを考慮
                double freq = (this.portament) ?
                    this.noteFreqOld + (target_freq - this.noteFreqOld) * this.portamentSpeed : target_freq;

                // 位相修正
                this.notePhase *= (this.noteFreqOld / freq);

                // サンプルバッファへの代入
                this.freqBuffer[i] = freq;
                this.phasBuffer[i] = this.notePhase;

                // 時間と位相の加算
                this.notePhase += SampleDeltaTime;
                this.vibratePhase += SampleDeltaTime;
                this.noteFreqOld = freq;
            }
            #endregion

            // 波形生成
            this.envelope.Generate(this.sampleTime, this.envlBuffer, sampleCount);
            this.waveform.GetWaveforms(this.smplBuffer, this.freqBuffer, this.phasBuffer, this.sampleTime, sampleCount);

            // ログスケール計算。分母は各パラメータの標準値
            // (削除厳禁。最後の1.0はエンベロープ用)
            float vtmp = (float)((this.volume * this.expression * Part.Amplifier * this.velocity * this.gain) /
                                 (        1.0 *             1.0 *            1.0 *           1.0 *       1.0 * 1.0));

            // 波形出力
            for (int i = 0, j = 0; i < sampleCount; i++)
            {
                float c = this.smplBuffer[i] * (float)Math.Pow(this.envlBuffer[i] * vtmp, Part.A);
                this.buffer[j++] = c * this.panpot.L;
                this.buffer[j++] = c * this.panpot.R;
            }

            this.sampleTime += sampleCount;
        }

        /// <summary>
        /// このパートに割当てられている設定値をリセットします。
        /// </summary>
        public void Reset()
        {
            this.waveform = new Square();
            this.volume = (float)(1.0 / 1.27);
            this.expression = 1.0f;
            this.gain = 1.0f;
            this.panpot = new Panpot(1.0f, 1.0f);
            this.vibrateDepth = 4.0;
            this.vibrateFreq = 4.0;
            this.vibrateDelay = 0.0;
            this.vibratePhase = 0.0;
            this.portamentSpeed = 1.0 * 0.001;
            this.portament = false;
            this.vibrate = false;
            this.velocity = 1.0f;

            this.sampleTime = 0;
            this.notePhase = 0.0;
            this.noteFreq = 0.0;
            this.noteFreqOld = 0.0;

            this.finetune = 1.0;
            this.keyShift = 0;

            this.envelope.Reset();
        }

        /// <summary>
        /// 長さ 0 で指定されたノートで内部状態を変更します。エンベロープはアタック状態に遷移せず、発音されません。
        /// </summary>
        /// <param name="note">ノート値。</param>
        public void ZeroGate(int note)
        {
            this.vibratePhase = 0.0;

            int key = note + this.keyShift;

            if (this.portament)
                this.noteFreqOld = (key < 128 && key >= 0) ? Part.NoteFactor[key] : 0.0;
            else
                this.noteFreq = (key < 128 && key >= 0) ? Part.NoteFactor[key] : 0.0;
        }

        /// <summary>
        /// 指定されたノートでエンベロープをアタック状態に遷移させます。
        /// </summary>
        /// <param name="note">ノート値。</param>
        public void Attack(int note)
        {
            this.sampleTime = 0;
            this.notePhase = 0.0;

            this.vibratePhase = 0.0;

            int key = note + this.keyShift;
            this.noteFreq = (key < 128 && key >= 0) ? Part.NoteFactor[key] : 0.0;

            this.envelope.Attack();
            this.waveform.Attack();
        }

        /// <summary>
        /// エンベロープをリリース状態に遷移させます。
        /// </summary>
        public void Release()
        {
            this.envelope.Release(this.sampleTime);
            this.waveform.Release(this.sampleTime);
        }

        /// <summary>
        /// エンベロープをサイレンス状態に遷移させます。
        /// </summary>
        public void Silence()
        {
            this.envelope.Silence();
        }

        /// <summary>
        /// このパートにハンドルを適用します。
        /// </summary>
        /// <param name="handle">適用されるハンドル。</param>
        public void ApplyHandle(Handle handle)
        {
            switch (handle.Type)
            {
                //パラメータリセット
                case HandleType.Reset:
                    this.Reset();
                    break;

                //サイレンス
                case HandleType.Silence:
                    this.Silence();
                    break;

                //ボリューム設定
                case HandleType.Volume:
                    this.ApplyForVolume(handle.Data1, handle.Data2);
                    break;

                //パンポット
                case HandleType.Panpot:
                    this.panpot = new Panpot(handle.Data2);
                    break;

                //ビブラート
                case HandleType.Vibrate:
                    this.ApplyForVibrate(handle.Data1, handle.Data2);
                    break;

                //波形追加
                case HandleType.Waveform:
                    this.ApplyForWaveform(handle.Data1, handle.Data2);
                    break;

                //波形編集
                case HandleType.EditWaveform:
                    this.waveform.SetParameter(handle.Data1, handle.Data2);
                    break;

                //エンベロープ
                case HandleType.Envelope:
                    this.envelope.SetParameter(handle.Data1, handle.Data2);
                    break;

                //ファインチューン
                case HandleType.FineTune:
                    this.finetune = handle.Data2.Clamp(float.MaxValue, 0.0f);
                    break;

                //キーシフト
                case HandleType.KeyShift:
                    this.keyShift = handle.Data1.Clamp(128, -128);
                    break;

                //ポルタメント
                case HandleType.Portament:
                    this.ApplyForPortament(handle.Data1, handle.Data2);
                    break;

                //ゼロゲート
                case HandleType.ZeroGate:
                    this.ZeroGate(handle.Data1);
                    break;

                //ノートオフ
                case HandleType.NoteOff:
                    this.Release();
                    break;

                //ノートオン
                case HandleType.NoteOn:
                    this.Attack(handle.Data1);
                    this.velocity = handle.Data2.Clamp(1.0f, 0.0f);
                    break;

                default:
                    break;
            }
        }
        #endregion
        
        #region -- Private Methods --
        /// <summary>
        /// バッファを指定されたカウント数で再確保します。
        /// </summary>
        /// <param name="requireCount">配列のサイズ。</param>
        private void ExtendBuffers(int requireCount)
        {
            this.smplBuffer = new float[requireCount];
            this.envlBuffer = new float[requireCount];
            this.freqBuffer = new double[requireCount];
            this.phasBuffer = new double[requireCount];
        }

        /// <summary>
        /// ヴォリュームに対する設定を適用します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        private void ApplyForVolume(int data1, float data2)
        {
            switch ((VolumeOperate)data1)
            {
                case VolumeOperate.Volume:
                    this.volume = data2.Clamp(1.0f, 0.0f);
                    break;

                case VolumeOperate.Expression:
                    this.expression = data2.Clamp(1.0f, 0.0f);
                    break;

                case VolumeOperate.Velocity:
                    this.velocity = data2.Clamp(1.0f, 0.0f);
                    break;

                case VolumeOperate.Gain:
                    this.gain = data2.Clamp(2.0f, 0.0f);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ビブラートに対する設定を適用します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        private void ApplyForVibrate(int data1, float data2)
        {
            switch ((VibrateOperate)data1)
            {
                // ビブラート無効化
                case VibrateOperate.Off:
                    this.vibrate = false;
                    break;

                // ビブラート有効化
                case VibrateOperate.On:
                    this.vibrate = true;
                    break;

                // ビブラートディレイ
                case VibrateOperate.Delay:
                    this.vibrateDelay = data2.Clamp(float.MaxValue, 0.0f);
                    break;

                // ビブラート深度
                case VibrateOperate.Depth:
                    this.vibrateDepth = data2.Clamp(float.MaxValue, 0.0f);
                    break;

                // ビブラート周波数
                case VibrateOperate.Freq:
                    this.vibrateFreq = data2.Clamp(float.MaxValue, 0.0f);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 波形に対する設定を適用します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        private void ApplyForWaveform(int data1, float data2)
        {
            switch ((WaveformType)data1)
            {
                case WaveformType.Square:
                    if (this.waveform is Square)
                        this.waveform.Reset();
                    else
                        this.waveform = new Square();
                    break;

                case WaveformType.Triangle:
                    if (this.waveform is Triangle)
                        this.waveform.Reset();
                    else
                        this.waveform = new Triangle();
                    break;

                case WaveformType.ShortNoise:
                    if (this.waveform is ShortNoise)
                        this.waveform.Reset();
                    else
                        this.waveform = new ShortNoise();
                    break;

                case WaveformType.LongNoise:
                    if (this.waveform is LongNoise)
                        this.waveform.Reset();
                    else
                        this.waveform = new LongNoise();
                    break;

                case WaveformType.RandomNoise:
                    if (this.waveform is RandomNoise)
                        this.waveform.Reset();
                    else
                        this.waveform = new RandomNoise();
                    break;

                case WaveformType.FM:
                    if (this.waveform is FM)
                        this.waveform.Reset();
                    else
                        this.waveform = new FM(this.samplingRate);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ポルタメントに対する設定を適用します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        private void ApplyForPortament(int data1, float data2)
        {
            switch ((PortamentOperate)data1)
            {
                // ポルタメント無効化
                case PortamentOperate.Off:
                    this.portament = false;
                    break;

                // ポルタメント有効化
                case PortamentOperate.On:
                    this.portament = true;
                    break;

                // ポルタメントスピード
                case PortamentOperate.Speed:
                    this.portamentSpeed = data2.Clamp(1000.0f, float.Epsilon * 1000.0f) * (0.001 * 44100.0) * this.SampleDeltaTime;
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
