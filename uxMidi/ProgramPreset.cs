/* uxMidi / Software Synthesizer Library
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System.Collections.Generic;
using ux.Component;

namespace uxMidi
{
    /// <summary>
    /// 音源プログラムのプリセットを表します。
    /// </summary>
    public class ProgramPreset
    {
        #region -- Private Fields --
        private readonly List<Handle> initHandles;
        private readonly List<Handle> finalHandles;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// プログラムナンバーを取得します。
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// プログラムナンバーの MSB 値を取得します。
        /// </summary>
        public int MSB { get; private set; }

        /// <summary>
        /// プログラムナンバーの LSB 値を取得します。
        /// </summary>
        public int LSB { get; private set; }

        /// <summary>
        /// 初期化時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> InitHandles { get { return this.initHandles; } }

        /// <summary>
        /// 破棄時に実行されるハンドルの列挙子を取得します。
        /// </summary>
        public IEnumerable<Handle> FinalHandles { get { return this.finalHandles; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい ProgramPreset クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="number">プログラムナンバー。</param>
        /// <param name="msb">プログラムナンバーの MSB 値。</param>
        /// <param name="lsb">プログラムナンバーの LSB 値。</param>
        /// <param name="initHandles">初期化時に実行されるハンドルの列挙子。</param>
        /// <param name="finalHandles">破棄時に実行されるハンドルの列挙子。</param>
        public ProgramPreset(int number, int msb, int lsb, IEnumerable<Handle> initHandles, IEnumerable<Handle> finalHandles)
        {
            this.Number = number;
            this.MSB = msb;
            this.LSB = lsb;
            this.initHandles = new List<Handle>(initHandles);

            if (finalHandles != null)
                this.finalHandles = new List<Handle>(finalHandles);
        }
        #endregion
    }
}
