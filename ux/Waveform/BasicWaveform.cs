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

using System;
using System.Collections.Generic;
using ux.Component;
using ux.Utils;

namespace ux.Waveform
{
    struct BaseWaveformCache : CacheObject<BaseWaveformCache>
    {
        public float[] DataValue { get; set; }

        public int Step { get; private set; }

        public int Length { get { return this.DataValue.Length; } }

        public BaseWaveformCache(int step)
            : this()
        {
            this.Step = step;
        }

        public bool Equals(BaseWaveformCache other)
        {
            return this.Step == other.Step;
        }

        public bool CanResize(BaseWaveformCache other)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// 矩形波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Square : CachedWaveform<BaseWaveformCache>
    {
        #region -- Public Methods --
        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="data1">整数パラメータ。</param>
        /// <param name="data2">実数パラメータ。</param>
        public override void SetParameter(int data1, float data2)
        {
            switch ((BasicWaveformOperate)data1)
            {
                case BasicWaveformOperate.Duty:
                    this.GenerateStep(data2.Clamp(1.0f, 0.0f));
                    break;

                default:
                    base.SetParameter(data1, data2);
                    break;
            }
        }

        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            this.GenerateStep(0.5f);
        }
        #endregion

        #region -- Protected Methods --
        protected override byte[] Generate(BaseWaveformCache parameter)
        {
            bool reverse = parameter.Step < 0;
            int onTime = (reverse) ? -parameter.Step : parameter.Step;

            byte[] l = new byte[onTime + 1];

            if (reverse)
            {
                // 10, 110, 1110, ...
                for (int i = 0; i < onTime; i++)
                    l[i] = (byte)1;
            }
            else
                // 10, 100, 1000, ...
                l[0] = (byte)1;

            return l;
        }
        #endregion

        #region -- Private Methods --
        /// <summary>
        /// デューティ比を指定して新しい矩形波を設定します。
        /// </summary>
        /// <param name="duty">デューティ比。</param>
        private void GenerateStep(float duty)
        {
            if (duty <= 0.0f || duty >= 1.0f)
                return;

            int onTime = (int)(1.0f / (duty <= 0.5f ? duty : (1.0f - duty))) - 1;

            if (onTime < 0 || onTime > StepWaveform.MaxDataSize)
                return;

            if (duty > 0.5f)
                onTime = -onTime;

            this.Cache(new BaseWaveformCache(onTime));
        }
        #endregion
    }

    /// <summary>
    /// 擬似三角波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Triangle : CachedWaveform<BaseWaveformCache>
    {
        #region -- Public Methods --
        /// <summary>
        /// パラメータを指定してこの波形の設定値を変更します。
        /// </summary>
        /// <param name="parameter">パラメータオブジェクトとなる PValue 値。</param>
        public override void SetParameter(int data1, float data2)
        {
            switch ((BasicWaveformOperate)data1)
            {
                case BasicWaveformOperate.Type:
                    this.GenerateStep((int)data2.Clamp(int.MaxValue, 1f));
                    break;

                default:
                    base.SetParameter(data1, data2);
                    break;
            }
        }

        /// <summary>
        /// 波形のパラメータをリセットします。
        /// </summary>
        public override void Reset()
        {
            this.GenerateStep(16);
        }
        #endregion

        #region -- Protected Methods --
        protected override byte[] Generate(BaseWaveformCache parameter)
        {
            byte[] l = new byte[parameter.Step * 2];

            for (int i = 0; i < parameter.Step; i++)
                l[i] = (byte)i;

            for (int i = parameter.Step; i < parameter.Step * 2; i++)
                l[i] = (byte)(parameter.Step * 2 - i - 1);

            return l;
        }
        #endregion

        #region -- Private Methods --
        /// <summary>
        /// ステップ数を指定して新しい擬似三角波を設定します。
        /// </summary>
        /// <param name="step">ステップ数。</param>
        private void GenerateStep(int step)
        {
            if (step <= 0 || step > 256)
                return;

            this.Cache(new BaseWaveformCache(step));
        }
        #endregion
    }
}
