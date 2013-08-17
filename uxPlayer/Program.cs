/* uxPlayer / Software Synthesizer
 * Copyright (C) 2013 Tomona Nanase. All rights reserved.
 */

using System;
using System.Windows.Forms;

namespace uxPlayer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
