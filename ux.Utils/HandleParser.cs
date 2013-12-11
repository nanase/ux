/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using ux.Component;
using ux.Utils.Midi;

namespace ux.Utils
{
    /// <summary>
    /// 文字列からハンドルを生成するためのパーサを提供します。
    /// </summary>
    public class HandleParser
    {
        #region -- Public Static Methods --
        /// <summary>
        /// 指定された文字列を解析し、複数のハンドルを出力します。
        /// </summary>
        /// <param name="code">解析される文字列。</param>
        /// <param name="handles">ハンドルオブジェクトの列挙子。</param>
        /// <returns>解析に成功した場合は true、失敗した場合は false。</returns>
        public static bool TryParse(string code, out IEnumerable<Handle> handles)
        {
            handles = null;

            var handle_pool = new List<Handle>();
            int part = 0;

            foreach (var line in code.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var tokens = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length < 2 && tokens.Length > 4)
                    return false;

                if (part == 0 && tokens[0] == ".")
                    return false;
                else if (tokens[0] != "." && !int.TryParse(tokens[0], out part))
                    return false;

                Handle h;

                switch (tokens.Length)
                {
                    case 2:
                        if (!HandleCreator.TryCreate(part, tokens[1], "", "", out h))
                            return false;
                        break;

                    case 3:
                        if (!HandleCreator.TryCreate(part, tokens[1], tokens[2], "", out h))
                            return false;
                        break;

                    case 4:
                        if (!HandleCreator.TryCreate(part, tokens[1], tokens[2], tokens[3], out h))
                            return false;
                        break;

                    default:
                        return false;
                }

                handle_pool.Add(h);
            }

            handles = handle_pool.ToArray();

            return true;
        }
        #endregion
    }
}
