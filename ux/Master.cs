/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using ux.Component;
using ux.Utils;

namespace ux
{
    /// <summary>
    /// シンセサイザのパート管理や出力データの最終処理を担うマスタークラスです。
    /// </summary>
    public class Master
    {
        #region Private Constant Field
        private const float DefaultSamplingFreq = 44100.0f;
        private const int PartCount = 16;
        internal const double A = Math.E * 20;
        #endregion

        #region Private Field
        private float masterVolume;
        private float compressorThreshold = 0.8f;
        private float compressorRatio = 1.0f / 1.666f;
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

        /// <summary>
        /// 現在演奏を受け付けているかを表す真偽値を取得します。
        /// </summary>
        public bool Playing { get; private set; }

        /// <summary>
        /// コンプレッサのしきい値を表す実数値を取得または設定します。
        /// </summary>
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

        /// <summary>
        /// コンプレッサの圧縮率を表す実数値を取得または設定します。
        /// </summary>
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
        /// <summary>
        /// 引数を指定せずに新しい Master クラスのインスタンスを初期化します。
        /// </summary>
        public Master()
            : this(Master.DefaultSamplingFreq)
        {
        }

        /// <summary>
        /// サンプリング周波数を指定して新しい Master クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingFreq">演奏に使用されるサンプリング周波数。</param>
        public Master(float samplingFreq)
        {
            if (samplingFreq > 0.0f && samplingFreq <= float.MaxValue)
                this.samplingFreq = samplingFreq;
            else
                throw new ArgumentOutOfRangeException("samplingFreq", String.Format("指定されたサンプリング周波数は無効です: {0:f0} Hz", samplingFreq));

            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i] = new Part(this);

            this.handleQueue = new Queue<Handle>();

            this.Reset();
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

        /// <summary>
        /// 単一のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handle">プッシュされるハンドル。</param>
        public void PushHandle(Handle handle)
        {
            lock (((ICollection)this.handleQueue).SyncRoot)
                this.handleQueue.Enqueue(handle);
        }

        /// <summary>
        /// 複数のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを列挙する IEnumerable<Handle> インスタンス。</param>
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
            // バッファクリア
            Array.Clear(buffer, offset, count);

            // ハンドルの適用
            this.ApplyHandle();

            // count は バイト数
            // Part.Generate にはサンプル数を与える
            for (int k = 0; k < Master.PartCount; k++)
            {
                if (!this.parts[k].IsSounding)
                    continue;

                this.parts[k].Generate(count / 2);

                // 波形合成
                for (int i = offset, j = 0; i < count; i++, j++)
                    buffer[i] += this.parts[k].Buffer[j];
            }

            // コンプレッサ増幅度
            float gain = 1.0f / (this.compressorThreshold + (1.0f - this.compressorThreshold) * this.compressorRatio);

            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                float output = buffer[i] * this.masterVolume;

                // 圧縮
                output =
                    gain *
                    ((output > this.compressorThreshold) ? this.compressorThreshold + (output - this.compressorThreshold) * this.compressorRatio :
                    (output < -this.compressorThreshold) ? -this.compressorThreshold + (output + this.compressorThreshold) * this.compressorRatio :
                    output);

                // クリッピングと代入
                buffer[i] = (output > 1.0f) ? 1.0f : (output < -1.0f) ? -1.0f : output;
            }

            return count;
        }

        /// <summary>
        /// マスターとパートのパラメータを初期化します。
        /// </summary>
        public void Reset()
        {
            this.masterVolume = (float)((Math.Pow(Master.A, 0.8) - 1.0) / (Master.A - 1.0));

            for (int i = 0; i < Master.PartCount; i++)
                this.parts[i].Reset();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// キューからハンドルをポップし、各パートに送信します。
        /// </summary>
        private void ApplyHandle()
        {
            var list = new List<Handle>();

            // リストに一時転送
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

                            this.masterVolume = (float)(Math.Pow(Master.A, handle.Parameter.Value.Clamp(2.0f, 0.0f) - 1.0) / (Master.A - 1.0));
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
