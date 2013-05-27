/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using ux.Component;

namespace ux.Waveform
{
    class FM : IWaveform
    {
        #region Private Members
        private readonly Operator op0, op1, op2, op3;
        #endregion

        public FM()
        {
            this.op0 = new Operator();
            this.op1 = new Operator();
            this.op2 = new Operator();
            this.op3 = new Operator();

            this.op0.OutAmplifier = 1.0;
            this.op0.Send0 = 0.75;
            this.op1.Send0 = 0.5;
        }

        #region IWaveform implementation
        public void GetWaveforms(float[] data, double[] frequency, double[] phase, int count)
        {
            double omega, tmp0, tmp1, tmp2, tmp3;

            for (int i = 0; i < count; i++)
            {
                omega = 2.0 * Math.PI * phase[i] * frequency[i];
                tmp0 =
                    Math.Sin(omega * this.op0.FreqFactor +
                    this.op0.Send0 * this.op0.Old +
                    this.op1.Send0 * this.op1.Old +
                    this.op2.Send0 * this.op2.Old +
                    this.op3.Send0 * this.op3.Old) * this.op0.Amplifier;

                tmp1 =
                    Math.Sin(omega * this.op1.FreqFactor +
                    this.op0.Send1 * this.op0.Old +
                    this.op1.Send1 * this.op1.Old +
                    this.op2.Send1 * this.op2.Old +
                    this.op3.Send1 * this.op3.Old) * this.op1.Amplifier;

                tmp2 =
                    Math.Sin(omega * this.op2.FreqFactor +
                    this.op0.Send2 * this.op0.Old +
                    this.op1.Send2 * this.op1.Old +
                    this.op2.Send2 * this.op2.Old +
                    this.op3.Send2 * this.op3.Old) * this.op2.Amplifier;

                tmp3 =
                    Math.Sin(omega * this.op3.FreqFactor +
                    this.op0.Send3 * this.op0.Old +
                    this.op1.Send3 * this.op1.Old +
                    this.op2.Send3 * this.op2.Old +
                    this.op3.Send3 * this.op3.Old) * this.op3.Amplifier;

                this.op0.Old = tmp0;
                this.op1.Old = tmp1;
                this.op2.Old = tmp2;
                this.op3.Old = tmp3;

                data[i] = (float)(this.op0.OutAmplifier * tmp0 +
                    this.op1.OutAmplifier * tmp1 +
                    this.op2.OutAmplifier * tmp2 +
                    this.op3.OutAmplifier * tmp3);
            }
        }

        public void SetParameter(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "op0out":
                    this.op0.OutAmplifier = parameter.Value;
                    break;
                case "op0amp":
                    this.op0.Amplifier = parameter.Value;
                    break;
                case "op0freq":
                    this.op0.FreqFactor = parameter.Value;
                    break;
                case "op0send0":
                    this.op0.Send0 = parameter.Value;
                    break;
                case "op0send1":
                    this.op0.Send1 = parameter.Value;
                    break;
                case "op0send2":
                    this.op0.Send2 = parameter.Value;
                    break;
                case "op0send3":
                    this.op0.Send3 = parameter.Value;
                    break;

                case "op1out":
                    this.op1.OutAmplifier = parameter.Value;
                    break;
                case "op1amp":
                    this.op1.Amplifier = parameter.Value;
                    break;
                case "op1freq":
                    this.op1.FreqFactor = parameter.Value;
                    break;
                case "op1send0":
                    this.op1.Send0 = parameter.Value;
                    break;
                case "op1send1":
                    this.op1.Send1 = parameter.Value;
                    break;
                case "op1send2":
                    this.op1.Send2 = parameter.Value;
                    break;
                case "op1send3":
                    this.op1.Send3 = parameter.Value;
                    break;

                case "op2out":
                    this.op2.OutAmplifier = parameter.Value;
                    break;
                case "op2amp":
                    this.op2.Amplifier = parameter.Value;
                    break;
                case "op2freq":
                    this.op2.FreqFactor = parameter.Value;
                    break;
                case "op2send0":
                    this.op2.Send0 = parameter.Value;
                    break;
                case "op2send1":
                    this.op2.Send1 = parameter.Value;
                    break;
                case "op2send2":
                    this.op2.Send2 = parameter.Value;
                    break;
                case "op2send3":
                    this.op2.Send3 = parameter.Value;
                    break;

                case "op3out":
                    this.op3.OutAmplifier = parameter.Value;
                    break;
                case "op3amp":
                    this.op3.Amplifier = parameter.Value;
                    break;
                case "op3freq":
                    this.op3.FreqFactor = parameter.Value;
                    break;
                case "op3send0":
                    this.op3.Send0 = parameter.Value;
                    break;
                case "op3send1":
                    this.op3.Send1 = parameter.Value;
                    break;
                case "op3send2":
                    this.op3.Send2 = parameter.Value;
                    break;
                case "op3send3":
                    this.op3.Send3 = parameter.Value;
                    break;

                default:
                    break;
            }

            return;
        }
        #endregion

        class Operator
        {
            public double OutAmplifier = 0.0f;
            public double Amplifier = 1.0f;
            public double FreqFactor = 1.0f;
            public double Send0 = 0.0f;
            public double Send1 = 0.0f;
            public double Send2 = 0.0f;
            public double Send3 = 0.0f;
            public double Old = 0.0f;
        }
    }
}

