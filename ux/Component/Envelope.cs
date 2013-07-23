/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using ux.Utils;

namespace ux.Component
{
    /// <summary>
    /// 時間によって変化するパラメータを実装するためのエンベロープ (包絡線) クラスです。
    /// </summary>
    class Envelope
    {
        #region Private Members
        private readonly float samplingFreq;
        private int releaseStartTime, t2, t3, t5, attackTime, peakTime, decayTime, releaseTime;
        private float da, dd, dr, sustainLevel;
        private EnvelopeState state;
        #endregion

        #region Public Proparties
        /// <summary>
        /// 現在のエンベロープの状態を表す列挙値を取得します。
        /// </summary>
        public EnvelopeState State { get { return this.state; } }
        #endregion

        #region Constructor
        /// <summary>
        /// サンプリング周波数を指定して新しい Envelope クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
        public Envelope(float samplingFreq)
        {
            this.samplingFreq = samplingFreq;
            this.Reset();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 値の変化しない、常に一定値を出力するエンベロープを作成します。
        /// </summary>
        /// <param name="samplingFreq">サンプリング周波数。</param>
        /// <returns>一定出力値を持つエンベロープ。</returns>
        public static Envelope CreateConstant(float samplingFreq)
        {
            Envelope envelope = new Envelope(samplingFreq);
            envelope.attackTime = 0;
            envelope.peakTime = 0;
            envelope.decayTime = 0;
            envelope.sustainLevel = 1.0f;
            envelope.releaseTime = 0;

            return envelope;
        }

        /// <summary>
        /// このインスタンスにおけるすべてのパラメータを既定値に戻します。
        /// </summary>
        public void Reset()
        {
            this.attackTime = (int)(0.05f * this.samplingFreq);
            this.peakTime = (int)(0.0f * this.samplingFreq);
            this.decayTime = (int)(0.0f * this.samplingFreq);
            this.sustainLevel = 1.0f;
            this.releaseTime = (int)(0.05f * this.samplingFreq);
            this.state = EnvelopeState.Silence;
        }

        /// <summary>
        /// エンベロープの状態をアタック状態に変更します。
        /// </summary>
        public void Attack()
        {
            this.state = EnvelopeState.Attack;

            //precalc
            this.t2 = this.attackTime + this.peakTime;
            this.t3 = t2 + this.decayTime;
            this.da = 1.0f / this.attackTime;
            this.dd = (1.0f - this.sustainLevel) / this.decayTime;
        }

        /// <summary>
        /// エンベロープの状態をリリース状態に変更します。
        /// </summary>
        /// <param name="time">リリースが開始された時間値。</param>
        public void Release(int time)
        {
            if (this.state == EnvelopeState.Attack)
            {
                this.state = EnvelopeState.Release;
                this.releaseStartTime = time;

                //precalc
                this.t5 = time + this.releaseTime;
                this.dr = this.sustainLevel / this.releaseTime;
            }
        }

        /// <summary>
        /// エンベロープの状態をサイレンス状態に変更します。
        /// </summary>
        public void Silence()
        {
            this.state = EnvelopeState.Silence;
        }

        /// <summary>
        /// 現在のエンベロープの状態に基づき、エンベロープ値を出力します。
        /// </summary>
        /// <param name="time">エンベロープの開始時間値。</param>
        /// <param name="envelopes">出力が格納される実数の配列。</param>
        /// <param name="offset">代入が開始される配列のインデックス。</param>
        /// <param name="count">代入される実数値の数。</param>
        public void Generate(int time, float[] envelopes, int count)
        {
            float res;
            for (int i = 0; i < count; i++, time++)
            {
                if (this.State == EnvelopeState.Attack)
                    res = (time < this.attackTime) ? time * this.da :
                      (time < this.t2) ? 1.0f :
                      (time < this.t3) ? 1.0f - (time - this.t2) * this.dd : this.sustainLevel;
                else if (this.State == EnvelopeState.Release)
                    if (time < this.t5)
                        res = this.sustainLevel - (time - this.releaseStartTime) * this.dr;
                    else
                    {
                        res = 0.0f;
                        this.state = EnvelopeState.Silence;
                    }
                else
                    res = 0.0f;

                envelopes[i] = res;
            }
        }

        /// <summary>
        /// パラメータを用いてこのエンベロープの設定値を変更します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        public void SetParameter(int data1, float data2)
        {
            switch ((EnvelopeOperate)data1)
            {
                case EnvelopeOperate.Attack:
                    this.attackTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
                    break;

                case EnvelopeOperate.Peak:
                    this.peakTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
                    break;

                case EnvelopeOperate.Decay:
                    this.decayTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
                    break;

                case EnvelopeOperate.Sustain:
                    this.sustainLevel = data2.Clamp(1.0f, 0.0f);
                    break;

                case EnvelopeOperate.Release:
                    this.releaseTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingFreq);
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
