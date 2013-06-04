/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ux.Component;
using ux.Utils;

namespace ux
{
    public class Master
    {
        #region Private Constant Field
        private const float DefaultSamplingFreq = 44100.0f;
        private const int PartCount = 8;
        #endregion

        #region Private Field
        private float masterVolume = 0.8f;
        private float compressorThreshold = 0.5f;
        private float compressorRatio = 1.0f / 10.0f;
        private readonly float samplingFreq;
        private readonly Part[] parts = new Part[PartCount];
        private readonly Queue<Handle> handleQueue;
        #endregion

        #region Public Property
        /// <summary>
        /// 再生に用いられるサンプリング周波数を取得します。
        /// </summary>
        /// <value>
        /// サンプリング周波数。単位はHz。
        /// </value>
        public float SamplingFreq { get { return this.samplingFreq; } }

        public bool Playing { get; private set; }

        public float Threshold
        {
            get { return this.compressorThreshold; }
            set
            {
                if (float.IsInfinity(value) || float.IsNaN(value))
                    throw new ArgumentException();
                if (value < 0.0f)
                    throw new ArgumentException();

                this.compressorThreshold = value;
            }
        }

        public float Ratio
        {
            get { return this.compressorRatio; }
            set
            {
                if (float.IsInfinity(value) || float.IsNaN(value))
                    throw new ArgumentException();
                if (value < 0.0f)
                    throw new ArgumentException();

                this.compressorRatio = value;
            }
        }
        #endregion

        #region Constructor
        public Master()
            : this(Master.DefaultSamplingFreq)
        {
        }

        public Master(float samplingFreq)
        {
            if (samplingFreq > 0.0f && samplingFreq <= float.MaxValue)
                this.samplingFreq = samplingFreq;
            else
                throw new ArgumentOutOfRangeException("samplingFreq", String.Format("指定されたサンプリング周波数は無効です: {0:f0} Hz", samplingFreq));

            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i] = new Part(this);

            this.handleQueue = new Queue<Handle>();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 再生を開始します。
        /// </summary>
        public void Play()
        {
            this.Playing = true;
        }

        /// <summary>
        /// 再生を停止します。
        /// </summary>
        public void Stop()
        {
            if (!this.Playing)
                return;

            this.Release();
            this.Playing = false;
        }

        public void PushHandle(Handle handle)
        {
            lock (((ICollection)this.handleQueue).SyncRoot)
                this.handleQueue.Enqueue(handle);
        }

        public void PushHandle(IEnumerable<Handle> handles)
        {
            lock (((ICollection)this.handleQueue).SyncRoot)
                foreach (var handle in handles)
                    this.handleQueue.Enqueue(handle);
        }

        /// <summary>
        /// 全てのパートにリリースを送信します。
        /// </summary>
        public void Release()
        {
            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i].Release();
        }

        /// <summary>
        /// 全てのパートにサイレンスを送信します。
        /// </summary>
        public void Silence()
        {
            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i].Silence();
        }

        /// <summary>
        /// 生成された PCM データを読み込みます。
        /// </summary>
        /// <param name="buffer">格納先のバッファ。</param>
        /// <param name="offset">バッファに対するオフセット。</param>
        /// <param name="count">読み込まれる要素数。</param>
        /// <returns>読み込みに成功した要素数。</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            float gain = 1.0f / (this.compressorThreshold + (1.0f - this.compressorThreshold) * this.compressorRatio);

            this.ApplyHandle();

            // countは バイト数
            // Part.Generate にはサンプル数を与える
            for (int i = 0; i < Master.PartCount; i++)
                if (this.parts[i].IsSounding)
                    this.parts[i].Generate(count / 2);
            
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                float input, output;

                output = input = this.parts.Where(p => p.IsSounding).Sum(p => p.Buffer[j]) * this.masterVolume;

                if (output > this.compressorThreshold)
                    output = this.compressorThreshold + (output - this.compressorThreshold) * this.compressorRatio;
                else if (input < -this.compressorThreshold)
                    output = -this.compressorThreshold + (input + this.compressorThreshold) * this.compressorRatio;

                output *= gain;

                if (output > 1.0f)
                    output = 1.0f;
                else if (output < -1.0f)
                    output = -1.0f;

                buffer[i] = output;
            }

            return count;
        }

        /// <summary>
        /// マスターとパートのパラメータを初期化します。
        /// </summary>
        public void Reset()
        {
            this.masterVolume = 0.8f;

            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i].Reset();
        }
        #endregion

        #region Private Method
        private void ApplyHandle()
        {
            var list = new List<Handle>();

            lock (((ICollection)this.handleQueue).SyncRoot)
            {
                list.AddRange(this.handleQueue);
                this.handleQueue.Clear();
            }

            foreach (var handle in list)
            {
                if (handle.TargetPart == 0)
                    switch (handle.Type)
                    {
                        case HandleType.Volume:
                            this.masterVolume = handle.Parameter.Value.Normalize(2.0f, 0.0f);
                            break;

                        default:
                            break;
                    }
                else if (handle.TargetPart >= 1 && handle.TargetPart <= Master.PartCount)
                    this.parts[handle.TargetPart - 1].ApplyHandle(handle);
            }
        }
        #endregion
    }
}
