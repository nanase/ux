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

#define OPERATOR_STRUCT

using System;
using ux.Component;
using ux.Utils;

namespace ux.Waveform
{
    /// <summary>
    /// FM (周波数変調) を用いた波形ジェネレータクラスです。
    /// </summary>
    class FM : IWaveform
    {
        #region -- Private Fields --
        private readonly float samplingRate;
        private Operator op0, op1, op2, op3;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 新しい FM クラスのインスタンスを初期化します。
        /// </summary>
        public FM(float samplingRate)
        {
            this.Reset();
            this.samplingRate = samplingRate;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 与えられた周波数と位相から波形を生成します。
        /// </summary>
        /// <param name="data">生成された波形データが代入される配列。</param>
        /// <param name="frequency">生成に使用される周波数の配列。</param>
        /// <param name="phase">生成に使用される位相の配列。</param>
        /// <param name="sampleTime">波形が開始されるサンプル時間。</param>
        /// <param name="count">配列に代入されるデータの数。</param>
        public void GetWaveforms(float[] data, double[] frequency, double[] phase, int sampleTime, int count)
        {
            double omega, old0, old1, old2, old3, tmp;
#if OPERATOR_STRUCT
            Operator op0, op1, op2, op3;
#endif

            this.op0.GenerateEnvelope(sampleTime, count);
            this.op1.GenerateEnvelope(sampleTime, count);
            this.op2.GenerateEnvelope(sampleTime, count);
            this.op3.GenerateEnvelope(sampleTime, count);

#if OPERATOR_STRUCT
            op0 = this.op0;
            op1 = this.op1;
            op2 = this.op2;
            op3 = this.op3;
#endif

            old0 = op0.Old;
            old1 = op1.Old;
            old2 = op2.Old;
            old3 = op3.Old;

            for (int i = 0; i < count; i++)
            {
                omega = 2.0 * Math.PI * phase[i] * frequency[i];
                tmp = 0.0;

                if (op0.IsSelected)
                {
                    old0 =
                        Math.Sin(omega * op0.FreqFactor +
                        op0.Send0 * old0 * op0.Send0EnvelopeBuffer[i] +
                        op1.Send0 * old1 * op1.Send0EnvelopeBuffer[i] +
                        op2.Send0 * old2 * op2.Send0EnvelopeBuffer[i] +
                        op3.Send0 * old3 * op3.Send0EnvelopeBuffer[i]);
                    tmp += op0.OutAmplifier * old0 * op0.OutAmplifierEnvelopeBuffer[i];
                }

                if (op1.IsSelected)
                {
                    old1 =
                        Math.Sin(omega * op1.FreqFactor +
                        op0.Send1 * old0 * op0.Send1EnvelopeBuffer[i] +
                        op1.Send1 * old1 * op1.Send1EnvelopeBuffer[i] +
                        op2.Send1 * old2 * op2.Send1EnvelopeBuffer[i] +
                        op3.Send1 * old3 * op3.Send1EnvelopeBuffer[i]);
                    tmp += op1.OutAmplifier * old1 * op1.OutAmplifierEnvelopeBuffer[i];
                }

                if (op2.IsSelected)
                {
                    old2 =
                       Math.Sin(omega * op2.FreqFactor +
                       op0.Send2 * old0 * op0.Send2EnvelopeBuffer[i] +
                       op1.Send2 * old1 * op1.Send2EnvelopeBuffer[i] +
                       op2.Send2 * old2 * op2.Send2EnvelopeBuffer[i] +
                       op3.Send2 * old3 * op3.Send2EnvelopeBuffer[i]);
                    tmp += op2.OutAmplifier * old2 * op2.OutAmplifierEnvelopeBuffer[i];
                }

                if (op3.IsSelected)
                {
                    old3 =
                        Math.Sin(omega * op3.FreqFactor +
                        op0.Send3 * old0 * op0.Send3EnvelopeBuffer[i] +
                        op1.Send3 * old1 * op1.Send3EnvelopeBuffer[i] +
                        op2.Send3 * old2 * op2.Send3EnvelopeBuffer[i] +
                        op3.Send3 * old3 * op3.Send3EnvelopeBuffer[i]);
                    tmp += op3.OutAmplifier * old3 * op3.OutAmplifierEnvelopeBuffer[i];
                }

                data[i] = (float)tmp;
            }

            this.op0.Old = old0;
            this.op1.Old = old1;
            this.op2.Old = old2;
            this.op3.Old = old3;
        }

        /// <summary>
        /// パラメータを指定して波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
        public void SetParameter(int data1, float data2)
        {
            if ((FMOperate)(data1 & 0x0f00) == FMOperate.Algorithm)
            {
                this.SetAlgorithm(data1, data2);
                return;
            }

            Operator op;
            switch ((FMOperate)(data1 & 0xf000))
            {
                case FMOperate.Operator0: op = this.op0; break;
                case FMOperate.Operator1: op = this.op1; break;
                case FMOperate.Operator2: op = this.op2; break;
                case FMOperate.Operator3: op = this.op3; break;
                default:
                    return;
            }

            if ((data1 & 0x00ff) == 0)
            {
                switch ((FMOperate)(data1 & 0x0f00))
                {
                    case FMOperate.Output:
                        op.OutAmplifier = data2.Clamp(2.0f, 0.0f);
                        break;

                    case FMOperate.Frequency:
                        op.FreqFactor = data2;
                        break;

                    case FMOperate.Send0:
                        op.Send0 = data2;
                        break;

                    case FMOperate.Send1:
                        op.Send1 = data2;
                        break;

                    case FMOperate.Send2:
                        op.Send2 = data2;
                        break;

                    case FMOperate.Send3:
                        op.Send3 = data2;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch ((FMOperate)(data1 & 0x0f00))
                {
                    case FMOperate.Output:
                        if (op.OutAmplifierEnvelope == null)
                            op.OutAmplifierEnvelope = Envelope.CreateConstant(this.samplingRate);

                        op.OutAmplifierEnvelope.SetParameter(data1 & 0x00ff, data2);
                        break;

                    case FMOperate.Frequency:
                        // Frequency に対するエンベロープは実装していない
                        break;

                    case FMOperate.Send0:
                        if (op.Send0Envelope == null)
                            op.Send0Envelope = Envelope.CreateConstant(this.samplingRate);

                        op.Send0Envelope.SetParameter(data1 & 0x00ff, data2);
                        break;

                    case FMOperate.Send1:
                        if (op.Send1Envelope == null)
                            op.Send1Envelope = Envelope.CreateConstant(this.samplingRate);

                        op.Send1Envelope.SetParameter(data1 & 0x00ff, data2);
                        break;

                    case FMOperate.Send2:
                        if (op.Send2Envelope == null)
                            op.Send2Envelope = Envelope.CreateConstant(this.samplingRate);

                        op.Send2Envelope.SetParameter(data1 & 0x00ff, data2);
                        break;

                    case FMOperate.Send3:
                        if (op.Send3Envelope == null)
                            op.Send3Envelope = Envelope.CreateConstant(this.samplingRate);

                        op.Send3Envelope.SetParameter(data1 & 0x00ff, data2);
                        break;

                    default:
                        break;
                }
            }

#if OPERATOR_STRUCT
            switch ((FMOperate)(data1 & 0xf000))
            {
                case FMOperate.Operator0: this.op0 = op; break;
                case FMOperate.Operator1: this.op1 = op; break;
                case FMOperate.Operator2: this.op2 = op; break;
                case FMOperate.Operator3: this.op3 = op; break;
                default:
                    return;
            }
#endif

            this.SelectProcessingOperator();
        }

        /// <summary>
        /// エンベロープをアタック状態に遷移させます。
        /// </summary>
        public void Attack()
        {
            this.op0.Attack();
            this.op1.Attack();
            this.op2.Attack();
            this.op3.Attack();
        }

        /// <summary>
        /// エンベロープをリリース状態に遷移させます。
        /// </summary>
        /// <param name="time">リリースされたサンプル時間。</param>
        public void Release(int time)
        {
            this.op0.Release(time);
            this.op1.Release(time);
            this.op2.Release(time);
            this.op3.Release(time);
        }

        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public void Reset()
        {
            this.op0 = new Operator(samplingRate);
            this.op1 = new Operator(samplingRate);
            this.op2 = new Operator(samplingRate);
            this.op3 = new Operator(samplingRate);

            this.op0.OutAmplifier = 1.0;
            this.op0.Send0 = 0.75;
            this.op1.Send0 = 0.5;

            this.SelectProcessingOperator();
        }
        #endregion

        #region -- Private Methods --
        /// <summary>
        /// 計算不要なオペレータを検出し、選択します。
        /// </summary>
        private void SelectProcessingOperator()
        {
            this.op0.IsSelected = (this.op0.OutAmplifier != 0.0);
            this.op1.IsSelected = (this.op1.OutAmplifier != 0.0);
            this.op2.IsSelected = (this.op2.OutAmplifier != 0.0);
            this.op3.IsSelected = (this.op3.OutAmplifier != 0.0);

            if (this.op0.IsSelected)
            {
                if (!this.op1.IsSelected && this.op1.Send0 != 0.0)
                    this.op1.IsSelected = true;
                if (!this.op2.IsSelected && this.op2.Send0 != 0.0)
                    this.op2.IsSelected = true;
                if (!this.op3.IsSelected && this.op3.Send0 != 0.0)
                    this.op3.IsSelected = true;
            }

            if (this.op1.IsSelected)
            {
                if (!this.op0.IsSelected && this.op0.Send1 != 0.0)
                    this.op0.IsSelected = true;
                if (!this.op2.IsSelected && this.op2.Send1 != 0.0)
                    this.op2.IsSelected = true;
                if (!this.op3.IsSelected && this.op3.Send1 != 0.0)
                    this.op3.IsSelected = true;
            }

            if (this.op2.IsSelected)
            {
                if (!this.op0.IsSelected && this.op0.Send2 != 0.0)
                    this.op0.IsSelected = true;
                if (!this.op1.IsSelected && this.op1.Send2 != 0.0)
                    this.op1.IsSelected = true;
                if (!this.op3.IsSelected && this.op3.Send2 != 0.0)
                    this.op3.IsSelected = true;
            }

            if (this.op3.IsSelected)
            {
                if (!this.op0.IsSelected && this.op0.Send3 != 0.0)
                    this.op0.IsSelected = true;
                if (!this.op1.IsSelected && this.op1.Send3 != 0.0)
                    this.op1.IsSelected = true;
                if (!this.op2.IsSelected && this.op2.Send3 != 0.0)
                    this.op2.IsSelected = true;
            }
        }

        private void SetAlgorithm(int data1, float data2)
        {
            this.op0.Reset();
            this.op1.Reset();
            this.op2.Reset();
            this.op3.Reset();

            switch ((int)data2.Clamp(int.MaxValue, 0))
            {
                case 0:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send1 = 1.0f;
                    this.op1.Send2 = 1.0f;
                    this.op2.Send3 = 1.0f;
                    this.op3.OutAmplifier = 1.0f;
                    break;

                case 1:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send2 = 1.0f;
                    this.op1.Send2 = 1.0f;
                    this.op2.Send3 = 1.0f;
                    this.op3.OutAmplifier = 1.0f;
                    break;

                case 2:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send3 = 1.0f;
                    this.op1.Send2 = 1.0f;
                    this.op2.Send3 = 1.0f;
                    this.op3.OutAmplifier = 1.0f;
                    break;

                case 3:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send1 = 1.0f;
                    this.op1.Send3 = 1.0f;
                    this.op2.Send3 = 1.0f;
                    this.op3.OutAmplifier = 1.0f;
                    break;

                case 4:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send1 = 1.0f;
                    this.op1.OutAmplifier = 0.5f;
                    this.op2.Send3 = 1.0f;
                    this.op3.OutAmplifier = 0.5f;
                    break;

                case 5:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send1 = 1.0f;
                    this.op0.Send2 = 1.0f;
                    this.op0.Send3 = 1.0f;
                    this.op1.OutAmplifier = 1.0f / 3.0f;
                    this.op2.OutAmplifier = 1.0f / 3.0f;
                    this.op3.OutAmplifier = 1.0f / 3.0f;
                    break;

                case 6:
                    this.op0.Send0 = 1.0f;
                    this.op0.Send1 = 1.0f;
                    this.op1.OutAmplifier = 1.0f / 3.0f;
                    this.op2.OutAmplifier = 1.0f / 3.0f;
                    this.op3.OutAmplifier = 1.0f / 3.0f;
                    break;

                case 7:
                    this.op0.Send0 = 1.0f;
                    this.op0.OutAmplifier = 0.25f;
                    this.op1.OutAmplifier = 0.25f;
                    this.op2.OutAmplifier = 0.25f;
                    this.op3.OutAmplifier = 0.25f;
                    break;

                default:
                    break;
            }

            this.SelectProcessingOperator();
        }
        #endregion

        /// <summary>
        /// FM 音源の 1 モジュールとなるオペレータクラスです。
        /// </summary>
#if OPERATOR_STRUCT
        internal struct Operator
#else
        internal class Operator
#endif
        {
            #region -- Public Fields --
            /// <summary>
            /// 出力に接続される増幅度。
            /// </summary>
            public double OutAmplifier;

            /// <summary>
            /// このオペレータが発振する周波数の補正係数。
            /// </summary>
            public double FreqFactor;

            /// <summary>
            /// オペレータ 0 に送信される波形のレベル。
            /// </summary>
            public double Send0;

            /// <summary>
            /// オペレータ 1 に送信される波形のレベル。
            /// </summary>
            public double Send1;

            /// <summary>
            /// オペレータ 2 に送信される波形のレベル。
            /// </summary>
            public double Send2;

            /// <summary>
            /// オペレータ 3 に送信される波形のレベル。
            /// </summary>
            public double Send3;

            /// <summary>
            /// オペレータが生成した古い値。
            /// </summary>
            public double Old;

            /// <summary>
            /// このオペレータが処理されるかのチェックフラグ。
            /// </summary>
            public bool IsSelected;

            public Envelope OutAmplifierEnvelope;
            public Envelope Send0Envelope;
            public Envelope Send1Envelope;
            public Envelope Send2Envelope;
            public Envelope Send3Envelope;

            public float[] OutAmplifierEnvelopeBuffer;
            public float[] Send0EnvelopeBuffer;
            public float[] Send1EnvelopeBuffer;
            public float[] Send2EnvelopeBuffer;
            public float[] Send3EnvelopeBuffer;

            public float[] ConstantValues;
            #endregion

            #region -- Private Fields --
            private float samplingRate;
            #endregion

            #region -- Constructors --
            public Operator(float samplingRate)
            {
                this.samplingRate = samplingRate;

                OutAmplifier = 0.0f;
                FreqFactor = 1.0f;
                Send0 = 0.0f;
                Send1 = 0.0f;
                Send2 = 0.0f;
                Send3 = 0.0f;
                Old = 0.0f;
                IsSelected = false;

                OutAmplifierEnvelope = null;
                Send0Envelope = null;
                Send1Envelope = null;
                Send2Envelope = null;
                Send3Envelope = null;

                OutAmplifierEnvelopeBuffer = new float[0];
                Send0EnvelopeBuffer = new float[0];
                Send1EnvelopeBuffer = new float[0];
                Send2EnvelopeBuffer = new float[0];
                Send3EnvelopeBuffer = new float[0];

                ConstantValues = new float[0];
            }
            #endregion

            #region -- Public Methods --
            /// <summary>
            /// エンベロープをアタック状態に遷移させます。
            /// </summary>
            public void Attack()
            {
                if (this.OutAmplifierEnvelope != null)
                    this.OutAmplifierEnvelope.Attack();

                if (this.Send0Envelope != null)
                    this.Send0Envelope.Attack();

                if (this.Send1Envelope != null)
                    this.Send1Envelope.Attack();

                if (this.Send2Envelope != null)
                    this.Send2Envelope.Attack();

                if (this.Send3Envelope != null)
                    this.Send3Envelope.Attack();
            }

            /// <summary>
            /// エンベロープをリリース状態に遷移させます。
            /// </summary>
            /// <param name="time">リリースされたサンプル時間。</param>
            public void Release(int time)
            {
                if (this.OutAmplifierEnvelope != null)
                    this.OutAmplifierEnvelope.Release(time);

                if (this.Send0Envelope != null)
                    this.Send0Envelope.Release(time);

                if (this.Send1Envelope != null)
                    this.Send1Envelope.Release(time);

                if (this.Send2Envelope != null)
                    this.Send2Envelope.Release(time);

                if (this.Send3Envelope != null)
                    this.Send3Envelope.Release(time);
            }

            public void GenerateEnvelope(int sampleTime, int sampleCount)
            {
                if (this.OutAmplifierEnvelopeBuffer.Length < sampleCount)
                    this.ExtendBuffer(sampleCount);

                if (this.OutAmplifierEnvelope != null)
                    this.OutAmplifierEnvelope.Generate(sampleTime, this.OutAmplifierEnvelopeBuffer, sampleCount);
                else
                    this.ConstantValues.CopyTo(this.OutAmplifierEnvelopeBuffer, 0);

                if (this.Send0Envelope != null)
                    this.Send0Envelope.Generate(sampleTime, this.Send0EnvelopeBuffer, sampleCount);
                else
                    this.ConstantValues.CopyTo(this.Send0EnvelopeBuffer, 0);

                if (this.Send1Envelope != null)
                    this.Send1Envelope.Generate(sampleTime, this.Send1EnvelopeBuffer, sampleCount);
                else
                    this.ConstantValues.CopyTo(this.Send1EnvelopeBuffer, 0);

                if (this.Send2Envelope != null)
                    this.Send2Envelope.Generate(sampleTime, this.Send2EnvelopeBuffer, sampleCount);
                else
                    this.ConstantValues.CopyTo(this.Send2EnvelopeBuffer, 0);

                if (this.Send3Envelope != null)
                    this.Send3Envelope.Generate(sampleTime, this.Send3EnvelopeBuffer, sampleCount);
                else
                    this.ConstantValues.CopyTo(this.Send3EnvelopeBuffer, 0);
            }

            public void Reset()
            {
                OutAmplifier = 0.0f;
                FreqFactor = 1.0f;
                Send0 = 0.0f;
                Send1 = 0.0f;
                Send2 = 0.0f;
                Send3 = 0.0f;
                Old = 0.0f;
                IsSelected = false;

                if (OutAmplifierEnvelope != null)
                    OutAmplifierEnvelope.Reset();

                if (Send0Envelope != null)
                    Send0Envelope.Reset();

                if (Send1Envelope != null)
                    Send1Envelope.Reset();

                if (Send2Envelope != null)
                    Send2Envelope.Reset();

                if (Send3Envelope != null)
                    Send3Envelope.Reset();

                Array.Clear(OutAmplifierEnvelopeBuffer, 0, OutAmplifierEnvelopeBuffer.Length);
                Array.Clear(Send0EnvelopeBuffer, 0, Send0EnvelopeBuffer.Length);
                Array.Clear(Send1EnvelopeBuffer, 0, Send1EnvelopeBuffer.Length);
                Array.Clear(Send2EnvelopeBuffer, 0, Send2EnvelopeBuffer.Length);
                Array.Clear(Send3EnvelopeBuffer, 0, Send3EnvelopeBuffer.Length);
                //Array.Clear(ConstantValues, 0, ConstantValues.Length);
            }
            #endregion

            #region -- Private Methods --
            private void ExtendBuffer(int length)
            {
                this.OutAmplifierEnvelopeBuffer = new float[length];
                this.Send0EnvelopeBuffer = new float[length];
                this.Send1EnvelopeBuffer = new float[length];
                this.Send2EnvelopeBuffer = new float[length];
                this.Send3EnvelopeBuffer = new float[length];

                this.ConstantValues = new float[length];

                for (int i = 0; i < length; i++)
                    this.ConstantValues[i] = 1f;
            }
            #endregion
        }
    }
}

