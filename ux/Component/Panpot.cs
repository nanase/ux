/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using ux.Utils;

namespace ux.Component
{
    /// <summary>
    /// 音の定位 (左右チャネルのバランス) を表す実数値を格納する構造体です。
    /// </summary>
    struct Panpot
    {
        #region Private Fields
        private readonly float l, r;
        #endregion

        #region Public Property
        /// <summary>
        /// 左チャネルのレベルを取得または設定します。
        /// </summary>
        public float L { get { return this.l; } }

        /// <summary>
        /// 右チャネルのレベルを取得または設定します。
        /// </summary>
        public float R { get { return this.r; } }
        #endregion

        #region Constructor
        /// <summary>
        /// 左右チャネルのレベルを指定して新しい Panpot 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="lChannel">左チャネルのレベル。</param>
        /// <param name="rChannel">右チャネルのレベル。</param>
        public Panpot(float lChannel, float rChannel)
            : this()
        {
            this.l = lChannel.Clamp(1.0f, 0.0f);
            this.r = rChannel.Clamp(1.0f, 0.0f);
        }

        /// <summary>
        /// 左右チャネルのレベルを制御するパンポット値を指定して新しい Panpot 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="value">パンポット値。</param>
        public Panpot(float value)
            : this()
        {
            this.l = value >= 0.0f ? (float)Math.Sin((value + 1f) * Math.PI / 2.0) : 1.0f;
            this.r = value <= 0.0f ? (float)Math.Sin((-value + 1f) * Math.PI / 2.0) : 1.0f;
        }
        #endregion
    }
}
