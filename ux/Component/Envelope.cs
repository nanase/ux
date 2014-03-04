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

using ux.Utils;

namespace ux.Component
{
    /// <summary>
    /// 時間によって変化するパラメータを実装するためのエンベロープ (包絡線) クラスです。
    /// </summary>
    public class Envelope
    {
        #region -- Private Members --
        private readonly float samplingRate;
        private int releaseStartTime, t2, t3, t5, attackTime, peakTime, decayTime, releaseTime;
        private float da, dd, dr, sustainLevel, releaseStartLevel;
        private EnvelopeState state;
        #endregion

        #region -- Public Proparties --
        /// <summary>
        /// 現在のエンベロープの状態を表す列挙値を取得します。
        /// </summary>
        public EnvelopeState State { get { return this.state; } }

        /// <summary>
        /// ノートが開始されてピークに達するまでの遷移時間を取得または設定します。
        /// </summary>
        public float AttackTime
        {
            get { return this.attackTime / this.samplingRate; }
            set { this.attackTime = (int)(value.Clamp(float.MaxValue, 0.0f) * this.samplingRate); }
        }

        /// <summary>
        /// ピークを維持する時間を取得または設定します。
        /// </summary>
        public float PeakTime
        {
            get { return this.peakTime / this.samplingRate; }
            set { this.peakTime = (int)(value.Clamp(float.MaxValue, 0.0f) * this.samplingRate); }
        }

        /// <summary>
        /// ピークからサスティンレベルに達するまでの遷移時間を取得または設定します。
        /// </summary>
        public float DecayTime
        {
            get { return this.decayTime / this.samplingRate; }
            set { this.decayTime = (int)(value.Clamp(float.MaxValue, 0.0f) * this.samplingRate); }
        }

        /// <summary>
        /// エンベロープがリリースされるまで持続するサスティンレベルを取得または設定します。
        /// </summary>
        public float SustainLevel
        {
            get { return this.sustainLevel; }
            set { this.sustainLevel = value.Clamp(1.0f, 0.0f); }
        }

        /// <summary>
        /// リリースされてからエンベロープが消滅するまでの時間を取得または設定します。
        /// </summary>
        public float ReleaseTime
        {
            get { return this.releaseTime / this.samplingRate; }
            set { this.releaseTime = (int)(value.Clamp(float.MaxValue, 0.0f) * this.samplingRate); }
        }

        /// <summary>
        /// このエンベロープで使われるサンプリング周波数を取得または設定します。
        /// </summary>
        public float SamplingRate { get { return this.samplingRate; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// サンプリング周波数を指定して新しい Envelope クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        public Envelope(float samplingRate)
        {
            this.samplingRate = samplingRate;
            this.Reset();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// このインスタンスにおけるすべてのパラメータを既定値に戻します。
        /// </summary>
        public void Reset()
        {
            this.attackTime = (int)(0.05f * this.samplingRate);
            this.peakTime = (int)(0.0f * this.samplingRate);
            this.decayTime = (int)(0.0f * this.samplingRate);
            this.sustainLevel = 1.0f;
            this.releaseTime = (int)(0.2f * this.samplingRate);
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
                this.releaseStartLevel = (time < this.attackTime) ? time * this.da :
                      (time < this.t2) ? 1.0f :
                      (time < this.t3) ? 1.0f - (time - this.t2) * this.dd : this.sustainLevel;
                this.t5 = time + this.releaseTime;
                this.dr = this.releaseStartLevel / this.releaseTime;
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
                        res = this.releaseStartLevel - (time - this.releaseStartTime) * this.dr;
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
                    this.attackTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingRate);
                    break;

                case EnvelopeOperate.Peak:
                    this.peakTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingRate);
                    break;

                case EnvelopeOperate.Decay:
                    this.decayTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingRate);
                    break;

                case EnvelopeOperate.Sustain:
                    this.sustainLevel = data2.Clamp(1.0f, 0.0f);
                    break;

                case EnvelopeOperate.Release:
                    this.releaseTime = (int)(data2.Clamp(float.MaxValue, 0.0f) * this.samplingRate);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region -- Public Static Methods --
        /// <summary>
        /// 値の変化しない、常に一定値を出力するエンベロープを作成します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <returns>一定出力値を持つエンベロープ。</returns>
        public static Envelope CreateConstant(float samplingRate)
        {
            Envelope envelope = new Envelope(samplingRate);
            envelope.attackTime = 0;
            envelope.peakTime = 0;
            envelope.decayTime = 0;
            envelope.sustainLevel = 1.0f;
            envelope.releaseTime = 0;

            return envelope;
        }
        #endregion
    }
}
