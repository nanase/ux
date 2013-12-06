/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System.Collections.Generic;
using System.IO;
using ux;
using uxMidi.IO;

namespace uxMidi
{
    /// <summary>
    /// 転送された MIDI イベントを選択し適切なパートに送信するための抽象クラスです。
    /// </summary>
    public abstract class Selector
    {
        #region -- Protected Fields --
        protected readonly List<DrumPreset> drumset;
        protected readonly List<ProgramPreset> presets;
        protected readonly List<string> presetFiles;
        protected readonly Master master;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// ux のマスターオブジェクトを取得します。
        /// </summary>
        public Master Master { get { return this.master; } }

        /// <summary>
        /// このセレクタがマスターオブジェクトに要求するパート数を取得します。
        /// </summary>
        public abstract int PartCount { get; }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// マスターオブジェクトを指定して新しい Selector クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="master">マスターオブジェクト。</param>
        public Selector(Master master)
        {
            this.master = master;

            this.presetFiles = new List<string>();
            this.presets = new List<ProgramPreset>();
            this.drumset = new List<DrumPreset>();
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// ファイル名を指定してプリセットを追加します。
        /// </summary>
        /// <param name="filename">追加されるプリセットが記述された XML ファイル名。</param>
        public void AddPreset(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            this.presets.AddRange(PresetReader.Load(filename));
            this.drumset.AddRange(PresetReader.DrumLoad(filename));
            this.presetFiles.Add(filename);
        }

        /// <summary>
        /// 読み込まれたプリセットをすべてクリアします。
        /// </summary>
        public void ClearPreset()
        {
            this.presets.Clear();
            this.drumset.Clear();
            this.presetFiles.Clear();
        }

        /// <summary>
        /// プリセットをリロードします。現在設定されている音源の更新はされません。
        /// </summary>
        public void ReloadPreset()
        {
            this.presets.Clear();
            this.drumset.Clear();

            foreach (var filename in this.presetFiles)
            {
                if (!File.Exists(filename))
                    continue;

                this.presets.AddRange(PresetReader.Load(filename));
                this.drumset.AddRange(PresetReader.DrumLoad(filename));
            }
        }

        /// <summary>
        /// ux にリセット命令を送ります。ドラムの初期化が発生します。
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 指定された MIDI イベントを処理します。
        /// </summary>
        /// <param name="events">イベントの列挙子。</param>
        public abstract void ProcessMidiEvent(IEnumerable<Event> events);
        #endregion
    }
}
