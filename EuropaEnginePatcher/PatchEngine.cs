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
            AppendLog(string.Format("Europe Engine Patcher Ver {0}\n", EuropaEnginePatcher.VersionName));

            SetPatchType(gameType);
            if (_patchType == PatchType.Unknown)
            {
                MessageBox.Show("パッチの種類が判別できません。", "エラー");
                return false;
            }

            try
            {
                ReadGameFile(fileName);

                if (!IdentifyGameVersion())
                {
                    return false;
                }
                if (!ParseHeader())
                {
                    return false;
                }
                if (!ScanPatchLocation())
                {
                    return false;
                }
                PatchBinary();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー");
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
            using (var s = new FileStream(fileName, FileMode.Create, FileAccess.Write))
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

            var info = new FileInfo(fileName);
            _fileSize = info.Length;

            _data = new byte[_fileSize];

            using (FileStream s = info.OpenRead())
            {
                s.Read(_data, 0, (int) _fileSize);
                s.Close();
            }

            AppendLog(string.Format("  TargetFile: {0}\n", fileName));
            AppendLog(string.Format("  FileSize: {0}Bytes\n", _fileSize));

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
                        _gameVersion = (_data[offset] - '0')*100 + (_data[offset + 2] - '0')*10 +
                                       (_data[offset + 3] - '0');
                    }
                    else
                    {
                        offset = l[0] + (uint) pattern.Length;
                        _gameVersion = (_data[offset] - '0')*100 + (_data[offset + 2] - '0')*10;
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
                    _gameVersion = (_data[offset] - '0')*100 + (_data[offset + 2] - '0')*10 + (_data[offset + 3] - '0');
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
                    _gameVersion = (_data[offset] - '0')*100 + (_data[offset + 2] - '0')*10 + (_data[offset + 3] - '0');
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
                    _gameVersion = (_data[offset] - '0')*100 + (_data[offset + 2] - '0')*10;
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
                    else
                    {
                        _patchType = PatchType.ArsenalOfDemocracy;
                        AppendLog("PatchType: Arsenal of Democracy\n\n");
                    }
                    break;

                case PatchType.DarkestHour:
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
            AppendLog(string.Format("  PEの位置: {0:X8}\n", _posPeHeader));

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
            AppendLog(string.Format("  セクション数: {0}\n", _countSections));

            uint posSymbolTable = GetLong(_posPeHeader + 12);
            uint countSymbols = GetLong(_posPeHeader + 16);
            AppendLog(string.Format("  シンボルテーブルの位置/数: ${0:X8}/${1:X8}\n", posSymbolTable, countSymbols));

            uint sizeOptionHeader = GetWord(_posPeHeader + 20);
            AppendLog(string.Format("  オプションヘッダのサイズ: ${0:X4}\n", sizeOptionHeader));

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
            AppendLog(string.Format("  エントリーポイント: ${0:X8}\n", addressEntryPoint));

            uint posCodeSection = GetLong(offsetOptionHeader + 20);
            AppendLog(string.Format("  コードセクションの位置: ${0:X8}\n", posCodeSection));

            uint posDataSectionHeader = GetLong(offsetOptionHeader + 24);
            AppendLog(string.Format("  データセクションの位置: ${0:X8}\n", posDataSectionHeader));

            _addrBase = GetLong(offsetOptionHeader + 28);
            AppendLog(string.Format("  ベースアドレス: ${0:X8}\n", _addrBase));

            _alignSection = GetLong(offsetOptionHeader + 32);
            AppendLog(string.Format("  セクション境界: ${0:X8} ({0}Bytes)\n", _alignSection));

            uint alignmentFile = GetLong(offsetOptionHeader + 36);
            AppendLog(string.Format("  ファイル境界: ${0:X8} ({0}Bytes)\n", alignmentFile));

            uint sizeImage = GetLong(offsetOptionHeader + 56);
            AppendLog(string.Format("  イメージサイズ: ${0:X8}\n", sizeImage));

            uint sizeHeader = GetLong(offsetOptionHeader + 60);
            AppendLog(string.Format("  ヘッダサイズ: ${0:X8}\n", sizeHeader));

            uint rvaExportTable = GetLong(offsetOptionHeader + 96);
            uint sizeExportTable = GetLong(offsetOptionHeader + 100);
            AppendLog(string.Format("  Export Table address/size: ${0:X8}/${1:X8}\n", rvaExportTable, sizeExportTable));

            _rvaImportTable = GetLong(offsetOptionHeader + 104);
            uint sizeImportTable = GetLong(offsetOptionHeader + 108);
            AppendLog(string.Format("  Import Table address/size: ${0:X8}/${1:X8}\n", _rvaImportTable, sizeImportTable));

            uint rvaResourceTable = GetLong(offsetOptionHeader + 112);
            uint sizeResourceTable = GetLong(offsetOptionHeader + 116);
            AppendLog(string.Format("  Resource Table address/size: ${0:X8}/${1:X8}\n", rvaResourceTable,
                sizeResourceTable));

            uint rvaImportAddressTable = GetLong(offsetOptionHeader + 192);
            uint sizeImportAddressTable = GetLong(offsetOptionHeader + 196);
            _countImportDir = sizeImportAddressTable/0x14;
            AppendLog(string.Format("  Import Address Table address/size: ${0:X8}/${1:X8}\n", rvaImportAddressTable,
                sizeImportAddressTable));

            AppendLog(string.Format("  * コード領域: ${0:X8}～${1:X8} (ファイル上: ${2:X8}～${3:X8})\n", _addrBase + posCodeSection,
                _addrBase + posCodeSection + sizeCodeSection - 1, posCodeSection,
                posCodeSection + sizeCodeSection - 1));
            AppendLog(string.Format("  * プログラムが開始されるアドレス: ${0:X8}\n", _addrBase + addressEntryPoint));

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
                AppendLog(string.Format("\n  セクション[{0}]\n", i));

                string nameSection = GetString(offsetSection, 8);
                AppendLog(string.Format("  セクション名: {0}\n", nameSection));

                uint virtualSize = GetLong(offsetSection + 8);
                AppendLog(string.Format("  セクション仮想サイズ: ${0:X8} ({0}Bytes)\n", virtualSize));

                uint virtualAddress = GetLong(offsetSection + 12);
                AppendLog(string.Format("  セクション仮想アドレス: ${0:X8}\n", virtualAddress));

                uint sizeRawData = GetLong(offsetSection + 16);
                AppendLog(string.Format("  セクション実サイズ: ${0:X8} ({0}Bytes)\n", sizeRawData));

                uint posRawData = GetLong(offsetSection + 20);
                AppendLog(string.Format("  セクション実位置: ${0:X8}\n", posRawData));

                if (nameSection.Equals(".text"))
                {
                    AppendLog(string.Format("  * 未使用領域: ${0:X8}～${1:X8}\n", _addrBase + virtualAddress + virtualSize + 1,
                        _addrBase + virtualAddress + sizeRawData - 1));
                    _sizeTextSection = sizeRawData;
                    _sizeTextFree = sizeRawData - virtualSize;
                    _posTextSection = posRawData;
                    _addrTextSection = virtualAddress;
                }
                else if (nameSection.Equals(".rdata"))
                {
                    AppendLog(string.Format("  * 未使用領域: ${0:X8}～${1:X8}\n", _addrBase + virtualAddress + virtualSize + 1,
                        _addrBase + virtualAddress + sizeRawData - 1));
                    _sizeRdataSection = sizeRawData;
                    _sizeRdataFree = sizeRawData - virtualSize;
                    _sizeDataSection = sizeRawData;
                    _posRdataSection = posRawData;
                    _addrRdataSection = virtualAddress;
                }
                else if (nameSection.Equals(".data"))
                {
                    uint addrDataSectionEnd = Ceiling(_addrBase + virtualAddress + virtualSize, _alignSection) - 1;
                    AppendLog(string.Format("  * 未使用領域: ${0:X8}～${1:X8}\n", _addrBase + virtualAddress + virtualSize + 1,
                        addrDataSectionEnd));
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
            AppendLog(string.Format("  インポートディレクトリテーブル数: {0}\n", _countImportDir));

            uint offsetImportDir = _posImportTable;
            uint rvaImportLookupTable = GetLong(offsetImportDir);
            while (rvaImportLookupTable != 0)
            {
                AppendLog(string.Format("\n  ImportLookupTableRVA: ${0:X8}\n", rvaImportLookupTable));

                uint timeStamp = GetLong(offsetImportDir + 4);
                AppendLog(string.Format("  TimeStamp: ${0:X8}\n", timeStamp));

                uint forwarderChain = GetLong(offsetImportDir + 8);
                AppendLog(string.Format("  FowarderChain: ${0:X8}\n", forwarderChain));

                uint rvaName = GetLong(offsetImportDir + 12);
                string name = GetString(_posImportSection + rvaName - _rvaImportSection, 0);
                AppendLog(string.Format("  NameRVA: ${0:X8} ({1})\n", rvaName, name));

                uint rvaImportAddressTable = GetLong(offsetImportDir + 16);
                AppendLog(string.Format("  ImportAddressTableRVA: ${0:X8}\n", rvaImportAddressTable));

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
                            _addrGetModuleHandleA = _addrBase + rvaImportAddressTable + i*4;
                        }
                        else if (importName.Equals("GetProcAddress"))
                        {
                            _addrGetProcAddress = _addrBase + rvaImportAddressTable + i*4;
                        }
                        else if (importName.Equals("IsDebuggerPresent"))
                        {
                            _addrIsDebuggerPresent = _addrBase + rvaImportAddressTable + i*4;
                            _posIsDebuggerPresent = offsetHintNameTable + 2;
                        }
                    }
                    AppendLog(string.Format("    {0:D4} {1}\n", hint, importName));

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
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
                    case PatchType.ArsenalOfDemocracy:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
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
            // ゲーム固有
            switch (_patchType)
            {
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
            var pattern = new byte[]
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

            var pattern = new byte[]
            {
                0x8B, 0x45, 0x18, 0x8B, 0x48, 0x0C, 0x89, 0x4D
            };
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

                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
                    pattern = new byte[]
                    {
                        0x8B, 0x55, 0x18, 0x8B, 0x02, 0x8B, 0x4D, 0x18,
                        0x8B, 0x50, 0x0C, 0xFF, 0xD2, 0x8B, 0x45, 0x0C,
                        0x8B, 0xE5, 0x5D, 0xC2, 0x18, 0x00
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
                    pattern = new byte[]
                    {
                        0xC7, 0x45, 0xFC, 0x00, 0x00, 0x00, 0x00, 0xC6,
                        0x45, 0xF4, 0x00, 0x8B, 0x45, 0x08, 0x8A, 0x08,
                        0x88, 0x4D
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
            var pattern = new byte[]
            {
                0x8B, 0x45, 0xFC, 0x8B, 0xE5, 0x5D, 0xC2, 0x04,
                0x00
            };
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
                case PatchType.ArsenalOfDemocracy107:
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

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
                        0x88, 0x8C, 0x04, 0x20, 0x01, 0x00, 0x00, 0x40,
                        0x38, 0x5C, 0x24, 0x13
                    };
                    l = BinaryScan(_data, pattern, _posTextSection, _sizeTextSection);
                    if (l.Count == 0)
                    {
                        return false;
                    }
                    _posCalcLineBreakStart5 = l[0];

                    pattern = new byte[]
                    {
                        0x88, 0x9C, 0x04, 0x1C, 0x01, 0x00, 0x00, 0x40,
                        0x84, 0xDB
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

                case PatchType.ArsenalOfDemocracy:
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

                case PatchType.ArsenalOfDemocracy:
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
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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

            var pattern = new byte[]
            {
                0x25, 0x64, 0x25, 0x73, 0x20, 0x25, 0x73, 0x2E,
                0x20, 0x25, 0x73, 0x00
            };
            List<uint> l;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
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
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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

            var pattern = new byte[]
            {
                0x25, 0x64, 0x25, 0x73, 0x20, 0x25, 0x73, 0x00
            };
            List<uint> l;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy107:
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

            var pattern = new byte[]
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
        /// 強制ウィンドウ化処理を埋め込む位置を探索する
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
            var pattern = new byte[]
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
            var pattern2 = new byte[5];
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
                    _posTermModelNameStart1 = l[0];
                    _posTermModelNameStart2 = l[1];
                    break;

                case PatchType.DarkestHour:
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
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchLatinToUpper();
                    PatchChatBlockChar();
                    break;
            }
            PatchWinMmDll();
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
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
                    case PatchType.ArsenalOfDemocracy:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
                        PatchGetDivisionName();
                        EmbedDivisionNameFormat();
                        PatchGetArmyName();
                        EmbedArmyNameFormat();
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
            // ゲーム固有
            switch (_patchType)
            {
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
            PatchByte(_data, offset, 0x83);
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                string.Format("%02 ${0:X8} varTextOutDC2Address", _addrVarTextOutDcAddress));
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75);
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
            PatchByte(_data, offset, 0x68);
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, string.Format("%06 ${0:X8} WINMM.dll", _addrWinMmDll));
            offset += 4;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        string.Format("%04 ${0:X8} IsDebuggerPresent", _addrIsDebuggerPresent));
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        string.Format("%04 ${0:X8} GetModuleHandleA", _addrGetModuleHandleA));
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85);
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4);
            offset++;
            PatchByte(_data, offset, 0x68);
            offset++;
            PatchLong(_data, offset, _addrTextOutDc, string.Format("%00 ${0:X8} TextOutDC2", _addrTextOutDc));
            offset += 4;
            PatchByte(_data, offset, 0x50);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                string.Format("%05 ${0:X8} GetProcAddress", _addrGetProcAddress));
            offset += 4;
            PatchByte(_data, offset, 0x85);
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4);
            offset++;
            PatchByte(_data, offset, 0xA3);
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                string.Format("%02 ${0:X8} varTextOutDC2Address", _addrVarTextOutDcAddress));
            offset += 4;
            PatchByte(_data, offset, 0x8B);
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
                    case PatchType.ArsenalOfDemocracy:
                    case PatchType.ArsenalOfDemocracy104:
                    case PatchType.ArsenalOfDemocracy107:
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
                        PatchByte(_data, offset, 0x94);
                        break;
                    default:
                        PatchByte(_data, offset, 0x98);
                        break;
                }
            }
            offset++;
            PatchByte(_data, offset, 0x8B);
            offset++;
            PatchByte(_data, offset, 0x51);
            offset++;
            PatchByte(_data, offset, 0x0C);
            offset++;
            if (_patchType == PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x33);
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
                PatchByte(_data, offset, 0x81);
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
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
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
                PatchByte(_data, offset, 0xB8);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74);
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                PatchByte(_data, offset, 0x33);
                offset++;
                PatchByte(_data, offset, 0xC0);
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
                PatchByte(_data, offset, 0x48);
                offset++;
                PatchByte(_data, offset, 0x48);
                offset++;
                PatchByte(_data, offset, 0x48);
                offset++;
                PatchByte(_data, offset, 0x2B);
                offset++;
                PatchByte(_data, offset, 0xC2);
                offset++;
                PatchByte(_data, offset, 0x8B);
                offset++;
                PatchByte(_data, offset, 0x51);
                offset++;
                PatchByte(_data, offset, 0x0C);
                offset++;
                PatchByte(_data, offset, 0x81);
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
                PatchByte(_data, offset, 0x75);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x48);
            }
            offset++;
            PatchByte(_data, offset, 0x50);
            offset++;
            PatchByte(_data, offset, 0x8B);
            offset++;
            PatchByte(_data, offset, 0x55);
            offset++;
            PatchByte(_data, offset, 0x18);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x72);
            offset++;
            PatchByte(_data, offset, 0x2C);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x08);
            offset++;
            PatchByte(_data, offset, 0x8D); // lea eax,[ebp+10h]
            offset++;
            PatchByte(_data, offset, 0x45);
            offset++;
            PatchByte(_data, offset, 0x10);
            offset++;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0x8D); // lea eax,[ebp+0Ch]
            offset++;
            PatchByte(_data, offset, 0x45);
            offset++;
            PatchByte(_data, offset, 0x0C);
            offset++;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // push [ebp+14h]
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x14);
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varTextOutDC2Address]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarTextOutDcAddress,
                string.Format("%02 ${0:X8} varTextOutDC2Address", _addrVarTextOutDcAddress));
            offset += 4;
            PatchByte(_data, offset, 0xE9); // jmp TEXT_OUT_END
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posTextOutEnd),
                string.Format("%08 ${0:X8} TextOutEnd", GetTextAddress(_posTextOutEnd)));
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
            PatchByte(_data, offset, 0x83);
            offset++;
            PatchByte(_data, offset, 0x3D);
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                string.Format("%03 ${0:X8} varGetTextWidthAddress", _addrVarGetTextWidthAddress));
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x29);
            offset++;
            PatchByte(_data, offset, 0x68);
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, string.Format("%06 ${0:X8} WINMM.dll", _addrWinMmDll));
            offset += 4;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        string.Format("%04 ${0:X8} IsDebuggerPresent", _addrIsDebuggerPresent));
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        string.Format("%04 ${0:X8} GetModuleHandleA", _addrGetModuleHandleA));
                    break;
            }
            offset += 4;
            PatchByte(_data, offset, 0x85);
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4);
            offset++;
            PatchByte(_data, offset, 0x68);
            offset++;
            PatchLong(_data, offset, _addrGetTextWidth, string.Format("%01 ${0:X8} GetTextWidth", _addrGetTextWidth));
            offset += 4;
            PatchByte(_data, offset, 0x50);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                string.Format("%05 ${0:X8} GetProcAddress", _addrGetProcAddress));
            offset += 4;
            PatchByte(_data, offset, 0x85);
            offset++;
            PatchByte(_data, offset, 0xC0);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x01);
            offset++;
            PatchByte(_data, offset, 0xF4);
            offset++;
            PatchByte(_data, offset, 0xA3);
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                string.Format("%03 ${0:X8} varGetTextWidthAddress", _addrVarGetTextWidthAddress));
            offset += 4;
            PatchByte(_data, offset, 0x8B);
            offset++;
            PatchByte(_data, offset, 0x4D);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    PatchByte(_data, offset, 0xE8);
                    break;
                default:
                    PatchByte(_data, offset, 0xEC);
                    break;
            }
            offset++;
            PatchByte(_data, offset, 0x8B);
            offset++;
            PatchByte(_data, offset, 0x51);
            offset++;
            PatchByte(_data, offset, 0x0C);
            offset++;
            if (_patchType == PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x33);
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
                PatchByte(_data, offset, 0x81);
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
                    case PatchType.DarkestHour:
                    case PatchType.DarkestHour102:
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
                PatchByte(_data, offset, 0xB8);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x00);
                offset++;
                PatchByte(_data, offset, 0x74);
                offset++;
                PatchByte(_data, offset, 0x02);
                offset++;
                PatchByte(_data, offset, 0x33);
                offset++;
                PatchByte(_data, offset, 0xC0);
            }
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
            PatchByte(_data, offset, 0x8B);
            offset++;
            PatchByte(_data, offset, 0x41);
            offset++;
            PatchByte(_data, offset, 0x10);
            offset++;
            if (_patchType != PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x48);
                offset++;
                PatchByte(_data, offset, 0x48);
                offset++;
                PatchByte(_data, offset, 0x48);
                offset++;
            }
            PatchByte(_data, offset, 0x2B);
            offset++;
            PatchByte(_data, offset, 0xC2);
            offset++;
            if (_patchType != PatchType.ForTheGlory)
            {
                PatchByte(_data, offset, 0x8B);
                offset++;
                PatchByte(_data, offset, 0x51);
                offset++;
                PatchByte(_data, offset, 0x0C);
                offset++;
                PatchByte(_data, offset, 0x81);
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
                PatchByte(_data, offset, 0x75);
                offset++;
                PatchByte(_data, offset, 0x01);
                offset++;
                PatchByte(_data, offset, 0x48);
                offset++;
            }
            PatchByte(_data, offset, 0x50);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x75);
            offset++;
            PatchByte(_data, offset, 0x08);
            offset++;
            PatchByte(_data, offset, 0xFF);
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarGetTextWidthAddress,
                string.Format("%03 ${0:X8} varGetTextWidthAddress", _addrVarGetTextWidthAddress));
            offset += 4;
            PatchByte(_data, offset, 0xE9);
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetTextWidthEnd),
                string.Format("%10 ${0:X8} GetTextWidthEnd", GetTextAddress(_posGetTextWidthEnd)));
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
            PatchByte(_data, offset, 0xEB);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy107:
                    PatchByte(_data, offset, 0x17);
                    break;

                case PatchType.ArsenalOfDemocracy:
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
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.ArsenalOfDemocracy:
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
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_CALC_LINE_BREAK_ADDR_3
            offset++;
            PatchByte(_data, offset, 0x26);
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, string.Format("%06 ${0:X8} WINMM.dll", _addrWinMmDll));
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ForTheGlory:
                case PatchType.ArsenalOfDemocracy:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        string.Format("%04 ${0:X8} IsDebuggerPresent", _addrIsDebuggerPresent));
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        string.Format("%04 ${0:X8} GetModuleHandleA", _addrGetModuleHandleA));
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
            PatchLong(_data, offset, _addrCalcLineBreak, string.Format("%00 ${0:X8} CalcLineBreak", _addrCalcLineBreak));
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                string.Format("%05 ${0:X8} GetProcAddress", _addrGetProcAddress));
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
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
            offset += 4;
            // GET_CALC_LINE_BREAK_ADDR_3
            PatchByte(_data, offset, 0xC3); // retn
            offset++;

            #endregion

            #region 改行位置計算処理1 - イベント文/シナリオ解説文/閣僚名/ポップアップ表示

            // CALC_LINE_BREAK_START1
            PatchByte(_data, _posCalcLineBreakStart1, 0xE9); // jmp CALC_LINE_BREAK1
            PatchLong(_data, _posCalcLineBreakStart1 + 1, GetRelativeOffset(_posCalcLineBreakStart1 + 5, offset),
                string.Format("%XX ${0:X8} CalcLineBreak1", GetTextAddress(offset)));

            // CALC_LINE_BREAK1
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                    break;

                case PatchType.ArsenalOfDemocracy:
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
                    break;
            }
            PatchByte(_data, offset, 0x51); // push ecx
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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

                case PatchType.ArsenalOfDemocracy:
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
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END1
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd1),
                string.Format("%XX ${0:X8} CalcLineBreakEnd1", GetTextAddress(_posCalcLineBreakEnd1)));
            offset += 4;

            #endregion

            #region 改行位置計算処理2 - 用途不明

            // CALC_LINE_BREAK_START2
            PatchByte(_data, _posCalcLineBreakStart2, 0xE9); // jmp CALC_LINE_BREAK2
            PatchLong(_data, _posCalcLineBreakStart2 + 1, GetRelativeOffset(_posCalcLineBreakStart2 + 5, offset),
                string.Format("%XX ${0:X8} CalcLineBreak2", GetTextAddress(offset)));

            // CALC_LINE_BREAK2
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                    break;

                case PatchType.ArsenalOfDemocracy:
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
                    break;
            }
            PatchByte(_data, offset, 0x51); // push ecx
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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

                case PatchType.ArsenalOfDemocracy:
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
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END2
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd2),
                string.Format("%XX ${0:X8} CalcLineBreakEnd2", GetTextAddress(_posCalcLineBreakEnd2)));
            offset += 4;

            #endregion

            #region 改行位置計算処理3 - 外交画面の説明文

            if (_patchType != PatchType.CrusaderKings)
            {
                // CALC_LINE_BREAK_START3
                PatchByte(_data, _posCalcLineBreakStart3, 0xE9); // jmp CALC_LINE_BREAK3
                PatchLong(_data, _posCalcLineBreakStart3 + 1, GetRelativeOffset(_posCalcLineBreakStart3 + 5, offset),
                    string.Format("%XX ${0:X8} CalcLineBreak3", GetTextAddress(offset)));

                // CALC_LINE_BREAK3
                PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                    string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                        break;

                    case PatchType.ArsenalOfDemocracy:
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
                        break;
                }
                PatchByte(_data, offset, 0x51); // push ecx
                offset++;
                PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
                offset++;
                PatchByte(_data, offset, 0x15);
                offset++;
                PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                    string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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
                PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END3
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd3),
                    string.Format("%XX ${0:X8} CalcLineBreakEnd3", GetTextAddress(_posCalcLineBreakEnd3)));
                offset += 4;
            }

            #endregion

            #region 改行位置計算処理4 - 用途不明

            if (_patchType == PatchType.EuropaUniversalis2)
            {
                // CALC_LINE_BREAK_START4
                PatchByte(_data, _posCalcLineBreakStart4, 0xE9); // jmp CALC_LINE_BREAK4
                PatchLong(_data, _posCalcLineBreakStart4 + 1, GetRelativeOffset(_posCalcLineBreakStart4 + 5, offset),
                    string.Format("%XX ${0:X8} CalcLineBreak4", GetTextAddress(offset)));

                // CALC_LINE_BREAK4
                PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                    string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                    string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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
                    string.Format("%XX ${0:X8} CalcLineBreakEnd4", GetTextAddress(_posCalcLineBreakEnd4)));
                offset += 4;
            }

            #endregion

            #region 改行位置計算処理5 - メッセージログ

            // CALC_LINE_BREAK_START5
            PatchByte(_data, _posCalcLineBreakStart5, 0xE9); // jmp CALC_LINE_BREAK5
            PatchLong(_data, _posCalcLineBreakStart5 + 1, GetRelativeOffset(_posCalcLineBreakStart5 + 5, offset),
                string.Format("%XX ${0:X8} CalcLineBreak5", GetTextAddress(offset)));

            // CALC_LINE_BREAK5
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
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
                    PatchByte(_data, offset, 0x51); // push ecx
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
                    break;

                case PatchType.ArsenalOfDemocracy:
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
                    break;
            }
            PatchByte(_data, offset, 0x51); // push ecx
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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
                    PatchByte(_data, offset, 0x89); // mov [esp+00000234h],ecx
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
                    PatchByte(_data, offset, 0x8B); // mov ecx,[ebp+4Ch]
                    offset++;
                    PatchByte(_data, offset, 0x4D);
                    offset++;
                    PatchByte(_data, offset, 0x4C);
                    offset++;
                    break;

                case PatchType.ArsenalOfDemocracy:
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
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END5
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd5),
                string.Format("%XX ${0:X8} CalcLineBreakEnd5", GetTextAddress(_posCalcLineBreakEnd5)));
            offset += 4;

            #endregion

            #region 改行位置計算処理6 - メッセージ設定画面

            // CALC_LINE_BREAK_START6
            PatchByte(_data, _posCalcLineBreakStart6, 0xE9); // jmp CALC_LINE_BREAK6
            PatchLong(_data, _posCalcLineBreakStart6 + 1, GetRelativeOffset(_posCalcLineBreakStart6 + 5, offset),
                string.Format("%XX ${0:X8} CalcLineBreak6", GetTextAddress(offset)));

            // CALC_LINE_BREAK6
            PatchByte(_data, offset, 0xE8); // call GET_CALC_LINE_BREAK_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetCalcLineBreakAddr),
                string.Format("%XX ${0:X8} GetCalcLineBreakAddr", GetTextAddress(posGetCalcLineBreakAddr)));
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
                    break;

                case PatchType.ArsenalOfDemocracy:
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
                    break;
            }
            PatchByte(_data, offset, 0x51); // push ecx
            offset++;
            PatchByte(_data, offset, 0xFF); // call [varCalcLineBreakAddress]
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrVarCalcLineBreakAddress,
                string.Format("%XX ${0:X8} varCalcLineBreakAddress", _addrVarCalcLineBreakAddress));
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
            PatchByte(_data, offset, 0xE9); // jmp CALC_LINE_BREAK_END6
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posCalcLineBreakEnd6),
                string.Format("%XX ${0:X8} CalcLineBreakEnd6", GetTextAddress(_posCalcLineBreakEnd6)));
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
            AppendLog(string.Format("  ${0:X8}\n\n", _posWinMmDll));
            _data[_posWinMmDll] = (byte) '_';
        }

        /// <summary>
        ///     IsDebuggerPresentをGetModuleHandleAに書き換える
        /// </summary>
        private static void PatchIsDebuggerPresent()
        {
            uint offset = _posIsDebuggerPresent;
            AppendLog("  IsDebuggerPresent書き換え\n");
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    offset = _posGetDivisionName1;
                    PatchByte(_data, offset, 0xE9); // jmp GetDivisionNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetDivisionName2 - 20),
                        string.Format("%XX ${0:X8} GetDivisionNameOtherCase",
                            GetTextAddress(_posGetDivisionName2 - 20)));
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
                        string.Format("%XX ${0:X8} DivisionNameFormat", addrDivisionNameFormat));
                    offset += 4;
                    PatchByte(_data, offset, 0xE8); // call GetDivisionOrderName
                    offset++;
                    PatchLong(_data, offset, offsetGetDivisionOrderName,
                        string.Format("%XX ${0:X8} GetDivisionOrderName",
                            GetRelativeAddress(GetTextAddress(offset + 4), offsetGetDivisionOrderName)));
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
                        string.Format("%XX ${0:X8} GetArmyNameOtherCase", GetTextAddress(_posGetArmyName2)));

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
                        string.Format("%XX ${0:X8} GetArmyNameOtherCase", GetTextAddress(_posGetArmyName2)));

                    offset = _posGetArmyName2 + 25;
                    PatchByte(_data, offset, 0x56); // push esi
                    offset++;
                    PatchByte(_data, offset, 0x50); // push eax
                    break;

                case PatchType.HeartsOfIron2:
                case PatchType.HeartsOfIron212:
                case PatchType.IronCrossHoI2:
                case PatchType.ArsenalOfDemocracy:
                case PatchType.ArsenalOfDemocracy104:
                case PatchType.ArsenalOfDemocracy107:
                case PatchType.DarkestHour:
                case PatchType.DarkestHour102:
                    offset = _posGetArmyName1;
                    PatchByte(_data, offset, 0xE9); // jmp GetArmyNameOtherCase
                    offset++;
                    PatchLong(_data, offset, GetRelativeOffset(offset + 4, _posGetArmyName2 - 13),
                        string.Format("%XX ${0:X8} GetArmyNameOtherCase", GetTextAddress(_posGetArmyName2 - 13)));
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
                        string.Format("%XX ${0:X8} ArmyNameFormat", addrArmyNameFormat));
                    offset += 4;
                    PatchByte(_data, offset, 0xE8); // call GetArmyOrderName
                    offset++;
                    PatchLong(_data, offset, offsetGetArmyOrderName,
                        string.Format("%XX ${0:X8} GetArmyOrderName",
                            GetRelativeAddress(GetTextAddress(offset + 4), offsetGetArmyOrderName)));
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
                string.Format("%XX ${0:X8} GetRankingNameOtherCase", GetTextAddress(_posGetRankingName2)));

            offset = _posGetRankingName2;
            PatchByte(_data, offset, 0x68); // push NATION_RANKING
            offset++;
            PatchLong(_data, offset, _addrRankingSuffix,
                string.Format("%XX ${0:X8} push RANKING_SUFFIX", _addrRankingSuffix));

            AppendLog("\n");
        }

        /// <summary>
        ///     師団名の書式文字列埋め込み
        /// </summary>
        private static void EmbedDivisionNameFormat()
        {
            uint offset = _posDivisionNameFormat;
            AppendLog("  師団名の書式文字列埋め込み\n");
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
                        string.Format("%XX ${0:X8} SetWindowed", GetTextAddress(_posWindowed2)));
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
                string.Format("%XX ${0:X8} push EE_MAX_AMPHIB_MOD", _addrEeMaxAmphibModTitle));
            AppendLog("\n");
        }

        /// <summary>
        ///     EE_MAX_AMPHIB_MOD_TITLE埋め込み
        /// </summary>
        private static void EmbedEeMaxAmphibModTitle()
        {
            uint offset = _posEeMaxAmphibModTitle;
            AppendLog("  EE_MAX_AMPHIB_MOD_TITLE埋め込み\n");
            AppendLog(string.Format("  ${0:X8}\n\n", offset));
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
                string.Format("%XX ${0:X8} varStrNLenAddress", _addrVarStrNLenAddress));
            offset += 4;
            PatchByte(_data, offset, 0x00);
            offset++;
            PatchByte(_data, offset, 0x75); // jnz GET_STRNLEN0_ADDR_3
            offset++;
            PatchByte(_data, offset, 0x26);
            offset++;
            PatchByte(_data, offset, 0x68); // push addrWinMmDll
            offset++;
            PatchLong(_data, offset, _addrWinMmDll, string.Format("%06 ${0:X8} WINMM.dll", _addrWinMmDll));
            offset += 4;
            PatchByte(_data, offset, 0xFF); // call GetModuleHandleA
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
                    PatchLong(_data, offset, _addrIsDebuggerPresent,
                        string.Format("%04 ${0:X8} IsDebuggerPresent", _addrIsDebuggerPresent));
                    break;

                default:
                    PatchLong(_data, offset, _addrGetModuleHandleA,
                        string.Format("%04 ${0:X8} GetModuleHandleA", _addrGetModuleHandleA));
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
            PatchLong(_data, offset, _addrStrNLen, string.Format("%00 ${0:X8} strnlen0", _addrCalcLineBreak));
            offset += 4;
            PatchByte(_data, offset, 0x50); // push eax
            offset++;
            PatchByte(_data, offset, 0xFF); // call GetProcAddress
            offset++;
            PatchByte(_data, offset, 0x15);
            offset++;
            PatchLong(_data, offset, _addrGetProcAddress,
                string.Format("%05 ${0:X8} GetProcAddress", _addrGetProcAddress));
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
                string.Format("%XX ${0:X8} varStrNLenAddress", _addrVarStrNLenAddress));
            offset += 4;
            // GET_STRNLEN0_ADDR_3
            PatchByte(_data, offset, 0xC3); // retn
            offset++;

            // TERM_MODEL_NAME
            uint posTermModelName = offset;
            PatchByte(_data, offset, 0xE8); // call GET_STRNLEN0_ADDR
            offset++;
            PatchLong(_data, offset, GetRelativeOffset(offset + 4, posGetStrNLenAddr),
                string.Format("%XX ${0:X8} GetStrNLenAddr", GetTextAddress(posGetStrNLenAddr)));
            offset += 4;
            PatchByte(_data, offset, 0x6A); // push 0000001Ch
            offset++;
            PatchByte(_data, offset, 0x1C);
            offset++;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
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
                string.Format("%02 ${0:X8} varStrNLenAddress", _addrVarStrNLenAddress));
            offset += 4;
            switch (_patchType)
            {
                case PatchType.ArsenalOfDemocracy:
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

            // TERM_MODEL_NAME_START1
            if (_patchType == PatchType.ArsenalOfDemocracy)
            {
                // TERM_MODEL_NAME_START1
                PatchByte(_data, _posTermModelNameStart1, 0xE9); // jmp TERM_MODEL_NAME
                PatchLong(_data, _posTermModelNameStart1 + 1, GetRelativeOffset(_posTermModelNameStart1 + 5, offset),
                    string.Format("%XX ${0:X8} TermBrigModelName", GetTextAddress(offset)));
                PatchByte(_data, _posTermModelNameStart1 + 5, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 6, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 7, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart1 + 8, 0x90); // nop

                // TERM_BRIG_MODEL_NAME
                PatchByte(_data, offset, 0xE8); // call TERM_MODEL_NAME
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posTermModelName),
                    string.Format("%08 ${0:X8} TermModelName", GetTextAddress(posTermModelName)));
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
                    string.Format("%08 ${0:X8} TermBrigModelNameEnd", GetTextAddress(_posTermModelNameStart1 + 9)));
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
                    string.Format("%XX ${0:X8} TermDivModelName", GetTextAddress(offset)));
                PatchByte(_data, _posTermModelNameStart2 + 5, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 6, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 7, 0x90); // nop
                PatchByte(_data, _posTermModelNameStart2 + 8, 0x90); // nop

                // TERM_DIV_MODEL_NAME
                PatchByte(_data, offset, 0xE8); // call TERM_MODEL_NAME
                offset++;
                PatchLong(_data, offset, GetRelativeOffset(offset + 4, posTermModelName),
                    string.Format("%08 ${0:X8} TermModelName", GetTextAddress(posTermModelName)));
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
                    string.Format("%08 ${0:X8} TermDivModelNameEnd", GetTextAddress(_posTermModelNameStart2 + 9)));
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
                PatchLong(_data, _posTermModelNameStart1 + 1,
                    GetRelativeOffset(_posTermModelNameStart1 + 5, posTermModelName),
                    string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                // TERM_MODEL_NAME_START2
                PatchByte(_data, _posTermModelNameStart2, 0xE8); // call TERM_MODEL_NAME
                PatchLong(_data, _posTermModelNameStart2 + 1,
                    GetRelativeOffset(_posTermModelNameStart2 + 5, posTermModelName),
                    string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                if (_patchType == PatchType.DarkestHour)
                {
                    // TERM_MODEL_NAME_START3
                    PatchByte(_data, _posTermModelNameStart3, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart3 + 1,
                        GetRelativeOffset(_posTermModelNameStart3 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                    // TERM_MODEL_NAME_START2
                    PatchByte(_data, _posTermModelNameStart4, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart4 + 1,
                        GetRelativeOffset(_posTermModelNameStart4 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                    // TERM_MODEL_NAME_START2
                    PatchByte(_data, _posTermModelNameStart5, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart5 + 1,
                        GetRelativeOffset(_posTermModelNameStart5 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                    // TERM_MODEL_NAME_START2
                    PatchByte(_data, _posTermModelNameStart6, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart6 + 1,
                        GetRelativeOffset(_posTermModelNameStart6 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                    // TERM_MODEL_NAME_START2
                    PatchByte(_data, _posTermModelNameStart7, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart7 + 1,
                        GetRelativeOffset(_posTermModelNameStart7 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));

                    // TERM_MODEL_NAME_START2
                    PatchByte(_data, _posTermModelNameStart8, 0xE8); // call TERM_MODEL_NAME
                    PatchLong(_data, _posTermModelNameStart8 + 1,
                        GetRelativeOffset(_posTermModelNameStart8 + 5, posTermModelName),
                        string.Format("%XX ${0:X8} TermModelName", GetTextAddress(posTermModelName)));
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
            var sb = new StringBuilder("  Binary:");
            foreach (byte b in pattern)
            {
                sb.AppendFormat(" {0:X2}", b);
            }
            sb.AppendLine();
            AppendLog(sb.ToString());
            AppendLog(string.Format("  ファイル上の開始位置: ${0:X8}\n", start));
            AppendLog(string.Format("  ファイル上の終了位置: ${0:X8}\n", start + size - 1));

            AppendLog(string.Format("  検索範囲 ${0:X8}～${1:X8} (のこり{2}Bytes)\n", start, start + size - 1, size));
            var result = new List<uint>();
            for (uint offset = start; offset <= start + size - pattern.Length; offset++)
            {
                if (IsBinaryMatch(target, pattern, offset))
                {
                    result.Add(offset);
                    AppendLog(string.Format("  *** Find ${0:X8}\n", offset));
                    AppendLog(string.Format("  検索範囲 ${0:X8}～${1:X8} (のこり{2}Bytes)\n", offset + 1, start + size - 1,
                        start + size - offset - 1));
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
            AppendLog(string.Format("  ${0:X8} {1:X2}->{2:X2}\n", offset, target[offset], val));
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
            AppendLog(string.Format("  ${0:X8} {1:X8}->{2:X8} ({3})\n", offset, GetLong(offset), val, message));
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
            return (ushort) (_data[offset] + _data[offset + 1]*0x0100);
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
                    (_data[offset] + _data[offset + 1]*0x00000100 + _data[offset + 2]*0x00010000 +
                     _data[offset + 3]*0x01000000);
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
            uint result = (val + unit - 1)/unit;
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
        ArsenalOfDemocracy, // Arsenal of Democracy 1.08-
        DarkestHour, // Darkest Hour 1.03-
        ArsenalOfDemocracy104, // Arsenal of Democracy 1.02-1.04
        ArsenalOfDemocracy107, // Arsenal of Democracy 1.05-1.07
        DarkestHour102, // Darkest Hour 1.00-1.02
        HeartsOfIron212, // Hearts of Iron 2 1.2
        IronCrossHoI2, // Iron Cross over Hearts of Iron 2
    }
}