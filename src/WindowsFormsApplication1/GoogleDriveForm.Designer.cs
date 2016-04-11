namespace WindowsFormsApplication1
{
    partial class GoogleDriveForm
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
            this.ListViewFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonMoveToUpDirectory = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxSelectedDirectory = new System.Windows.Forms.TextBox();
            this.ButtonSelect = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListViewFile
            // 
            this.ListViewFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.ListViewFile.Location = new System.Drawing.Point(12, 37);
            this.ListViewFile.Name = "ListViewFile";
            this.ListViewFile.Size = new System.Drawing.Size(470, 457);
            this.ListViewFile.TabIndex = 0;
            this.ListViewFile.UseCompatibleStateImageBehavior = false;
            this.ListViewFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListViewFile_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Owner";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ID";
            // 
            // ButtonMoveToUpDirectory
            // 
            this.ButtonMoveToUpDirectory.Location = new System.Drawing.Point(12, 8);
            this.ButtonMoveToUpDirectory.Name = "ButtonMoveToUpDirectory";
            this.ButtonMoveToUpDirectory.Size = new System.Drawing.Size(75, 23);
            this.ButtonMoveToUpDirectory.TabIndex = 1;
            this.ButtonMoveToUpDirectory.Text = "Move to up";
            this.ButtonMoveToUpDirectory.UseVisualStyleBackColor = true;
            this.ButtonMoveToUpDirectory.Click += new System.EventHandler(this.ButtonMoveToUpDirectory_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 507);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected directory";
            // 
            // TextBoxSelectedDirectory
            // 
            this.TextBoxSelectedDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSelectedDirectory.Location = new System.Drawing.Point(117, 504);
            this.TextBoxSelectedDirectory.Name = "TextBoxSelectedDirectory";
            this.TextBoxSelectedDirectory.Size = new System.Drawing.Size(203, 19);
            this.TextBoxSelectedDirectory.TabIndex = 3;
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSelect.Location = new System.Drawing.Point(326, 502);
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelect.TabIndex = 4;
            this.ButtonSelect.Text = "Select";
            this.ButtonSelect.UseVisualStyleBackColor = true;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.Location = new System.Drawing.Point(407, 502);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // GoogleDriveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 531);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonSelect);
            this.Controls.Add(this.TextBoxSelectedDirectory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonMoveToUpDirectory);
            this.Controls.Add(this.ListViewFile);
            this.Name = "GoogleDriveForm";
            this.Text = "GoogleDriveForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ListViewFile;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button ButtonMoveToUpDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxSelectedDirectory;
        private System.Windows.Forms.Button ButtonSelect;
        private System.Windows.Forms.Button ButtonCancel;
    }
}