using System;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
    /// <summary>
    ///     アプリケーションクラス
    /// </summary>
    public static class EuropaEnginePatcher
    {
        /// <summary>
        ///     バージョン名
        /// </summary>
        internal const string VersionName = "0.52";

        /// <summary>
        ///     エントリーポイント
        /// </summary>
        [STAThread]
        public static void Main()
        {
            if (!PatchController.CheckDll())
            {
                return;
            }
            if (PatchController.ParseCommandLine())
            {
                PatchController.AutoProcess();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new MainForm());
            }
        }
    }
}