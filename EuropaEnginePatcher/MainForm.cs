using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
    /// <summary>
    ///     メインフォーム
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        ///     静的呼び出しのためにフォームのインスタンスを保存
        /// </summary>
        private static MainForm _form;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MainForm()
        {
            _form = this;

            InitializeComponent();

            UpdateTitle();
            typeComboBox.SelectedIndex = 0;
        }

        /// <summary>
        ///     ログを追加する
        /// </summary>
        /// <param name="s">出力する文字列</param>
        public static void AppendLog(string s)
        {
            Debug.WriteLine(s);
            if (_form == null)
            {
                return;
            }
            _form.logRichTextBox.AppendText(s);
            Application.DoEvents();
        }

        /// <summary>
        ///     タイトル文字列を更新する
        /// </summary>
        private void UpdateTitle()
        {
            Text = string.Format("Europe Engine Patcher Ver {0}", EuropaEnginePatcher.VersionName);
        }

        /// <summary>
        ///     参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            pathTextBox.Text = dialog.FileName;
            // 自動判別処理
            if (typeComboBox.SelectedIndex == 0)
            {
                PatchController.DetectGameType(pathTextBox.Text);
                typeComboBox.SelectedIndex = PatchController.GetGameIndex();
            }
            // 自動処理モード
            if (PatchController.AutoMode)
            {
                saveButton.Enabled = PatchController.AutoProcess();
            }
        }

        /// <summary>
        ///     クリアボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearButtonClick(object sender, EventArgs e)
        {
            typeComboBox.SelectedIndex = 0;
            pathTextBox.Clear();
            logRichTextBox.Clear();

            autoModeCheckBox.Checked = true;
            renameOriginalCheckBox.Checked = true;
            autoLineBreakCheckBox.Checked = true;
            wordOrderCheckBox.Checked = false;
            windowedCheckBox.Checked = false;
            introSkipCheckBox.Checked = false;
            ntlCheckBox.Checked = false;

            saveButton.Enabled = false;
        }

        /// <summary>
        ///     開始ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartButtonClick(object sender, EventArgs e)
        {
            if (!File.Exists(PatchController.TargetFileName))
            {
                return;
            }
            saveButton.Enabled = PatchController.AutoMode ? PatchController.AutoProcess() : PatchController.Patch();
        }

        /// <summary>
        ///     保存ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveButtonClick(object sender, EventArgs e)
        {
            if (!File.Exists(PatchController.TargetFileName))
            {
                return;
            }
            PatchController.Save();
            PatchController.CopyDll();
        }

        /// <summary>
        ///     終了ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        ///     ファイルをドラッグした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        ///     ファイルをドロップした時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormDragDrop(object sender, DragEventArgs e)
        {
            string fileName = ((string[]) e.Data.GetData(DataFormats.FileDrop, false))[0];
            // フォルダをドロップした場合はその下の実行ファイルを対象とする
            if (Directory.Exists(fileName))
            {
                fileName = PatchController.DetectExeFileName(fileName);
            }
            pathTextBox.Text = fileName;
            // 自動判別処理
            if (typeComboBox.SelectedIndex == 0)
            {
                PatchController.DetectGameType(pathTextBox.Text);
                typeComboBox.SelectedIndex = PatchController.GetGameIndex();
            }
            // 自動処理モード
            if (PatchController.AutoMode)
            {
                saveButton.Enabled = PatchController.AutoProcess();
            }
        }

        /// <summary>
        ///     パッチの種類が変更された時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            PatchController.SetGameType(typeComboBox.SelectedIndex);

            autoLineBreakCheckBox.Enabled = PatchController.GetAutoLineBreakEffective();
            wordOrderCheckBox.Enabled = PatchController.GetWordOrderEffective();
            windowedCheckBox.Enabled = PatchController.GetWindowedEffective();
            introSkipCheckBox.Enabled = PatchController.GetIntroSkipEffective();
            ntlCheckBox.Enabled = PatchController.GetNtlEffective();

            if (PatchController.GetWordOrderDefault())
            {
                if (!wordOrderCheckBox.Checked)
                {
                    wordOrderCheckBox.Checked = true;
                }
            }
            else
            {
                if (!PatchController.GetWordOrderEffective() && wordOrderCheckBox.Checked)
                {
                    wordOrderCheckBox.Checked = false;
                }
            }
            if (!PatchController.GetWindowedEffective() && windowedCheckBox.Checked)
            {
                windowedCheckBox.Checked = false;
            }
            if (!PatchController.GetIntroSkipEffective() && introSkipCheckBox.Checked)
            {
                introSkipCheckBox.Checked = false;
            }
            if (!PatchController.GetNtlEffective() && ntlCheckBox.Checked)
            {
                ntlCheckBox.Checked = false;
            }
        }

        /// <summary>
        ///     パス名変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPathTextBoxTextChanged(object sender, EventArgs e)
        {
            PatchController.TargetFileName = pathTextBox.Text;
            saveButton.Enabled = false;
        }

        /// <summary>
        ///     自動処理モードのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoModeCheckedBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.AutoMode = autoModeCheckBox.Checked;
        }

        /// <summary>
        ///     元のファイルをリネームのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRenameOriginalCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.RenameOriginal = renameOriginalCheckBox.Checked;
        }

        /// <summary>
        ///     テキスト自動折り返しのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAutoLineBreakCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.AutoLineBreak = autoLineBreakCheckBox.Checked;
            saveButton.Enabled = false;
        }

        /// <summary>
        ///     自動命名時の語順変更のチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWordOrderCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.WordOrder = wordOrderCheckBox.Checked;
            saveButton.Enabled = false;
        }

        /// <summary>
        ///     強制ウィンドウ化のチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowedCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.Windowed = windowedCheckBox.Checked;
            saveButton.Enabled = false;
        }

        /// <summary>
        ///     イントロスキップのチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIntroSkipCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.IntroSkip = introSkipCheckBox.Checked;
            saveButton.Enabled = false;
        }

        /// <summary>
        ///     時間制限解除のチェック状態変更時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNtlCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            PatchController.Ntl = ntlCheckBox.Checked;
            saveButton.Enabled = false;
        }
    }
}