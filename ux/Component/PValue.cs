/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;

namespace ux.Component
{
    /// <summary>
    /// 文字列と実数値を格納したパラメータです。
    /// </summary>
    public struct PValue
    {
        #region Private Fields
        private readonly string name;
        private readonly float value;
        #endregion

        #region Public Properties
        /// <summary>
        /// パラメータ名を取得します。
        /// </summary>
        public string Name { get { return this.name; } }

        /// <summary>
        /// パラメータ値である実数値を取得します。
        /// </summary>
        public float Value { get { return this.value; } }
        #endregion

        #region Constructors
        /// <summary>
        /// パラメータ名と実数値を指定して新しい PValue 構造体のインスタンスを初期化します。
        /// </summary>
        /// <param name="name">パラメータの名前を表す文字列。自動的に小文字に変換されます。</param>
        /// <param name="value">パラメータの値を表す実数値。</param>
        public PValue(string name, float value)
            : this()
        {
            if (name != null)
                this.name = name.ToLower();
            this.value = value;
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// このパラメータを表す文字列を取得します。
        /// </summary>
        /// <returns>パラメータ名と実数値を含む文字列。</returns>
        public override string ToString()
        {
            return String.Format("{0}={1}", (this.name ?? "(名前なし)"), this.value);
        }
        #endregion
    }
}
