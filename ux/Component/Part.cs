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
        #region Static Private Field
        private static readonly double[] NoteFactor;
        private const float Amplifier = 0.5f;
        private const double A = 2.0;
        #endregion

        #region Private Field
        private readonly double SampleDeltaTime;
        private readonly Envelope envelope;
        private readonly Master master;
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

        #region Public Property
        /// <summary>
        /// 生成された波形のバッファ配列を取得します。
        /// </summary>
        public float[] Buffer { get { return this.buffer; } }

        /// <summary>
        /// このパートが発音状態にあるかを表す真偽値を取得します。
        /// </summary>
        public bool IsSounding { get { return this.envelope.State != EnvelopeState.Silence; } }
        #endregion

        #region Static Constructor
        static Part()
        {
            Part.NoteFactor = new double[128];
            for (int i = 0; i < 128; i++)
                Part.NoteFactor[i] = (Math.Pow(2.0, (i - 69) / 12.0)) * 440.0;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// パートの属するマスタークラスを指定して新しい Part クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="master">このパートが属するマスタークラス。</param>
        public Part(Master master)
        {
            this.envelope = new Envelope(master.SamplingFreq);
            this.buffer = new float[1024];
            this.Reset();

            this.ExtendBuffers(256);
            this.SampleDeltaTime = 1.0 / master.SamplingFreq;
            this.master = master;
        }
        #endregion

        #region Public Methods
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
            this.envelope.Generate(this.sampleTime, this.envlBuffer, 0, sampleCount);
            this.waveform.GetWaveforms(this.smplBuffer, this.freqBuffer, this.phasBuffer, sampleCount);

            float vtmp = (float)((this.volume * this.expression * Part.Amplifier * this.velocity * this.gain) / (1.0 * 1.0 * 1.0 * 1.0 * 1.0 * 1.0));

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
        public void ZeroGate(float note)
        {
            this.vibratePhase = 0.0;

            if (float.IsInfinity(note))
                this.noteFreq = 0.0;
            else if (note < 0.0f)
                this.noteFreq = -note;
            else
            {
                int k = (int)note + this.keyShift;
                this.noteFreq = (k < 128 && k >= 0) ? Part.NoteFactor[k] : 0.0;
            }
        }

        /// <summary>
        /// 指定されたノートでエンベロープをアタック状態に遷移させます。
        /// </summary>
        /// <param name="note">ノート値。</param>
        public void Attack(float note)
        {
            this.sampleTime = 0;
            this.notePhase = 0.0;
            this.ZeroGate(note);
            this.envelope.Attack();
        }

        /// <summary>
        /// エンベロープをリリーズ状態に遷移させます。
        /// </summary>
        public void Release()
        {
            this.envelope.Release(this.sampleTime);
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
                    this.ApplyForVolume(handle.Parameter);
                    break;

                //パンポッド
                case HandleType.Panpot:
                    this.panpot = new Panpot(handle.Parameter.Value);
                    break;

                //ビブラート
                case HandleType.Vibrate:
                    this.ApplyForVibrate(handle.Parameter);
                    break;

                //波形追加
                case HandleType.Waveform:
                    this.ApplyForWaveform(handle.Parameter);
                    break;

                //波形編集
                case HandleType.EditWaveform:
                    this.waveform.SetParameter(handle.Parameter);
                    break;

                //エンベロープ（uxではボリュームエンベロープのみ存在）
                case HandleType.Envelope:
                    this.envelope.SetParameter(handle.Parameter);
                    break;

                //ファインチューン
                case HandleType.FineTune:
                    this.finetune = handle.Parameter.Value;
                    break;

                //キーシフト
                case HandleType.KeyShift:
                    this.keyShift = (short)handle.Parameter.Value;
                    break;

                //ポルタメント
                case HandleType.Portament:
                    this.ApplyForPortament(handle.Parameter);
                    break;

                //ゼロゲート
                case HandleType.ZeroGate:
                    this.ZeroGate(handle.Parameter.Value);
                    break;

                //ノートオフ
                case HandleType.NoteOff:
                    this.Release();
                    break;

                //ノートオン
                case HandleType.NoteOn:
                    this.Attack(handle.Parameter.Value);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Private Methods
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
        /// <param name="parameter">パラメータ値。</param>
        private void ApplyForVolume(PValue parameter)
        {
            switch (parameter.Name)
            {
                case null:
                case "":
                case "volume":
                    this.volume = parameter.Value.Clamp(1.0f, 0.0f);
                    break;

                case "expression":
                    this.expression = parameter.Value.Clamp(1.0f, 0.0f);
                    break;

                case "velocity":
                    this.velocity = parameter.Value.Clamp(1.0f, 0.0f);
                    break;

                case "gain":
                    this.gain = parameter.Value.Clamp(1.0f, 0.0f);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ビブラートに対する設定を適用します。
        /// </summary>
        /// <param name="parameter">パラメータ値。</param>
        private void ApplyForVibrate(PValue parameter)
        {
            switch (parameter.Name)
            {
                // ビブラート有効化
                case "on":
                    this.vibrate = true;
                    break;

                // ビブラート無効化
                case "off":
                    this.vibrate = false;
                    break;

                // ビブラートディレイ
                case "delay":
                    this.vibrateDelay = parameter.Value;
                    break;

                // ビブラート深度
                case "depth":
                    this.vibrateDepth = parameter.Value;
                    break;

                // ビブラート周波数
                case "freq":
                    this.vibrateFreq = parameter.Value;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 波形に対する設定を適用します。
        /// </summary>
        /// <param name="parameter">パラメータ値。</param>
        private void ApplyForWaveform(PValue parameter)
        {
            switch ((int)parameter.Value)
            {
                case 0:
                    this.waveform = new Square();
                    break;

                case 1:
                    this.waveform = new Triangle();
                    break;

                case 2:
                    this.waveform = new ShortNoise();
                    break;

                case 3:
                    this.waveform = new LongNoise();
                    break;

                case 4:
                    this.waveform = new FM();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ポルタメントに対する設定を適用します。
        /// </summary>
        /// <param name="parameter">パラメータ値。</param>
        private void ApplyForPortament(PValue parameter)
        {
            switch (parameter.Name)
            {
                // ポルタメント有効化
                case "on":
                    this.portament = true;
                    break;

                // ポルタメント無効化
                case "off":
                    this.portament = false;
                    break;

                // ポルタメントスピード
                case "speed":
                    this.portamentSpeed = parameter.Value * 0.001 * (44100.0 * this.SampleDeltaTime);
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
