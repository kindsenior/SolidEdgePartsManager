﻿namespace WindowsFormsApplication1
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
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonLoadDestAsm = new System.Windows.Forms.Button();
            this.ButtonUpdateAllPartsNumber = new System.Windows.Forms.Button();
            this.ButtonUpdatePartsProperties = new System.Windows.Forms.Button();
            this.CheckboxAutoRetry = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ButtonSelectDestSheet = new System.Windows.Forms.Button();
            this.ComboBoxDestSheet = new System.Windows.Forms.ComboBox();
            this.ComboBoxDestAsm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.ButtonLoadDestAsm.Location = new System.Drawing.Point(571, 10);
            this.ButtonLoadDestAsm.Name = "ButtonLoadDestAsm";
            this.ButtonLoadDestAsm.Size = new System.Drawing.Size(135, 23);
            this.ButtonLoadDestAsm.TabIndex = 2;
            this.ButtonLoadDestAsm.Text = "選択";
            this.ButtonLoadDestAsm.UseVisualStyleBackColor = true;
            this.ButtonLoadDestAsm.Click += new System.EventHandler(this.ButtonLoadDestAsm_Click);
            // 
            // ButtonUpdateAllPartsNumber
            // 
            this.ButtonUpdateAllPartsNumber.Location = new System.Drawing.Point(98, 83);
            this.ButtonUpdateAllPartsNumber.Name = "ButtonUpdateAllPartsNumber";
            this.ButtonUpdateAllPartsNumber.Size = new System.Drawing.Size(135, 23);
            this.ButtonUpdateAllPartsNumber.TabIndex = 3;
            this.ButtonUpdateAllPartsNumber.Text = "Update All Parts List";
            this.ButtonUpdateAllPartsNumber.UseVisualStyleBackColor = true;
            this.ButtonUpdateAllPartsNumber.Click += new System.EventHandler(this.ButtonUpdateAllPartsNumber_Click);
            // 
            // ButtonUpdatePartsProperties
            // 
            this.ButtonUpdatePartsProperties.Location = new System.Drawing.Point(98, 112);
            this.ButtonUpdatePartsProperties.Name = "ButtonUpdatePartsProperties";
            this.ButtonUpdatePartsProperties.Size = new System.Drawing.Size(135, 23);
            this.ButtonUpdatePartsProperties.TabIndex = 8;
            this.ButtonUpdatePartsProperties.Text = "Update Parts Propeties";
            this.ButtonUpdatePartsProperties.UseVisualStyleBackColor = true;
            this.ButtonUpdatePartsProperties.Click += new System.EventHandler(this.ButtonUpdatePartsProperties_Click);
            // 
            // CheckboxAutoRetry
            // 
            this.CheckboxAutoRetry.AutoSize = true;
            this.CheckboxAutoRetry.Checked = true;
            this.CheckboxAutoRetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckboxAutoRetry.Location = new System.Drawing.Point(12, 87);
            this.CheckboxAutoRetry.Name = "CheckboxAutoRetry";
            this.CheckboxAutoRetry.Size = new System.Drawing.Size(80, 16);
            this.CheckboxAutoRetry.TabIndex = 9;
            this.CheckboxAutoRetry.Text = "Auto Retry";
            this.CheckboxAutoRetry.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Dest Sheet";
            // 
            // ButtonSelectDestSheet
            // 
            this.ButtonSelectDestSheet.Location = new System.Drawing.Point(571, 35);
            this.ButtonSelectDestSheet.Name = "ButtonSelectDestSheet";
            this.ButtonSelectDestSheet.Size = new System.Drawing.Size(135, 23);
            this.ButtonSelectDestSheet.TabIndex = 12;
            this.ButtonSelectDestSheet.Text = "選択";
            this.ButtonSelectDestSheet.UseVisualStyleBackColor = true;
            this.ButtonSelectDestSheet.Click += new System.EventHandler(this.ButtonSelectDestSheet_Click);
            // 
            // ComboBoxDestSheet
            // 
            this.ComboBoxDestSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxDestSheet.Location = new System.Drawing.Point(74, 37);
            this.ComboBoxDestSheet.Name = "ComboBoxDestSheet";
            this.ComboBoxDestSheet.Size = new System.Drawing.Size(491, 20);
            this.ComboBoxDestSheet.TabIndex = 0;
            this.ComboBoxDestSheet.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDestSheet_SelectedIndexChanged);
            // 
            // ComboBoxDestAsm
            // 
            this.ComboBoxDestAsm.AllowDrop = true;
            this.ComboBoxDestAsm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxDestAsm.FormattingEnabled = true;
            this.ComboBoxDestAsm.Location = new System.Drawing.Point(74, 10);
            this.ComboBoxDestAsm.Name = "ComboBoxDestAsm";
            this.ComboBoxDestAsm.Size = new System.Drawing.Size(491, 20);
            this.ComboBoxDestAsm.TabIndex = 13;
            this.ComboBoxDestAsm.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxDestAsm_SelectionChangeCommitted);
            this.ComboBoxDestAsm.TextChanged += new System.EventHandler(this.ComboBoxDestAsm_TextChanged);
            this.ComboBoxDestAsm.DragDrop += new System.Windows.Forms.DragEventHandler(this.ComboBoxDestAsm_DragDrop);
            this.ComboBoxDestAsm.DragEnter += new System.Windows.Forms.DragEventHandler(this.ComboBoxDestAsm_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "Asmからパーツリストのcsvを作成";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "Spreadsheetの内容をプロパティに反映";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ComboBoxDestAsm);
            this.Controls.Add(this.ComboBoxDestSheet);
            this.Controls.Add(this.ButtonSelectDestSheet);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CheckboxAutoRetry);
            this.Controls.Add(this.ButtonUpdatePartsProperties);
            this.Controls.Add(this.ButtonUpdateAllPartsNumber);
            this.Controls.Add(this.ButtonLoadDestAsm);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "SolidEdge Parts Manager";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ComboBoxDestAsm_DragDrop);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonLoadDestAsm;
        private System.Windows.Forms.Button ButtonUpdateAllPartsNumber;
        private System.Windows.Forms.Button ButtonUpdatePartsProperties;
        private System.Windows.Forms.CheckBox CheckboxAutoRetry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ButtonSelectDestSheet;
        private System.Windows.Forms.ComboBox ComboBoxDestSheet;
        private System.Windows.Forms.ComboBox ComboBoxDestAsm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

