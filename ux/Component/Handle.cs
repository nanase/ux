/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;

namespace ux.Component
{
	/// <summary>
	/// シンセサイザに対する命令をサポートします。
	/// </summary>
	public class Handle
	{
        #region Private Field
        private readonly int targetPart;
        private readonly HandleType type;
        private readonly int data1;
        private readonly float data2;
        #endregion

        #region Public Properties
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

		#region Constructor
		/// <summary>
		/// パラメータを指定せずに新しい Handle クラスのインスタンスを初期化します。
		/// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
		/// <param name="type">ハンドルの種類。</param>
        public Handle(int targetPart, HandleType type)
		{
			this.targetPart = targetPart;
			this.type = type;

            this.data1 = 0;
            this.data2 = 0.0f;
		}

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data1">パラメータに対する整数パラメータ。</param>
        public Handle(int targetPart, HandleType type, int data1)
        {
            this.targetPart = targetPart;
            this.type = type;

            this.data1 = data1;
            this.data2 = 0.0f;
        }

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data2">パラメータに対する実数パラメータ。</param>
        public Handle(int targetPart, HandleType type, float data2)
        {
            this.targetPart = targetPart;
            this.type = type;

            this.data1 = 0;
            this.data2 = data2;
        }

        /// <summary>
        /// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="targetPart">ハンドルが適用されるパート。</param>
        /// <param name="type">ハンドルの種類。</param>
        /// <param name="data1">パラメータに対する整数パラメータ。</param>
        /// <param name="data2">パラメータに対する実数パラメータ。</param>
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
        {
            this.targetPart = newTargetPart;
            this.type = handle.Type;

            this.data1 = handle.data1;
            this.data2 = handle.data2;
        }
		#endregion

		#region Override Methods
		/// <summary>
		/// このクラスのインスタンスを表す文字列を取得します。
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0} Part:{1}, Data1:{2}, Data2:{3:f2}", this.type, this.targetPart, this.data1, this.data2);
		}
		#endregion
	}
}
