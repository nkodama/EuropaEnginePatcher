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
        }

        #endregion

        #region パッチ処理

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
        public static void Save()
        {
            if (!File.Exists(TargetFileName))
            {
                return;
            }

            string jpFileName;
            if (RenameOriginal)
            {
                var dialog = new SaveFileDialog
                    {
                        FileName = Path.GetFileNameWithoutExtension(TargetFileName) + "En",
                        DefaultExt = "exe",
                        Filter = "実行ファイル (*.exe)|*.exe",
                        InitialDirectory = Path.GetDirectoryName(TargetFileName),
                        OverwritePrompt = true,
                        Title = "元のファイルをリネームして保存"
                    };
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                string enFileName = dialog.FileName;
                jpFileName = Path.Combine(Path.GetDirectoryName(enFileName) ?? Environment.CurrentDirectory,
                                          Path.GetFileName(TargetFileName) ?? "HoI2.exe");
                if (File.Exists(enFileName))
                {
                    File.Delete(enFileName);
                }
                if (File.Exists(jpFileName))
                {
                    File.Move(jpFileName, enFileName);
                }
            }
            else
            {
                var dialog = new SaveFileDialog
                    {
                        FileName = Path.GetFileNameWithoutExtension(TargetFileName) + "Jp",
                        DefaultExt = "exe",
                        Filter = "実行ファイル (*.exe)|*.exe",
                        InitialDirectory = Path.GetDirectoryName(TargetFileName),
                        OverwritePrompt = true,
                        Title = "パッチを当てたファイルを保存"
                    };
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                jpFileName = dialog.FileName;
                if (File.Exists(jpFileName))
                {
                    File.Delete(jpFileName);
                }
            }
            PatchEngine.SavePatchedFile(jpFileName);
            CopyDll(Path.GetDirectoryName(jpFileName));
        }

        /// <summary>
        ///     _inmm.dllをコピーする
        /// </summary>
        /// <param name="dirName">対象ディレクトリ名</param>
        private static void CopyDll(string dirName)
        {
            string destName = Path.Combine(dirName, "_inmm.dll");
            string srcName = Path.Combine(Environment.CurrentDirectory, "_inmm.dll");
            if (File.Exists(srcName))
            {
                if (File.Exists(destName))
                {
                    if (!File.GetLastWriteTimeUtc(destName).Equals(File.GetLastWriteTimeUtc(srcName)))
                    {
                        if (MessageBox.Show("_inmm.dllのバージョンが一致しません。\n上書きコピーしてもよろしいですか？",
                                            "日本語化DLLのコピー", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            File.Copy(srcName, destName, true);
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("_inmm.dllがありません。\nコピーしてもよろしいですか？",
                                        "日本語化DLLのコピー", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                        DialogResult.Yes)
                    {
                        File.Copy(srcName, destName);
                    }
                }
            }
        }

        #endregion

        #region ゲーム種類判別

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
            if (gameName.Equals("crusaders"))
            {
                GameType = GameType.CrusaderKings;
                return;
            }
            if (gameName.Equals("eu2"))
            {
                GameType = GameType.EuropaUniversalis2;
                return;
            }
            if (gameName.Equals("ftg"))
            {
                GameType = GameType.ForTheGlory;
                return;
            }
            if (gameName.Equals("victoria"))
            {
                GameType = GameType.Victoria;
                return;
            }
            if (gameName.Equals("hoi"))
            {
                GameType = GameType.HeartsOfIron;
                return;
            }
            if (gameName.Equals("hoi2"))
            {
                GameType = GameType.HeartsOfIron2;
                return;
            }
            if (gameName.Equals("aodgame"))
            {
                GameType = GameType.ArsenalOfDemocracy;
                return;
            }
            if (gameName.Equals("darkest hour"))
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
        ///     テキスト自動折り返しのデフォルト値を取得する
        /// </summary>
        /// <returns>テキスト自動折り返しのデフォルト値</returns>
        public static bool GetAutoLineBreakDefault()
        {
            switch (GameType)
            {
                case GameType.CrusaderKings:
                case GameType.EuropaUniversalis2:
                case GameType.ForTheGlory:
                case GameType.Victoria:
                case GameType.HeartsOfIron:
                case GameType.HeartsOfIron2:
                case GameType.ArsenalOfDemocracy:
                case GameType.DarkestHour:
                    return true;

                default:
                    return false;
            }
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