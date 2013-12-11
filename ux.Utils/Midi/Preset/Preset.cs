﻿/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
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
        #endregion
    }
}