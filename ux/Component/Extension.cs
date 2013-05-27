/* ux - Micro Xylph / Software Synthesizer Core Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

namespace ux.Utils
{
	/// <summary>
	/// 拡張メソッドを集めたクラスです。
	/// </summary>
	public static class ValueTypeEx
	{
		/// <summary>
		/// Double 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <returns>正規化された値。</returns>
		public static double Normalize (this double value, double max)
		{
			return value < 0.0 ? 0.0 : value > max ? max : value;
		}

		/// <summary>
		/// Double 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <param name="min">範囲の最小値。</param>
		/// <returns>正規化された値。</returns>
		public static double Normalize (this double value, double max, double min)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// Single 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <returns>正規化された値。</returns>
		public static float Normalize (this float value, float max)
		{
			return value < 0.0f ? 0.0f : value > max ? max : value;
		}

		/// <summary>
		/// Single 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <param name="min">範囲の最小値。</param>
		/// <returns>正規化された値。</returns>
		public static float Normalize (this float value, float max, float min)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// float 値を単純に Int32 値に変換します。
		/// </summary>
		/// <param name="value">変換される float 値。</param>
		/// <returns>変換された Int32 値。</returns>
		public static int ToInt32(this float value)
		{
			return (int)value;
		}

		/// <summary>
		/// float 値を 0 から指定された数値の範囲で Int32 値に変換します。
		/// </summary>
		/// <param name="value">変換される float 値。</param>
		/// <param name="max">変換の上限となる 最大値。</param>
		/// <returns>変換された Int32 値。</returns>
		public static int ToInt32(this float value, int max)
		{
			int tmp = (int)value;
			return tmp < 0 ? 0 : tmp > max ? max : tmp;
		}

		/// <summary>
		/// float 値を指定された数値の範囲で Int32 値に変換します。
		/// </summary>
		/// <param name="value">変換される float 値。</param>
		/// <param name="max">変換の上限となる 最大値。</param>
		/// <param name="min">変換の下限となる 最小値。</param>
		/// <returns>変換された Int32 値。</returns>
		public static int ToInt32(this float value, int max, int min)
		{
			int tmp = (int)value;
			return tmp < min ? min : tmp > max ? max : tmp;
		}

		/// <summary>
		/// Int32 値を 0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <returns>正規化された値。</returns>
		public static int Normalize(this int value, int max)
		{
			return value < 0 ? 0 : value > max ? max : value;
		}

		/// <summary>
		/// Int32 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <param name="min">範囲の最小値。</param>
		/// <returns>正規化された値。</returns>
		public static int Normalize (this int value, int max, int min)
		{
			return value < min ? min : value > max ? max : value;
		}

		/// <summary>
		/// Int16 値を 0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <returns>正規化された値。</returns>
		public static short Normalize (this short value, short max)
		{
			return value < (short)0 ? (short)0 : value > max ? max : value;
		}

		/// <summary>
		/// Int16 値を 0.0 から指定された数値の範囲で正規化します。
		/// </summary>
		/// <param name="value">正規化される値。</param>
		/// <param name="max">範囲の最大値。</param>
		/// <param name="min">範囲の最小値。</param>
		/// <returns>正規化された値。</returns>
		public static short Normalize (this short value, short max, short min)
		{
			return value < min ? min : value > max ? max : value;
		}
	}
}
