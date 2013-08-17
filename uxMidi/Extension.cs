/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Xml.Linq;

namespace uxMidi
{
    /// <summary>
    /// 拡張メソッドを含んだ静的クラスです。
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 与えられた Int16 値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">バイトオーダが逆転される Int16 値。</param>
        /// <returns>バイトオーダが逆転された結果。</returns>
        public static Int16 ToLittleEndian(this Int16 value)
        {
            var array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return BitConverter.ToInt16(array, 0);
        }

        /// <summary>
        /// 与えられた Int32 値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">バイトオーダが逆転される Int32 値。</param>
        /// <returns>バイトオーダが逆転された結果。</returns>
        public static Int32 ToLittleEndian(this Int32 value)
        {
            var array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return BitConverter.ToInt32(array, 0);
        }

        /// <summary>
        /// 与えられた UInt32 値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">バイトオーダが逆転される UInt32 値。</param>
        /// <returns>バイトオーダが逆転された結果。</returns>
        public static UInt32 ToLittleEndian(this UInt32 value)
        {
            var array = BitConverter.GetBytes(value);
            Array.Reverse(array);
            return BitConverter.ToUInt32(array, 0);
        }

        public static string GetAttribute(this XElement element, XName name)
        {
            var result = element.Attribute(name);
            return result == null ? null : result.Value;
        }
    }
}
