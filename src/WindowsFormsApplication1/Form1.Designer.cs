namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.TextboxDestAsm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonLoadDestAsm = new System.Windows.Forms.Button();
            this.ButtonUpdateAllPartsNumber = new System.Windows.Forms.Button();
            this.TextboxImiAccount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxImiPass = new System.Windows.Forms.TextBox();
            this.ButtonUpdatePartsProperties = new System.Windows.Forms.Button();
            this.CheckboxAutoRetry = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TextboxDestAsm
            // 
            this.TextboxDestAsm.AllowDrop = true;
            this.TextboxDestAsm.Location = new System.Drawing.Point(74, 12);
            this.TextboxDestAsm.Name = "TextboxDestAsm";
            this.TextboxDestAsm.Size = new System.Drawing.Size(551, 19);
            this.TextboxDestAsm.TabIndex = 0;
            this.TextboxDestAsm.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextboxDestAsm_DragDrop);
            this.TextboxDestAsm.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextboxDestAsm_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dest Asm";
            // 
            // ButtonLoadDestAsm
            // 
            this.ButtonLoadDestAsm.Location = new System.Drawing.Point(631, 10);
            this.ButtonLoadDestAsm.Name = "ButtonLoadDestAsm";
            this.ButtonLoadDestAsm.Size = new System.Drawing.Size(75, 23);
            this.ButtonLoadDestAsm.TabIndex = 2;
            this.ButtonLoadDestAsm.Text = "選択";
            this.ButtonLoadDestAsm.UseVisualStyleBackColor = true;
            this.ButtonLoadDestAsm.Click += new System.EventHandler(this.ButtonLoadDestAsm_Click);
            // 
            // ButtonUpdateAllPartsNumber
            // 
            this.ButtonUpdateAllPartsNumber.Location = new System.Drawing.Point(571, 73);
            this.ButtonUpdateAllPartsNumber.Name = "ButtonUpdateAllPartsNumber";
            this.ButtonUpdateAllPartsNumber.Size = new System.Drawing.Size(135, 23);
            this.ButtonUpdateAllPartsNumber.TabIndex = 3;
            this.ButtonUpdateAllPartsNumber.Text = "Update All Parts List";
            this.ButtonUpdateAllPartsNumber.UseVisualStyleBackColor = true;
            this.ButtonUpdateAllPartsNumber.Click += new System.EventHandler(this.ButtonUpdateAllPartsNumber_Click);
            // 
            // TextboxImiAccount
            // 
            this.TextboxImiAccount.AllowDrop = true;
            this.TextboxImiAccount.Location = new System.Drawing.Point(109, 46);
            this.TextboxImiAccount.Name = "TextboxImiAccount";
            this.TextboxImiAccount.Size = new System.Drawing.Size(225, 19);
            this.TextboxImiAccount.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "JSK imi Account";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pass";
            // 
            // TextBoxImiPass
            // 
            this.TextBoxImiPass.AllowDrop = true;
            this.TextBoxImiPass.Location = new System.Drawing.Point(376, 46);
            this.TextBoxImiPass.Name = "TextBoxImiPass";
            this.TextBoxImiPass.PasswordChar = '*';
            this.TextBoxImiPass.Size = new System.Drawing.Size(189, 19);
            this.TextBoxImiPass.TabIndex = 7;
            // 
            // ButtonUpdatePartsProperties
            // 
            this.ButtonUpdatePartsProperties.Location = new System.Drawing.Point(571, 102);
            this.ButtonUpdatePartsProperties.Name = "ButtonUpdatePartsProperties";
            this.ButtonUpdatePartsProperties.Size = new System.Drawing.Size(135, 23);
            this.ButtonUpdatePartsProperties.TabIndex = 8;
            this.ButtonUpdatePartsProperties.Text = "Update Parts Propeties";
            this.ButtonUpdatePartsProperties.UseVisualStyleBackColor = true;
            this.ButtonUpdatePartsProperties.Click += new System.EventHandler(this.ButtonUpdatePartsProperties_Click);
            // 
            // CheckboxConfirmUpdate
            // 
            this.CheckboxAutoRetry.AutoSize = true;
            this.CheckboxAutoRetry.Checked = true;
            this.CheckboxAutoRetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckboxAutoRetry.Location = new System.Drawing.Point(461, 77);
            this.CheckboxAutoRetry.Name = "CheckboxConfirmUpdate";
            this.CheckboxAutoRetry.Size = new System.Drawing.Size(80, 16);
            this.CheckboxAutoRetry.TabIndex = 9;
            this.CheckboxAutoRetry.Text = "Auto Retry";
            this.CheckboxAutoRetry.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 261);
            this.Controls.Add(this.CheckboxAutoRetry);
            this.Controls.Add(this.ButtonUpdatePartsProperties);
            this.Controls.Add(this.TextBoxImiPass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextboxImiAccount);
            this.Controls.Add(this.ButtonUpdateAllPartsNumber);
            this.Controls.Add(this.ButtonLoadDestAsm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextboxDestAsm);
            this.Name = "Form1";
            this.Text = "SolidEdge Parts Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextboxDestAsm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonLoadDestAsm;
        private System.Windows.Forms.Button ButtonUpdateAllPartsNumber;
        private System.Windows.Forms.TextBox TextboxImiAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxImiPass;
        private System.Windows.Forms.Button ButtonUpdatePartsProperties;
        private System.Windows.Forms.CheckBox CheckboxAutoRetry;
    }
}

