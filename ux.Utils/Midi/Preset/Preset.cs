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

namespace ux.Utils.Midi
{
    /// <summary>
    /// 音源に対する動作を定めたプリセットを格納します。
    /// </summary>
    public class Preset
    {
        #region -- Private Fields --
        private readonly List<ProgramPreset> programs = new List<ProgramPreset>();
        private readonly List<DrumPreset> drums = new List<DrumPreset>();
        private readonly List<string> presetFiles = new List<string>();
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 全てのプリセットをクリアします。
        /// </summary>
        public void Clear()
        {
            this.programs.Clear();
            this.drums.Clear();
        }

        /// <summary>
        /// XML 形式のファイルからプリセットを読み込みます。
        /// </summary>
        /// <param name="file">XML ファイルを表すファイル名。</param>
        public void Load(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
                this.Load(fs);

            this.presetFiles.Add(file);
        }

        /// <summary>
        /// ストリームからプリセットを読み込みます。
        /// </summary>
        /// <param name="stream">読み込まれるストリーム。</param>
        public void Load(Stream stream)
        {
            this.programs.AddRange(PresetReader.Load(stream));
            stream.Seek(0L, SeekOrigin.Begin);
            this.drums.AddRange(PresetReader.DrumLoad(stream));
        }

        /// <summary>
        /// プリセットをリロードします。現在設定されている音源の更新はされません。
        /// </summary>
        public void Reload()
        {
            this.Clear();

            var presetFiles = this.presetFiles.ToArray();
            this.presetFiles.Clear();

            foreach (var filename in presetFiles)
            {
                if (!File.Exists(filename))
                    continue;

                this.Load(filename);
            }
        }

        /// <summary>
        /// 指定した述語に一致するプログラムプリセットを検索します。
        /// </summary>
        /// <param name="match">条件を記述する述語。</param>
        /// <returns>述語に一致したプリセット。一致しない場合は null。</returns>
        public ProgramPreset FindProgram(Predicate<ProgramPreset> match)
        {
            return this.programs.Find(match);
        }

        /// <summary>
        /// 指定した述語に一致するドラムプリセットを検索します。
        /// </summary>
        /// <param name="match">条件を記述する述語。</param>
        /// <returns>述語に一致したプリセット。一致しない場合は null。</returns>
        public DrumPreset FindDrum(Predicate<DrumPreset> match)
        {
            return this.drums.Find(match);
        }
        #endregion
    }
}
