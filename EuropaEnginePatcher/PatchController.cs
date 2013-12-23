using System;
using System.IO;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
    /// <summary>
    ///     パッチ処理のコントローラクラス
    /// </summary>
    public static class PatchController
    {
        #region 公開プロパティ

        /// <summary>
        ///     パッチ対象のファイル名
        /// </summary>
        public static string TargetFileName { get; set; }

        /// <summary>
        ///     ゲームの種類
        /// </summary>
        public static GameType GameType { get; set; }

        /// <summary>
        ///     自動処理モード
        /// </summary>
        public static bool AutoMode { get; set; }

        /// <summary>
        ///     元のファイルをリネーム
        /// </summary>
        public static bool RenameOriginal { get; set; }

        /// <summary>
        ///     テキスト自動折り返し
        /// </summary>
        public static bool AutoLineBreak { get; set; }

        /// <summary>
        ///     自動命名時の語順変更
        /// </summary>
        public static bool WordOrder { get; set; }

        /// <summary>
        ///     強制ウィンドウ化
        /// </summary>
        public static bool Windowed { get; set; }

        #endregion

        #region 内部定数

        /// <summary>
        ///     実行ファイル名リスト
        /// </summary>
        private static readonly string[] ExeNames =
        {
            "CrusadersEn.exe",
            "Crusaders.exe",
            "EU2En.exe",
            "EU2.exe",
            "FTGEn.exe",
            "FTG.exe",
            "VictoriaEn.exe",
            "Victoria.exe",
            "HoIEn.exe",
            "HoI.exe",
            "Hoi2En.exe",
            "Hoi2.exe",
            "AODGameEn.exe",
            "AODGame.exe",
            "Darkest HourEn.exe",
            "Darkest Hour.exe"
        };

        #endregion

        #region 初期化

        /// <summary>
        ///     静的コンストラクタ
        /// </summary>
        static PatchController()
        {
            AutoMode = true;
            RenameOriginal = true;
            AutoLineBreak = true;
            WordOrder = false;
            Windowed = false;
        }

        #endregion

        #region パッチ処理

        /// <summary>
        ///     自動処理
        /// </summary>
        /// <returns>パッチ処理が成功すればtrueを返す</returns>
        /// <remarks>
        ///     戻り値は保存キーを有効化するかの判定に使用するので、保存/DLLコピーに失敗してもtrueを返す
        /// </remarks>
        public static bool AutoProcess()
        {
            if (!Patch())
            {
                return false;
            }
            if (!Save())
            {
                return true;
            }
            if (!CopyDll())
            {
                return true;
            }

            MessageBox.Show("成功しました。", "Europa Engine Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;
        }

        /// <summary>
        ///     パッチ処理
        /// </summary>
        /// <returns>パッチ処理が成功すればtrueを返す</returns>
        public static bool Patch()
        {
            if (!File.Exists(TargetFileName))
            {
                return false;
            }

            if (GameType == GameType.Unknown)
            {
                DetectGameType(TargetFileName);
                if (GameType == GameType.Unknown)
                {
                    return false;
                }
            }

            return PatchEngine.Patch(TargetFileName, GameType);
        }

        /// <summary>
        ///     パッチ後のファイル保存処理
        /// </summary>
        /// <returns>保存が成功すればtrueを返す</returns>
        public static bool Save()
        {
            try
            {
                if (!File.Exists(TargetFileName))
                {
                    return false;
                }

                string jpFileName;
                if (RenameOriginal)
                {
                    string enFileName = GetExeFileName(TargetFileName, "En");
                    if (!AutoMode)
                    {
                        var dialog = new SaveFileDialog
                        {
                            FileName = Path.GetFileNameWithoutExtension(enFileName),
                            DefaultExt = "exe",
                            Filter = "実行ファイル (*.exe)|*.exe",
                            InitialDirectory = Path.GetDirectoryName(enFileName),
                            OverwritePrompt = true,
                            Title = "元のファイルをリネームして保存"
                        };
                        if (dialog.ShowDialog() == DialogResult.Cancel)
                        {
                            return false;
                        }
                        enFileName = dialog.FileName;
                    }
                    jpFileName = GetExeFileName(TargetFileName, "");
                    if (!Path.GetFullPath(enFileName).Equals(Path.GetFullPath(TargetFileName)))
                    {
                        if (File.Exists(enFileName))
                        {
                            File.Delete(enFileName);
                        }
                        if (File.Exists(TargetFileName))
                        {
                            File.Move(TargetFileName, enFileName);
                        }
                    }
                }
                else
                {
                    jpFileName = GetExeFileName(TargetFileName, "Jp");
                    if (!AutoMode)
                    {
                        var dialog = new SaveFileDialog
                        {
                            FileName = Path.GetFileNameWithoutExtension(jpFileName),
                            DefaultExt = "exe",
                            Filter = "実行ファイル (*.exe)|*.exe",
                            InitialDirectory = Path.GetDirectoryName(jpFileName),
                            OverwritePrompt = true,
                            Title = "パッチを当てたファイルを保存"
                        };
                        if (dialog.ShowDialog() == DialogResult.Cancel)
                        {
                            return false;
                        }
                        jpFileName = dialog.FileName;
                    }
                }

                PatchEngine.SavePatchedFile(jpFileName);
            }
            catch (Exception)
            {
                MessageBox.Show("パッチを当てたファイルの保存に失敗しました。", "ファイルの保存", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     _inmm.dllをコピーする
        /// </summary>
        /// <returns>コピーが成功すればtrueを返す</returns>
        public static bool CopyDll()
        {
            try
            {
                string folderName = Path.GetDirectoryName(TargetFileName);
                if (string.IsNullOrEmpty(folderName))
                {
                    folderName = Environment.CurrentDirectory;
                }

                string destName = Path.Combine(folderName, "_inmm.dll");
                string srcName = Path.Combine(Environment.CurrentDirectory, "_inmm.dll");

                if (!File.Exists(srcName))
                {
                    MessageBox.Show("_inmm.dllが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (File.Exists(destName))
                {
                    if (!File.GetLastWriteTimeUtc(destName).Equals(File.GetLastWriteTimeUtc(srcName)))
                    {
                        if (!AutoMode)
                        {
                            if (MessageBox.Show("_inmm.dllのバージョンが一致しません。\n上書きコピーしてもよろしいですか？",
                                "日本語化DLLのコピー",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return false;
                            }
                        }
                        File.Copy(srcName, destName, true);
                    }
                }
                else
                {
                    if (!AutoMode)
                    {
                        if (MessageBox.Show("_inmm.dllがありません。\nコピーしてもよろしいですか？",
                            "日本語化DLLのコピー",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    File.Copy(srcName, destName, true);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("日本語化DLLのコピーに失敗しました。", "日本語化DLLのコピー", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion

        #region ゲーム種類判別

        /// <summary>
        ///     ゲームの実行ファイルを判別する
        /// </summary>
        /// <param name="folderName">対象フォルダ名</param>
        /// <returns>実行ファイル名</returns>
        public static string DetectExeFileName(string folderName)
        {
            foreach (string exeName in ExeNames)
            {
                string name = Path.Combine(folderName, exeName);
                if (File.Exists(name))
                {
                    return name;
                }
            }
            return folderName;
        }

        /// <summary>
        ///     実行ファイル名を取得する
        /// </summary>
        /// <param name="pathName">パス名</param>
        /// <param name="suffix">ファイル名の接尾辞(En/Jp)</param>
        /// <returns>実行ファイル名</returns>
        private static string GetExeFileName(string pathName, string suffix)
        {
            string dirName = Path.GetDirectoryName(pathName) ?? Environment.CurrentDirectory;
            string fileName = Path.GetFileNameWithoutExtension(pathName);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "HoI2";
            }
            if (fileName.Length > 2 && fileName.Substring(fileName.Length - 2).ToLower().Equals("en"))
            {
                fileName = fileName.Substring(0, fileName.Length - 2);
            }
            return Path.Combine(dirName, fileName + suffix + ".exe");
        }

        /// <summary>
        ///     ゲームの種類を判別する
        /// </summary>
        /// <param name="fileName">実行ファイルのパス名</param>
        public static void DetectGameType(string fileName)
        {
            string singleFileName = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrEmpty(singleFileName))
            {
                GameType = GameType.Unknown;
                return;
            }

            string gameName = singleFileName.ToLower();
            if (gameName.Equals("crusaders") || gameName.Equals("crusadersen"))
            {
                GameType = GameType.CrusaderKings;
                return;
            }
            if (gameName.Equals("eu2") || gameName.Equals("eu2en"))
            {
                GameType = GameType.EuropaUniversalis2;
                return;
            }
            if (gameName.Equals("ftg") || gameName.Equals("ftgen"))
            {
                GameType = GameType.ForTheGlory;
                return;
            }
            if (gameName.Equals("victoria") || gameName.Equals("victoriaen"))
            {
                GameType = GameType.Victoria;
                return;
            }
            if (gameName.Equals("hoi") || gameName.Equals("hoien"))
            {
                GameType = GameType.HeartsOfIron;
                return;
            }
            if (gameName.Equals("hoi2") || gameName.Equals("hoi2en"))
            {
                GameType = GameType.HeartsOfIron2;
                return;
            }
            if (gameName.Equals("aodgame") || gameName.Equals("aodgameen"))
            {
                GameType = GameType.ArsenalOfDemocracy;
                return;
            }
            if (gameName.Equals("darkest hour") || gameName.Equals("darkest houren"))
            {
                GameType = GameType.DarkestHour;
                return;
            }

            GameType = GameType.Unknown;
        }

        /// <summary>
        ///     ゲームの種類を設定する
        /// </summary>
        /// <param name="index">ゲームの種類のインデックス値</param>
        public static void SetGameType(int index)
        {
            switch (index)
            {
                case 0: // 不明
                    GameType = GameType.Unknown;
                    return;

                case 1: // Crusader Kings
                    GameType = GameType.CrusaderKings;
                    return;

                case 2: // Europa Universalis 2
                    GameType = GameType.EuropaUniversalis2;
                    return;

                case 3: // For the Glory
                    GameType = GameType.ForTheGlory;
                    return;

                case 4: // Victoria
                    GameType = GameType.Victoria;
                    return;

                case 5: // Hearts of Iron
                    GameType = GameType.HeartsOfIron;
                    return;

                case 6: // Hearts of Iron 2
                    GameType = GameType.HeartsOfIron2;
                    return;

                case 7: // Arsenal of Democracy
                    GameType = GameType.ArsenalOfDemocracy;
                    return;

                case 8: // Darkest Hour
                    GameType = GameType.DarkestHour;
                    return;
            }

            GameType = GameType.Unknown;
        }

        /// <summary>
        ///     ゲームの種類のインデックス値を取得する
        /// </summary>
        /// <returns>ゲームの種類のインデックス値</returns>
        public static int GetGameIndex()
        {
            switch (GameType)
            {
                case GameType.Unknown: // 不明
                    return 0;

                case GameType.CrusaderKings:
                    return 1;

                case GameType.EuropaUniversalis2:
                    return 2;

                case GameType.ForTheGlory:
                    return 3;

                case GameType.Victoria:
                    return 4;

                case GameType.HeartsOfIron:
                    return 5;

                case GameType.HeartsOfIron2:
                    return 6;

                case GameType.ArsenalOfDemocracy:
                    return 7;

                case GameType.DarkestHour:
                    return 8;

                default:
                    return 0;
            }
        }

        /// <summary>
        ///     テキスト自動折り返しの設定が有効かを取得する
        /// </summary>
        /// <returns>テキスト自動折り返しの設定が有効ならばtrueを返す</returns>
        public static bool GetAutoLineBreakEffective()
        {
            switch (GameType)
            {
                case GameType.EuropaUniversalis2:
                case GameType.ForTheGlory:
                case GameType.CrusaderKings:
                case GameType.Victoria:
                case GameType.HeartsOfIron:
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                case GameType.DarkestHour:
                    return true;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     語順変更の設定が有効かを取得する
        /// </summary>
        /// <returns>語順変更の設定が有効ならばtrueを返す</returns>
        public static bool GetWordOrderEffective()
        {
            switch (GameType)
            {
                case GameType.Victoria:
                case GameType.HeartsOfIron:
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                case GameType.DarkestHour:
                    return true;

                case GameType.CrusaderKings:
                case GameType.EuropaUniversalis2:
                case GameType.ForTheGlory:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     強制ウィンドウ化の設定が有効かを取得する
        /// </summary>
        /// <returns>強制ウィンドウ化の設定が有効ならばtrueを返す</returns>
        public static bool GetWindowedEffective()
        {
            switch (GameType)
            {
                case GameType.CrusaderKings:
                case GameType.EuropaUniversalis2:
                case GameType.Victoria:
                case GameType.HeartsOfIron:
                case GameType.HeartsOfIron2:
                    return true;

                case GameType.ForTheGlory:
                case GameType.ArsenalOfDemocracy:
                case GameType.DarkestHour:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     テキスト自動折り返しのデフォルト値を取得する
        /// </summary>
        /// <returns>テキスト自動折り返しのデフォルト値</returns>
        public static bool GetAutoLineBreakDefault()
        {
            return true;
        }

        /// <summary>
        ///     語順変更のデフォルト値を取得する
        /// </summary>
        /// <returns>語順変更のデフォルト値</returns>
        public static bool GetWordOrderDefault()
        {
            switch (GameType)
            {
                case GameType.DarkestHour:
                    return true;

                case GameType.Victoria:
                case GameType.HeartsOfIron:
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        ///     強制ウィンドウ化のデフォルト値を取得する
        /// </summary>
        /// <returns>強制ウィンドウ化のデフォルト値</returns>
        public static bool GetWindowedDefault()
        {
            return false;
        }

        #endregion
    }

    /// <summary>
    ///     ゲームの種類
    /// </summary>
    public enum GameType
    {
        Unknown, // 不明
        CrusaderKings, // Crusader Kings
        EuropaUniversalis2, // Europa Universalis 2
        ForTheGlory, // For The Glory
        Victoria, // Victoria
        HeartsOfIron, // Hearts of Iron
        HeartsOfIron2, // Hearts of Iron 2 1.3-
        ArsenalOfDemocracy, // Arsenal of Democracy 1.08-
        DarkestHour, // Darkest Hour 1.03-
    }
}