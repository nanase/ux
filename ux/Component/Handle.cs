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

namespace ux.Component
{
    /// <summary>
    /// シンセサイザに対する命令をサポートします。
    /// </summary>
    public class Handle
    {
        #region -- Private Fields --
        private readonly int targetPart;
        private readonly HandleType type;
        private readonly int data1;
        private readonly float data2;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// 対象となるパートを取得します。
        /// </summary>
        public int TargetPart { get { return this.targetPart; } }

        /// <summary>
        /// ハンドルのタイプを取得します。
        /// </summary>
        public HandleType Type { get { return this.type; } }

        /// <summary>
        /// ハンドルに対する整数パラメータを取得します。
        /// </summary>
        public int Data1 { get { return this.data1; } }

        /// <summary>
        /// ハンドルに対する実数パラメータを取得します。
        /// </summary>
        public float Data2 { get { return this.data2; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定せずに新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        public Handle(int targetPart, HandleType type)
            : this(targetPart, type, 0, 0.0f)
        {
        }

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data1">ハンドルに対する整数パラメータ。</param>
        public Handle(int targetPart, HandleType type, int data1)
            : this(targetPart, type, data1, 0.0f)
        {
        }

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data2">ハンドルに対する実数パラメータ。</param>
        public Handle(int targetPart, HandleType type, float data2)
            : this(targetPart, type, 0, data2)
        {
        }

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data1">ハンドルに対する整数パラメータ。</param>
        /// <param name="data2">ハンドルに対する実数パラメータ。</param>
        public Handle(int targetPart, HandleType type, int data1, float data2)
        {
            this.targetPart = targetPart;
            this.type = type;

            this.data1 = data1;
            this.data2 = data2;
        }

        /// <summary>
        /// ベースとなる Handle オブジェクトと新しいパートを指定して Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="handle">ベースとなる Handle オブジェクト。</param>
        /// <param name="newTargetPart">新しいパート。</param>
        public Handle(Handle handle, int newTargetPart)
            : this(newTargetPart, handle.type, handle.data1, handle.data2)
        {
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// このクラスのインスタンスを表す文字列を取得します。
        /// </summary>
        /// <returns>このクラスのインスタンスを表す文字列。</returns>
        public override string ToString()
        {
            return String.Format("{0} Part:{1}, Data1:{2}, Data2:{3:f2}",
                                 this.type,
                                 this.targetPart,
                                 this.data1,
                                 this.data2);
        }
        #endregion
    }
}
