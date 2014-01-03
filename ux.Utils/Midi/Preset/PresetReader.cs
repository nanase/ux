/* ux.Utils / Software Synthesizer Library

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
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ux.Utils.Midi
{
    /// <summary>
    /// XML ファイルからプリセットを読み込むためのメソッドを提供します。
    /// </summary>
    public static class PresetReader
    {
        #region -- Private Fields --
        private static readonly XName xtype = XName.Get("type");
        private static readonly XName xvalue = XName.Get("value");
        private static readonly XName xnumber = XName.Get("number");
        private static readonly XName xmsb = XName.Get("msb");
        private static readonly XName xlsb = XName.Get("lsb");
        private static readonly XName xfinal = XName.Get("final");
        private static readonly XName xnote = XName.Get("note");
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// XML を格納したファイルを読み込み、プログラムプリセットを生成します。
        /// </summary>
        /// <param name="filename">読み込まれるファイル名。</param>
        /// <returns>プログラムプリセットの列挙子。</returns>
        public static IEnumerable<ProgramPreset> Load(string filename)
        {
            return PresetReader.Load(XDocument.Load(filename));
        }

        /// <summary>
        /// XML を格納したストリームを読み込み、プログラムプリセットを生成します。
        /// </summary>
        /// <param name="stream">読み取られるストリーム。</param>
        /// <returns>プログラムプリセットの列挙子。</returns>
        public static IEnumerable<ProgramPreset> Load(Stream stream)
        {
            return PresetReader.Load(XDocument.Load(stream));
        }

        /// <summary>
        /// XML を格納したファイルを読み込み、ドラムプリセットを生成します。
        /// </summary>
        /// <param name="filename">読み込まれるファイル名。</param>
        /// <returns>ドラムプリセットの列挙子。</returns>
        public static IEnumerable<DrumPreset> DrumLoad(string filename)
        {
            return PresetReader.DrumLoad(XDocument.Load(filename));
        }

        /// <summary>
        /// XML を格納したストリームを読み込み、ドラムプリセットを生成します。
        /// </summary>
        /// <param name="stream">読み取られるストリーム。</param>
        /// <returns>ドラムプリセットの列挙子。</returns>
        public static IEnumerable<DrumPreset> DrumLoad(Stream stream)
        {
            return PresetReader.DrumLoad(XDocument.Load(stream));
        }
        #endregion

        #region -- Private Methods --
        private static IEnumerable<ProgramPreset> Load(XDocument document)
        {
            var preset = document.Elements("ux").Select(e => e.Elements("preset")).FirstOrDefault();

            if (preset == null)
                throw new ArgumentException();

            var programs = preset.Elements("program");

            foreach (var program in programs)
            {
                var number = program.GetAttribute(PresetReader.xnumber);
                var str_msb = program.GetAttribute(PresetReader.xmsb);
                var str_lsb = program.GetAttribute(PresetReader.xlsb);

                int num = 0, msb = 0, lsb = 0;

                if (!int.TryParse(number, out num))
                    throw new ArgumentException();

                if (!string.IsNullOrWhiteSpace(str_msb) && !int.TryParse(str_msb, out msb))
                    throw new ArgumentException();

                if (!string.IsNullOrWhiteSpace(str_lsb) && !int.TryParse(str_lsb, out lsb))
                    throw new ArgumentException();

                if (program.Elements(PresetReader.xfinal).Count() > 1)
                    throw new ArgumentException();

                yield return new ProgramPreset(num, msb, lsb,
                    program.Elements()
                    .Where(h => h.Name.LocalName.ToLower() != "final")
                    .Select(h => HandleCreator.Create(h.Name.LocalName.ToLower(), h.GetAttribute(PresetReader.xtype), h.GetAttribute(PresetReader.xvalue))),
                    program.Elements(PresetReader.xfinal)
                    .SelectMany(f => f.Elements())
                    .Select(h => HandleCreator.Create(h.Name.LocalName.ToLower(), h.GetAttribute(PresetReader.xtype), h.GetAttribute(PresetReader.xvalue))));
            }
        }

        private static IEnumerable<DrumPreset> DrumLoad(XDocument document)
        {
            var preset = document.Elements("ux").Select(e => e.Elements("preset")).Select(e => e.Elements("drum")).FirstOrDefault();

            if (preset == null)
                throw new ArgumentException();

            var notes = preset.Elements("note");

            foreach (var note in notes)
            {
                var number = note.GetAttribute(PresetReader.xnumber);
                var note_number = note.GetAttribute(PresetReader.xnote);
                int num = 0, note_num = 0;

                if (!int.TryParse(number, out num))
                    throw new ArgumentException();

                if (!int.TryParse(note_number, out note_num))
                    throw new ArgumentException();

                yield return new DrumPreset(num, note_num,
                    note.Elements()
                    .Where(h => h.Name.LocalName.ToLower() != "final")
                    .Select(h => HandleCreator.Create(h.Name.LocalName.ToLower(), h.GetAttribute(PresetReader.xtype), h.GetAttribute(PresetReader.xvalue))));
            }
        }
        #endregion
    }
}
