/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace uxMidi
{
    /// <summary>
    /// XML ファイルからプリセットを読み込むためのメソッドを提供します。
    /// </summary>
    public static class PresetReader
    {
        #region Private Field
        private static readonly XName xtype = XName.Get("type");
        private static readonly XName xvalue = XName.Get("value");
        private static readonly XName xnumber = XName.Get("number");
        private static readonly XName xmsb = XName.Get("msb");
        private static readonly XName xlsb = XName.Get("lsb");
        private static readonly XName xfinal = XName.Get("final");
        private static readonly XName xnote = XName.Get("note");
        #endregion

        #region Public Method
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

        #region Private Method
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
