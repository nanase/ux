/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using ux.Component;

namespace ux.Waveform
{
    /// <summary>
    /// 矩形波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Square : StepWaveform
    {
        #region Constructors
        /// <summary>
        /// 新しい Square クラスのインスタンスを初期化します。
        /// </summary>
        public Square()
            : base()
        {
            this.GenerateStep(0.5f);
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
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
        /// <summary>
        /// デューティ比を指定して新しい矩形波を設定します。
        /// </summary>
        /// <param name="duty">デューティ比。</param>
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

    /// <summary>
    /// 擬似三角波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Triangle : StepWaveform
    {
        #region Constructors
        /// <summary>
        /// 新しい Triangle クラスのインスタンスを初期化します。
        /// </summary>
        public Triangle()
            : base()
        {
            this.GenerateStep(16);
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
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
        /// <summary>
        /// ステップ数を指定して新しい擬似三角波を設定します。
        /// </summary>
        /// <param name="step">ステップ数。</param>
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
