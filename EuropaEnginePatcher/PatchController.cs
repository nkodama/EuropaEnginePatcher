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
        private static GameType GameType { get; set; }

        /// <summary>
        ///     自動処理モード
        /// </summary>
        public static bool AutoMode { get; set; }

        /// <summary>
        ///     元のファイルをリネーム
        /// </summary>
        public static bool RenameOriginal { private get; set; }

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

        /// <summary>
        ///     イントロスキップ
        /// </summary>
        public static bool IntroSkip { get; set; }

        /// <summary>
        ///     時間制限解除
        /// </summary>
        public static bool Ntl { get; set; }

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
            IntroSkip = false;
            Ntl = false;
        }

        #endregion

        #region 自動処理

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
                MessageBox.Show("パッチの適用に失敗しました。", "Europa Engine Patcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            MessageBox.Show("パッチの適用に成功しました。", "Europa Engine Patcher", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;
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

        #endregion

        #region ファイル保存

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
                    string enFileName = AutoMode ? GetExeFileName(TargetFileName, "En") : QueryOriginalFileName();
                    SaveOriginalFile(enFileName);
                    jpFileName = GetExeFileName(TargetFileName, "");
                }
                else
                {
                    jpFileName = AutoMode ? GetExeFileName(TargetFileName, "Jp") : QueryPatchedFileName();
                }

                return SavePatchedFiIle(jpFileName);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "パッチを当てたファイルの保存に失敗しました。\n" +
                    "おそらくWindowsのユーザーアカウント制御(UAC)が原因です。\n" +
                    "ゲームフォルダを「C:\\Program Files」「C:\\Program Files (x86)」以外の場所へ" +
                    "丸ごとコピーしてから実行して下さい。",
                    "ファイルの保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        ///     元のファイルをリネームする先を問い合わせる
        /// </summary>
        /// <returns>リネーム先のファイル名</returns>
        private static string QueryOriginalFileName()
        {
            string fileName = GetExeFileName(TargetFileName, "En");
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(fileName),
                DefaultExt = "exe",
                Filter = "実行ファイル (*.exe)|*.exe",
                InitialDirectory = Path.GetDirectoryName(fileName),
                OverwritePrompt = true,
                Title = "元のファイルの名前を変えて保存"
            };
            return dialog.ShowDialog() != DialogResult.Cancel ? dialog.FileName : null;
        }

        /// <summary>
        ///     パッチを当てたファイルを保存する先を問い合わせる
        /// </summary>
        /// <returns>保存先のファイル名</returns>
        private static string QueryPatchedFileName()
        {
            string fileName = GetExeFileName(TargetFileName, "Jp");
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(fileName),
                DefaultExt = "exe",
                Filter = "実行ファイル (*.exe)|*.exe",
                InitialDirectory = Path.GetDirectoryName(fileName),
                OverwritePrompt = true,
                Title = "パッチを当てたファイルを保存"
            };
            return dialog.ShowDialog() != DialogResult.Cancel ? dialog.FileName : null;
        }

        /// <summary>
        ///     元のファイルをリネームして保存する
        /// </summary>
        /// <param name="fileName">リネーム先のファイル名</param>
        private static void SaveOriginalFile(string fileName)
        {
            // ファイル名が空ならば何もしない
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            // リネーム先のファイル名と元のファイル名が同じならば何もしない
            if (Path.GetFullPath(fileName).Equals(Path.GetFullPath(TargetFileName)))
            {
                return;
            }
            // リネーム先にファイルがあれば削除する
            if (File.Exists(fileName))
            {
                // リネーム先のファイルのタイムスタンプと元のファイルのタイムスタンプが同じならば何もしない
                if (File.GetLastWriteTimeUtc(fileName).Equals(File.GetLastWriteTimeUtc(TargetFileName)))
                {
                    return;
                }
                // 自動処理モードでなければ既に上書き確認しているので問い合わせない
                if (AutoMode)
                {
                    if (MessageBox.Show(
                        "元のファイルの退避先に他のファイルがあります。\n" +
                        "上書きしてもよろしいですか？",
                        "元のファイルの名前を変えて保存", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                File.Delete(fileName);
            }
            // 元のファイルを移動する
            if (File.Exists(TargetFileName))
            {
                File.Move(TargetFileName, fileName);
            }
        }

        /// <summary>
        ///     パッチを当てたファイルを保存する
        /// </summary>
        /// <param name="fileName">保存先のファイル名</param>
        /// <returns>保存に成功すればtrueを返す</returns>
        private static bool SavePatchedFiIle(string fileName)
        {
            // ファイル名が空ならば何もしない
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }
            // 自動処理モードで保存先にファイルがあれば上書き確認する
            if (File.Exists(fileName) && AutoMode)
            {
                if (MessageBox.Show(
                    "パッチを当てたファイルの保存先に他のファイルがあります。\n" +
                    "上書きしてもよろしいですか？",
                    "パッチを当てたファイルを保存", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }
            }
            // パッチを当てたファイルを保存する
            PatchEngine.SavePatchedFile(fileName);
            return true;
        }

        #endregion

        #region DLLのコピー

        /// <summary>
        ///     _inmm.dllが存在するかチェックする
        /// </summary>
        /// <returns>_inmm.dllが存在しなければfalseを返す</returns>
        public static bool CheckDll()
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, "_inmm.dll");
            if (!File.Exists(fileName))
            {
                MessageBox.Show(
                    "日本語化DLLが見つかりません。\n" +
                    "おそらく圧縮ファイルを展開していないのが原因です。\n" +
                    "先に圧縮ファイルを展開してから実行して下さい。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("日本語化DLLが見つかりません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (File.Exists(destName))
                {
                    if (!File.GetLastWriteTimeUtc(destName).Equals(File.GetLastWriteTimeUtc(srcName)))
                    {
                        if (!AutoMode)
                        {
                            if (MessageBox.Show("日本語化DLLを上書きコピーしてもよろしいですか？",
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
                        if (MessageBox.Show("ゲームフォルダに日本語化DLLがありません。\nコピーしてもよろしいですか？",
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

        #region コマンドライン引数

        /// <summary>
        ///     コマンドライン引数を解釈する
        /// </summary>
        /// <returns>コマンドラインモードならばtrueを返す</returns>
        public static bool ParseCommandLine()
        {
            string[] args = Environment.GetCommandLineArgs();

            // 引数指定のない場合は通常起動する
            if (args.Length <= 1)
            {
                return false;
            }

            string pathName = args[1];
            if (File.Exists(pathName))
            {
                DetectGameType(pathName);
                if (GameType == GameType.Unknown)
                {
                    return false;
                }
                TargetFileName = pathName;
                return true;
            }

            if (Directory.Exists(pathName))
            {
                string fileName = DetectExeFileName(pathName);
                if (fileName.Equals(pathName))
                {
                    return false;
                }
                TargetFileName = fileName;
                return true;
            }

            return false;
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
        ///     イントロスキップの設定が有効かを取得する
        /// </summary>
        /// <returns>イントロスキップの設定が有効ならばtrueを返す</returns>
        public static bool GetIntroSkipEffective()
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
        ///     時間制限解除の設定が有効かを取得する
        /// </summary>
        /// <returns>イントロスキップの設定が有効ならばtrueを返す</returns>
        public static bool GetNtlEffective()
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
        DarkestHour // Darkest Hour 1.03-
    }
}