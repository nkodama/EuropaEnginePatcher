using System;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
    /// <summary>
    ///     アプリケーションクラス
    /// </summary>
    public class EuropaEnginePatcher
    {
        /// <summary>
        ///     バージョン名
        /// </summary>
        internal const string VersionName = "0.45";

        /// <summary>
        ///     エントリーポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm());
        }
    }
}