using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.Runtime.InteropServices; // for com
using IWshRuntimeLibrary; // for shortcuts, needs windows script host object model

using System.Xml;
using System.IO;
//using Google.GData.Client;
//using Google.GData.Spreadsheets;



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool debug;

        public Form1()
        {
            InitializeComponent();
#if DEBUG
            debug = true;
#else
            debug = false;
#endif
        }


        private string ResolveShorcut(string filename)
        {
            //WshShellClass shell = new WshShellClass(); // WshShellClassの型でエラーが出る
            //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(filename);
            //return shortcut.TargetPath;
            return "hoge";
        }

        private bool IsSolidEdge()
        {
            SolidEdgeFramework.Application app = null;
            bool ret = false;

            try
            {
                //connect to solidedge instance
                app = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                ret = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("SolidEdgeが見つかりません");
            }
            finally
            {
                if (app != null)
                {
                    Marshal.ReleaseComObject(app);
                    app = null;
                }
            }
            return ret;
        }

        private void TextboxDestAsm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                if (debug) MessageBox.Show(s[0]);
                string ext = System.IO.Path.GetExtension(s[0]);
                if (ext == ".asm")
                {
                    TextboxDestAsm.Text = s[0];
                }
                else if (ext == ".lnk")
                {
                    string filename = ResolveShorcut(s[0]);
                    if (System.IO.Path.GetExtension(filename) == ".asm")
                    {
                        TextboxDestAsm.Text = s[0];
                    }
                }
            }
        }

        private void TextboxDestAsm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ButtonLoadDestAsm_Click(object sender, EventArgs e)
        {
            // OpenFileDialog の新しいインスタンスを生成する (デザイナから追加している場合は必要ない)
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Asmを追加";
            dialog.Filter = "asmファル|*.asm";
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TextboxDestAsm.Text = dialog.FileName;
            }
            dialog.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(TextboxDestAsm.Text);

            if (!IsSolidEdge()) return;
            getChildFile();
        }

        private void getChildFile()
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument draft = null;
            bool IsOverwrite = CheckboxIsOverwrite.Checked;
            bool ret = false;

            try
            {
                //check if the file exists
                if (!System.IO.File.Exists(filename))
                    throw (new System.Exception("file not found: " + filename));

                //check if the directory
                if (!System.IO.Directory.Exists(destdir))
                    throw (new System.Exception("directory not found: " + destdir));

                //check if the file ext is dft
                if (System.IO.Path.GetExtension(filename) != ".dft")
                    throw (new System.Exception("This is not a Draft file: " + filename));

                //connect to solidedge instance
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject("SolidEdge.Application");
                documents = application.Documents;
                if (debug) MessageBox.Show("solid edge found");

                if (debug) MessageBox.Show("open draft");
                //draft = (SolidEdgeDraft.DraftDocument)documents.Add("SolidEdge.DraftDocument", Missing.Value);
                draft = (SolidEdgeDraft.DraftDocument)documents.Open(filename);


                string outfile = destdir + "\\" + System.IO.Path.GetFileNameWithoutExtension(filename) + "." + ext;
                if (debug) MessageBox.Show("filename: " + outfile);

                if (IsOverwrite)
                {
                    //delete exist file
                    if (debug) MessageBox.Show("delete file");
                    System.IO.File.Delete(outfile);
                }

                if (debug) MessageBox.Show("save file");
                draft.SaveAs(outfile);

                if (debug) MessageBox.Show("close draft");
                draft.Close(false);

                ret = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (draft != null)
                {
                    Marshal.ReleaseComObject(draft);
                    draft = null;
                }
                if (documents != null)
                {
                    Marshal.ReleaseComObject(documents);
                    documents = null;
                }
                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                    application = null;
                }
            }

            return;
        }
    }
}
