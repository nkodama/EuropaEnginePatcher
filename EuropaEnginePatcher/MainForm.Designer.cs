namespace EuropaEnginePatcher
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.clearButton = new System.Windows.Forms.Button();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.optionGroupBox = new System.Windows.Forms.GroupBox();
            this.autoModeCheckBox = new System.Windows.Forms.CheckBox();
            this.renameOriginalCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLineBreakCheckBox = new System.Windows.Forms.CheckBox();
            this.wordOrderCheckBox = new System.Windows.Forms.CheckBox();
            this.optionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.Location = new System.Drawing.Point(377, 10);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 3;
            this.clearButton.Text = "クリア";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.OnClearButtonClick);
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logRichTextBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.logRichTextBox.Location = new System.Drawing.Point(12, 154);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.Size = new System.Drawing.Size(440, 156);
            this.logRichTextBox.TabIndex = 8;
            this.logRichTextBox.Text = "";
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Location = new System.Drawing.Point(377, 125);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "終了";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.OnExitButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(377, 96);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.OnSaveButtonClick);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(377, 67);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "開始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.OnStartButtonClick);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(377, 38);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "参照";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.OnBrowseButtonClick);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(12, 40);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(359, 19);
            this.pathTextBox.TabIndex = 1;
            this.pathTextBox.TextChanged += new System.EventHandler(this.OnPathTextBoxTextChanged);
            // 
            // typeComboBox
            // 
            this.typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "自動判別",
            "Crusader Kings",
            "Europa Universalis 2",
            "For The Glory",
            "Victoria",
            "Hearts of Iron",
            "Hearts of Iron 2",
            "Arsenal of Democracy",
            "Darkest Hour"});
            this.typeComboBox.Location = new System.Drawing.Point(12, 12);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(359, 20);
            this.typeComboBox.TabIndex = 7;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.OnTypeComboBoxSelectedIndexChanged);
            // 
            // optionGroupBox
            // 
            this.optionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionGroupBox.Controls.Add(this.autoModeCheckBox);
            this.optionGroupBox.Controls.Add(this.renameOriginalCheckBox);
            this.optionGroupBox.Controls.Add(this.autoLineBreakCheckBox);
            this.optionGroupBox.Controls.Add(this.wordOrderCheckBox);
            this.optionGroupBox.Location = new System.Drawing.Point(12, 65);
            this.optionGroupBox.Name = "optionGroupBox";
            this.optionGroupBox.Size = new System.Drawing.Size(359, 83);
            this.optionGroupBox.TabIndex = 9;
            this.optionGroupBox.TabStop = false;
            this.optionGroupBox.Text = "オプション";
            // 
            // autoModeCheckBox
            // 
            this.autoModeCheckBox.AutoSize = true;
            this.autoModeCheckBox.Checked = true;
            this.autoModeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoModeCheckBox.Location = new System.Drawing.Point(15, 18);
            this.autoModeCheckBox.Name = "autoModeCheckBox";
            this.autoModeCheckBox.Size = new System.Drawing.Size(100, 16);
            this.autoModeCheckBox.TabIndex = 3;
            this.autoModeCheckBox.Text = "自動処理モード";
            this.autoModeCheckBox.UseVisualStyleBackColor = true;
            this.autoModeCheckBox.CheckedChanged += new System.EventHandler(this.OnAutoModeCheckedBoxCheckedChanged);
            // 
            // renameOriginalCheckBox
            // 
            this.renameOriginalCheckBox.AutoSize = true;
            this.renameOriginalCheckBox.Checked = true;
            this.renameOriginalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.renameOriginalCheckBox.Location = new System.Drawing.Point(146, 16);
            this.renameOriginalCheckBox.Name = "renameOriginalCheckBox";
            this.renameOriginalCheckBox.Size = new System.Drawing.Size(126, 16);
            this.renameOriginalCheckBox.TabIndex = 2;
            this.renameOriginalCheckBox.Text = "元のファイルをリネーム";
            this.renameOriginalCheckBox.UseVisualStyleBackColor = true;
            this.renameOriginalCheckBox.CheckedChanged += new System.EventHandler(this.OnRenameOriginalCheckBoxCheckedChanged);
            // 
            // autoLineBreakCheckBox
            // 
            this.autoLineBreakCheckBox.AutoSize = true;
            this.autoLineBreakCheckBox.Checked = true;
            this.autoLineBreakCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoLineBreakCheckBox.Location = new System.Drawing.Point(15, 38);
            this.autoLineBreakCheckBox.Name = "autoLineBreakCheckBox";
            this.autoLineBreakCheckBox.Size = new System.Drawing.Size(125, 16);
            this.autoLineBreakCheckBox.TabIndex = 1;
            this.autoLineBreakCheckBox.Text = "テキスト自動折り返し";
            this.autoLineBreakCheckBox.UseVisualStyleBackColor = true;
            this.autoLineBreakCheckBox.CheckedChanged += new System.EventHandler(this.OnAutoLineBreakCheckBoxCheckedChanged);
            // 
            // wordOrderCheckBox
            // 
            this.wordOrderCheckBox.AutoSize = true;
            this.wordOrderCheckBox.Location = new System.Drawing.Point(146, 38);
            this.wordOrderCheckBox.Name = "wordOrderCheckBox";
            this.wordOrderCheckBox.Size = new System.Drawing.Size(142, 16);
            this.wordOrderCheckBox.TabIndex = 0;
            this.wordOrderCheckBox.Text = "自動命名時の語順変更";
            this.wordOrderCheckBox.UseVisualStyleBackColor = true;
            this.wordOrderCheckBox.CheckedChanged += new System.EventHandler(this.OnWordOrderCheckBoxCheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 322);
            this.Controls.Add(this.optionGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.logRichTextBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(this.typeComboBox);
            this.MinimumSize = new System.Drawing.Size(400, 240);
            this.Name = "MainForm";
            this.Text = "Europa Engine Patcher";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnFormDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnFormDragEnter);
            this.optionGroupBox.ResumeLayout(false);
            this.optionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.RichTextBox logRichTextBox;
		private System.Windows.Forms.Button exitButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.GroupBox optionGroupBox;
        private System.Windows.Forms.CheckBox wordOrderCheckBox;
		private System.Windows.Forms.CheckBox autoLineBreakCheckBox;
        private System.Windows.Forms.CheckBox renameOriginalCheckBox;
        private System.Windows.Forms.CheckBox autoModeCheckBox;
	}
}