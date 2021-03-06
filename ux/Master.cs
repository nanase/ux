﻿/* ux - Micro Xylph / Software Synthesizer Core Library

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
using System.Linq;
using ux.Component;
using ux.Utils;

namespace ux
{
    /// <summary>
    /// シンセサイザのパート管理や出力データの最終処理を担うマスタークラスです。
    /// </summary>
    public class Master
    {
        #region -- Private Fields --
        private const float DefaultSamplingRate = 44100.0f;
        private const int DefaultPartCount = 16;

        private readonly float samplingRate;
        private readonly Part[] parts;
        private readonly Queue<Handle> handleQueue;
        private readonly int partCount;
        private float masterVolume = 1.0f;

        #region Compressor
        private float gain, upover, downover;
        private float compressorThreshold = 0.8f;
        private float compressorRatio = 1.0f / 2.0f;
        #endregion
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 再生に用いられるサンプリング周波数を取得します。
        /// </summary>
        public float SamplingRate { get { return this.samplingRate; } }

        /// <summary>
        /// 現在演奏を受け付けているかを表す真偽値を取得します。
        /// </summary>
        public bool Playing { get; private set; }

        /// <summary>
        /// パートの数を取得します。
        /// </summary>
        public int PartCount { get { return this.partCount; } }

        /// <summary>
        /// 発音しているパートの数を取得します。
        /// </summary>
        public int ToneCount { get { return this.parts.Count(p => p.IsSounding); } }

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

                this.PrepareCompressor();
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

                this.PrepareCompressor();
            }
        }

        public float MasterVolume
        {
            get { return this.masterVolume; }
            set
            {
                if (float.IsInfinity(value) || float.IsNaN(value))
                    throw new ArgumentException();

                this.masterVolume = value.Clamp(2.0f, 0.0f);
            }
        }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// 引数を指定せずに新しい Master クラスのインスタンスを初期化します。
        /// </summary>
        public Master()
            : this(Master.DefaultSamplingRate, Master.DefaultPartCount)
        {
        }

        /// <summary>
        /// サンプリング周波数を指定して新しい Master クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">演奏に使用されるサンプリング周波数。</param>
        public Master(float samplingRate, int partCount)
        {
            if (samplingRate > 0.0f && samplingRate <= float.MaxValue)
                this.samplingRate = samplingRate;
            else
                throw new ArgumentOutOfRangeException("samplingRate",
                                                      samplingRate,
                                                      "指定されたサンプリング周波数は無効です。");

            if (partCount < 0)
                throw new ArgumentOutOfRangeException("partCount",
                                                      partCount,
                                                      "無効なパート数が渡されました。");

            this.partCount = partCount;

            this.parts = new Part[partCount];

            for (int i = 0; i < this.partCount; i++)
                this.parts[i] = new Part(this.samplingRate);

            this.handleQueue = new Queue<Handle>();

            this.Reset();
            this.PrepareCompressor();
        }
        #endregion

        #region -- Public Methods --
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

        #region PushHandle
        /// <summary>
        /// 単一のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handle">プッシュされるハンドル。</param>
        public void PushHandle(Handle handle)
        {
            lock (this.handleQueue)
                this.handleQueue.Enqueue(handle);
        }

        /// <summary>
        /// 複数のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handle">プッシュされるハンドル。</param>
        /// <param name="targetParts">ハンドルが適用されるパートを列挙する IEnumerable&lt;int&gt; インスタンス。</param>
        public void PushHandle(Handle handle, IEnumerable<int> targetParts)
        {
            lock (this.handleQueue)
                for (var partEnum = targetParts.GetEnumerator(); partEnum.MoveNext(); )
                    this.handleQueue.Enqueue(new Handle(handle, partEnum.Current));
        }

        /// <summary>
        /// 複数のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handle">プッシュされるハンドル。</param>
        /// <param name="targetParts">ハンドルが適用されるパートを格納する int の配列。</param>
        public void PushHandle(Handle handle, int[] targetParts)
        {
            lock (this.handleQueue)
                for (int i = 0, l = targetParts.Length; i < l; i++)
                    this.handleQueue.Enqueue(new Handle(handle, targetParts[i]));
        }

        /// <summary>
        /// 複数のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを列挙する IEnumerable&lt;Handle&gt; インスタンス。</param>
        public void PushHandle(IEnumerable<Handle> handles)
        {
            lock (this.handleQueue)
                // optimized: foreach (var handle in handles)
                for (var handleEnum = handles.GetEnumerator(); handleEnum.MoveNext(); )
                    this.handleQueue.Enqueue(handleEnum.Current);
        }

        /// <summary>
        /// 複数のハンドルをキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを格納する Handle の配列。</param>
        public void PushHandle(params Handle[] handles)
        {
            lock (this.handleQueue)
                for (int i = 0, l = handles.Length; i < l; i++)
                    this.handleQueue.Enqueue(handles[i]);
        }

        /// <summary>
        /// 複数のハンドルを指定されたパートに適用するようキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを列挙する IEnumerable&lt;Handle&gt; インスタンス。</param>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        public void PushHandle(IEnumerable<Handle> handles, int targetPart)
        {
            lock (this.handleQueue)
                // optimized: foreach (var handle in handles)
                for (var handleEnum = handles.GetEnumerator(); handleEnum.MoveNext(); )
                    this.handleQueue.Enqueue(new Handle(handleEnum.Current, targetPart));
        }

        /// <summary>
        /// 複数のハンドルを指定されたパートに適用するようキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを格納する Handle の配列。</param>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        public void PushHandle(Handle[] handles, int targetPart)
        {
            lock (this.handleQueue)
                for (int i = 0, l = handles.Length; i < l; i++)
                    this.handleQueue.Enqueue(new Handle(handles[i], targetPart));
        }

        /// <summary>
        /// 複数のハンドルを指定されたパートに適用するようキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを列挙する IEnumerable&lt;Handle&gt; インスタンス。</param>
        /// <param name="targetParts">ハンドルが適用されるパートを列挙する IEnumerable&lt;int&gt; インスタンス。</param>
        public void PushHandle(IEnumerable<Handle> handles, IEnumerable<int> targetParts)
        {
            lock (this.handleQueue)
            {
                var handles_array = handles.ToArray();
                var j = handles_array.Length;
                for (var partEnum = targetParts.GetEnumerator(); partEnum.MoveNext(); )
                    for (int i = 0, part = partEnum.Current; i < j; i++)
                        this.handleQueue.Enqueue(new Handle(handles_array[i], part));
            }
        }

        /// <summary>
        /// 複数のハンドルを指定されたパートに適用するようキューにプッシュします。
        /// </summary>
        /// <param name="handles">複数ハンドルを格納する Handle の配列。</param>
        /// <param name="targetParts">ハンドルが適用されるパートを格納する int の配列。</param>
        public void PushHandle(Handle[] handles, int[] targetParts)
        {
            lock (this.handleQueue)
                for (int i = 0, l = targetParts.Length, m = handles.Length; i < l; i++)
                    for (int j = 0; j < m; j++)
                        this.handleQueue.Enqueue(new Handle(handles[j], targetParts[i]));
        }
        #endregion

        /// <summary>
        /// 全てのパートにリリースを送信します。
        /// </summary>
        public void Release()
        {
            for (int i = 0; i < this.partCount; i++)
                this.parts[i].Release();
        }

        /// <summary>
        /// 全てのパートにサイレンスを送信します。
        /// </summary>
        public void Silence()
        {
            for (int i = 0; i < this.partCount; i++)
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
            for (int k = 0; k < this.partCount; k++)
            {
                Part part = this.parts[k];

                if (part.IsSounding)
                {
                    part.Generate(count / 2);

                    // 波形合成
                    for (int i = offset, j = 0; j < count; i++, j++)
                        buffer[i] += part.Buffer[j];
                }
            }

            for (int i = offset, length = offset + count; i < length; i++)
            {
                float output = buffer[i] * this.masterVolume * this.gain;

                if (output == 0.0f)
                {
                    buffer[i] = 0.0f;
                }
                else
                {
                    // 圧縮
                    if (output > this.compressorThreshold)
                        output = this.upover + this.compressorRatio * output;
                    else if (output < -this.compressorThreshold)
                        output = this.downover + this.compressorRatio * output;

                    // クリッピングと代入
                    buffer[i] = (output > 1.0f) ? 1.0f : (output < -1.0f) ? -1.0f : output;
                }
            }

            return count;
        }

        /// <summary>
        /// 生成された PCM データを読み込みます。
        /// </summary>
        /// <param name="buffer">格納先のバッファ。</param>
        /// <param name="offset">バッファに対するオフセット。</param>
        /// <param name="count">読み込まれる要素数。</param>
        /// <returns>読み込みに成功した要素数。</returns>
        public int Read(double[] buffer, int offset, int count)
        {
            // バッファクリア
            Array.Clear(buffer, offset, count);

            // ハンドルの適用
            this.ApplyHandle();

            // count は バイト数
            // Part.Generate にはサンプル数を与える
            for (int k = 0; k < this.partCount; k++)
            {
                Part part = this.parts[k];

                if (part.IsSounding)
                {
                    part.Generate(count / 2);

                    // 波形合成
                    for (int i = offset, j = 0; j < count; i++, j++)
                        buffer[i] += (double)part.Buffer[j];
                }
            }

            for (int i = offset, length = offset + count; i < length; i++)
            {
                double output = buffer[i] * this.masterVolume * this.gain;

                if (output == 0.0)
                {
                    buffer[i] = 0.0;
                }
                else
                {
                    // 圧縮
                    if (output > this.compressorThreshold)
                        output = this.upover + this.compressorRatio * output;
                    else if (output < -this.compressorThreshold)
                        output = this.downover + this.compressorRatio * output;

                    // クリッピングと代入
                    buffer[i] = (output > 1.0) ? 1.0 : (output < -1.0) ? -1.0 : output;
                }
            }

            return count;
        }

        /// <summary>
        /// マスターとパートのパラメータを初期化します。
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < this.partCount; i++)
                this.parts[i].Reset();
        }
        #endregion

        #region -- Private Methods --
        /// <summary>
        /// キューからハンドルをポップし、各パートに送信します。
        /// </summary>
        private void ApplyHandle()
        {
            if (this.handleQueue.Count == 0)
                return;

            lock (this.handleQueue)
            {
                // optimized: foreach (var handle in this.handleQueue)
                var enumerator = this.handleQueue.GetEnumerator();
                for (Handle handle; enumerator.MoveNext(); )
                {
                    handle = enumerator.Current;

                    if (handle.TargetPart > 0 && handle.TargetPart <= this.partCount)
                        this.parts[handle.TargetPart - 1].ApplyHandle(handle);
                }

                this.handleQueue.Clear();
            }
        }

        private void PrepareCompressor()
        {
            float threshold = this.compressorThreshold;
            float ratio = this.compressorRatio;
            this.gain = 1.0f / (threshold + (1.0f - threshold) * ratio);
            this.upover = threshold * (1.0f - ratio);
            this.downover = -threshold * (1.0f - ratio);
        }
        #endregion
    }
}
