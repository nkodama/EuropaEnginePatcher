using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
    /// <summary>
    ///     パッチエンジン
    /// </summary>
    public static class PatchEngine
    {
        #region メンバ変数

        private static uint _addrBase; // ベースアドレス
        private static uint _addrCalcLineBreak; // CalcLineBreakの埋め込みアドレス
        private static uint _addrDataSection; // .dataセクションの開始アドレス
        private static uint _addrEeMaxAmphibModTitle; // EE_MAX_AMPHIB_MOD_TITLEの埋め込みアドレス
        private static uint _addrGetModuleHandleA; // GetModuleHandleAの呼び出しアドレス
        private static uint _addrGetProcAddress; // GetProcAddressの呼び出しアドレス
        private static uint _addrGetTextWidth; // GetTextWidthの埋め込みアドレス
        private static uint _addrIdataSection; // .idataセクションの開始アドレス
        private static uint _addrIsDebuggerPresent; // IsDebuggerPresentの呼び出しアドレス
        private static uint _addrRankingSuffix; // RANKING_SUFFIXの埋め込みアドレス
        private static uint _addrRdataSection; // .rdataセクションの開始アドレス
        private static uint _addrStrNLen; // strnlen0の埋め込みアドレス
        private static uint _addrTextOutDc; // TextOutDC0/1/2の埋め込みアドレス
        private static uint _addrTextSection; // .textセクションの開始アドレス
        private static uint _addrVarCalcLineBreakAddress; // varCalcLineBreakの割り当てアドレス
        private static uint _addrVarGetTextWidthAddress; // varGetTextWidthAddressの割り当てアドレス
        private static uint _addrVarStrNLenAddress; // varStrNLenAddressの割り当てアドレス
        private static uint _addrVarTextOutDcAddress; // varTextOutDC0/1/2Addressの割り当てアドレス
        private static uint _addrWinMmDll; // WINMM.DLLの定義アドレス
        private static uint _alignSection; // セクションアラインメント
        private static uint _countImportDir; // インポートディレクトリの数
        private static uint _countSections; // セクションの数
        private static byte[] _data;
        private static long _fileSize;
        private static int _gameVersion; // ゲームバージョン
        private static PatchType _patchType; // パッチの種類
        private static uint _posArmyNameFormat; // 軍団名の書式の定義位置
        private static uint _posCalcLineBreak; // CalcLineBreakのファイル上の埋め込み位置
        private static uint _posCalcLineBreakEnd1; // CalcLineBreakの処理後のジャンプ位置1
        private static uint _posCalcLineBreakEnd2; // CalcLineBreakの処理後のジャンプ位置2
        private static uint _posCalcLineBreakEnd3; // CalcLineBreakの処理後のジャンプ位置3
        private static uint _posCalcLineBreakEnd4; // CalcLineBreakの処理後のジャンプ位置4
        private static uint _posCalcLineBreakEnd5; // CalcLineBreakの処理後のジャンプ位置5
        private static uint _posCalcLineBreakEnd6; // CalcLineBreakの処理後のジャンプ位置6
        private static uint _posCalcLineBreakStart1; // CalcLineBreakの処理へジャンプする位置1
        private static uint _posCalcLineBreakStart2; // CalcLineBreakの処理へジャンプする位置2
        private static uint _posCalcLineBreakStart3; // CalcLineBreakの処理へジャンプする位置3
        private static uint _posCalcLineBreakStart4; // CalcLineBreakの処理へジャンプする位置4
        private static uint _posCalcLineBreakStart5; // CalcLineBreakの処理へジャンプする位置5
        private static uint _posCalcLineBreakStart6; // CalcLineBreakの処理へジャンプする位置6
        private static uint _posDataSection; // .dataセクションのファイル上の開始位置
        private static uint _posDivisionNameFormat; // 師団名の書式の定義位置
        private static uint _posEeMaxAmphibModTitle; // EE_MAX_AMPHIB_MOD_TITLEのファイル上の埋め込み位置
        private static uint _posGetArmyName1; // 軍団名取得処理の位置1
        private static uint _posGetArmyName2; // 軍団名取得処理の位置2
        private static uint _posGetDivisionName1; // 師団名取得処理の位置1
        private static uint _posGetDivisionName2; // 師団名取得処理の位置2
        private static uint _posGetRankingName1; // 国家序列取得処理の位置1
        private static uint _posGetRankingName2; // 国家序列取得処理の位置2
        private static uint _posGetTextWidth; // GetTextWidthのファイル上の埋め込み位置
        private static uint _posGetTextWidthEnd; // GetTextWidthの処理後のジャンプ位置
        private static uint _posGetTextWidthStart; // GetTextWidthの処理開始位置
        private static uint _posIdataSection; // .idataセクションのファイル上の開始位置
        private static uint _posImportSection; // インポートセクションのファイル上の位置
        private static uint _posImportTable; // インポートテーブルのファイル上の位置
        private static uint _posIsDebuggerPresent; // IsDebuggerPresentのファイル上の定義位置
        private static uint _posLatinToUpper; // Latin文字の大文字化処理の位置
        private static uint _posPeHeader; // PEヘッダのファイル上の位置
        private static uint _posPushEeMaxAmphibModTitle; // push EE_MAX_AMPHIB_MOD_TITLEの位置
        private static uint _posRankingSuffix; // RANKING_SUFFIXのファイル上の埋め込み位置
        private static uint _posRdataSection; // .rdataセクションのファイル上の開始位置
        private static uint _posStrNLen; // strnlen0のファイル上の埋め込み位置
        private static uint _posTermModelNameStart1; // モデル名の終端文字設定位置1
        private static uint _posTermModelNameStart2; // モデル名の終端文字設定位置2
        private static uint _posTermModelNameStart3; // モデル名の終端文字設定位置3
        private static uint _posTermModelNameStart4; // モデル名の終端文字設定位置4
        private static uint _posTermModelNameStart5; // モデル名の終端文字設定位置5
        private static uint _posTermModelNameStart6; // モデル名の終端文字設定位置6
        private static uint _posTermModelNameStart7; // モデル名の終端文字設定位置7
        private static uint _posTermModelNameStart8; // モデル名の終端文字設定位置8
        private static uint _posTermModelNameStart9; // モデル名の終端文字設定位置9
        private static uint _posTermModelNameStart10; // モデル名の終端文字設定位置10
        private static uint _posTextOutDc; // TextOutDC0/1/2のファイル上の埋め込み位置
        private static uint _posTextOutDcFree; // TextOutの埋め込み位置後の空き領域
        private static uint _posTextOutEnd; // TextOutの処理後のジャンプ位置
        private static uint _posTextOutStart; // TextOutの処理開始位置
        private static uint _posTextSection; // .textセクションのファイル上の開始位置
        private static uint _posWinMmDll; // WINMM.DLLのファイル上の定義位置
        private static uint _rvaImportSection; // インポートセクションのRVA
        private static uint _rvaImportTable; // インポートテーブルのアドレス
        private static uint _sizeDataSection; // .dataセクションのサイズ
        private static uint _sizeRdataFree; // .rdataセクションの空きサイズ
        private static uint _sizeRdataSection; // .rdataセクションのサイズ
        private static uint _sizeTextFree; // .textセクションの空きサイズ
        private static uint _sizeTextSection; // .textセクションのサイズ
        private static uint _posChatBlockChar1; // チャットウィンドウの特殊文字ブロック処理の位置1
        private static uint _posChatBlockChar2; // チャットウィンドウの特殊文字ブロック処理の位置2
        private static uint _posChatBlockChar3; // チャットウィンドウの特殊文字ブロック処理の位置3
        private static uint _posWindowed1; // 強制ウィンドウ化処理の位置1
        private static uint _posWindowed2; // 強制ウィンドウ化処理の位置2
        private static uint _posBinkPlay1; // binkplay.exeの定義位置1
        private static uint _posBinkPlay2; // binkplay.exeの定義位置2
        private static uint _posMinYear1; // 最小年次の定義位置1
        private static uint _posMinYear2; // 最小年次の定義位置2
        private static uint _posMaxYear1; // 最大年次の定義位置1
        private static uint _posMaxYear2; // 最大年次の定義位置2
        private static uint _posLimitYear; // 制限年次の定義位置

        #endregion

        #region メイン処理

        /// <summary>
        ///     実行ファイルにパッチを当てる
        /// </summary>
        /// <param name="fileName">実行ファイルのパス名</param>
        /// <param name="gameType">ゲームの種類</param>
        /// <returns>パッチ当てに成功すればtrueを返す</returns>
        public static bool Patch(string fileName, GameType gameType)
        {
            AppendLog($"Europe Engine Patcher Ver {EuropaEnginePatcher.VersionName}\n");

            SetPatchType(gameType);
            if (_patchType == PatchType.Unknown)
            {
                return false;
            }

            try
            {
                ReadGameFile(fileName);

                if (IsPatchedFile())
                {
                    MessageBox.Show("既にパッチ済みのファイルです。\nパッチを当てる前のファイルを指定して下さい。", "エラー", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                if (!IdentifyGameVersion())
                {
                    MessageBox.Show("ゲームのバージョンが判別できません。\nゲーム本体の実行ファイルを指定して下さい。", "エラー", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return false;
                }
                if (!ParseHeader())
                {
                    MessageBox.Show("パッチの適用に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!ScanPatchLocation())
                {
                    MessageBox.Show("パッチの適用に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                PatchBinary();
            }
            catch (Exception)
            {
                MessageBox.Show("パッチの適用に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            AppendLog("PatchBinary passed\n");

            return true;
        }

        /// <summary>
        ///     パッチ後の実行ファイルを保存する
        /// </summary>
        /// <param name="fileName">保存するファイルのパス名</param>
        public static void SavePatchedFile(string fileName)
        {
            using (FileStream s = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                s.Write(_data, 0, _data.Length);
                s.Close();
            }
        }

        /// <summary>
        ///     実行ファイルを読み込む
        /// </summary>
        /// <param name="fileName">実行ファイルのパス名</param>
        private static void ReadGameFile(string fileName)
        {
            AppendLog("Phase 1 - ファイルの読み込み\n");

            FileInfo info = new FileInfo(fileName);
            _fileSize = info.Length;

            _data = new byte[_fileSize];

            using (FileStream s = info.OpenRead())
            {
                s.Read(_data, 0, (int) _fileSize);
                s.Close();
            }

            AppendLog($"  TargetFile: {fileName}\n");
            AppendLog($"  FileSize: {_fileSize}Bytes\n");

            AppendLog("Phase 1 passed\n\n");
        }

        #endregion

        #region パッチ種類判別

        /// <summary>
        ///     パッチの種類を設定する
        /// </summary>
        /// <param name="gameType">ゲームの種類</param>
        private static void SetPatchType(GameType gameType)
        {
            switch (gameType)
            {
                case GameType.Unknown: // 不明
                    _patchType = PatchType.Unknown;
                    AppendLog("PatchType: Unknown\n\n");
                    break;

                case GameType.CrusaderKings:
                    _patchType = PatchType.CrusaderKings;
                    AppendLog("PatchType: Crusader Kings\n\n");
                    break;

                case GameType.EuropaUniversalis2:
                    _patchType = PatchType.EuropaUniversalis2;
                    AppendLog("PatchType: Europa Universalis 2\n\n");
                    break;

                case GameType.ForTheGlory:
                    _patchType = PatchType.ForTheGlory;
                    AppendLog("PatchType: For the Glory\n\n");
                    break;

                case GameType.Victoria:
                    _patchType = PatchType.Victoria;
                    AppendLog("PatchType: Victoria\n\n");
                    break;

                case GameType.HeartsOfIron:
                    _patchType = PatchType.HeartsOfIron;
                    AppendLog("PatchType: Hearts of Iron\n\n");
                    break;

                case GameType.HeartsOfIron2:
                    _patchType = PatchType.HeartsOfIron2;
                    AppendLog("PatchType: Hearts of Iron 2\n\n");
                    break;

                case GameType.ArsenalOfDemocracy:
                    _patchType = PatchType.ArsenalOfDemocracy;
                    AppendLog("PatchType: Arsenal of Democracy\n\n");
                    break;

                case GameType.DarkestHour:
                    _patchType = PatchType.DarkestHour;
                    AppendLog("PatchType: Darkest Hour\n\n");
                    break;

                default:
                    _patchType = PatchType.Unknown;
                    AppendLog("PatchType: Unknown\n\n");
                    break;
            }
        }

        /// <summary>
        ///     バージョンを判別する
        /// </summary>
        /// <returns>判別に成功すればtrueを返す</returns>
        private static bool IdentifyGameVersion()
        {
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.DarkestHour:
                    break;

                default:
                    // バージョン判別の必要ないゲームでは何もせずに戻る
                    return true;
            }

            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX バージョン文字列\"を探します\n");

            byte[] pattern;
            List<uint> l;
            uint offset;
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                    // Doomsday Armageddon v X.X
                    pattern = new byte[]
                    {
                        0x44, 0x6F, 0x6F, 0x6D, 0x73, 0x64, 0x61, 0x79,
                        0x20, 0x41, 0x72, 0x6D, 0x61, 0x67, 0x65, 0x64,
                        0x64, 0x6F, 0x6E, 0x20, 0x76, 0x20
                    };
                    l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                    if (l.Count == 0)
                    {
                        // Iron Cross Armageddon X.XX
                        pattern = new byte[]
                        {
                            0x49, 0x72, 0x6F, 0x6E, 0x20, 0x43, 0x72, 0x6F,
                            0x73, 0x73, 0x20, 0x41, 0x72, 0x6D, 0x61, 0x67,
                            0x65, 0x64, 0x64, 0x6F, 0x6E, 0x20
                        };
                        l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                        if (l.Count == 0)
                        {
                            return false;
                        }
                        offset = l[0] + (uint) pattern.Length;
                        _gameVersion = (_data[offset] - '0') * 100 + (_data[offset + 2] - '0') * 10 +
                                       (_data[offset + 3] - '0');
                    }
                    else
                    {
                        offset = l[0] + (uint) pattern.Length;
                        _gameVersion = (_data[offset] - '0') * 100 + (_data[offset + 2] - '0') * 10;
                    }
                    break;

                case PatchType.ArsenalOfDemocracy:
                    // Arsenal of Democracy X.XX
                    pattern = new byte[]
                    {
                        0x41, 0x72, 0x73, 0x65, 0x6E, 0x61, 0x6C, 0x20,
                        0x6F, 0x66, 0x20, 0x44, 0x65, 0x6D, 0x6F, 0x63,
                        0x72, 0x61, 0x63, 0x79, 0x20
                    };
                    l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                    if (l.Count == 0)
                    {
                        // Arsenal Of Democracy v X.XX
                        pattern = new byte[]
                        {
                            0x41, 0x72, 0x73, 0x65, 0x6E, 0x61, 0x6C, 0x20,
                            0x4F, 0x66, 0x20, 0x44, 0x65, 0x6D, 0x6F, 0x63,
                            0x72, 0x61, 0x63, 0x79, 0x20, 0x76, 0x20
                        };
                        l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                        if (l.Count == 0)
                        {
                            return false;
                        }
                    }
                    offset = l[0] + (uint) pattern.Length;
                    _gameVersion = (_data[offset] - '0') * 100 + (_data[offset + 2] - '0') * 10 +
                                   (_data[offset + 3] - '0');
                    break;

                case PatchType.DarkestHour:
                    // Darkest Hour v X.XX
                    pattern = new byte[]
                    {
                        0x44, 0x61, 0x72, 0x6B, 0x65, 0x73, 0x74, 0x20,
                        0x48, 0x6F, 0x75, 0x72, 0x20, 0x76, 0x20
                    };
                    l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    offset = l[0] + (uint) pattern.Length;
                    _gameVersion = (_data[offset] - '0') * 100 + (_data[offset + 2] - '0') * 10 +
                                   (_data[offset + 3] - '0');
                    break;

                default:
                    // Doomsday Armageddon v X.X
                    pattern = new byte[]
                    {
                        0x44, 0x6F, 0x6F, 0x6D, 0x73, 0x64, 0x61, 0x79,
                        0x20, 0x41, 0x72, 0x6D, 0x61, 0x67, 0x65, 0x64,
                        0x64, 0x6F, 0x6E, 0x20, 0x76, 0x20
                    };
                    l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    offset = l[0] + (uint) pattern.Length;
                    _gameVersion = (_data[offset] - '0') * 100 + (_data[offset + 2] - '0') * 10;
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                    if (_gameVersion < 110)
                    {
                        _patchType = PatchType.IronCrossHoI2;
                        AppendLog("PatchType: Iron Cross / Hearts of Iron 2\n\n");
                    }
                    else if (_gameVersion <= 120)
                    {
                        _patchType = PatchType.HeartsOfIron212;
                        AppendLog("PatchType: Hearts of Iron 2 1.2\n\n");
                    }
                    else
                    {
                        _patchType = PatchType.HeartsOfIron2;
                        AppendLog("PatchType: Hearts of Iron 2\n\n");
                    }
                    break;

                case PatchType.ArsenalOfDemocracy:
                    if (_gameVersion <= 104)
                    {
                        _patchType = PatchType.ArsenalOfDemocracy104;
                        AppendLog("PatchType: Arsenal of Democracy 1.04\n\n");
                    }
                    else if (_gameVersion <= 107)
                    {
                        _patchType = PatchType.ArsenalOfDemocracy107;
                        AppendLog("PatchType: Arsenal of Democracy 1.07\n\n");
                    }
                    else if (_gameVersion <= 109)
                    {
                        _patchType = PatchType.ArsenalOfDemocracy109;
                        AppendLog("PatchType: Arsenal of Democracy 1.09\n\n");
                    }
                    else
                    {
                        _patchType = PatchType.ArsenalOfDemocracy;
                        AppendLog("PatchType: Arsenal of Democracy\n\n");
                    }
                    break;

                case PatchType.DarkestHour:
                    if (_gameVersion == 105)
                    {
                        _patchType = PatchType.DarkestHour105;
                        AppendLog("PatchType: Darkest Hour 1.05\n\n");
                    } else 
                    if (_gameVersion <= 102)
                    {
                        _patchType = PatchType.DarkestHour102;
                        AppendLog("PatchType: Darkest Hour 1.02\n\n");
                    }
                    else
                    {
                        _patchType = PatchType.DarkestHour;
                        AppendLog("PatchType: Darkest Hour\n\n");
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        ///     既にパッチ済みのファイルかどうかを判定する
        /// </summary>
        /// <returns>パッチ済みのファイルならばtrueを返す</returns>
        private static bool IsPatchedFile()
        {
            AppendLog("ScanBinary - パッチ済みファイルの判別\n");
            AppendLog("  \"_INMM.dll\"を探します\n");
            // _INMM.DLLが見つかればパッチ済みだとみなす
            byte[] pattern =
            {
                0x5F, 0x49, 0x4E, 0x4D, 0x4D, 0x2E, 0x64, 0x6C,
                0x6C
            };
            List<uint> l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
            if (l.Count > 0)
            {
                return true;
            }

            AppendLog("ScanBinary passed\n\n");
            return false;
        }

        #endregion

        #region ヘッダ解析

        /// <summary>
        ///     ヘッダを解析する
        /// </summary>
        /// <returns>解析に成功すればtrueを返す</returns>
        private static bool ParseHeader()
        {
            if (!ParseExeHeader())
            {
                return false;
            }
            if (!ParsePeHeader())
            {
                return false;
            }
            if (!ParseOptionHeader())
            {
                return false;
            }
            if (!ParseSection())
            {
                return false;
            }
            ParseImportTable();

            return true;
        }

        /// <summary>
        ///     EXEヘッダを解析する
        /// </summary>
        /// <returns>解析に成功すればtrueを返す</returns>
        private static bool ParseExeHeader()
        {
            AppendLog("Phase 2 - 実行ファイルの確認\n");

            if (GetWord(0) != 0x5A4D)
            {
                return false;
            }
            AppendLog("  exe識別子: $5A4D\n");

            _posPeHeader = GetLong(60);
            AppendLog($"  PEの位置: {_posPeHeader:X8}\n");

            AppendLog("Phase 2 passed\n\n");

            return true;
        }

        /// <summary>
        ///     PEヘッダを解析する
        /// </summary>
        /// <returns>解析に成功すればtrueを返す</returns>
        private static bool ParsePeHeader()
        {
            AppendLog("Phase 3 - PEヘッダの確認\n");

            if (GetWord(_posPeHeader) != 0x4550)
            {
                return false;
            }
            AppendLog("  PE識別子: $4550\n");

            _countSections = GetWord(_posPeHeader + 6);
            AppendLog($"  セクション数: {_countSections}\n");

            uint posSymbolTable = GetLong(_posPeHeader + 12);
            uint countSymbols = GetLong(_posPeHeader + 16);
            AppendLog($"  シンボルテーブルの位置/数: ${posSymbolTable:X8}/${countSymbols:X8}\n");

            uint sizeOptionHeader = GetWord(_posPeHeader + 20);
            AppendLog($"  オプションヘッダのサイズ: ${sizeOptionHeader:X4}\n");

            AppendLog("Phase 3 passed\n\n");

            return true;
        }

        /// <summary>
        ///     オプションヘッダを解析する
        /// </summary>
        /// <returns>解析に成功すればtrueを返す</returns>
        private static bool ParseOptionHeader()
        {
            AppendLog("Phase 4 - オプションヘッダの表示\n");

            uint offsetOptionHeader = _posPeHeader + 24;
            if (GetWord(offsetOptionHeader) != 0x010B)
            {
                return false;
            }
            AppendLog("  オプション部識別子: $010B\n");

            uint sizeCodeSection = GetLong(offsetOptionHeader + 4);
            AppendLog(string.Format("  コードセクションのサイズ: ${0:X8} ({0}Bytes)\n", sizeCodeSection));

            uint sizeInitializedData = GetLong(offsetOptionHeader + 8);
            AppendLog(string.Format("  初期化済みデータセクションのサイズ: ${0:X8} ({0}Bytes)\n", sizeInitializedData));

            uint sizeUninitializedData = GetLong(offsetOptionHeader + 12);
            AppendLog(string.Format("  未初期化データセクションのサイズ: ${0:X8} ({0}Bytes)\n", sizeUninitializedData));

            uint addressEntryPoint = GetLong(offsetOptionHeader + 16);
            AppendLog($"  エントリーポイント: ${addressEntryPoint:X8}\n");

            uint posCodeSection = GetLong(offsetOptionHeader + 20);
            AppendLog($"  コードセクションの位置: ${posCodeSection:X8}\n");

            uint posDataSectionHeader = GetLong(offsetOptionHeader + 24);
            AppendLog($"  データセクションの位置: ${posDataSectionHeader:X8}\n");

            _addrBase = GetLong(offsetOptionHeader + 28);
            AppendLog($"  ベースアドレス: ${_addrBase:X8}\n");

            _alignSection = GetLong(offsetOptionHeader + 32);
            AppendLog(string.Format("  セクション境界: ${0:X8} ({0}Bytes)\n", _alignSection));

            uint alignmentFile = GetLong(offsetOptionHeader + 36);
            AppendLog(string.Format("  ファイル境界: ${0:X8} ({0}Bytes)\n", alignmentFile));

            uint sizeImage = GetLong(offsetOptionHeader + 56);
            AppendLog($"  イメージサイズ: ${sizeImage:X8}\n");

            uint sizeHeader = GetLong(offsetOptionHeader + 60);
            AppendLog($"  ヘッダサイズ: ${sizeHeader:X8}\n");

            uint rvaExportTable = GetLong(offsetOptionHeader + 96);
            uint sizeExportTable = GetLong(offsetOptionHeader + 100);
            AppendLog($"  Export Table address/size: ${rvaExportTable:X8}/${sizeExportTable:X8}\n");

            _rvaImportTable = GetLong(offsetOptionHeader + 104);
            uint sizeImportTable = GetLong(offsetOptionHeader + 108);
            AppendLog($"  Import Table address/size: ${_rvaImportTable:X8}/${sizeImportTable:X8}\n");

            uint rvaResourceTable = GetLong(offsetOptionHeader + 112);
            uint sizeResourceTable = GetLong(offsetOptionHeader + 116);
            AppendLog($"  Resource Table address/size: ${rvaResourceTable:X8}/${sizeResourceTable:X8}\n");

            uint rvaImportAddressTable = GetLong(offsetOptionHeader + 192);
            uint sizeImportAddressTable = GetLong(offsetOptionHeader + 196);
            _countImportDir = sizeImportAddressTable / 0x14;
            AppendLog($"  Import Address Table address/size: ${rvaImportAddressTable:X8}/${sizeImportAddressTable:X8}\n");

            AppendLog(
                $"  * コード領域: ${_addrBase + posCodeSection:X8}～${_addrBase + posCodeSection + sizeCodeSection - 1:X8} (ファイル上: ${posCodeSection:X8}～${posCodeSection + sizeCodeSection - 1:X8})\n");
            AppendLog($"  * プログラムが開始されるアドレス: ${_addrBase + addressEntryPoint:X8}\n");

            AppendLog("Phase 4 passed\n\n");

            return true;
        }

        /// <summary>
        ///     セクションテーブルを解析する
        /// </summary>
        /// <returns>解析に成功すればtrueを返す</returns>
        private static bool ParseSection()
        {
            AppendLog("Phase 5 - セクションテーブルの表示");

            uint offsetSection = _posPeHeader + 24 + 224;

            for (int i = 0; i < _countSections; i++, offsetSection += 40)
            {
                AppendLog($"\n  セクション[{i}]\n");

                string nameSection = GetString(offsetSection, 8);
                AppendLog($"  セクション名: {nameSection}\n");

                uint virtualSize = GetLong(offsetSection + 8);
                AppendLog(string.Format("  セクション仮想サイズ: ${0:X8} ({0}Bytes)\n", virtualSize));

                uint virtualAddress = GetLong(offsetSection + 12);
                AppendLog($"  セクション仮想アドレス: ${virtualAddress:X8}\n");

                uint sizeRawData = GetLong(offsetSection + 16);
                AppendLog(string.Format("  セクション実サイズ: ${0:X8} ({0}Bytes)\n", sizeRawData));

                uint posRawData = GetLong(offsetSection + 20);
                AppendLog($"  セクション実位置: ${posRawData:X8}\n");

                if (nameSection.Equals(".text"))
                {
                    AppendLog(
                        $"  * 未使用領域: ${_addrBase + virtualAddress + virtualSize + 1:X8}～${_addrBase + virtualAddress + sizeRawData - 1:X8}\n");
                    _sizeTextSection = sizeRawData;
                    _sizeTextFree = sizeRawData - virtualSize;
                    _posTextSection = posRawData;
                    _addrTextSection = virtualAddress;
                }
                else if (nameSection.Equals(".rdata"))
                {
                    AppendLog(
                        $"  * 未使用領域: ${_addrBase + virtualAddress + virtualSize + 1:X8}～${_addrBase + virtualAddress + sizeRawData - 1:X8}\n");
                    _sizeRdataSection = sizeRawData;
                    _sizeRdataFree = sizeRawData - virtualSize;
                    _sizeDataSection = sizeRawData;
                    _posRdataSection = posRawData;
                    _addrRdataSection = virtualAddress;
                }
                else if (nameSection.Equals(".data"))
                {
                    uint addrDataSectionEnd = Ceiling(_addrBase + virtualAddress + virtualSize, _alignSection) - 1;
                    AppendLog(
                        $"  * 未使用領域: ${_addrBase + virtualAddress + virtualSize + 1:X8}～${addrDataSectionEnd:X8}\n");
                    _addrVarTextOutDcAddress = addrDataSectionEnd - 0x0000000F;
                    _addrVarGetTextWidthAddress = addrDataSectionEnd - 0x0000000B;
                    _addrVarCalcLineBreakAddress = addrDataSectionEnd - 0x00000007;
                    _addrVarStrNLenAddress = addrDataSectionEnd - 0x00000003;
                    _sizeDataSection = sizeRawData;
                    _posDataSection = posRawData;
                    _addrDataSection = virtualAddress;
                }
                else if (nameSection.Equals(".idata"))
                {
                    _posIdataSection = posRawData;
                    _addrIdataSection = virtualAddress;
                }
                if ((_rvaImportTable >= virtualAddress) && (_rvaImportTable < virtualAddress + virtualSize))
                {
                    _posImportTable = _rvaImportTable - virtualAddress + posRawData;
                    _rvaImportSection = virtualAddress;
                    _posImportSection = posRawData;
                }
            }

            // TextOutDC0/1/2/GetTextWidth/CalcLineBreak/EE_MAX_AMPHIB_MOD_TITLEの埋め込み位置算出
            int nReserveSize;
            switch (_patchType)
            {
                case PatchType.DarkestHour:
				case PatchType.DarkestHour105:
                case PatchType.DarkestHour102:
                    nReserveSize = 0x60;
                    break;

                case PatchType.Victoria:
                    nReserveSize = 0x50;
                    break;

                default:
                    nReserveSize = 0x40;
                    break;
            }

            if (_sizeTextFree >= nReserveSize)
            {
                uint posTextSectionEnd = _posTextSection + _sizeTextSection - 1;
                _posTextOutDc = posTextSectionEnd - 0x0000003F;
                _addrTextOutDc = GetTextAddress(_posTextOutDc);
                _posGetTextWidth = posTextSectionEnd - 0x0000002F;
                _addrGetTextWidth = GetTextAddress(_posGetTextWidth);
                _posCalcLineBreak = posTextSectionEnd - 0x0000001F;
                _addrCalcLineBreak = GetTextAddress(_posCalcLineBreak);
                _posStrNLen = posTextSectionEnd - 0x0000000F;
                _addrStrNLen = GetTextAddress(_posStrNLen);
                _posRankingSuffix = posTextSectionEnd - 0x0000004F;
                _addrRankingSuffix = GetTextAddress(_posRankingSuffix);
                _posEeMaxAmphibModTitle = posTextSectionEnd - 0x0000005F;
                _addrEeMaxAmphibModTitle = GetTextAddress(_posEeMaxAmphibModTitle);
            }
            else if (_sizeRdataFree >= nReserveSize)
            {
                uint posRdataSectionEnd = _posRdataSection + _sizeRdataSection - 1;
                _posTextOutDc = posRdataSectionEnd - 0x0000003F;
                _addrTextOutDc = GetRdataAddress(_posTextOutDc);
                _posGetTextWidth = posRdataSectionEnd - 0x0000002F;
                _addrGetTextWidth = GetRdataAddress(_posGetTextWidth);
                _posCalcLineBreak = posRdataSectionEnd - 0x0000001F;
                _addrCalcLineBreak = GetRdataAddress(_posCalcLineBreak);
                _posStrNLen = posRdataSectionEnd - 0x0000000F;
                _addrStrNLen = GetRdataAddress(_posStrNLen);
                _posRankingSuffix = posRdataSectionEnd - 0x0000004F;
                _addrRankingSuffix = GetRdataAddress(_posRankingSuffix);
                _posEeMaxAmphibModTitle = posRdataSectionEnd - 0x0000005F;
                _addrEeMaxAmphibModTitle = GetRdataAddress(_posEeMaxAmphibModTitle);
            }
            else
            {
                return false;
            }

            AppendLog("Phase 5 passed\n\n");

            return true;
        }

        /// <summary>
        ///     インポートディレクトリテーブルを解析する
        /// </summary>
        private static void ParseImportTable()
        {
            AppendLog("Phase 6 - インポートディレクトリテーブルの表示\n");
            AppendLog($"  インポートディレクトリテーブル数: {_countImportDir}\n");

            uint offsetImportDir = _posImportTable;
            uint rvaImportLookupTable = GetLong(offsetImportDir);
            while (rvaImportLookupTable != 0)
            {
                AppendLog($"\n  ImportLookupTableRVA: ${rvaImportLookupTable:X8}\n");

                uint timeStamp = GetLong(offsetImportDir + 4);
                AppendLog($"  TimeStamp: ${timeStamp:X8}\n");

                uint forwarderChain = GetLong(offsetImportDir + 8);
                AppendLog($"  FowarderChain: ${forwarderChain:X8}\n");

                uint rvaName = GetLong(offsetImportDir + 12);
                string name = GetString(_posImportSection + rvaName - _rvaImportSection, 0);
                AppendLog($"  NameRVA: ${rvaName:X8} ({name})\n");

                uint rvaImportAddressTable = GetLong(offsetImportDir + 16);
                AppendLog($"  ImportAddressTableRVA: ${rvaImportAddressTable:X8}\n");

                uint offsetImportLookupTable = _posImportSection + rvaImportLookupTable - _rvaImportSection;
                uint i = 0;
                uint entry = GetLong(offsetImportLookupTable);
                while (entry != 0)
                {
                    uint hint;
                    string importName;

                    if ((entry & 0x80000000) != 0)
                    {
                        hint = entry & 0x7FFFFFFF;
                        importName = "(Index)";
                    }
                    else
                    {
                        uint rvaHintNameTable = entry & 0x7FFFFFFF;
                        uint offsetHintNameTable = _posImportSection + rvaHintNameTable - _rvaImportSection;
                        hint = GetWord(offsetHintNameTable);
                        importName = GetString(offsetHintNameTable + 2, 0);

                        if (importName.Equals("GetModuleHandleA"))
                        {
                            _addrGetModuleHandleA = _addrBase + rvaImportAddressTable + i * 4;
                        }
                        else if (importName.Equals("GetProcAddress"))
                        {
                            _addrGetProcAddress = _addrBase + rvaImportAddressTable + i * 4;
                        }
                        else if (importName.Equals("IsDebuggerPresent"))
                        {
                            _addrIsDebuggerPresent = _addrBase + rvaImportAddressTable + i * 4;
                            _posIsDebuggerPresent = offsetHintNameTable + 2;
                        }
                    }
                    AppendLog($"    {hint:D4} {importName}\n");

                    offsetImportLookupTable += 4;

                    i++;
                    entry = GetLong(offsetImportLookupTable);
                }

                offsetImportDir += 0x14;

                rvaImportLookupTable = GetLong(offsetImportDir);
            }

            AppendLog("Phase 6 passed\n\n");
        }

        #endregion

        #region パッチ位置探索

        /// <summary>
        ///     パッチを当てる位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanPatchLocation()
        {
            // 日本語化コア部分
            if (!ScanWinMmDll())
            {
                return false;
            }
            if (!ScanTextOutStart())
            {
                return false;
            }
            if (!ScanTextOutEnd())
            {
                return false;
            }
            if (!ScanGetTextWidthStart())
            {
                return false;
            }
            if (!ScanGetTextWidthEnd())
            {
                return false;
            }
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                case PatchType.DarkestHour105:
                    if (!ScanLatinToUpper())
                    {
                        return false;
                    }
                    if (!ScanChatBlockChar())
                    {
                        return false;
                    }
                    break;
            }
            // テキスト自動折り返し
            if (PatchController.AutoLineBreak)
            {
                if (!ScanCalcLineBreakStart())
                {
                    return false;
                }
                if (!ScanCalcLineBreakEnd())
                {
                    return false;
                }
            }
            // 語順入れ替え
            if (PatchController.WordOrder)
            {
                switch (_patchType)
                {
                    case PatchType.Victoria:
                        if (!ScanGetDivisionName())
                        {
                            return false;
                        }
                        if (!ScanDivisionNameFormat())
                        {
                            return false;
                        }
                        if (!ScanGetArmyName())
                        {
                            return false;
                        }
                        if (!ScanArmyNameFormat())
                        {
                            return false;
                        }
                        if (!ScanGetRankingName())
                        {
                            return false;
                        }
                        break;

                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.ArsenalOfDemocracy109:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
                    case PatchType.DarkestHour105:
                        if (!ScanGetDivisionName())
                        {
                            return false;
                        }
                        if (!ScanDivisionNameFormat())
                        {
                            return false;
                        }
                        if (!ScanGetArmyName())
                        {
                            return false;
                        }
                        if (!ScanArmyNameFormat())
                        {
                            return false;
                        }
                        break;

                    case PatchType.ArsenalOfDemocracy:
                        if (!ScanGetDivisionName())
                        {
                            return false;
                        }
                        if (!ScanGetArmyName())
                        {
                            return false;
                        }
                        break;
                }
            }
            // 強制ウィンドウ化
            if (PatchController.Windowed)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                        if (!ScanWindowed())
                        {
                            return false;
                        }
                        break;
                    case PatchType.IronCrossHoI2:
                        // Iron Crossの場合は付属ツールでウィンドウ化できるのでパッチを当てない
                        break;
                }
            }
            // イントロスキップ
            if (PatchController.IntroSkip)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                        if (!ScanIntroSkip())
                        {
                            return false;
                        }
                        break;
                }
            }
            // 時間制限解除
            if (PatchController.Ntl)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                        if (!ScanNtl())
                        {
                            return false;
                        }
                        break;
                    case PatchType.IronCrossHoI2:
                        // Iron Crossの場合は初期状態で制限解除されているのでパッチを当てない
                        break;
                }
            }
            // ゲーム固有
            switch (_patchType)
            {
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                case PatchType.DarkestHour105:
                    if (!ScanEeMaxAmphibModTitle())
                    {
                        return false;
                    }
                    break;
            }
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                case PatchType.DarkestHour105:
                    if (!ScanTermModelName())
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        #endregion

        #region パッチ位置探索 - 日本語化コア部分

        /// <summary>
        ///     WINMM.dllの位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanWinMmDll()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%06 WINMM.dll\"を探します\n");
            byte[] pattern =
            {
                0x57, 0x49, 0x4E, 0x4D, 0x4D, 0x2E, 0x64, 0x6C,
                0x6C
            };
            List<uint> l = BinaryScan(_data, pattern, 0, (uint) _fileSize);
            if (l.Count == 0)
            {
                return false;
            }
            _posWinMmDll = l[0];
            _addrWinMmDll = _patchType == PatchType.ArsenalOfDemocracy104
                ? GetIdataAddress(_posWinMmDll)
                : GetRdataAddress(_posWinMmDll);
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     TextOutの処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanTextOutStart()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%07 TextOutの処理を埋め込む位置\"を探します\n");

            byte[] pattern;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0x78, 0x8B, 0x48, 0x0C, 0x89, 0x4D
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0x18, 0x8B, 0x48, 0x0C, 0x89, 0x4D
                    };
                    break;
            }
            List<uint> l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posTextOutStart = l[0];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     TextOutの処理が終了したらジャンプさせる位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanTextOutEnd()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%08 TextOutの処理が終了したらジャンプさせる位置\"を探します\n");

            byte[] pattern;
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x18, 0x8B, 0x11, 0x8B, 0x4D, 0x18,
                        0xFF, 0x52, 0x0C, 0x8B, 0x45, 0x0C, 0x8B, 0xE5,
                        0x5D, 0xC2, 0x18, 0x00
                    };
                    break;

                case PatchType.ForTheGlory:
                    pattern = new byte[]
                    {
                        0x8B, 0x55, 0x18, 0x8B, 0x02, 0x8B, 0x4D, 0x18,
                        0x8B, 0x50, 0x0C, 0xFF, 0xD2, 0x8B, 0x45, 0x0C,
                        0x8B, 0xE5, 0x5D, 0xC2, 0x18, 0x00
                    };
                    break;

                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0x8B, 0x55, 0x18, 0x8B, 0x02, 0x8B, 0x4D, 0x18,
                        0x8B, 0x50, 0x0C, 0xFF, 0xD2, 0x8B, 0x45, 0x0C,
                        0x8B, 0xE5, 0x5D, 0xC2, 0x18, 0x00
                    };
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x8B, 0x55, 0x78, 0x8B, 0x02, 0x8B, 0x4D, 0x78,
                        0x8B, 0x50, 0x0C, 0xFF, 0xD2, 0x8B, 0x45, 0x6C,
                        0x83, 0xC5, 0x60, 0x8B, 0xE5, 0x5D, 0xC2, 0x18,
                        0x00
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0x18, 0x8B, 0x10, 0x8B, 0x4D, 0x18,
                        0xFF, 0x52, 0x0C, 0x8B, 0x45, 0x0C, 0x8B, 0xE5,
                        0x5D, 0xC2, 0x18, 0x00
                    };
                    break;
            }

            List<uint> l = BinaryScan(_data, pattern, _posTextOutStart,
                _sizeTextSection - (_posTextOutStart - _posTextSection));
            if (l.Count == 0)
            {
                return false;
            }
            _posTextOutEnd = l[0];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     文字列幅の処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanGetTextWidthStart()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%09 文字列幅の処理を埋め込む位置\"を探します\n");

            byte[] pattern;
            switch (_patchType)
            {
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0xC7, 0x45, 0xFC, 0x00, 0x00, 0x00, 0x00, 0xC6,
                        0x45, 0xF4, 0x00, 0x8B, 0x45, 0x08, 0x8A, 0x08,
                        0x88, 0x4D
                    };
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0xC7, 0x45, 0xF4, 0x00, 0x00, 0x00, 0x00, 0x8B,
                        0x45, 0x08, 0x8A, 0x08, 0x88, 0x4D
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0xC7, 0x45, 0xFC, 0x00, 0x00, 0x00, 0x00, 0x8B,
                        0x45, 0x08, 0x8A, 0x08, 0x88, 0x4D
                    };
                    break;
            }

            List<uint> l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posGetTextWidthStart = l[0];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     文字列幅の処理が終了したらジャンプさせる位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanGetTextWidthEnd()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%10 文字列幅の処理が終了したらジャンプさせる位置(の3バイト前)\"を探します\n");

            byte[] pattern;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0xF4, 0x8B, 0xE5, 0x5D, 0xC2, 0x04,
                        0x00
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0xFC, 0x8B, 0xE5, 0x5D, 0xC2, 0x04,
                        0x00
                    };
                    break;
            }
            List<uint> l = BinaryScan(_data, pattern, _posGetTextWidthStart,
                _sizeTextSection - (_posGetTextWidthStart - _posTextSection));
            if (l.Count == 0)
            {
                return false;
            }
            _posGetTextWidthEnd = l[0] + 3;
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     Latin文字を大文字化している処理の位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanLatinToUpper()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX Latin文字を大文字化している処理の位置\"を探します。\n");

            byte[] pattern;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x75, 0x1A, 0x8B, 0xCB, 0x8D, 0x51, 0x01
                    };
                    break;

                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0x75, 0x1A, 0x8B, 0xC7, 0x8D, 0x50, 0x01
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0x75, 0x18, 0x8B, 0xFB, 0x83, 0xC9, 0xFF, 0x33
                    };
                    break;
            }
            List<uint> l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posLatinToUpper = l[0];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     チャットウィンドウの特殊文字ブロック処理の位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanChatBlockChar()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX チャットウィンドウの特殊文字ブロック処理の位置\"を探します。\n");

            byte[] pattern;
            List<uint> l;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0x3D, 0xA7, 0x00, 0x00, 0x00, 0x0F, 0x8F
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posChatBlockChar3 = l[0] + 11;

                    pattern = new byte[]
                    {
                        0x00, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
                        0x08
                    };
                    break;

                default:
                    pattern = new byte[]
                    {
                        0x00, 0x01, 0x02, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x02, 0x03, 0x03, 0x03,
                        0x03
                    };
                    break;
            }
            l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posChatBlockChar1 = l[0];

            pattern = new byte[]
            {
                0x00, 0x01, 0x00, 0x00, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00
            };
            l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posChatBlockChar2 = l[0];

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        #endregion

        #region パッチ位置探索 - テキスト自動折り返し

        /// <summary>
        ///     改行位置計算処理へジャンプする位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanCalcLineBreakStart()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 改行位置計算処理へジャンプする位置\"を探します。\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    pattern = new byte[]
                    {
                        0x8A, 0x4C, 0x24, 0x13, 0x88, 0x8C, 0x04, 0x14,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x40,
                        0x84, 0xDB
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    _posCalcLineBreakStart6 = l[1];
                    break;

                case PatchType.EuropaUniversalis2:
                    pattern = new byte[]
                    {
                        0x8A, 0x4C, 0x24, 0x13, 0x88, 0x8C, 0x04, 0x14,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x8C, 0x04, 0x24, 0x01, 0x00, 0x00, 0x40,
                        0x38, 0x5C, 0x24, 0x13
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x38, 0x01, 0x00, 0x00, 0x40,
                        0x84, 0xDB
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart4 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x40,
                        0x84, 0xDB
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    _posCalcLineBreakStart6 = l[1];
                    break;

                case PatchType.ForTheGlory:
                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x14, 0x40, 0x84, 0xDB, 0x0F,
                        0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x20, 0x40, 0x84, 0xDB, 0x0F,
                        0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart2 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x30, 0x40, 0x84, 0xDB, 0x0F,
                        0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x54, 0x04, 0x1C, 0x40, 0x38, 0x5C, 0x24,
                        0x17, 0x0F, 0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    _posCalcLineBreakStart6 = l[1];
                    break;

                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0x8A, 0x4C, 0x24, 0x13, 0x88, 0x8C, 0x04, 0x14,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x8C, 0x04, 0x24, 0x01, 0x00, 0x00, 0x40,
                        0x38, 0x5C, 0x24, 0x13
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x40,
                        0x84, 0xDB
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    _posCalcLineBreakStart6 = l[1];
                    break;

                case PatchType.DarkestHour105:

                    pattern = new byte[]
                    {
                        0x8A, 0x4C, 0x24, 0x13, // mov     cl, [esp+914h+var_901]
                        0x88, 0x4C, 0x04, 0x14 // mov     [esp+eax+914h+var_900], cl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x8C, 0x04, 0x24, 0x01, 0x00, 0x00, // mov     [esp+eax+230h+var_10C], cl
                        0x40, // inc eax
                        0x38, 0x5C, 0x24, 0x13 // cmp     byte ptr [esp+230h+var_220+3], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x4C, 0x04, 0x20, // mov     [esp+eax+92Ch+var_90C], cl
                        0x40, // inc eax
                        0x38, 0x5C, 0x24, 0x13 // cmp     [esp+92Ch+var_919], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x1C, // mov     [esp+eax+928h+var_90C], bl
                        0x40, // inc eax
                        0x84, 0xDB // test    bl, bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart6 = l[0];
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:

                    pattern = new byte[]
                    {
                        0x8A, 0x4C, 0x24, 0x13, // mov     cl, [esp+214h+var_201]
                        0x88, 0x8C, 0x04, 0x14, 0x01, 0x00, 0x00 // mov     [esp+eax+214h+var_100], cl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x8C, 0x04, 0x24, 0x01, 0x00, 0x00, // mov     [esp+eax+230h+var_10C], cl
                        0x40, // inc eax
                        0x38, 0x5C, 0x24, 0x13 // cmp     byte ptr [esp+230h+var_220+3], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x8C, 0x04, 0x20, 0x01, 0x00, 0x00, // mov     [esp+eax+22Ch+var_10C], cl
                        0x40, // inc eax
                        0x38, 0x5C, 0x24, 0x13 // cmp     [esp+22Ch+var_219], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x1C, 0x01, 0x00, 0x00, // mov     [esp+eax+228h+var_10C], bl
                        0x40, // inc eax
                        0x84, 0xDB // test    bl, bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart6 = l[0];
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x0C, 0x40, 0x84, 0xDB, 0x0F,
                        0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x04, 0x2C, 0x40, 0x84, 0xDB, 0x0F,
                        0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x54, 0x04, 0x1C, 0x40, 0x38, 0x5C, 0x24,
                        0x17
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    _posCalcLineBreakStart6 = l[1];
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x05, 0xFC, 0xFB, 0xFF, 0xFF, 0x40
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];
                    _posCalcLineBreakStart2 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x05, 0xF0, 0xFB, 0xFF, 0xFF, 0x40
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];
                    _posCalcLineBreakStart6 = l[1];

                    pattern = new byte[]
                    {
                        0x88, 0x94, 0x05, 0xF0, 0xFD, 0xFF, 0xFF, 0x40
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x14, 0x0C, 0x42
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart1 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x5C, 0x14, 0x10, 0x42
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart2 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x94, 0x0D, 0xE8, 0xFD, 0xFF, 0xFF, 0x41
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart3 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x0D, 0xE8, 0xFD, 0xFF, 0xFF, 0x41
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x0D, 0xEC, 0xFD, 0xFF, 0xFF, 0x41
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart6 = l[0];
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     改行位置計算処理が終了したらジャンプさせる位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanCalcLineBreakEnd()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 改行位置計算処理が終了したらジャンプさせる位置\"を探します。\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x14, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x54, 0x88, 0x9C, 0x04, 0x19,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x54, 0xC6, 0x84, 0x04, 0x21,
                        0x01, 0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.EuropaUniversalis2:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x18, 0x01, 0x00, 0x00, 0x20,
                        0x88, 0x9C, 0x04, 0x19, 0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x40, 0x88, 0x9C, 0x04, 0x29, 0x01,
                        0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x38, 0x01, 0x00, 0x00, 0x20,
                        0xC6, 0x84, 0x04, 0x39, 0x01, 0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd4 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x4C, 0xC6, 0x84, 0x04, 0x21, 0x01,
                        0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.ForTheGlory:
                    pattern = new byte[]
                    {
                        0x8D, 0x7C, 0x24, 0x18, 0xC6, 0x44, 0x04, 0x19,
                        0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x94, 0x24, 0x20, 0x01, 0x00, 0x00, 0xC6,
                        0x44, 0x04, 0x21, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd2 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x8C, 0x24, 0x30, 0x01, 0x00, 0x00, 0xC6,
                        0x44, 0x04, 0x31, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x44, 0x04, 0x1C, 0x20, 0x88, 0x5C, 0x04,
                        0x1D
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.Victoria:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x18, 0x01, 0x00, 0x00, 0x20,
                        0x88, 0x9C, 0x04, 0x19, 0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x24, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x48, 0x88, 0x9C, 0x04, 0x29,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x54, 0xC6, 0x84, 0x04, 0x21,
                        0x01, 0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x18, 0x01, 0x00, 0x00, 0x20,
                        0x88, 0x9C, 0x04, 0x19, 0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x24, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x40, 0x88, 0x9C, 0x04, 0x29,
                        0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x20,
                        0x51, 0x8B, 0x4D, 0x4C, 0xC6, 0x84, 0x04, 0x21,
                        0x01, 0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0xC6, 0x44, 0x04, 0x18, 0x20, // mov     [esp+eax+918h+var_900], 20h
                        0x88, 0x5C, 0x04, 0x19 // mov     [esp+eax+918h+var_8FF], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint)pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint)pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x40, 0x88, 0x9C, 0x04, 0x29, 0x01,
                        0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint)pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x4C,  // ecx, [ebp+4Ch]
                        0x88, 0x5C, 0x04, 0x25 // mov     [esp+eax+930h+var_90B], bl
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint)pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x4C, // ecx, [ebp+4Ch]
                        0xC6, 0x44, 0x04, 0x21, 0x00 // [esp+eax+92Ch+var_90B], 0
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd6 = l[0] + (uint)pattern.Length;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x04, 0x18, 0x01, 0x00, 0x00, 0x20,
                        0x88, 0x9C, 0x04, 0x19, 0x01, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x40, 0x88, 0x9C, 0x04, 0x29, 0x01,
                        0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x4C, 0x88, 0x9C, 0x04, 0x25, 0x01,
                        0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8B, 0x4D, 0x4C, 0xC6, 0x84, 0x04, 0x21, 0x01,
                        0x00, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd6 = l[0] + (uint) pattern.Length;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    pattern = new byte[]
                    {
                        0xC6, 0x44, 0x04, 0x0C, 0x20, 0x8D, 0x94, 0x24,
                        0x0C, 0x02, 0x00, 0x00, 0xC6, 0x44, 0x04, 0x0D,
                        0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x44, 0x04, 0x2C, 0x20, 0x8D, 0x94, 0x24,
                        0x2C, 0x02, 0x00, 0x00, 0xC6, 0x44, 0x04, 0x2D,
                        0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x44, 0x04, 0x1C, 0x20, 0x8D, 0x94, 0x24,
                        0x1C, 0x02, 0x00, 0x00, 0x88, 0x5C, 0x04, 0x1D
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd6 = l[1] + (uint) pattern.Length;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x05, 0xFC, 0xFB, 0xFF, 0xFF, 0x20,
                        0x52, 0x8B, 0xCE, 0xC6, 0x84, 0x05, 0xFD, 0xFB,
                        0xFF, 0xFF, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;
                    _posCalcLineBreakEnd2 = l[1] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x05, 0xF0, 0xFB, 0xFF, 0xFF, 0x20,
                        0xC6, 0x84, 0x05, 0xF1, 0xFB, 0xFF, 0xFF, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x05, 0xF0, 0xFD, 0xFF, 0xFF, 0x20,
                        0x8D, 0x95, 0xF0, 0xF9, 0xFF, 0xFF, 0x88, 0x9C,
                        0x05, 0xF1, 0xFD, 0xFF, 0xFF
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0xC6, 0x84, 0x05, 0xF0, 0xFB, 0xFF, 0xFF, 0x20,
                        0x8D, 0x95, 0xF0, 0xFD, 0xFF, 0xFF, 0xC6, 0x84,
                        0x05, 0xF1, 0xFB, 0xFF, 0xFF, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd6 = l[0] + (uint) pattern.Length;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x8D, 0x84, 0x24, 0x0C, 0x02, 0x00, 0x00, 0x66,
                        0xC7, 0x44, 0x14, 0x0C, 0x20, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd1 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x84, 0x24, 0x10, 0x02, 0x00, 0x00, 0x66,
                        0xC7, 0x44, 0x14, 0x10, 0x20, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd2 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x85, 0xE8, 0xFB, 0xFF, 0xFF, 0x66, 0xC7,
                        0x84, 0x0D, 0xE8, 0xFD, 0xFF, 0xFF, 0x20, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd3 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x85, 0xE8, 0xF9, 0xFF, 0xFF, 0x66, 0xC7,
                        0x84, 0x0D, 0xE8, 0xFD, 0xFF, 0xFF, 0x20, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd5 = l[0] + (uint) pattern.Length;

                    pattern = new byte[]
                    {
                        0x8D, 0x85, 0xEC, 0xFB, 0xFF, 0xFF, 0x66, 0xC7,
                        0x84, 0x0D, 0xEC, 0xFD, 0xFF, 0xFF, 0x20, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakEnd6 = l[0] + (uint) pattern.Length;
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        #endregion

        #region パッチ位置探索 - 語順入れ替え

        /// <summary>
        ///     師団名の取得処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanGetDivisionName()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 師団名の取得処理を埋め込む位置\"を探します\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0xB9, 0x0A, 0x00, 0x00, 0x00, 0x50, 0x8B, 0xC6,
                        0x99, 0xF7, 0xF9, 0x8B, 0x54, 0x94, 0x1C, 0x52
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetDivisionName1 = l[0] + (uint) pattern.Length + 10;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0x8B, 0x45, 0xF8, 0x99, 0xB9, 0x64, 0x00, 0x00,
                        0x00, 0xF7, 0xF9, 0x83, 0xFA, 0x0D
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetDivisionName1 = l[0];

                    pattern = new byte[]
                    {
                        0x99, 0xB9, 0x0A, 0x00, 0x00, 0x00, 0xF7, 0xF9,
                        0x8B, 0x54, 0x95, 0xC0, 0x52, 0xB9
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetDivisionName2 = l[0];
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0x2E, 0x20, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posRdataSection, _sizeRdataSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    uint addrDivisionAbbrev = GetRdataAddress(l[0]);
                    pattern = new byte[]
                    {
                        0x68, (byte) (addrDivisionAbbrev & 0xFF), (byte) ((addrDivisionAbbrev >> 8) & 0xFF),
                        (byte) ((addrDivisionAbbrev >> 16) & 0xFF), (byte) (addrDivisionAbbrev >> 24)
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetDivisionName1 = l[0] + 1;
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     師団名の書式文字列の定義位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanDivisionNameFormat()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 師団名の書式文字列の定義位置\"を探します\n");

            byte[] pattern =
            {
                0x25, 0x64, 0x25, 0x73, 0x20, 0x25, 0x73, 0x2E,
                0x20, 0x25, 0x73, 0x00
            };
            List<uint> l;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    l = BinaryScan(_data, pattern, _posRdataSection, _sizeRdataSection);
                    break;

                default:
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    break;
            }
            if (l.Count == 0)
            {
                return false;
            }
            _posDivisionNameFormat = l[l.Count - 1];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     軍団名の取得処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanGetArmyName()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 軍団名の取得処理を埋め込む位置\"を探します\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.Victoria:
                    pattern = new byte[]
                    {
                        0x8B, 0xC6, 0xB9, 0x64, 0x00, 0x00, 0x00, 0x99,
                        0xF7, 0xF9, 0x83, 0xFA, 0x0D, 0x75, 0x1E
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName1 = l[0];

                    pattern = new byte[]
                    {
                        0x8B, 0xC6, 0xB9, 0x0A, 0x00, 0x00, 0x00, 0x99,
                        0xF7, 0xF9, 0x8B, 0x54, 0x94, 0x14, 0x52
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName2 = l[0];
                    break;

                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0x8B, 0xC6, 0xB9, 0x64, 0x00, 0x00, 0x00, 0x99,
                        0xF7, 0xF9, 0x83, 0xFA, 0x0D, 0x75, 0x1E
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName1 = l[0];

                    pattern = new byte[]
                    {
                        0x8B, 0xC6, 0xB9, 0x0A, 0x00, 0x00, 0x00, 0x99,
                        0xF7, 0xF9, 0x8B, 0x54, 0x94, 0x1C, 0x52
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName2 = l[0];
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0xFF, 0xFF, 0x99, 0xB9, 0x64, 0x00, 0x00, 0x00,
                        0xF7, 0xF9, 0x83, 0xFA, 0x0D
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName1 = l[0] - 4;

                    pattern = new byte[]
                    {
                        0x99, 0xB9, 0x0A, 0x00, 0x00, 0x00, 0xF7, 0xF9,
                        0x8B, 0x94, 0x95
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName2 = l[0];
                    break;

                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0xC7, 0x45, 0xFC, 0xFF, 0xFF, 0xFF, 0xFF, 0x8B,
                        0x4D, 0x08, 0x51
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName1 = l[0] + 7;

                    pattern = new byte[]
                    {
                        0x83, 0xC4, 0x08, 0x6A, 0x20
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posGetArmyName2 = l[0] + 3;
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     軍団名の書式文字列の定義位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanArmyNameFormat()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 軍団名の書式文字列の定義位置\"を探します\n");

            byte[] pattern =
            {
                0x25, 0x64, 0x25, 0x73, 0x20, 0x25, 0x73, 0x00
            };
            List<uint> l;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    l = BinaryScan(_data, pattern, _posRdataSection, _sizeRdataSection);
                    break;

                default:
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    break;
            }
            if (l.Count == 0)
            {
                return false;
            }
            _posArmyNameFormat = l[l.Count - 1];
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     国家序列の取得処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanGetRankingName()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 国家序列の取得処理を埋め込む位置\"を探します\n");

            byte[] pattern =
            {
                0x8B, 0xC6, 0xB9, 0x64, 0x00, 0x00, 0x00, 0x99,
                0xF7, 0xF9, 0xC7, 0x44, 0x24
            };
            List<uint> l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posGetRankingName1 = l[0];

            pattern = new byte[]
            {
                0x83, 0xFA, 0x0D, 0x75, 0x28
            };
            l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (l.Count == 0)
            {
                return false;
            }
            _posGetRankingName2 = l[0] + (byte) pattern.Length;

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        #endregion

        #region パッチ位置探索 - ゲーム設定

        /// <summary>
        ///     強制ウィンドウ化処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanWindowed()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 強制ウィンドウ化処理を埋め込む位置\"を探します\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    pattern = new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x83, 0x7D, 0x10, 0x00,
                        0x74, 0x1C
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posWindowed1 = l[0];
                    break;

                case PatchType.EuropaUniversalis2:
                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x8B, 0x4D, 0x08, 0x89,
                        0x0D
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posWindowed1 = l[0];
                    break;

                case PatchType.Victoria:
                    pattern = new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x8B, 0x55, 0x08, 0x89,
                        0x15
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posWindowed1 = l[0];
                    break;

                case PatchType.HeartsOfIron2:
                    pattern = new byte[]
                    {
                        0x83, 0xFA, 0x57, 0x0F, 0x85
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posWindowed1 = l[0] + 3;
                    _posWindowed2 = _posWindowed1 + 6 + GetLong(_posWindowed1 + 2) - 10;
                    break;

                case PatchType.HeartsOfIron212:
                    pattern = new byte[]
                    {
                        0xFF, 0xFF, 0x83, 0xFA, 0x57, 0x75
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posWindowed1 = l[0] + 5;
                    _posWindowed2 = _posWindowed1 + 2 + _data[_posWindowed1 + 1] - 10;
                    break;

                case PatchType.IronCrossHoI2:
                    // Iron Crossの場合はゲーム付属のツールで設定変更できるのでパッチを当てない
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     binkplay.exeの位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanIntroSkip()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX binkplay.exe\"を探します\n");

            byte[] pattern =
            {
                0x62, 0x69, 0x6E, 0x6B, 0x70, 0x6C, 0x61, 0x79,
                0x2E, 0x65, 0x78, 0x65, 0x00
            };
            List<uint> l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);

            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posBinkPlay1 = l[0];
                    _posBinkPlay2 = l[1];
                    break;

                case PatchType.HeartsOfIron:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posBinkPlay1 = l[0];
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     時間制限解除処理を埋め込む位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanNtl()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");
            AppendLog("  \"%XX 時間制限解除処理を埋め込む位置を探します\n");

            byte[] pattern;
            List<uint> l;

            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x1E, 0xAE, 0x05
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x46, 0x06, 0xAE, 0x05
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x1E, 0x2A, 0x04
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x46, 0x06, 0x2A, 0x04
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0xAD, 0x05, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posLimitYear = l[0];
                    break;

                case PatchType.EuropaUniversalis2:
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x32, 0x1C, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x2E, 0x1C, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x2E, 0x8B, 0x05
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count < 2)
                    {
                        return false;
                    }
                    _posMinYear1 = l[0] + (uint) pattern.Length - 2;
                    _posMinYear2 = l[1] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x1B, 0x07, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posLimitYear = l[0];
                    break;

                case PatchType.Victoria:
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x1E, 0x91, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x46, 0x06, 0x91, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x1E, 0x2B, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x46, 0x06, 0x2B, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x90, 0x07, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posLimitYear = l[0];
                    break;

                case PatchType.HeartsOfIron:
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x24, 0x9D, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xB8, 0x9D, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x24, 0x8F, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xB8, 0x8F, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x9C, 0x07, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posLimitYear = l[0];
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x24, 0xAE, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xB8, 0xAE, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMaxYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xC7, 0x44, 0x24, 0x24, 0x8F, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear1 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0x66, 0xB8, 0x8F, 0x07
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posMinYear2 = l[0] + (uint) pattern.Length - 2;
                    pattern = new byte[]
                    {
                        0xBA, 0xAC, 0x07, 0x00, 0x00
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posLimitYear = l[0] + (uint) pattern.Length - 4;
                    break;

                case PatchType.IronCrossHoI2:
                    // Iron Crossの場合は初期状態で制限解除されているのでパッチを当てない
                    break;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        #endregion

        #region パッチ位置探索 - ゲーム固有

        /// <summary>
        ///     EE_MAX_AMPHIB_MOD_TITLEの呼び出し位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanEeMaxAmphibModTitle()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");

            AppendLog("  \"%XX EE_MAX_AMPHIB_MODの定義位置\"を探します\n");
            byte[] pattern =
            {
                0x45, 0x45, 0x5F, 0x4D, 0x41, 0x58, 0x5F, 0x41,
                0x4D, 0x50, 0x48, 0x49, 0x42, 0x5F, 0x4D, 0x4F,
                0x44
            };
            List<uint> l = BinaryScan(_data, pattern, _posDataSection, _sizeDataSection);
            if (l.Count == 0)
            {
                return false;
            }
            uint posEeMaxAmphibMod = l[0];

            AppendLog("\n  \"%XX EE_MAX_AMPHIB_MODの呼び出し位置\"を探します\n");
            uint addrEeMaxAmphibMod = GetDataAddress(posEeMaxAmphibMod);
            byte[] pattern2 = new byte[5];
            pattern2[0] = 0x68;
            pattern2[1] = (byte) (addrEeMaxAmphibMod & 0x000000FF);
            pattern2[2] = (byte) ((addrEeMaxAmphibMod & 0x0000FF00) >> 8);
            pattern2[3] = (byte) ((addrEeMaxAmphibMod & 0x00FF0000) >> 16);
            pattern2[4] = (byte) ((addrEeMaxAmphibMod & 0xFF000000) >> 24);
            List<uint> l2 = BinaryScan(_data, pattern2, _posTextSection, _sizeTextSection);
            if (l2.Count == 0)
            {
                return false;
            }
            _posPushEeMaxAmphibModTitle = l2[0] + 1;
            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        /// <summary>
        ///     モデル名の終端文字設定位置を探索する
        /// </summary>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static bool ScanTermModelName()
        {
            AppendLog("ScanBinary - 特定バイナリを探す\n");

            AppendLog("  \"%XX モデル名の終端文字設定位置\"を探します\n");

            byte[] pattern = null;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    pattern = new byte[]
                    {
                        0xC6, 0x85, 0x50, 0xFF, 0xFF, 0xFF, 0x00, 0x6A,
                        0x00
                    };
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    pattern = new byte[]
                    {
                        0x68, 0xFF, 0x00, 0x00, 0x00, 0xC6, 0x45, 0x90,
                        0x00
                    };
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.ArsenalOfDemocracy107:
                    pattern = new byte[]
                    {
                        0x68, 0xFF, 0x00, 0x00, 0x00, 0xC6, 0x44, 0x24,
                        0x68, 0x00
                    };
                    break;

                case PatchType.DarkestHour:
                case PatchType.DarkestHour105:
                    pattern = new byte[]
                    {
                        0x68, 0xFF, 0x00, 0x00, 0x00, 0xC6, 0x44, 0x24,
                        0x64, 0x00
                    };
                    break;

                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour102:
                    pattern = new byte[]
                    {
                        0x68, 0xFF, 0x00, 0x00, 0x00, 0xC6, 0x44, 0x24,
                        0x60, 0x00
                    };
                    break;
            }
            List<uint> l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
            if (_patchType == PatchType.DarkestHour)
            {
                if (l.Count < 8)
                {
                    return false;
                }
            }
            else
            {
                if (l.Count < 2)
                {
                    return false;
                }
            }
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    _posTermModelNameStart1 = l[0];
                    _posTermModelNameStart2 = l[1];
                    break;

                case PatchType.DarkestHour:
                case PatchType.DarkestHour105:
                    _posTermModelNameStart1 = l[0] + 5;
                    _posTermModelNameStart2 = l[1] + 5;
                    _posTermModelNameStart3 = l[2] + 5;
                    _posTermModelNameStart4 = l[3] + 5;
                    _posTermModelNameStart5 = l[4] + 5;
                    _posTermModelNameStart6 = l[5] + 5;
                    _posTermModelNameStart7 = l[6] + 5;
                    _posTermModelNameStart8 = l[7] + 5;
                    break;

                default:
                    _posTermModelNameStart1 = l[0] + 5;
                    _posTermModelNameStart2 = l[1] + 5;
                    break;
            }
            if (_patchType == PatchType.DarkestHour || _patchType == PatchType.DarkestHour105)
            {
                byte[] pattern2 =
                {
                    0x68, 0xFF, 0x00, 0x00, 0x00, 0xC6, 0x44, 0x24,
                    0x78, 0x00
                };
                List<uint> l2 = BinaryScan(_data, pattern2, _posTextSection, _sizeTextSection);
                if (l2.Count < 2)
                {
                    return false;
                }
                _posTermModelNameStart9 = l2[0] + 5;
                _posTermModelNameStart10 = l2[1] + 5;
            }

            AppendLog("ScanBinary passed\n\n");

            return true;
        }

        #endregion

        #region パッチ処理

        /// <summary>
        ///     パッチを当てる
        /// </summary>
        private static void PatchBinary()
        {
            AppendLog("PatchBinary - パッチをあてる\n");

            // 日本語化コア部分
            PatchTextOut();
            PatchGetTextWidth();
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    PatchLatinToUpper();
                    PatchChatBlockChar();
                    break;
            }
            PatchWinMmDll();
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchIsDebuggerPresent();
                    break;
            }
            EmbedTextOutDc2();
            EmbedGetTextWidth();
            if (PatchController.AutoLineBreak)
            {
                EmbedCalcLineBreak();
            }
            // テキスト自動折り返し
            if (PatchController.AutoLineBreak)
            {
                PatchCalcLineBreak();
            }
            // 語順入れ替え
            if (PatchController.WordOrder)
            {
                switch (_patchType)
                {
                    case PatchType.Victoria:
                        PatchGetDivisionName();
                        EmbedDivisionNameFormat();
                        PatchGetArmyName();
                        EmbedArmyNameFormat();
                        PatchGetRankingName();
                        EmbedRankingSuffix();
                        break;

                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.ArsenalOfDemocracy109:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchGetDivisionName();
                        EmbedDivisionNameFormat();
                        PatchGetArmyName();
                        EmbedArmyNameFormat();
                        break;

                    case PatchType.ArsenalOfDemocracy:
                        PatchGetDivisionName();
                        PatchGetArmyName();
                        break;
                }
            }
            // 強制ウィンドウ化
            if (PatchController.Windowed)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                        PatchWindowed();
                        break;
                    case PatchType.IronCrossHoI2:
                        // Iron Crossの場合は付属ツールでウィンドウ化できるのでパッチを当てない
                        break;
                }
            }
            // イントロスキップ
            if (PatchController.IntroSkip)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                        PatchBinkPlay();
                        break;
                }
            }
            // 時間制限解除
            if (PatchController.Ntl)
            {
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                        PatchNtl();
                        break;
                    case PatchType.IronCrossHoI2:
                        // Iron Crossの場合は初期状態で制限解除されているのでパッチを当てない
                        break;
                }
            }
            // 4GBメモリ使用
            if (PatchController.Memory4Gb)
            {
                Patch4Gb();
            }
            // ゲーム固有
            switch (_patchType)
            {
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    PatchPushEeMaxAmphibModTitle();
                    EmbedEeMaxAmphibModTitle();
                    break;
            }
            switch (_patchType)
            {
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    PatchTermModelName();
                    EmbedStrNLen0();
                    break;
            }
        }

        #endregion

        #region パッチ処理 - 日本語化コア部分

        /// <summary>
        ///     TextOutの処理を埋め込む
        /// </summary>
        private static void PatchTextOut()
        {
            AppendLog("  proc TextOut書き換え\n");
            uint offset = _posTextOutStart;
            PatchByte(_data, offset, 0x83); // cmp [varTextOutDc2Address],0
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                $"%02 ${_addrVarTextOutDcAddress:X8} varTextOutDC2Address");
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jne TEXT_OUT_3
            offset++;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    PatchByte(_data, offset, 0x2C);
                    break;

                default:
                    PatchByte(_data, offset, 0x29);
                    break;
            }
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, $"%06 ${_addrWinMmDll:X8} WINMM.dll");
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA/IsDebuggerPresent
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        $"%04 ${_addrIsDebuggerPresent:X8} IsDebuggerPresent");
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        $"%04 ${_addrGetModuleHandleA:X8} GetModuleHandleA");
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jne TEXT_OUT_1
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // TEXT_OUT_1
            PatchByte(_data, offset, 0x68); // push addrTextOutDc2
            offset++;
            PatchLong(_data, offset, _addrTextOutDc, $"%00 ${_addrTextOutDc:X8} TextOutDC2");
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                $"%05 ${_addrGetProcAddress:X8} GetProcAddress");
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jne TEXT_OUT_2
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // TEXT_OUT_2
            PatchByte(_data, offset, 0xA3); // mov [varTextOutDc2Address],eax
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                $"%02 ${_addrVarTextOutDcAddress:X8} varTextOutDC2Address");
            offset += 4;
            PatchByte(_data, offset, 0x8B); // mov ecx,[ebp-64h/6Ch/9Ch/+58h]
            offset++;
            if (_patchType == PatchType.CrusaderKings)
            {
                PatchByte(_data, offset, 0x8D);
                offset++;
                PatchByte(_data, offset, 0x64);
                offset++;
                PatchByte(_data, offset, 0xFF);
                offset++;
                PatchByte(_data, offset, 0xFF);
                offset++;
                PatchByte(_data, offset, 0xFF);
            }
            else
            {
                PatchByte(_data, offset, 0x4D);
                offset++;
                switch (_patchType)
                {
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.ArsenalOfDemocracy109:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchByte(_data, offset, 0x94);
                        break;
                    case PatchType.ArsenalOfDemocracy:
                        PatchByte(_data, offset, 0x58);
                        break;
                    default:
                        PatchByte(_data, offset, 0x98);
                        break;
                }
            }
            offset++;
            // TEXT_OUT_3
            PatchByte(_data, offset, 0x8B); // mov edx,[ecx+0Ch]
            offset++;
            PatchByte(_data, offset, 0x51);
            offset++;
            PatchByte(_data, offset, 0x0C);
            offset++;
            if (_patchType == PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x33); // TODO: アセンブリコード記載
                offset++;
                PatchByte(_data, offset, 0xC0);
                offset++;
                PatchByte(_data, offset, 0x80);
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                PatchByte(_data, offset, 0x13);
                offset++;
                PatchByte(_data, offset, 0x2E);
                offset++;
                PatchByte(_data, offset, 0x74);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x40);
                offset++;
                PatchByte(_data, offset, 0x8B);
                offset++;
                PatchByte(_data, offset, 0xD0);
                offset++;
                PatchByte(_data, offset, 0x80);
                offset++;
                PatchByte(_data, offset, 0xB9);
                offset++;
                PatchByte(_data, offset, 0x14);
                offset++;
                PatchByte(_data, offset, 0x04);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74);
                offset++;
                PatchByte(_data, offset, 0x03);
                offset++;
                PatchByte(_data, offset, 0x83);
                offset++;
                PatchByte(_data, offset, 0xC8);
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                PatchByte(_data, offset, 0x50);
                offset++;
                PatchByte(_data, offset, 0xFF);
                offset++;
                PatchByte(_data, offset, 0x75);
                offset++;
                PatchByte(_data, offset, 0x1C);
                offset++;
                PatchByte(_data, offset, 0x8B);
                offset++;
                PatchByte(_data, offset, 0x41);
                offset++;
                PatchByte(_data, offset, 0x10);
                offset++;
                PatchByte(_data, offset, 0x2B);
                offset++;
                PatchByte(_data, offset, 0xC2);
            }
            else
            {
                PatchByte(_data, offset, 0x81); // cmp [edx+14h/19h],646C6F62h/62616D72h
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.Victoria:
                        PatchByte(_data, offset, 0x0F);
                        break;
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.ArsenalOfDemocracy109:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchByte(_data, offset, 0x19);
                        break;
                    default:
                        PatchByte(_data, offset, 0x14);
                        break;
                }
                offset++;
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.Victoria:
                        PatchByte(_data, offset, 0x72);
                        offset++;
                        PatchByte(_data, offset, 0x6D);
                        offset++;
                        PatchByte(_data, offset, 0x61);
                        offset++;
                        PatchByte(_data, offset, 0x62);
                        break;
                    default:
                        PatchByte(_data, offset, 0x62);
                        offset++;
                        PatchByte(_data, offset, 0x6F);
                        offset++;
                        PatchByte(_data, offset, 0x6C);
                        offset++;
                        PatchByte(_data, offset, 0x64);
                        break;
                }
                offset++;
                PatchByte(_data, offset, 0xB8); // mov eax,00000001h
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74); // je TEXT_OUT_4
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                PatchByte(_data, offset, 0x33); // xor eax,eax
                offset++;
                PatchByte(_data, offset, 0xC0);
                offset++;
                // TEXT_OUT_4
                PatchByte(_data, offset, 0x8B); // mov edx,eax
                offset++;
                PatchByte(_data, offset, 0xD0);
                offset++;
                PatchByte(_data, offset, 0x80); // cmp [ecx+00000414h],0
                offset++;
                PatchByte(_data, offset, 0xB9);
                offset++;
                PatchByte(_data, offset, 0x14);
                offset++;
                PatchByte(_data, offset, 0x04);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74); // je TEXT_OUT_5
                offset++;
                PatchByte(_data, offset, 0x03);
                offset++;
                PatchByte(_data, offset, 0x83); // or eax,00000002h
                offset++;
                PatchByte(_data, offset, 0xC8);
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                // TEXT_OUT_5
                PatchByte(_data, offset, 0x50); // push eax
                offset++;
                PatchByte(_data, offset, 0xFF); // push [ebp+1Ch/7Ch]
                offset++;
                PatchByte(_data, offset, 0x75);
                offset++;
                PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x7C : 0x1C));
                offset++;
                PatchByte(_data, offset, 0x8B); // mov eax,[ecx+10h]
                offset++;
                PatchByte(_data, offset, 0x41);
                offset++;
                PatchByte(_data, offset, 0x10);
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax
                offset++;
                PatchByte(_data, offset, 0x2B); // sub eax,edx
                offset++;
                PatchByte(_data, offset, 0xC2);
                offset++;
                PatchByte(_data, offset, 0x8B); // mov edx,[ecx+0Ch]
                offset++;
                PatchByte(_data, offset, 0x51);
                offset++;
                PatchByte(_data, offset, 0x0C);
                offset++;
                PatchByte(_data, offset, 0x81); // cmp [edx+0Ah],61697261h
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                PatchByte(_data, offset, 0x0A);
                offset++;
                PatchByte(_data, offset, 0x61);
                offset++;
                PatchByte(_data, offset, 0x72);
                offset++;
                PatchByte(_data, offset, 0x69);
                offset++;
                PatchByte(_data, offset, 0x61);
                offset++;
                PatchByte(_data, offset, 0x75); // jne TEXT_OUT_6
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax
            }
            offset++;
            // TEXT_OUT_6
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0x8B); // mov edx,[ebp+18h/78h]
            offset++;
            PatchByte(_data, offset, 0x55);
            offset++;
            PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x78 : 0x18));
            offset++;
            PatchByte(_data, offset, 0xFF); // push [edx+2Ch]
            offset++;
            PatchByte(_data, offset, 0x72);
            offset++;
            PatchByte(_data, offset, 0x2C);
            offset++;
            PatchByte(_data, offset, 0xFF); // push [ebp+08h/68h]
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x68 : 0x08));
            offset++;
            PatchByte(_data, offset, 0x8D); // lea eax,[ebp+10h/70h]
            offset++;
            PatchByte(_data, offset, 0x45);
            offset++;
            PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x70 : 0x10));
            offset++;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0x8D); // lea eax,[ebp+0Ch/6Ch]
            offset++;
            PatchByte(_data, offset, 0x45);
            offset++;
            PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x6C : 0x0C));
            offset++;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // push [ebp+14h/74h]
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, (byte) (_patchType == PatchType.ArsenalOfDemocracy ? 0x74 : 0x14));
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varTextOutDC2Address]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                $"%02 ${_addrVarTextOutDcAddress:X8} varTextOutDC2Address");
            offset += 4;
            PatchByte(_data, offset, 0xE9); // jmp TEXT_OUT_END
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posTextOutEnd),
                $"%08 ${GetTextAddress(_posTextOutEnd):X8} TextOutEnd");
            offset += 4;

            _posTextOutDcFree = offset;

            AppendLog("\n");
        }

        /// <summary>
        ///     文字列幅の処理を埋め込む
        /// </summary>
        private static void PatchGetTextWidth()
        {
            AppendLog("  proc GetTextWidth書き換え\n");
            uint offset = _posGetTextWidthStart;
            PatchByte(_data, offset, 0x83); // cmp [varGetTextWidthAddress],0
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                $"%03 ${_addrVarGetTextWidthAddress:X8} varGetTextWidthAddress");
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jne GET_TEXT_WIDTH_3
            offset++;
            PatchByte(_data, offset, 0x29);
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, $"%06 ${_addrWinMmDll:X8} WINMM.dll");
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA/IsDebuggerPresent
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        $"%04 ${_addrIsDebuggerPresent:X8} IsDebuggerPresent");
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        $"%04 ${_addrGetModuleHandleA:X8} GetModuleHandleA");
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jne GET_TEXT_WIDTH_1
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_TEXT_WIDTH_1
            PatchByte(_data, offset, 0x68); // push addrGetTextWidth
            offset++;
            PatchLong(_data, offset, _addrGetTextWidth, $"%01 ${_addrGetTextWidth:X8} GetTextWidth");
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                $"%05 ${_addrGetProcAddress:X8} GetProcAddress");
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jne GET_TEXT_WIDTH_2
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_TEXT_WIDTH_2
            PatchByte(_data, offset, 0xA3); // mov [varGetTextWidthAddress],eax
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                $"%03 ${_addrVarGetTextWidthAddress:X8} varGetTextWidthAddress");
            offset += 4;
            PatchByte(_data, offset, 0x8B); // mov ecx,[ebp-08h/14h/18h]
            offset++;
            PatchByte(_data, offset, 0x4D);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0xF8);
                    break;
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
				case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0xE8);
                    break;
                default:
                    PatchByte(_data, offset, 0xEC);
                    break;
            }
            offset++;
            // GET_TEXT_WIDTH_3
            PatchByte(_data, offset, 0x8B); // mov edx,[ecx+0Ch]
            offset++;
            PatchByte(_data, offset, 0x51);
            offset++;
            PatchByte(_data, offset, 0x0C);
            offset++;
            if (_patchType == PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x33); // TODO: アセンブリコード記載
                offset++;
                PatchByte(_data, offset, 0xC0);
                offset++;
                PatchByte(_data, offset, 0x80);
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                PatchByte(_data, offset, 0x13);
                offset++;
                PatchByte(_data, offset, 0x2E);
                offset++;
                PatchByte(_data, offset, 0x74);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x40);
            }
            else
            {
                PatchByte(_data, offset, 0x81); // cmp [edx+0Fh/14h/19h],646C6F62h/62616D72h
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.Victoria:
                        PatchByte(_data, offset, 0x0F);
                        break;
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.ArsenalOfDemocracy109:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchByte(_data, offset, 0x19);
                        break;
                    default:
                        PatchByte(_data, offset, 0x14);
                        break;
                }
                offset++;
                switch (_patchType)
                {
                    case PatchType.CrusaderKings:
                    case PatchType.Victoria:
                        PatchByte(_data, offset, 0x72);
                        offset++;
                        PatchByte(_data, offset, 0x6D);
                        offset++;
                        PatchByte(_data, offset, 0x61);
                        offset++;
                        PatchByte(_data, offset, 0x62);
                        break;
                    default:
                        PatchByte(_data, offset, 0x62);
                        offset++;
                        PatchByte(_data, offset, 0x6F);
                        offset++;
                        PatchByte(_data, offset, 0x6C);
                        offset++;
                        PatchByte(_data, offset, 0x64);
                        break;
                }
                offset++;
                PatchByte(_data, offset, 0xB8); // mov eax,00000001h
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74); // je GET_TEXT_WIDTH_4
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                PatchByte(_data, offset, 0x33); // xor eax,eax
                offset++;
                PatchByte(_data, offset, 0xC0);
            }
            offset++;
            // GET_TEXT_WIDTH_4
            PatchByte(_data, offset, 0x8B); // mov edx,eax
            offset++;
            PatchByte(_data, offset, 0xD0);
            offset++;
            PatchByte(_data, offset, 0x80); // cmp [ecx+00000414h],0
            offset++;
            PatchByte(_data, offset, 0xB9);
            offset++;
            PatchByte(_data, offset, 0x14);
            offset++;
            PatchByte(_data, offset, 0x04);
            offset++;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x74); // je GET_TEXT_WIDTH_5
            offset++;
            PatchByte(_data, offset, 0x03);
            offset++;
            PatchByte(_data, offset, 0x83); // or eax,00000002h
            offset++;
            PatchByte(_data, offset, 0xC8);
            offset++;
            PatchByte(_data, offset, 0x02);
            offset++;
            // GET_TEXT_WIDTH_5
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0x8B); // mov eax,[ecx+10h]
            offset++;
            PatchByte(_data, offset, 0x41);
            offset++;
            PatchByte(_data, offset, 0x10);
            offset++;
            if (_patchType != PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x48); // dec eax,eax
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax,eax
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax,eax
                offset++;
            }
            PatchByte(_data, offset, 0x2B); // sub eax,edx
            offset++;
            PatchByte(_data, offset, 0xC2);
            offset++;
            if (_patchType != PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x8B); // mov edx,[ecx+0Ch]
                offset++;
                PatchByte(_data, offset, 0x51);
                offset++;
                PatchByte(_data, offset, 0x0C);
                offset++;
                PatchByte(_data, offset, 0x81); // cmp [edx+0Ah],61697261h
                offset++;
                PatchByte(_data, offset, 0x7A);
                offset++;
                PatchByte(_data, offset, 0x0A);
                offset++;
                PatchByte(_data, offset, 0x61);
                offset++;
                PatchByte(_data, offset, 0x72);
                offset++;
                PatchByte(_data, offset, 0x69);
                offset++;
                PatchByte(_data, offset, 0x61);
                offset++;
                PatchByte(_data, offset, 0x75); // jne GET_TEXT_WIDTH_6
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x48); // dec eax,eax
                offset++;
            }
            // GET_TEXT_WIDTH_6
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // push [ebp+08h]
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x08);
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varGetTextWidthAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                $"%03 ${_addrVarGetTextWidthAddress:X8} varGetTextWidthAddress");
            offset += 4;
            PatchByte(_data, offset, 0xE9); // jmp GET_TEXT_WIDTH_END
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetTextWidthEnd),
                $"%10 ${GetTextAddress(_posGetTextWidthEnd):X8} GetTextWidthEnd");
            offset += 4;
            PatchByte(_data, offset, 0x90);
            offset++;
            PatchByte(_data, offset, 0x90);
            AppendLog("\n");
        }

        /// <summary>
        ///     Latin文字を大文字に変換する処理をスキップするように書き換える
        /// </summary>
        private static void PatchLatinToUpper()
        {
            AppendLog("  proc LatinToUpper書き換え\n");
            uint offset = _posLatinToUpper;
            PatchByte(_data, offset, 0xEB); // jmp 14h/16h/17h
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x17);
                    break;

                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x16);
                    break;

                default:
                    PatchByte(_data, offset, 0x14);
                    break;
            }
            AppendLog("\n");
        }

        /// <summary>
        ///     チャットウィンドウの特殊文字ブロック処理を書き換える
        /// </summary>
        private static void PatchChatBlockChar()
        {
            AppendLog("  proc ChatBlockChar書き換え\n");
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                    // 0x5C
                    PatchByte(_data, _posChatBlockChar1 + 0x5C + 0x59, 0x08);
                    PatchByte(_data, _posChatBlockChar2 + 0x5C - 0x2C, 0x01);

                    // 0x7C
                    PatchByte(_data, _posChatBlockChar1 + 0x7C + 0x59, 0x08);

                    // 0xA7
                    uint offset = _posChatBlockChar3;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    break;

                default:
                    // 0x5C
                    PatchByte(_data, _posChatBlockChar1 + 0x5C - 0x10, 0x03);
                    PatchByte(_data, _posChatBlockChar2 + 0x5C - 0x2C, 0x01);

                    // 0x7C
                    PatchByte(_data, _posChatBlockChar1 + 0x7C - 0x10, 0x03);

                    // 0xA7
                    PatchByte(_data, _posChatBlockChar1 + 0xA7 - 0x10, 0x03);
                    break;
            }
            AppendLog("\n");
        }

        #endregion

        #region パッチ処理 - テキスト自動折り返し

        /// <summary>
        ///     改行位置の計算処理を埋め込む
        /// </summary>
        private static void PatchCalcLineBreak()
        {
            AppendLog("  proc 改行位置の計算処理書き換え\n");

            #region 改行位置計算処理 - _inmm.dll内CalcLineBreakのアドレス取得

            // GET_CALC_LINE_BREAK_ADDR
            uint posGetCalcLineBreakAddr = _posTextOutDcFree;
            uint offset = _posTextOutDcFree;
            PatchByte(_data, offset, 0x83); // cmp [varCalcLineBreakAddress],0
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_CALC_LINE_BREAK_ADDR_3
            offset++;
            PatchByte(_data, offset, 0x26);
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, $"%06 ${_addrWinMmDll:X8} WINMM.dll");
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA/IsDebuggerPresent
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        $"%04 ${_addrIsDebuggerPresent:X8} IsDebuggerPresent");
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        $"%04 ${_addrGetModuleHandleA:X8} GetModuleHandleA");
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_CALC_LINE_BREAK_ADDR_1
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_CALC_LINE_BREAK_ADDR_1
            PatchByte(_data, offset, 0x68); // push addrCalcLineBreak
            offset++;
            PatchLong(_data, offset, _addrCalcLineBreak, $"%00 ${_addrCalcLineBreak:X8} CalcLineBreak");
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                $"%05 ${_addrGetProcAddress:X8} GetProcAddress");
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_CALC_LINE_BREAK_ADDR_2
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_CALC_LINE_BREAK_ADDR_2
            PatchByte(_data, offset, 0xA3); // mov [varCalcLineBreakAddress],eax
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            // GET_CALC_LINE_BREAK_ADDR_3
            PatchByte(_data, offset, 0xC3); // retn
            offset++;

            #endregion

            #region 改行位置計算処理1 - イベント文/シナリオ解説文/閣僚名/ポップアップ表示

            // CALC_LINE_BREAK_START_1
            PatchByte(_data, _posCalcLineBreakStart1, 0xE9); // jmp CALC_LINE_BREAK_1
            PatchLong(_data, _posCalcLineBreakStart1 + 1, GetRelativeOffset(_posCalcLineBreakStart1 + 5, offset),
                $"%XX ${GetTextAddress(offset):X8} CalcLineBreak1");

            // CALC_LINE_BREAK_1
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:

                    PatchByte(_data, offset, 0x4E); // dec esi
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000118h] // 118はendの218h+var_100から. 0x20は空白
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                // パターンのバイナリはこれを使うと便利。0hは0xにすること
                // https://defuse.ca/online-x86-assembler.htm
                // それと別にこの書き方は別にasmに修正したい気持ち...
                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x4E); // dec esi
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000018h] // 918h+var_900
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x4D); // dec ebp
                    offset++;
                    PatchByte(_data, offset, 0x55); // push ebp
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+18h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x8D); // lea esi,[esi-1]
                    offset++;
                    PatchByte(_data, offset, 0x76);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[esp+0Ch]
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0xE4);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+08h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+08h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000404h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x4F); // dec edi
                    offset++;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+10h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x10);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.EuropaUniversalis2:
                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Ah]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2A);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+52h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x52);
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x03); // add ebp,eax
                    offset++;
                    PatchByte(_data, offset, 0xE8);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // mov edi,[ebp+18h]
                    offset++;
                    PatchByte(_data, offset, 0x7C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    break;

                case PatchType.Victoria:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Ch]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2C);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi // 214h + arg_0
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Eh]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2E);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h] // 214h+var_100
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+56h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x56);
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000918h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Eh] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2E);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000014h] // 914h+var_900
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+56h] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x56);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x01); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xC6);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[esp+0000020Ch]
                    offset++;
                    PatchByte(_data, offset, 0x84);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+08h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+08h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000204h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[ebp-00000404h]
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x52); // push edx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,esi
                    offset++;
                    PatchByte(_data, offset, 0xCE);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x03); // add edi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF8);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[esp+0000020Ch]
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END_1
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd1),
                $"%XX ${GetTextAddress(_posCalcLineBreakEnd1):X8} CalcLineBreakEnd1");
            offset += 4;

            #endregion

            #region 改行位置計算処理2 - 生産画面

            // CALC_LINE_BREAK_START_2
            PatchByte(_data, _posCalcLineBreakStart2, 0xE9); // jmp CALC_LINE_BREAK_2
            PatchLong(_data, _posCalcLineBreakStart2 + 1, GetRelativeOffset(_posCalcLineBreakStart2 + 5, offset),
                $"%XX ${GetTextAddress(offset):X8} CalcLineBreak2");

            // CALC_LINE_BREAK_2
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x4E); // dec esi
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000118h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x4E); // dec esi
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000018h] // 918h+var_900
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+18h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+24h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x8D); // lea edi,[edi-01h]
                    offset++;
                    PatchByte(_data, offset, 0x7F);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[esp+10h]
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0xE4);
                    offset++;
                    PatchByte(_data, offset, 0x10);
                    offset++;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+08h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+08h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000404h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x4F); // dec edi
                    offset++;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+10h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x10);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.EuropaUniversalis2:
                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Ah]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2A);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+52h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x52);
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+18h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+18h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[esp+00000120h]
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.Victoria:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Ch]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2C);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000218h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Eh]
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2E);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000114h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+56h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x56);
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000918h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x18);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov esi,[ebp+2Eh] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x75);
                    offset++;
                    PatchByte(_data, offset, 0x2E);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000014h] // 914h+var_900
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x14);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+56h] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x56);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x01); // add edi,eax
                    offset++;
                    PatchByte(_data, offset, 0xC7);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000210h]
                    offset++;
                    PatchByte(_data, offset, 0x84);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x10);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+08h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+08h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000204h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[ebp-00000404h]
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    PatchByte(_data, offset, 0xFC);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x52); // push edx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,esi
                    offset++;
                    PatchByte(_data, offset, 0xCE);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x03); // add edi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF8);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[esp+0000020Ch]
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END_2
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd2),
                $"%XX ${GetTextAddress(_posCalcLineBreakEnd2):X8} CalcLineBreakEnd2");
            offset += 4;

            #endregion

            #region 改行位置計算処理3 - 外交画面の説明文

            if (_patchType != PatchType.CrusaderKings)
            {
                // CALC_LINE_BREAK_START_3
                PatchByte(_data, _posCalcLineBreakStart3, 0xE9); // jmp CALC_LINE_BREAK_3
                PatchLong(_data, _posCalcLineBreakStart3 + 1, GetRelativeOffset(_posCalcLineBreakStart3 + 5, offset),
                    $"%XX ${GetTextAddress(offset):X8} CalcLineBreak3");

                // CALC_LINE_BREAK_3
                PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                    $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
                offset += 4;
                switch (_patchType)
                {
                    case PatchType.EuropaUniversalis2:
                    case PatchType.Victoria:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000238h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x38);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x49); // dec ecx
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000128h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x28);
                        offset++;
                        PatchByte(_data, offset, 0x01);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        break;

                    case PatchType.ForTheGlory:
                        PatchByte(_data, offset, 0x4D); // dec ebp
                        offset++;
                        PatchByte(_data, offset, 0x55); // push ebp
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+34h]
                        offset++;
                        PatchByte(_data, offset, 0x4C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x34);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy:
                        PatchByte(_data, offset, 0x8D); // lea esi,[esi-01h]
                        offset++;
                        PatchByte(_data, offset, 0x76);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000218h]
                        offset++;
                        PatchByte(_data, offset, 0x85);
                        offset++;
                        PatchByte(_data, offset, 0xE8);
                        offset++;
                        PatchByte(_data, offset, 0xFD);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0x56); // push esi
                        offset++;
                        PatchByte(_data, offset, 0x50); // push eax
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy109:
                        PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x0C);
                        offset++;
                        PatchByte(_data, offset, 0x49); // dec ecx
                        offset++;
                        PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x0C);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000410h]
                        offset++;
                        PatchByte(_data, offset, 0x8D);
                        offset++;
                        PatchByte(_data, offset, 0xF0);
                        offset++;
                        PatchByte(_data, offset, 0xFB);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy107:
                        PatchByte(_data, offset, 0x4F); // dec edi
                        offset++;
                        PatchByte(_data, offset, 0x57); // push edi
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+30h]
                        offset++;
                        PatchByte(_data, offset, 0x4C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x30);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        break;
                }
                PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
                offset++;
                PatchByte(_data, offset, 0x15);
                offset++;
                PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                    $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
                offset += 4;
                switch (_patchType)
                {
                    case PatchType.EuropaUniversalis2:
                    case PatchType.HeartsOfIron:
                    case PatchType.HeartsOfIron2:
                    case PatchType.HeartsOfIron212:
                    case PatchType.IronCrossHoI2:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
					case PatchType.DarkestHour105:
                        PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000238h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x38);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x49); // dec ecx
                        offset++;
                        PatchByte(_data, offset, 0x03); // add ecx,eax
                        offset++;
                        PatchByte(_data, offset, 0xC8);
                        offset++;
                        PatchByte(_data, offset, 0x89); // mov [esp+00000238h],ecx
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x38);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000124h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x01);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+40h]
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x40);
                        offset++;
                        break;

                    case PatchType.ForTheGlory:
                        PatchByte(_data, offset, 0x03); // add ebp,eax
                        offset++;
                        PatchByte(_data, offset, 0xE8);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000130h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x30);
                        offset++;
                        PatchByte(_data, offset, 0x01);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        break;

                    case PatchType.Victoria:
                        PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000238h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x38);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x49); // dec ecx
                        offset++;
                        PatchByte(_data, offset, 0x03); // add ecx,eax
                        offset++;
                        PatchByte(_data, offset, 0xC8);
                        offset++;
                        PatchByte(_data, offset, 0x89); // mov [esp+00000238h],ecx
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x38);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000124h]
                        offset++;
                        PatchByte(_data, offset, 0x8C);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x01);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x51); // push ecx
                        offset++;
                        PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+48h]
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x48);
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy:
                        PatchByte(_data, offset, 0x01); // add esi,eax
                        offset++;
                        PatchByte(_data, offset, 0xC6);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000418h]
                        offset++;
                        PatchByte(_data, offset, 0x85);
                        offset++;
                        PatchByte(_data, offset, 0xE8);
                        offset++;
                        PatchByte(_data, offset, 0xFB);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        PatchByte(_data, offset, 0xFF);
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy109:
                        PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x0C);
                        offset++;
                        PatchByte(_data, offset, 0x03); // add ecx,eax
                        offset++;
                        PatchByte(_data, offset, 0xC8);
                        offset++;
                        PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                        offset++;
                        PatchByte(_data, offset, 0x4D);
                        offset++;
                        PatchByte(_data, offset, 0x0C);
                        offset++;
                        PatchByte(_data, offset, 0x8B); // mov ecx,[esi+40h]
                        offset++;
                        PatchByte(_data, offset, 0x4E);
                        offset++;
                        PatchByte(_data, offset, 0x40);
                        offset++;
                        break;

                    case PatchType.ArsenalOfDemocracy107:
                        PatchByte(_data, offset, 0x03); // add edi,eax
                        offset++;
                        PatchByte(_data, offset, 0xF8);
                        offset++;
                        PatchByte(_data, offset, 0x8D); // lea edx,[esp+0000022Ch]
                        offset++;
                        PatchByte(_data, offset, 0x94);
                        offset++;
                        PatchByte(_data, offset, 0x24);
                        offset++;
                        PatchByte(_data, offset, 0x2C);
                        offset++;
                        PatchByte(_data, offset, 0x02);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        PatchByte(_data, offset, 0x00);
                        offset++;
                        break;
                }
                PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END_3
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd3),
                    $"%XX ${GetTextAddress(_posCalcLineBreakEnd3):X8} CalcLineBreakEnd3");
                offset += 4;
            }

            #endregion

            #region 改行位置計算処理4 - 用途不明

            if (_patchType == PatchType.EuropaUniversalis2)
            {
                // CALC_LINE_BREAK_START4
                PatchByte(_data, _posCalcLineBreakStart4, 0xE9); // jmp CALC_LINE_BREAK4
                PatchLong(_data, _posCalcLineBreakStart4 + 1, GetRelativeOffset(_posCalcLineBreakStart4 + 5, offset),
                    $"%XX ${GetTextAddress(offset):X8} CalcLineBreak4");

                // CALC_LINE_BREAK4
                PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                    $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
                offset += 4;
                PatchByte(_data, offset, 0x8B); // mov ecx,[esp+20h]
                offset++;
                PatchByte(_data, offset, 0x4C);
                offset++;
                PatchByte(_data, offset, 0x24);
                offset++;
                PatchByte(_data, offset, 0x20);
                offset++;
                PatchByte(_data, offset, 0x49); // dec ecx
                offset++;
                PatchByte(_data, offset, 0x51); // push ecx
                offset++;
                PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000140h]
                offset++;
                PatchByte(_data, offset, 0x8C);
                offset++;
                PatchByte(_data, offset, 0x24);
                offset++;
                PatchByte(_data, offset, 0x40);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x51); // push ecx
                offset++;
                PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
                offset++;
                PatchByte(_data, offset, 0x15);
                offset++;
                PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                    $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
                offset += 4;
                PatchByte(_data, offset, 0x8B); // mov ecx,[esp+20h]
                offset++;
                PatchByte(_data, offset, 0x4C);
                offset++;
                PatchByte(_data, offset, 0x24);
                offset++;
                PatchByte(_data, offset, 0x20);
                offset++;
                PatchByte(_data, offset, 0x49); // dec ecx
                offset++;
                PatchByte(_data, offset, 0x03); // add ecx,eax
                offset++;
                PatchByte(_data, offset, 0xC8);
                offset++;
                PatchByte(_data, offset, 0x89); // mov [esp+20h],ecx
                offset++;
                PatchByte(_data, offset, 0x4C);
                offset++;
                PatchByte(_data, offset, 0x24);
                offset++;
                PatchByte(_data, offset, 0x20);
                offset++;
                PatchByte(_data, offset, 0x8B); // mov ecx,[esi+40h]
                offset++;
                PatchByte(_data, offset, 0x4E);
                offset++;
                PatchByte(_data, offset, 0x40);
                offset++;
                PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END4
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd4),
                    $"%XX ${GetTextAddress(_posCalcLineBreakEnd4):X8} CalcLineBreakEnd4");
                offset += 4;
            }

            #endregion

            #region 改行位置計算処理5 - メッセージログ

            // CALC_LINE_BREAK_START_5
            PatchByte(_data, _posCalcLineBreakStart5, 0xE9); // jmp CALC_LINE_BREAK_5
            PatchLong(_data, _posCalcLineBreakStart5 + 1, GetRelativeOffset(_posCalcLineBreakStart5 + 5, offset),
                $"%XX ${GetTextAddress(offset):X8} CalcLineBreak5");

            // CALC_LINE_BREAK_5
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000120h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x4D); // dec ebp
                    offset++;
                    PatchByte(_data, offset, 0x55); // push ebp
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+20h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000234h] // mov     esi, [esp+22Ch+arg_4]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000124h] // 230h+var_10Bから？
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000934h] // 92Ch + arg_4から
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+24h] // 930h+var_90B
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x8D); // lea esi,[esi-01h]
                    offset++;
                    PatchByte(_data, offset, 0x76);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000218h]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0xE8);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000210h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x4F); // dec edi
                    offset++;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+20h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.Victoria:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000011Ch]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.EuropaUniversalis2:
                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000011Ch]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x03); // add ebp,eax
                    offset++;
                    PatchByte(_data, offset, 0xE8);
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000234h] // 22Ch+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000234h],ecx // 22Ch+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000120h] // 22Ch + var_10C
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000234h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000934h],ecx // 92Ch+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000020h] // 92Ch+var_90C
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x01); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xC6);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000618h]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0xE8);
                    offset++;
                    PatchByte(_data, offset, 0xF9);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[ebp-00000610h]
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0xF9);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x03); // add edi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF8);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[esp+0000021Ch]
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END_5
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd5),
                $"%XX ${GetTextAddress(_posCalcLineBreakEnd5):X8} CalcLineBreakEnd5");
            offset += 4;

            #endregion

            #region 改行位置計算処理6 - メッセージ設定画面

            // CALC_LINE_BREAK_START_6
            PatchByte(_data, _posCalcLineBreakStart6, 0xE9); // jmp CALC_LINE_BREAK_6
            PatchLong(_data, _posCalcLineBreakStart6 + 1, GetRelativeOffset(_posCalcLineBreakStart6 + 5, offset),
                $"%XX ${GetTextAddress(offset):X8} CalcLineBreak6");

            // CALC_LINE_BREAK_6
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                $"%XX ${GetTextAddress(posGetCalcLineBreakAddr):X8} GetCalcLineBreakAddr");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h] // mov     ecx, [esp+228h+arg_4]から
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000120h]　// mov     [esp+eax+22Ch+var_10B], 0　から？
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000930h] // ecx, [esp+928h+arg_4]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+00000020h] //  [esp+eax+92Ch+var_90B], から
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x8B); // mov esi,[esp+00000238h]
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x38);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x4E); // dec esi
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+20h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x8D); // lea esi,[esi-01h]
                    offset++;
                    PatchByte(_data, offset, 0x76);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000214h]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0xEC);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[ebp-00000410h]
                    offset++;
                    PatchByte(_data, offset, 0x8D);
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x4F); // dec edi
                    offset++;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+20h]
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x20);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                $"%XX ${_addrVarCalcLineBreakAddress:X8} varCalcLineBreakAddress");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.Victoria:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000011Ch]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+54h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    break;

                case PatchType.EuropaUniversalis2:
                case PatchType.HeartsOfIron2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h] // 228h+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx  // 228h+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000011Ch]  // 228h+var_10C
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.DarkestHour105:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000930h] // 928h+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx // 928h+arg_4
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x09);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000001Ch] // 928h+var_90C
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch] // そのまま
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.ForTheGlory:
                    PatchByte(_data, offset, 0x03); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000238h],esi
                    offset++;
                    PatchByte(_data, offset, 0xB4);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x38);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[esp+00000230h]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x49); // dec ecx
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [esp+00000230h],ecx
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x30);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea ecx,[esp+0000011Ch]
                    offset++;
                    PatchByte(_data, offset, 0x8C);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x01);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x01); // add esi,eax
                    offset++;
                    PatchByte(_data, offset, 0xC6);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea eax,[ebp-00000414h]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0xEC);
                    offset++;
                    PatchByte(_data, offset, 0xFB);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+0Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x03); // add ecx,eax
                    offset++;
                    PatchByte(_data, offset, 0xC8);
                    offset++;
                    PatchByte(_data, offset, 0x89); // mov [ebp+0Ch],ecx
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x0C);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[ebp-00000210h]
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    PatchByte(_data, offset, 0xF0);
                    offset++;
                    PatchByte(_data, offset, 0xFD);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x03); // add edi,eax
                    offset++;
                    PatchByte(_data, offset, 0xF8);
                    offset++;
                    PatchByte(_data, offset, 0x8D); // lea edx,[esp+0000021Ch]
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x1C);
                    offset++;
                    PatchByte(_data, offset, 0x02);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END_6
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd6),
                $"%XX ${GetTextAddress(_posCalcLineBreakEnd6):X8} CalcLineBreakEnd6");
            offset += 4;

            #endregion

            _posTextOutDcFree = offset;

            AppendLog("\n");
        }

        #endregion

        #region パッチ処理 - 文字列書き換え

        /// <summary>
        ///     WINMM.dllを_INMM.dllに書き換える
        /// </summary>
        private static void PatchWinMmDll()
        {
            AppendLog("  WINMM.dll書き換え\n");
            AppendLog($"  ${_posWinMmDll:X8}\n\n");
            _data[_posWinMmDll] = (byte) '_';
        }

        /// <summary>
        ///     IsDebuggerPresentをGetModuleHandleAに書き換える
        /// </summary>
        private static void PatchIsDebuggerPresent()
        {
            uint offset = _posIsDebuggerPresent;
            AppendLog("  IsDebuggerPresent書き換え\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'G';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'M';
            _data[offset++] = (byte) 'o';
            _data[offset++] = (byte) 'd';
            _data[offset++] = (byte) 'u';
            _data[offset++] = (byte) 'l';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'H';
            _data[offset++] = (byte) 'a';
            _data[offset++] = (byte) 'n';
            _data[offset++] = (byte) 'd';
            _data[offset++] = (byte) 'l';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'A';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     TextOutDC2を埋め込む
        /// </summary>
        private static void EmbedTextOutDc2()
        {
            uint offset = _posTextOutDc;
            AppendLog("  TextOutDC2埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'T';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'x';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'O';
            _data[offset++] = (byte) 'u';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'D';
            _data[offset++] = (byte) 'C';
            _data[offset++] = (byte) '2';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     GetTextWidthを埋め込む
        /// </summary>
        private static void EmbedGetTextWidth()
        {
            uint offset = _posGetTextWidth;
            AppendLog("  GetTextWidth埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'G';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'T';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'x';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'W';
            _data[offset++] = (byte) 'i';
            _data[offset++] = (byte) 'd';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'h';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     CalcLineBreakを埋め込む
        /// </summary>
        private static void EmbedCalcLineBreak()
        {
            uint offset = _posCalcLineBreak;
            AppendLog("  CalcLineBreak埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'C';
            _data[offset++] = (byte) 'a';
            _data[offset++] = (byte) 'l';
            _data[offset++] = (byte) 'c';
            _data[offset++] = (byte) 'L';
            _data[offset++] = (byte) 'i';
            _data[offset++] = (byte) 'n';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'B';
            _data[offset++] = (byte) 'r';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'a';
            _data[offset++] = (byte) 'k';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     strnlen0を埋め込む
        /// </summary>
        private static void EmbedStrNLen0()
        {
            uint offset = _posStrNLen;
            AppendLog("  strnlen0埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) 't';
            _data[offset++] = (byte) 'r';
            _data[offset++] = (byte) 'n';
            _data[offset++] = (byte) 'l';
            _data[offset++] = (byte) 'e';
            _data[offset++] = (byte) 'n';
            _data[offset++] = (byte) '0';
            _data[offset] = (byte) '\0';
        }

        #endregion

        #region パッチ処理 - 語順入れ替え

        /// <summary>
        ///     師団名を取得する処理を埋め込む
        /// </summary>
        private static void PatchGetDivisionName()
        {
            AppendLog("  proc GetDivisionName書き換え\n");

            uint offset;

            switch (_patchType)
            {
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                    offset = _posGetDivisionName1;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    offset = _posGetDivisionName1;
                    PatchByte(_data, offset, 0xE9); // jmp GetDivisionNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetDivisionName2 - 20),
                        $"%XX ${GetTextAddress(_posGetDivisionName2 - 20):X8} GetDivisionNameOtherCase");
                    offset += 4;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop

                    uint addrDivisionNameFormat = GetLong(_posGetDivisionName2 + 14);
                    uint offsetGetDivisionOrderName = GetLong(_posGetDivisionName2 + 19) - 4;
                    offset = _posGetDivisionName2;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov eax,[ebp-08h]
                    offset++;
                    PatchByte(_data, offset, 0x45);
                    offset++;
                    PatchByte(_data, offset, 0xF8);
                    offset++;
                    PatchByte(_data, offset, 0x99); // cdq
                    offset++;
                    PatchByte(_data, offset, 0xB9); // mov ecx,0000000Ah
                    offset++;
                    PatchByte(_data, offset, 0x0A);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0xF7); // idiv ecx
                    offset++;
                    PatchByte(_data, offset, 0xF9);
                    offset++;
                    PatchByte(_data, offset, 0x8B); // mov edx,[ebp+eax*4-40h]
                    offset++;
                    PatchByte(_data, offset, 0x54);
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    PatchByte(_data, offset, 0xC0);
                    offset++;
                    PatchByte(_data, offset, 0x52); // push edx
                    offset++;
                    PatchByte(_data, offset, 0xB9); // mov ecx,DivisionNameFormat
                    offset++;
                    PatchLong(_data, offset, addrDivisionNameFormat,
                        $"%XX ${addrDivisionNameFormat:X8} DivisionNameFormat");
                    offset += 4;
                    PatchByte(_data, offset, 0xE8); // call GetDivisionOrderName
                    offset++;
                    PatchLong(_data, offset, offsetGetDivisionOrderName,
                        $"%XX ${GetRelativeAddress(GetTextAddress(offset + 4), offsetGetDivisionOrderName):X8} GetDivisionOrderName");
                    break;

                case PatchType.ArsenalOfDemocracy:
                    uint addrDivisionAbbrev = GetLong(_posGetDivisionName1);
                    PatchLong(_data, _posGetDivisionName1, addrDivisionAbbrev + 2,
                        $"%XX ${(addrDivisionAbbrev + 2):X8} addrDivisionAbbrev");
                    break;
            }

            AppendLog("\n");
        }

        /// <summary>
        ///     軍団名を取得する処理を埋め込む
        /// </summary>
        private static void PatchGetArmyName()
        {
            AppendLog("  proc GetArmyName書き換え\n");

            uint offset;

            switch (_patchType)
            {
                case PatchType.Victoria:
                    offset = _posGetArmyName1;
                    PatchByte(_data, offset, 0x53); // push ebx
                    offset++;
                    PatchByte(_data, offset, 0xE9); // jmp GetArmyNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetArmyName2),
                        $"%XX ${GetTextAddress(_posGetArmyName2):X8} GetArmyNameOtherCase");

                    offset = _posGetArmyName2 + 25;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    break;

                case PatchType.HeartsOfIron:
                    offset = _posGetArmyName1;
                    PatchByte(_data, offset, 0x57); // push edi
                    offset++;
                    PatchByte(_data, offset, 0xE9); // jmp GetArmyNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetArmyName2),
                        $"%XX ${GetTextAddress(_posGetArmyName2):X8} GetArmyNameOtherCase");

                    offset = _posGetArmyName2 + 25;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy109:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    offset = _posGetArmyName1;
                    PatchByte(_data, offset, 0xE9); // jmp GetArmyNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetArmyName2 - 13),
                        $"%XX ${GetTextAddress(_posGetArmyName2 - 13):X8} GetArmyNameOtherCase");
                    offset += 4;
                    PatchByte(_data, offset, 0x90); // nop

                    uint addrArmyNameFormat = GetLong(_posGetArmyName2 + 17);
                    uint offsetGetArmyOrderName = GetLong(_posGetArmyName2 + 22) - 7;
                    offset = _posGetArmyName2;
                    PatchByte(_data, offset, 0x50); // push eax
                    offset++;
                    PatchByte(_data, offset, 0x8B);
                    // mov eax,[ebp-00000130h]: DH / mov eax,[ebp-00000230h]: AoD / mov eax,[bsp-0000012Ch]:HoI2,AoD104
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    switch (_patchType)
                    {
                        case PatchType.HeartsOfIron2:
                        case PatchType.HeartsOfIron212:
                        case PatchType.IronCrossHoI2:
                        case PatchType.ArsenalOfDemocracy104:
                            PatchByte(_data, offset, 0xD4);
                            offset++;
                            PatchByte(_data, offset, 0xFE);
                            break;
                        case PatchType.ArsenalOfDemocracy:
                        case PatchType.ArsenalOfDemocracy107:
                        case PatchType.ArsenalOfDemocracy109:
                            PatchByte(_data, offset, 0xD0);
                            offset++;
                            PatchByte(_data, offset, 0xFD);
                            break;
                        case PatchType.DarkestHour:
                        case PatchType.DarkestHour102:
                            PatchByte(_data, offset, 0xD0);
                            offset++;
                            PatchByte(_data, offset, 0xFE);
                            break;
                    }
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x99); // cdq
                    offset++;
                    PatchByte(_data, offset, 0xB9); // mov ecx,0000000Ah
                    offset++;
                    PatchByte(_data, offset, 0x0A);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    PatchByte(_data, offset, 0xF7); // idiv ecx
                    offset++;
                    PatchByte(_data, offset, 0xF9);
                    offset++;
                    PatchByte(_data, offset, 0x8B);
                    // mov edx,[ebp+eax*4-00000158h]: DH / mov eax,[ebp+eax*4-0000025Ch]: AoD / mov eas,[ebp+eax*4-00000154h]: HoI2,AoD104
                    offset++;
                    PatchByte(_data, offset, 0x94);
                    offset++;
                    PatchByte(_data, offset, 0x95);
                    offset++;
                    switch (_patchType)
                    {
                        case PatchType.HeartsOfIron2:
                        case PatchType.HeartsOfIron212:
                        case PatchType.IronCrossHoI2:
                        case PatchType.ArsenalOfDemocracy104:
                            PatchByte(_data, offset, 0xAC);
                            offset++;
                            PatchByte(_data, offset, 0xFE);
                            break;
                        case PatchType.ArsenalOfDemocracy:
                        case PatchType.ArsenalOfDemocracy107:
                        case PatchType.ArsenalOfDemocracy109:
                            PatchByte(_data, offset, 0xA4);
                            offset++;
                            PatchByte(_data, offset, 0xFD);
                            break;
                        case PatchType.DarkestHour:
                        case PatchType.DarkestHour102:
                            PatchByte(_data, offset, 0xA8);
                            offset++;
                            PatchByte(_data, offset, 0xFE);
                            break;
                    }
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x52); // push edx
                    offset++;
                    PatchByte(_data, offset, 0xB9); // mov ecx,ArmyNameFormat
                    offset++;
                    PatchLong(_data, offset, addrArmyNameFormat,
                        $"%XX ${addrArmyNameFormat:X8} ArmyNameFormat");
                    offset += 4;
                    PatchByte(_data, offset, 0xE8); // call GetArmyOrderName
                    offset++;
                    PatchLong(_data, offset, offsetGetArmyOrderName,
                        $"%XX ${GetRelativeAddress(GetTextAddress(offset + 4), offsetGetArmyOrderName):X8} GetArmyOrderName");
                    break;

                case PatchType.ArsenalOfDemocracy:
                    uint addrBufOrdinal = GetLong(_posGetArmyName1 + 5);
                    uint addrPutNumber = GetLong(_posGetArmyName1 + 11);
                    offset = _posGetArmyName1;
                    PatchByte(_data, offset, 0xEB); // jmp GET_ORDINAL_SUFFIX
                    offset++;
                    PatchByte(_data, offset, 0x0D);
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop
                    offset++;
                    PatchByte(_data, offset, 0x90); // nop

                    offset = _posGetArmyName2;
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+08h]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x08);
                    offset++;
                    PatchByte(_data, offset, 0x51); // push ecx
                    offset++;
                    PatchByte(_data, offset, 0xB9); // mov ecx,addrBufOrdinal
                    offset++;
                    PatchLong(_data, offset, addrBufOrdinal, $"%XX ${addrBufOrdinal:X8} addrBufOrdinal");
                    offset += 4;
                    PatchByte(_data, offset, 0xFF); // call PutNumber
                    offset++;
                    PatchByte(_data, offset, 0x15);
                    offset++;
                    PatchLong(_data, offset, addrPutNumber, $"%XX ${addrPutNumber:X8} addrPutNumber");
                    break;
            }


            AppendLog("\n");
        }

        /// <summary>
        ///     国家序列を取得する処理を埋め込む
        /// </summary>
        private static void PatchGetRankingName()
        {
            AppendLog("  proc GetRankingName書き換え\n");

            uint offset = _posGetRankingName1;
            PatchByte(_data, offset, 0xE9); // jmp GetRankingNameOtherCase
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetRankingName2),
                $"%XX ${GetTextAddress(_posGetRankingName2):X8} GetRankingNameOtherCase");

            offset = _posGetRankingName2;
            PatchByte(_data, offset, 0x68); // push NATION_RANKING
            offset++;
            PatchLong(_data, offset, _addrRankingSuffix,
                $"%XX ${_addrRankingSuffix:X8} push RANKING_SUFFIX");

            AppendLog("\n");
        }

        /// <summary>
        ///     師団名の書式文字列埋め込み
        /// </summary>
        private static void EmbedDivisionNameFormat()
        {
            uint offset = _posDivisionNameFormat;
            AppendLog("  師団名の書式文字列埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 'd';
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) '\0';
            _data[offset++] = (byte) '\0';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     軍団名の書式文字列埋め込み
        /// </summary>
        private static void EmbedArmyNameFormat()
        {
            uint offset = _posArmyNameFormat;
            AppendLog("  軍団名の書式文字列埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 'd';
            _data[offset++] = (byte) '%';
            _data[offset++] = (byte) 's';
            _data[offset++] = (byte) '\0';
            _data[offset] = (byte) '\0';
        }


        /// <summary>
        ///     NATION_RANKING埋め込み
        /// </summary>
        private static void EmbedRankingSuffix()
        {
            uint offset = _posRankingSuffix;
            AppendLog("  RANKING_SUFFIX埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'R';
            _data[offset++] = (byte) 'A';
            _data[offset++] = (byte) 'N';
            _data[offset++] = (byte) 'K';
            _data[offset++] = (byte) 'I';
            _data[offset++] = (byte) 'N';
            _data[offset++] = (byte) 'G';
            _data[offset++] = (byte) '_';
            _data[offset++] = (byte) 'S';
            _data[offset++] = (byte) 'U';
            _data[offset++] = (byte) 'F';
            _data[offset++] = (byte) 'F';
            _data[offset++] = (byte) 'I';
            _data[offset++] = (byte) 'X';
            _data[offset] = (byte) '\0';
        }

        #endregion

        #region パッチ処理 - ゲーム設定

        /// <summary>
        ///     強制ウィンドウ化処理を埋め込む
        /// </summary>
        private static void PatchWindowed()
        {
            AppendLog("  push 強制ウィンドウ化処理埋め込み\n");
            uint offset = _posWindowed1;
            switch (_patchType)
            {
                case PatchType.CrusaderKings:
                case PatchType.EuropaUniversalis2:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                    PatchByte(_data, offset, 0x01);
                    break;

                case PatchType.HeartsOfIron2:
                    PatchByte(_data, offset, 0xE9); // jmp SetWindowed
                    offset++;
                    PatchLong(_data, offset, _posWindowed2 - (offset + 4),
                        $"%XX ${GetTextAddress(_posWindowed2):X8} SetWindowed");
                    offset += 4;
                    PatchByte(_data, offset, 0x90); // nop
                    break;

                case PatchType.HeartsOfIron212:
                    PatchByte(_data, offset, 0xEB); // jmp SetWindowed
                    offset++;
                    PatchByte(_data, offset, (byte) (_posWindowed2 - (offset + 1)));
                    break;
            }
            AppendLog("\n");
        }

        /// <summary>
        ///     binkplay.exeを_inkplay.exeに書き換える
        /// </summary>
        private static void PatchBinkPlay()
        {
            AppendLog("  binkplay.exe書き換え\n");
            AppendLog($"  ${_posBinkPlay1:X8}\n");
            _data[_posBinkPlay1] = (byte) '_';
            if (_patchType == PatchType.CrusaderKings || _patchType == PatchType.EuropaUniversalis2)
            {
                AppendLog($"  ${_posBinkPlay2:X8}\n");
                _data[_posBinkPlay2] = (byte) '_';
            }
            AppendLog("\n");
        }

        /// <summary>
        ///     時間制限解除処理を埋め込む
        /// </summary>
        private static void PatchNtl()
        {
            AppendLog("  push 時間制限解除処理埋め込み\n");

            PatchByte(_data, _posMaxYear1, 0x0F);
            PatchByte(_data, _posMaxYear1 + 1, 0x27);
            PatchByte(_data, _posMaxYear2, 0x0F);
            PatchByte(_data, _posMaxYear2 + 1, 0x27);
            PatchByte(_data, _posMinYear1, 0x01);
            PatchByte(_data, _posMinYear1 + 1, 0x00);
            PatchByte(_data, _posMinYear2, 0x01);
            PatchByte(_data, _posMinYear2 + 1, 0x00);
            switch (_patchType)
            {
                case PatchType.EuropaUniversalis2:
                    PatchByte(_data, _posLimitYear, 0x0F);
                    PatchByte(_data, _posLimitYear + 1, 0x27);
                    break;

                case PatchType.CrusaderKings:
                case PatchType.Victoria:
                case PatchType.HeartsOfIron:
                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                    PatchByte(_data, _posLimitYear, 0x10);
                    PatchByte(_data, _posLimitYear + 1, 0x27);
                    break;
            }

            AppendLog("\n");
        }

        /// <summary>
        ///     4GBメモリ使用設定に変更する
        /// </summary>
        private static void Patch4Gb()
        {
            const uint offset = 0x00000126;

            AppendLog("  4GBメモリ使用設定\n");

            PatchByte(_data, offset, (byte) (_data[offset] | 0x20));

            AppendLog("\n");
        }

        #endregion

        #region パッチ処理 - ゲーム固有

        /// <summary>
        ///     EE_MAX_AMPHIB_MODの呼び出しをEE_MAX_AMPHIB_MOD_TITLEに書き換える
        /// </summary>
        private static void PatchPushEeMaxAmphibModTitle()
        {
            AppendLog("  push EE_MAX_AMPHIB_MOD書き換え\n");
            uint offset = _posPushEeMaxAmphibModTitle;
            PatchLong(_data, offset, _addrEeMaxAmphibModTitle,
                $"%XX ${_addrEeMaxAmphibModTitle:X8} push EE_MAX_AMPHIB_MOD");
            AppendLog("\n");
        }

        /// <summary>
        ///     EE_MAX_AMPHIB_MOD_TITLE埋め込み
        /// </summary>
        private static void EmbedEeMaxAmphibModTitle()
        {
            uint offset = _posEeMaxAmphibModTitle;
            AppendLog("  EE_MAX_AMPHIB_MOD_TITLE埋め込み\n");
            AppendLog($"  ${offset:X8}\n\n");
            _data[offset++] = (byte) 'E';
            _data[offset++] = (byte) 'E';
            _data[offset++] = (byte) '_';
            _data[offset++] = (byte) 'M';
            _data[offset++] = (byte) 'A';
            _data[offset++] = (byte) 'X';
            _data[offset++] = (byte) '_';
            _data[offset++] = (byte) 'A';
            _data[offset++] = (byte) 'M';
            _data[offset++] = (byte) 'P';
            _data[offset++] = (byte) 'H';
            _data[offset++] = (byte) 'I';
            _data[offset++] = (byte) 'B';
            _data[offset++] = (byte) '_';
            _data[offset++] = (byte) 'M';
            _data[offset++] = (byte) 'O';
            _data[offset++] = (byte) 'D';
            _data[offset++] = (byte) '_';
            _data[offset++] = (byte) 'T';
            _data[offset++] = (byte) 'I';
            _data[offset++] = (byte) 'T';
            _data[offset++] = (byte) 'L';
            _data[offset++] = (byte) 'E';
            _data[offset] = (byte) '\0';
        }

        /// <summary>
        ///     モデル名の終端文字設定処理書き換え
        /// </summary>
        private static void PatchTermModelName()
        {
            AppendLog("  proc モデル名の終端文字設定処理書き換え\n");

            // GET_STRNLEN0_ADDR
            uint offset = _posTextOutDcFree;
            uint posGetStrNLenAddr = offset;
            PatchByte(_data, offset, 0x83); // cmp [varStrNLenAddress],0
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarStrNLenAddress,
                $"%XX ${_addrVarStrNLenAddress:X8} varStrNLenAddress");
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_STRNLEN0_ADDR_3
            offset++;
            PatchByte(_data, offset, 0x26);
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, $"%06 ${_addrWinMmDll:X8} WINMM.dll");
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA/IsDebuggerPresent
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy109:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        $"%04 ${_addrIsDebuggerPresent:X8} IsDebuggerPresent");
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        $"%04 ${_addrGetModuleHandleA:X8} GetModuleHandleA");
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_STRNLEN0_ADDR_1
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_STRNLEN0_ADDR_1
            PatchByte(_data, offset, 0x68); // push addrStrNLen
            offset++;
            PatchLong(_data, offset, _addrStrNLen, $"%00 ${_addrCalcLineBreak:X8} strnlen0");
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                $"%05 ${_addrGetProcAddress:X8} GetProcAddress");
            offset += 4;
            PatchByte(_data, offset, 0x85); // test eax,eax
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_STRNLEN0_ADDR_2
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4); // hlt
            offset++;
            // GET_STRNLEN0_ADDR_2
            PatchByte(_data, offset, 0xA3); // mov [varStrNLenAddress],eax
            offset++;
            PatchLong(_data, offset, _addrVarStrNLenAddress,
                $"%XX ${_addrVarStrNLenAddress:X8} varStrNLenAddress");
            offset += 4;
            // GET_STRNLEN0_ADDR_3
            PatchByte(_data, offset, 0xC3); // retn
            offset++;

            // TERM_MODEL_NAME
            uint posTermModelName = offset;
            PatchByte(_data, offset, 0xE8); // call GET_STRNLEN0_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetStrNLenAddr),
                $"%XX ${GetTextAddress(posGetStrNLenAddr):X8} GetStrNLenAddr");
            offset += 4;
            PatchByte(_data, offset, 0x6A); // push 0000001Ch
            offset++;
            PatchByte(_data, offset, 0x1C);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0x8D); // lea eax,[ebp-CCh]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0x8D); // lea eax, byte ptr [ebp-90h]
                    offset++;
                    PatchByte(_data, offset, 0x85);
                    offset++;
                    PatchByte(_data, offset, 0x70);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x8D); // lea eax, byte ptr [esp+50h]
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x50);
                    offset++;
                    break;

                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0x8D); // lea eax, byte ptr [esp+48h]
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x48);
                    offset++;
                    break;

                case PatchType.DarkestHour:
                    PatchByte(_data, offset, 0x8D); // lea eax, byte ptr [esp+4Ch]
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x24);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varStrNLenAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarStrNLenAddress,
                $"%02 ${_addrVarStrNLenAddress:X8} varStrNLenAddress");
            offset += 4;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    PatchByte(_data, offset, 0xC6); // mov byte ptr [ebp+eax-CCh],0
                    offset++;
                    PatchByte(_data, offset, 0x84);
                    offset++;
                    PatchByte(_data, offset, 0x28);
                    offset++;
                    PatchByte(_data, offset, 0x34);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy109:
                    PatchByte(_data, offset, 0xC6); // mov [ebp+eax-90h],0
                    offset++;
                    PatchByte(_data, offset, 0x84);
                    offset++;
                    PatchByte(_data, offset, 0x28);
                    offset++;
                    PatchByte(_data, offset, 0x70);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0xFF);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0xC6); // mov [esp+eax+4Ch],0
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x04);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0xC6); // mov [esp+eax+44h],0
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x04);
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;

                case PatchType.DarkestHour:
                    PatchByte(_data, offset, 0xC6); // mov [esp+eax+48h],0
                    offset++;
                    PatchByte(_data, offset, 0x44);
                    offset++;
                    PatchByte(_data, offset, 0x04);
                    offset++;
                    PatchByte(_data, offset, 0x48);
                    offset++;
                    PatchByte(_data, offset, 0x00);
                    offset++;
                    break;
            }
            PatchByte(_data, offset, 0xC3); // retn
            offset++;

            uint posTermModelName2 = offset;
            if (_patchType == PatchType.DarkestHour)
            {
                // TERM_MODEL_NAME2
                PatchByte(_data, offset, 0xE8); // call GET_STRNLEN0_ADDR
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetStrNLenAddr),
                    $"%XX ${GetTextAddress(posGetStrNLenAddr):X8} GetStrNLenAddr");
                offset += 4;
                PatchByte(_data, offset, 0x6A); // push 0000001Ch
                offset++;
                PatchByte(_data, offset, 0x1C);
                offset++;
                PatchByte(_data, offset, 0x8D); // lea eax, byte ptr [esp+60h]
                offset++;
                PatchByte(_data, offset, 0x44);
                offset++;
                PatchByte(_data, offset, 0x24);
                offset++;
                PatchByte(_data, offset, 0x60);
                offset++;
                PatchByte(_data, offset, 0x50); // push eax
                offset++;
                PatchByte(_data, offset, 0xFF); // call [varStrNLenAddress]
                offset++;
                PatchByte(_data, offset, 0x15);
                offset++;
                PatchLong(_data, offset, _addrVarStrNLenAddress,
                    $"%02 ${_addrVarStrNLenAddress:X8} varStrNLenAddress");
                offset += 4;
                PatchByte(_data, offset, 0xC6); // mov [esp+eax+5Ch],0
                offset++;
                PatchByte(_data, offset, 0x44);
                offset++;
                PatchByte(_data, offset, 0x04);
                offset++;
                PatchByte(_data, offset, 0x5C);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0xC3); // retn
                offset++;
            }

            // TERM_MODEL_NAME_START1
            if (_patchType == PatchType.ArsenalOfDemocracy109)
            {
                // TERM_MODEL_NAME_START1
                PatchByte(_data, _posTermModelNameStart1, 0xE9); // jmp TERM_MODEL_NAME
                PatchLong(_data, _posTermModelNameStart1 + 1, GetRelativeOffset(_posTermModelNameStart1 + 5, offset),
                    $"%XX ${GetTextAddress(offset):X8} TermBrigModelName");
                PatchByte(_data, _posTermModelNameStart1 + 5, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 6, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 7, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 8, 0x90); // nop

                // TERM_BRIG_MODEL_NAME
                PatchByte(_data, offset, 0xE8); // call TERM_MODEL_NAME
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posTermModelName),
                    $"%08 ${GetTextAddress(posTermModelName):X8} TermModelName");
                offset += 4;
                PatchByte(_data, offset, 0x68); // push 000000FFh
                offset++;
                PatchByte(_data, offset, 0xFF);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0xE9); // jmp TermBrigModelNameEnd
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posTermModelNameStart1 + 9),
                    $"%08 ${GetTextAddress(_posTermModelNameStart1 + 9):X8} TermBrigModelNameEnd");
                offset += 4;

                // TERM_MODEL_NAME_START2
                PatchByte(_data, _posTermModelNameStart2, 0x89); // mov [ebp-00000094h],eax
                PatchByte(_data, _posTermModelNameStart2 + 1, 0x85);
                PatchByte(_data, _posTermModelNameStart2 + 2, 0x6C);
                PatchByte(_data, _posTermModelNameStart2 + 3, 0xFF);
                PatchByte(_data, _posTermModelNameStart2 + 4, 0xFF);
                PatchByte(_data, _posTermModelNameStart2 + 5, 0xFF);
                _posTermModelNameStart2 += 6;
                PatchByte(_data, _posTermModelNameStart2, 0xE9); // jmp TERM_DIV_MODEL_NAME
                PatchLong(_data, _posTermModelNameStart2 + 1, GetRelativeOffset(_posTermModelNameStart2 + 5, offset),
                    $"%XX ${GetTextAddress(offset):X8} TermDivModelName");
                PatchByte(_data, _posTermModelNameStart2 + 5, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 6, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 7, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 8, 0x90); // nop

                // TERM_DIV_MODEL_NAME
                PatchByte(_data, offset, 0xE8); // call TERM_MODEL_NAME
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posTermModelName),
                    $"%08 ${GetTextAddress(posTermModelName):X8} TermModelName");
                offset += 4;
                PatchByte(_data, offset, 0x68); // push 000000FFh
                offset++;
                PatchByte(_data, offset, 0xFF);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0xE9); // jmp TermDivModelNameEnd
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posTermModelNameStart2 + 9),
                    $"%08 ${GetTextAddress(_posTermModelNameStart2 + 9):X8} TermDivModelNameEnd");
                offset += 4;
            }
            else
            {
                // TERM_MODEL_NAME_START1
                if (_patchType == PatchType.ArsenalOfDemocracy107)
                {
                    PatchByte(_data, _posTermModelNameStart1, 0x89); // mov [esp+20h],edx
                    _posTermModelNameStart1++;
                    PatchByte(_data, _posTermModelNameStart1, 0x54);
                    _posTermModelNameStart1++;
                    PatchByte(_data, _posTermModelNameStart1, 0x24);
                    _posTermModelNameStart1++;
                    PatchByte(_data, _posTermModelNameStart1, 0x20);
                    _posTermModelNameStart1++;
                }
                PatchByte(_data, _posTermModelNameStart1, 0xE8); // call TERM_MODEL_NAME
                _posTermModelNameStart1++;
                PatchLong(_data, _posTermModelNameStart1,
                    GetRelativeOffset(_posTermModelNameStart1 + 4, posTermModelName),
                    $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");
                _posTermModelNameStart1 += 4;

                if (_patchType == PatchType.ArsenalOfDemocracy)
                {
                    PatchByte(_data, _posTermModelNameStart1, 0x90); // nop
                    _posTermModelNameStart1++;
                    PatchByte(_data, _posTermModelNameStart1, 0x90); // nop
                }

                // TERM_MODEL_NAME_START2
                PatchByte(_data, _posTermModelNameStart2, 0xE8); // call TERM_MODEL_NAME
                _posTermModelNameStart2++;
                PatchLong(_data, _posTermModelNameStart2,
                    GetRelativeOffset(_posTermModelNameStart2 + 4, posTermModelName),
                    $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");
                _posTermModelNameStart2 += 4;

                if (_patchType == PatchType.ArsenalOfDemocracy)
                {
                    PatchByte(_data, _posTermModelNameStart2, 0x90); // nop
                    _posTermModelNameStart2++;
                    PatchByte(_data, _posTermModelNameStart2, 0x90); // nop
                }

                if (_patchType == PatchType.DarkestHour)
                {
                    // TERM_MODEL_NAME_START3
                    PatchByte(_data, _posTermModelNameStart3, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart3 + 1,
                        GetRelativeOffset(_posTermModelNameStart3 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START4
                    PatchByte(_data, _posTermModelNameStart4, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart4 + 1,
                        GetRelativeOffset(_posTermModelNameStart4 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START5
                    PatchByte(_data, _posTermModelNameStart5, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart5 + 1,
                        GetRelativeOffset(_posTermModelNameStart5 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START6
                    PatchByte(_data, _posTermModelNameStart6, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart6 + 1,
                        GetRelativeOffset(_posTermModelNameStart6 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START7
                    PatchByte(_data, _posTermModelNameStart7, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart7 + 1,
                        GetRelativeOffset(_posTermModelNameStart7 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START8
                    PatchByte(_data, _posTermModelNameStart8, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart8 + 1,
                        GetRelativeOffset(_posTermModelNameStart8 + 5, posTermModelName),
                        $"%XX ${GetTextAddress(posTermModelName):X8} TermModelName");

                    // TERM_MODEL_NAME_START9
                    PatchByte(_data, _posTermModelNameStart9, 0xE8); // call TERM_MODEL_NAME2
                    PatchLong(_data, _posTermModelNameStart9 + 1,
                        GetRelativeOffset(_posTermModelNameStart9 + 5, posTermModelName2),
                        $"%XX ${GetTextAddress(posTermModelName2):X8} TermModelName2");

                    // TERM_MODEL_NAME_START10
                    PatchByte(_data, _posTermModelNameStart10, 0xE8); // call TERM_MODEL_NAME2
                    PatchLong(_data, _posTermModelNameStart10 + 1,
                        GetRelativeOffset(_posTermModelNameStart10 + 5, posTermModelName2),
                        $"%XX ${GetTextAddress(posTermModelName2):X8} TermModelName2");
                }
            }

            _posTextOutDcFree = offset;

            AppendLog("\n");
        }

        #endregion

        #region ユーティリティ関数群

        /// <summary>
        ///     バイナリ列を探索する
        /// </summary>
        /// <param name="target">探索対象のデータ</param>
        /// <param name="pattern">探索するバイトパターン</param>
        /// <param name="start">開始位置</param>
        /// <param name="size">探索するバイトサイズ</param>
        /// <returns>探索に成功すればtrueを返す</returns>
        private static List<uint> BinaryScan(byte[] target, byte[] pattern, uint start, uint size)
        {
            StringBuilder sb = new StringBuilder("  Binary:");
            foreach (byte b in pattern)
            {
                sb.AppendFormat(" {0:X2}", b);
            }
            sb.AppendLine();
            AppendLog(sb.ToString());
            AppendLog($"  ファイル上の開始位置: ${start:X8}\n");
            AppendLog($"  ファイル上の終了位置: ${start + size - 1:X8}\n");

            AppendLog($"  検索範囲 ${start:X8}～${start + size - 1:X8} (のこり{size}Bytes)\n");
            List<uint> result = new List<uint>();
            for (uint offset = start; offset <= start + size - pattern.Length; offset++)
            {
                if (IsBinaryMatch(target, pattern, offset))
                {
                    result.Add(offset);
                    AppendLog($"  *** Find ${offset:X8}\n");
                    AppendLog($"  検索範囲 ${offset + 1:X8}～${start + size - 1:X8} (のこり{start + size - offset - 1}Bytes)\n");
                }
            }
            return result;
        }

        /// <summary>
        ///     バイナリ列が一致しているかを判定する
        /// </summary>
        /// <param name="target">探索対象のデータ</param>
        /// <param name="pattern">探索するバイトパターン</param>
        /// <param name="offset">判定する位置</param>
        /// <returns>バイナリ列が一致していればtrueを返す</returns>
        private static bool IsBinaryMatch(byte[] target, byte[] pattern, uint offset)
        {
            int i;
            for (i = 0; i < pattern.Length; i++)
            {
                if (target[offset + i] != pattern[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     1バイトにパッチを当てる
        /// </summary>
        /// <param name="target">パッチ対象のデータ</param>
        /// <param name="offset">パッチを当てる位置</param>
        /// <param name="val">書き換える値</param>
        private static void PatchByte(byte[] target, uint offset, byte val)
        {
            AppendLog($"  ${offset:X8} {target[offset]:X2}->{val:X2}\n");
            target[offset] = val;
        }

        /// <summary>
        ///     4バイトにパッチを当てる
        /// </summary>
        /// <param name="target">パッチ対象のデータ</param>
        /// <param name="offset">パッチを当てる位置</param>
        /// <param name="val">書き換える値</param>
        /// <param name="message">ログ出力するメッセージ</param>
        private static void PatchLong(byte[] target, uint offset, uint val, string message)
        {
            AppendLog($"  ${offset:X8} {GetLong(offset):X8}->{val:X8} ({message})\n");
            target[offset] = (byte) (val & 0x000000FF);
            target[offset + 1] = (byte) ((val & 0x0000FF00) >> 8);
            target[offset + 2] = (byte) ((val & 0x00FF0000) >> 16);
            target[offset + 3] = (byte) ((val & 0xFF000000) >> 24);
        }

        /// <summary>
        ///     2バイトデータを取得する
        /// </summary>
        /// <param name="offset">取得する位置</param>
        /// <returns>取得した値</returns>
        private static ushort GetWord(uint offset)
        {
            return (ushort) (_data[offset] + _data[offset + 1] * 0x0100);
        }

        /// <summary>
        ///     4バイトデータを取得する
        /// </summary>
        /// <param name="offset">取得する位置</param>
        /// <returns>取得した値</returns>
        private static uint GetLong(uint offset)
        {
            return
                (uint)
                    (_data[offset] + _data[offset + 1] * 0x00000100 + _data[offset + 2] * 0x00010000 +
                     _data[offset + 3] * 0x01000000);
        }

        /// <summary>
        ///     文字列データを取得する
        /// </summary>
        /// <param name="offset">取得する位置</param>
        /// <param name="max">取得する最大値</param>
        /// <returns>取得した値</returns>
        private static string GetString(uint offset, int max)
        {
            string result = "";
            for (int i = 0;; i++)
            {
                if (max > 0 && i >= max)
                {
                    break;
                }
                if (_data[offset + i] == 0x00)
                {
                    break;
                }
                result += (char) _data[offset + i];
            }
            return result;
        }

        /// <summary>
        ///     ファイル上の位置を.textセクションのアドレスに変換する
        /// </summary>
        /// <param name="pos">ファイル上の位置</param>
        /// <returns>変換したアドレス</returns>
        private static uint GetTextAddress(uint pos)
        {
            return _addrBase + _addrTextSection - _posTextSection + pos;
        }

        /// <summary>
        ///     ファイル上の位置を.dataセクションのアドレスに変換する
        /// </summary>
        /// <param name="pos">ファイル上の位置</param>
        /// <returns>変換したアドレス</returns>
        private static uint GetDataAddress(uint pos)
        {
            return _addrBase + _addrDataSection - _posDataSection + pos;
        }

        /// <summary>
        ///     ファイル上の位置を.rdataセクションのアドレスに変換する
        /// </summary>
        /// <param name="pos">ファイル上の位置</param>
        /// <returns>変換したアドレス</returns>
        private static uint GetRdataAddress(uint pos)
        {
            return _addrBase + _addrRdataSection - _posRdataSection + pos;
        }

        /// <summary>
        ///     ファイル上の位置を.idataセクションのアドレスに変換する
        /// </summary>
        /// <param name="pos">ファイル上の位置</param>
        /// <returns>変換したアドレス</returns>
        private static uint GetIdataAddress(uint pos)
        {
            return _addrBase + _addrIdataSection - _posIdataSection + pos;
        }

        /// <summary>
        ///     相対参照時のオフセットを求める
        /// </summary>
        /// <param name="current">現在のアドレス/ファイル上の位置</param>
        /// <param name="target">参照先のアドレス/ファイル上の位置</param>
        /// <returns>計算結果のオフセット</returns>
        private static uint GetRelativeOffset(uint current, uint target)
        {
            if (target < current)
            {
                return ~(current - target) + 1;
            }
            return target - current;
        }

        /// <summary>
        ///     相対参照時のアドレスを求める
        /// </summary>
        /// <param name="current">現在のアドレス/ファイル上の位置</param>
        /// <param name="offset">オフセット</param>
        /// <returns>計算結果のアドレス</returns>
        private static uint GetRelativeAddress(uint current, uint offset)
        {
            if (offset >= 0x80000000)
            {
                return current - (~offset + 1);
            }
            return current + offset;
        }

        /// <summary>
        ///     指定した値を切り上げる
        /// </summary>
        /// <param name="val">切り上げ対象の値</param>
        /// <param name="unit">切り上げる単位</param>
        /// <returns>切り上げた値</returns>
        private static uint Ceiling(uint val, uint unit)
        {
            uint result = (val + unit - 1) / unit;
            result *= unit;
            return result;
        }

        /// <summary>
        ///     ログを追記する
        /// </summary>
        /// <param name="s">追記する文字列</param>
        private static void AppendLog(string s)
        {
            MainForm.AppendLog(s);
        }

        #endregion
    }

    /// <summary>
    ///     パッチの種類
    /// </summary>
    public enum PatchType
    {
        Unknown, // 不明
        CrusaderKings, // Crusader Kings
        EuropaUniversalis2, // Europa Universalis 2
        ForTheGlory, // For The Glory
        Victoria, // Victoria
        HeartsOfIron, // Hearts of Iron
        HeartsOfIron2, // Hearts of Iron 2 1.3-
        ArsenalOfDemocracy, // Arsenal of Democracy 1.10-
        DarkestHour105, // Darkest Hour 1.05
        DarkestHour, // Darkest Hour 1.03-
        ArsenalOfDemocracy104, // Arsenal of Democracy 1.02-1.04
        ArsenalOfDemocracy107, // Arsenal of Democracy 1.05-1.07
        ArsenalOfDemocracy109, // Arsenal of Democracy 1.08-1.09
        DarkestHour102, // Darkest Hour 1.00-1.02
        HeartsOfIron212, // Hearts of Iron 2 1.2
        IronCrossHoI2 // Iron Cross over Hearts of Iron 2
    }
}