using System;
using System.IO;
using System.Windows.Forms;

namespace EuropaEnginePatcher
{
	/// <summary>
	/// メインフォーム
	/// </summary>
	internal partial class MainForm : Form
	{
		/// <summary>
		/// パッチエンジン
		/// </summary>
		private readonly PatchEngine _engine;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		internal MainForm()
		{
			_engine = new PatchEngine(this);

			InitializeComponent();

			UpdateTitle();
			typeComboBox.SelectedIndex = 0;

			string[] args = Environment.GetCommandLineArgs();
			if (args.Length > 1)
			{
				pathTextBox.Text = args[1];
			}
		}

		/// <summary>
		/// ログを追加する
		/// </summary>
		/// <param name="s">出力する文字列</param>
		internal void AppendLog(string s)
		{
			logRichTextBox.AppendText(s);
		}

		/// <summary>
		/// タイトル文字列を更新する
		/// </summary>
		private void UpdateTitle()
		{
			Text = string.Format("Europe Engine Patcher Ver {0}", EuropaEnginePatcher.VersionName);
		}

		/// <summary>
		/// _inmm.dllをコピーする
		/// </summary>
		/// <param name="dirName">対象ディレクトリ名</param>
		private void CopyDll(string dirName)
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
					                    "日本語化DLLのコピー", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						File.Copy(srcName, destName);
					}
				}
			}
		}

		/// <summary>
		/// 参照ボタン押下時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnBrowseButtonClick(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				pathTextBox.Text = dialog.FileName;
				// 自動判別処理
				if (typeComboBox.SelectedIndex == 0)
				{
					PatchType patchType = _engine.DetectPatchType(dialog.FileName);
					typeComboBox.SelectedIndex = _engine.GetPatchIndex(patchType);
				}
			}
		}

		/// <summary>
		/// クリアボタン押下時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnClearButtonClick(object sender, EventArgs e)
		{
			typeComboBox.SelectedIndex = 0;
			pathTextBox.Clear();
			logRichTextBox.Clear();
			saveButton.Enabled = false;
		}

		/// <summary>
		/// 開始ボタン押下時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStartButtonClick(object sender, EventArgs e)
		{
			saveButton.Enabled = false;
			_engine.IsAutoLineBreak = autoLineBreakCheckBox.Checked;
			_engine.IsWordOrder = wordOrderCheckBox.Checked;
			if (_engine.PatchGameFile(pathTextBox.Text, _engine.GetPatchType(typeComboBox.SelectedIndex)))
			{
				saveButton.Enabled = true;
			}
		}

		/// <summary>
		/// 保存ボタン押下時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSaveButtonClick(object sender, EventArgs e)
		{
			var dialog = new SaveFileDialog();
			if (string.IsNullOrEmpty(pathTextBox.Text))
			{
				dialog.FileName = "HoI2Jp";
			}
			else
			{
				dialog.FileName = Path.GetFileNameWithoutExtension(pathTextBox.Text) + "Jp";
			}
			dialog.DefaultExt = "exe";
			dialog.Filter = "実行ファイル (*.exe)|*.exe";
			dialog.InitialDirectory = Path.GetDirectoryName(pathTextBox.Text);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				_engine.SavePatchedFile(dialog.FileName);
				CopyDll(Path.GetDirectoryName(dialog.FileName));
			}
		}

		/// <summary>
		/// 終了ボタン押下時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnExitButtonClick(object sender, EventArgs e)
		{
			Application.Exit();
		}

		/// <summary>
		/// ファイルをドラッグした時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFormDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		/// <summary>
		/// ファイルをドロップした時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFormDragDrop(object sender, DragEventArgs e)
		{
			var fileNames = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
			pathTextBox.Text = fileNames[0];
			// 自動判別処理
			if (typeComboBox.SelectedIndex == 0)
			{
				typeComboBox.SelectedIndex = _engine.GetPatchIndex(_engine.DetectPatchType(fileNames[0]));
			}
		}

		/// <summary>
		/// パッチの種類が変更された時の処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			PatchType patchType = _engine.GetPatchType(typeComboBox.SelectedIndex);

			if (patchType == PatchType.Unknown)
			{
				autoLineBreakCheckBox.Enabled = true;
				wordOrderCheckBox.Enabled = true;
			}
			else
			{
				autoLineBreakCheckBox.Enabled = _engine.GetAutoLineBreakEffective(patchType);
				autoLineBreakCheckBox.Checked = autoLineBreakCheckBox.Enabled && _engine.GetAutoLineBreakDefault(patchType);
				wordOrderCheckBox.Enabled = _engine.GetWordOrderEffective(patchType);
				wordOrderCheckBox.Checked = wordOrderCheckBox.Enabled && _engine.GetWordOrderDefault(patchType);
			}
		}
	}
}