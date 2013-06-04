/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using ux.Utils;
using ux.Waveform;

namespace ux.Component
{
    class Part
    {
        #region Static Private Members
        static private readonly double[] NoteFactor;
        #endregion

        #region Private Fields
        private readonly double SampleDeltaTime;
        private readonly Envelope envelope;
        private IWaveform waveform;

        private float volume;
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

        #region Public Properties
        public float[] Buffer { get { return this.buffer; } }

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

        #region Constructors
        public Part(Master master)
        {
            this.envelope = new Envelope(master.SamplingFreq);
            this.buffer = new float[1024];
            this.Reset();

            this.ExtendBuffers(256);
            this.SampleDeltaTime = 1.0 / master.SamplingFreq;
        }
        #endregion

        #region Public Methods
        public void Generate(int sampleCount)
        {
            if (!this.IsSounding)
                return;

            if (this.smplBuffer.Length < sampleCount)
                this.ExtendBuffers(sampleCount);

            if (this.buffer.Length < sampleCount * 2)
                this.buffer = new float[(int)(sampleCount * 2.5)];
            else
                Array.Clear(this.buffer, 0, this.buffer.Length);

            #region Generate Parameter
            for (int i = 0; i < sampleCount; i++)
            {
                double target_freq = this.noteFreq * this.finetune +
                            ((this.vibrate && this.notePhase > this.vibrateDelay) ?
                            this.vibrateDepth * Math.Sin(2.0 * Math.PI * this.vibrateFreq * this.vibratePhase) : 0.0);

                double freq = (this.portament) ?
                    this.noteFreqOld + (target_freq - this.noteFreqOld) * this.portamentSpeed : target_freq;

                this.notePhase *= (this.noteFreqOld / freq);

                this.freqBuffer[i] = freq;
                this.phasBuffer[i] = this.notePhase;
                this.notePhase += SampleDeltaTime;
                this.vibratePhase += SampleDeltaTime;
                this.noteFreqOld = freq;
            }
            #endregion

            this.envelope.Generate(this.sampleTime, this.envlBuffer, 0, sampleCount);
            this.waveform.GetWaveforms(this.smplBuffer, this.freqBuffer, this.phasBuffer, sampleCount);

            for (int i = 0, j = 0; i < sampleCount; i++)
            {
                float c = this.smplBuffer[i] * this.envlBuffer[i] * this.volume;
                this.buffer[j++] = c * this.panpot.L;
                this.buffer[j++] = c * this.panpot.R;
            }

            this.sampleTime += sampleCount;
        }

        public void Reset()
        {
            this.waveform = new Square();
            this.volume = 0.25f;
            this.panpot.L = 1.0f;
            this.panpot.R = 1.0f;
            this.vibrateDepth = 4.0;
            this.vibrateFreq = 4.0;
            this.vibrateDelay = 0.0;
            this.vibratePhase = 0.0;
            this.portamentSpeed = 1.0 * 0.001;
            this.portament = false;
            this.vibrate = false;

            this.sampleTime = 0;
            this.notePhase = 0.0;
            this.noteFreq = 0.0;
            this.noteFreqOld = 0.0;

            this.finetune = 1.0;
            this.keyShift = 0;

            this.envelope.Reset();
        }

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

        public void Attack(float note)
        {
            this.sampleTime = 0;
            this.notePhase = 0.0;
            this.ZeroGate(note);
            this.envelope.Attack();
        }

        public void Release()
        {
            this.envelope.Release(this.sampleTime);
        }

        public void Silence()
        {
            this.envelope.Silence();
        }

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
                    this.volume = handle.Parameter.Value.Normalize(2.0f, 0.0f);
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
        private void ExtendBuffers(int requireCount)
        {
            this.smplBuffer = new float[requireCount];
            this.envlBuffer = new float[requireCount];
            this.freqBuffer = new double[requireCount];
            this.phasBuffer = new double[requireCount];
        }

        private void ApplyForVibrate(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "on":
                    this.vibrate = true;
                    break;

                case "off":
                    this.vibrate = false;
                    break;

                case "delay":
                    this.vibrateDelay = parameter.Value;
                    break;

                case "depth":
                    this.vibrateDepth = parameter.Value;
                    break;

                case "freq":
                    this.vibrateFreq = parameter.Value;
                    break;

                default:
                    break;
            }
        }

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

        private void ApplyForPortament(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "on":
                    this.portament = true;
                    break;

                case "off":
                    this.portament = false;
                    break;

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
