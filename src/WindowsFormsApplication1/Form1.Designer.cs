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
            this.TextboxDestAsm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonLoadDestAsm = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(631, 94);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 261);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.Button button1;
    }
}

