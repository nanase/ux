/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using ux.Component;

namespace ux.Waveform
{
    class Square : StepWaveform
    {
        #region Constructors
        public Square()
            : base()
        {
            this.GenerateStep(0.5f);
        }
        #endregion

        #region Public Override Methods
        public override void SetParameter(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "duty":
                    this.GenerateStep(parameter.Value);
                    break;
                default:
                    base.SetParameter(parameter);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void GenerateStep(float duty)
        {
            if (duty <= 0.0f || duty >= 1.0f)
                return;

            int onTime = (int)(1.0f / (duty <= 0.5f ? duty : (1.0f - duty))) - 1;

            if (onTime > 32767)
                return;

            byte[] l = new byte[onTime + 1];

            if (duty <= 0.5f)
            {
                // 10, 100, 1000, ...
                l[0] = (byte)1;
            }
            else
            {
                // 10, 110, 1110, ...
                for (int i = 0; i < onTime; i++)
                    l[i] = (byte)1;
            }

            this.SetStep(l);
        }
        #endregion
    }

    class Triangle : StepWaveform
    {
        #region Constructors
        public Triangle()
            : base()
        {
            this.GenerateStep(16);
        }
        #endregion

        #region Public Override Methods
        public override void SetParameter(PValue parameter)
        {
            switch (parameter.Name)
            {
                case "type":
                    this.GenerateStep((int)parameter.Value);
                    break;

                default:
                    base.SetParameter(parameter);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void GenerateStep(int step)
        {
            if (step <= 0 || step > 256)
                return;

            byte[] l = new byte[step * 2];


            for (int i = 0; i < step; i++)
                l[i] = (byte)i;

            for (int i = step; i < step * 2; i++)
                l[i] = (byte)(step * 2 - i - 1);

            this.SetStep(l);
        }
        #endregion
    }
}
