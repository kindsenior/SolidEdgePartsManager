namespace WindowsFormsApplication1
{
    partial class ProcessTypeForm
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
            this.ComboBoxProcessType = new System.Windows.Forms.ComboBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonSkip = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxFileName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ComboBoxProcessType
            // 
            this.ComboBoxProcessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxProcessType.FormattingEnabled = true;
            this.ComboBoxProcessType.Location = new System.Drawing.Point(90, 103);
            this.ComboBoxProcessType.Name = "ComboBoxProcessType";
            this.ComboBoxProcessType.Size = new System.Drawing.Size(150, 20);
            this.ComboBoxProcessType.TabIndex = 0;
            // 
            // ButtonOK
            // 
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(246, 101);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonSkip
            // 
            this.ButtonSkip.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.ButtonSkip.Location = new System.Drawing.Point(327, 101);
            this.ButtonSkip.Name = "ButtonSkip";
            this.ButtonSkip.Size = new System.Drawing.Size(75, 23);
            this.ButtonSkip.TabIndex = 2;
            this.ButtonSkip.Text = "Skip";
            this.ButtonSkip.UseVisualStyleBackColor = true;
            this.ButtonSkip.Click += new System.EventHandler(this.ButtonSkip_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Process type";
            // 
            // TextBoxFileName
            // 
            this.TextBoxFileName.Location = new System.Drawing.Point(90, 23);
            this.TextBoxFileName.Name = "TextBoxFileName";
            this.TextBoxFileName.ReadOnly = true;
            this.TextBoxFileName.Size = new System.Drawing.Size(292, 19);
            this.TextBoxFileName.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 48);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(388, 47);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Add: パーツリストを作成する(Linkレベル)\r\nOpen: この階層ではパーツリストを作成せずに，アセンブリを展開する(Limbレベル)\r\nSkip: 何も" +
    "処理せずに次のアセンブリまたはパーツ,シートメタルを処理する\r\nException: 使わない";
            // 
            // ProcessTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 136);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.TextBoxFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonSkip);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ComboBoxProcessType);
            this.Name = "ProcessTypeForm";
            this.Text = "Process type選択フォーム";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonSkip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxFileName;
        public System.Windows.Forms.ComboBox ComboBoxProcessType;
        private System.Windows.Forms.TextBox textBox1;
    }
}