/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using ux.Component;
using ux.Utils;

namespace ux.Waveform
{
    /// <summary>
    /// 矩形波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Square : StepWaveform
    {
        #region -- Private Fields --
        private static readonly LinkedList<Tuple<int, float[]>> cache = new LinkedList<Tuple<int, float[]>>();
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 新しい Square クラスのインスタンスを初期化します。
        /// </summary>
        public Square()
            : base()
        {
        }
        #endregion

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

            if (onTime > StepWaveform.MaxDataSize)
                return;

            Tuple<int, float[]> nc = null;

            for (var now = cache.First; now != null; now = now.Next)
            {
                if (now.Value.Item1 == onTime)
                {
                    nc = now.Value;
                    break;
                }
            }

            if (nc == null)
            {
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

                cache.AddFirst(new Tuple<int, float[]>(onTime, this.value));

                if (cache.Count > 32)
                    cache.RemoveLast();
            }
            else
            {
                this.value = nc.Item2;
                this.length = nc.Item2.Length;
            }
        }
        #endregion
    }

    /// <summary>
    /// 擬似三角波を生成する波形ジェネレータクラスです。
    /// </summary>
    class Triangle : StepWaveform
    {
        #region -- Private Fields --
        private static readonly LinkedList<Tuple<int, float[]>> cache = new LinkedList<Tuple<int, float[]>>();
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 新しい Triangle クラスのインスタンスを初期化します。
        /// </summary>
        public Triangle()
            : base()
        {
        }
        #endregion

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

        #region -- Private Methods --
        /// <summary>
        /// ステップ数を指定して新しい擬似三角波を設定します。
        /// </summary>
        /// <param name="step">ステップ数。</param>
        private void GenerateStep(int step)
        {
            if (step <= 0 || step > 256)
                return;

            Tuple<int, float[]> nc = null;

            for (var now = cache.First; now != null; now = now.Next)
            {
                if (now.Value.Item1 == step)
                {
                    nc = now.Value;
                    break;
                }
            }

            if (nc == null)
            {
                byte[] l = new byte[step * 2];


                for (int i = 0; i < step; i++)
                    l[i] = (byte)i;

                for (int i = step; i < step * 2; i++)
                    l[i] = (byte)(step * 2 - i - 1);

                this.SetStep(l);

                cache.AddFirst(new Tuple<int, float[]>(step, this.value));

                if (cache.Count > 32)
                    cache.RemoveLast();
            }
            else
            {
                this.value = nc.Item2;
                this.length = nc.Item2.Length;
            }
        }
        #endregion
    }
}
