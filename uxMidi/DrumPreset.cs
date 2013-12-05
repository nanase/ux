/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System.Collections.Generic;
using ux.Component;

namespace uxMidi
{
    /// <summary>
    /// ドラムまたはノイズのプリセットを表します。
    /// </summary>
    public class DrumPreset
    {
        #region -- Private Fields --
        private readonly List<Handle> initHandles;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// プログラムナンバーを取得します。
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// ドラムのノートナンバーを取得します。
        /// </summary>
        public int Note { get; private set; }

        /// <summary>
        /// 初期化時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> InitHandles { get { return this.initHandles; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい DrumPreset クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="number">プログラムナンバー。</param>
        /// <param name="note">ドラムのノートナンバー。</param>
        /// <param name="initHandles">初期化時に実行されるハンドルの列挙子。</param>
        public DrumPreset(int number, int note, IEnumerable<Handle> initHandles)
        {
            this.Number = number;
            this.Note = note;
            this.initHandles = new List<Handle>(initHandles);
        }
        #endregion
    }
}
