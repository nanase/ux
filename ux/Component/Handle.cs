/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */
using System;

namespace ux.Component
{
	/// <summary>
	/// シンセサイザに対する命令をサポートします。
	/// </summary>
	public class Handle : IComparable<Handle>
	{
		private static long idNum = 0;

		#region Public Properties
		/// <summary>
		/// 対象となるパートを取得します。
		/// </summary>
		public int TargetPart { get; private set; }

		/// <summary>
		/// ハンドルのタイプを取得します。
		/// </summary>
        public HandleType Type { get; private set; }

		/// <summary>
		/// ハンドルに割り当てられたパラメータを取得します。
		/// </summary>
        public PValue Parameter { get; private set; }

		/// <summary>
		/// このハンドルに割当てられている一意のIDを取得します。
		/// </summary>
		public long ID { get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// パラメータを指定して新しい Handle クラスのインスタンスを初期化します。
		/// </summary>
		/// <param name="targetPart">対象パート。</param>
		/// <param name="type">ハンドルの種類。</param>
		/// <param name="parameter1">一つ目のパラメータ。</param>
        public Handle(int targetPart, HandleType type, PValue parameter)
		{
			this.TargetPart = targetPart;
			this.Type = type;

            this.Parameter = parameter;

			this.ID = idNum++;
		}
		#endregion

		#region IComparable[Handle] Implementations
		/// <summary>
		/// このオブジェクトと別のオブジェクトを比較します。
		/// </summary>
		/// <param name="other">比較先となるオブジェクト。</param>
		/// <returns>0より小さい値のとき、このオブジェクトが先行します。0より大きい値のとき、比較先のオブジェクトが先行します。0のとき、二つのオブジェクトは同じ順序となります。</returns>
		public int CompareTo(Handle other)
		{
			return Handle.Compare(this, other);
		}
		#endregion

		#region Public Static Methods
		//GetParameterType => InternalHandle.cs

		/// <summary>
		/// 2つのオブジェクトを比較します。
		/// </summary>
		/// <param name="x">一つ目のオブジェクト。</param>
		/// <param name="y">二つ目のオブジェクト。</param>
		/// <returns>0より小さい値のとき、オブジェクト x が先行します。0より大きい値のとき、オブジェクト y が先行します。0のとき、二つのオブジェクトは同じ順序となります。</returns>
		public static int Compare(Handle x, Handle y)
		{
			int tmp;

			tmp = x.TargetPart.CompareTo(y.TargetPart);
			if (tmp != 0)
				return tmp;

			tmp = x.Type.CompareTo(y.Type);
			if (tmp != 0)
				return tmp;

			tmp = x.ID.CompareTo(y.ID);
			if (tmp != 0)
				return tmp;

			return 0;
		}
		#endregion

		#region Override Methods
		/// <summary>
		/// このクラスのインスタンスを表す文字列を取得します。
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0} Part:{1}, Param:{2}", this.Type, this.TargetPart, this.Parameter);
		}
		#endregion
	}
}
