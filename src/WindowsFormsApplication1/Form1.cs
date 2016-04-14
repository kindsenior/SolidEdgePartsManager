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
//using IWshRuntimeLibrary; // for shortcuts, needs windows script host object model

//using System.Xml;
//using System.IO;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        bool debug;
        private CustomedSettings settings;
        private SheetData destSheetData;


        public Form1()
        {
            InitializeComponent();
            settings = new CustomedSettings();
            destSheetData = new SheetData();

            //reflect ComboBoxの設定値
            if (settings.DestAsmHashSet == null)// 設定が保存されていない場合
            {
                settings.DestAsmHashSet = new HashSet<string>();
                settings.SelectedAsmIdx = -1;
            }
            else
            {
                foreach (String fname in settings.DestAsmHashSet)
                {
                    ComboBoxDestAsm.Items.Add(fname);
                }
                ComboBoxDestAsm.SelectedIndex = settings.SelectedAsmIdx;
            }
            if (settings.DestSheetDataHashSet == null)// 設定が保存されていない場合
            {
                settings.DestSheetDataHashSet = new HashSet<SheetData>();
                settings.SelectedSheetIdx = -1;
            }
            else
            {
                foreach (SheetData sheetData in settings.DestSheetDataHashSet)
                {
                    ComboBoxDestSheet.Items.Add(sheetData.name);
                }
                ComboBoxDestSheet.SelectedIndex = settings.SelectedSheetIdx;
            }

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

        private void AddAndSelectComboBoxDestAsm(String str)
        {
            settings.DestAsmHashSet.Add(str);
            if (settings.DestAsmHashSet.Count != ComboBoxDestAsm.Items.Count) ComboBoxDestAsm.Items.Add(str);
            //ComboBoxDestAsm.Text = str;
            ComboBoxDestAsm.SelectedIndex = ComboBoxDestAsm.Items.IndexOf(str);
        }

        private void ComboBoxDestAsm_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length > 0)
            {
                if (debug) MessageBox.Show(s[0]);
                string ext = System.IO.Path.GetExtension(s[0]);
                if (ext == ".asm")
                {
                    AddAndSelectComboBoxDestAsm(s[0]);
                }
                else if (ext == ".lnk")
                {
                    string filename = ResolveShorcut(s[0]);
                    if (System.IO.Path.GetExtension(filename) == ".asm")
                    {
                        AddAndSelectComboBoxDestAsm(s[0]);
                    }
                }
            }
        }

        private void ComboBoxDestAsm_DragEnter(object sender, DragEventArgs e)
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
                AddAndSelectComboBoxDestAsm(dialog.FileName);
            }
            dialog.Dispose();
        }

        private void ButtonUpdateAllPartsNumber_Click(object sender, EventArgs e)
        {
            Console.WriteLine("UpdateAllPartsNumber()");

            if (!IsSolidEdge()) return;

            //spreadsheetManager.GetPartsPathFromGDrive(service);

            SolidEdgeManager solidedgeManager = new SolidEdgeManager();

            //TextboxDestAsm.Text = "\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\Jaxon2.asm";
            Dictionary<string, string> partsListDictionary = new Dictionary<string, string>();
            solidedgeManager.createPartsListDictionary(ref partsListDictionary, ComboBoxDestAsm.Text, CheckboxAutoRetry.Checked);

            //List<SpreadsheetManager> spreadsheetManagerList = new List<SpreadsheetManager>();
            foreach (System.Collections.Generic.KeyValuePair<string,string> partsListPair in partsListDictionary)
            {
                //SpreadsheetManager spreadsheetManager = new SpreadsheetManager(service);
                //spreadsheetManager.PasteToWorksheet(partsListPair.Key, partsListPair.Value);
                //spreadsheetManagerList.Add(spreadsheetManager);
                string fileName = convertStringToCsv(partsListPair.Key, partsListPair.Value);
                uploadCsvFile(fileName);
            }

            //foreach (SpreadsheetManager spreadsheetManager in spreadsheetManagerList)
            //{
                //spreadsheetManager.Join();
            //}
             
            //solidedgeManager.CopyPartsListToClipboard("\\\\andromeda\\share1\\STARO\\CAD\\JAXON2\\LEG\\dft\\Hip-yaw-link.dft", CheckboxConfirmUpdate.Checked);

            MessageBox.Show("Finished updating all parts number");
        }

        private string convertStringToCsv(string linkName, string partsListStr)
        {
            Console.WriteLine("convertStringToCsv( " + linkName + ",<partsListStr>)");

            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+ "\\" + linkName + ".csv";
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName, false, sjisEnc);
            
            List<string> rowStrings = new List<string>(partsListStr.Split(new Char[] { '\n' }));
            rowStrings.RemoveAt(rowStrings.Count - 1);//空行を削除
            foreach (string rowString in rowStrings)
            {
                string replacedString = rowString.Replace(',', '-');
                replacedString = replacedString.Replace('\t', ',');
                Console.WriteLine(replacedString);
                writer.WriteLine(replacedString);
            }
            writer.Close();

            return fileName;
        }

        private void uploadCsvFile(string fileName)
        {
            Console.WriteLine("uploadCsvFile(" + fileName + ")");

            // file creation
            File body = new File();
            body.Title = System.IO.Path.GetFileName(fileName);
            body.Description = "parts list of " + System.IO.Path.GetFileNameWithoutExtension(fileName);
            body.MimeType = "text/plain";
            body.Parents = new List<ParentReference>() { new ParentReference() { Id = destSheetData.parentId } };// DestSheetのディレクトリを指定
            
            // file contents
            byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            //file upload
            GoogleDriveManager googleDriveManager = GoogleDriveManager.Instance;
            googleDriveManager.uploadFile(body, stream);

            System.IO.File.Delete(fileName);
        }

        private void ButtonUpdatePartsProperties_Click(object sender, EventArgs e)
        {
            Console.WriteLine("UpdatePartsProperties()");

            if (!IsSolidEdge()) return;

            //RefreshAccessToken();
            //SpreadsheetManager spreadsheetManager = new SpreadsheetManager(TextboxImiAccount.Text, TextBoxImiPass.Text);
            SpreadsheetManager spreadsheetManager = SpreadsheetManager.Instance;
            //if (!spreadsheetManager.m_feedFlg) return;

            SolidEdgeManager solidedgeManager = new SolidEdgeManager();

            //spreadsheetManager.SetSpreadsheetByName("JAXON2図番管理表");
            spreadsheetManager.SetSpreadsheetById(destSheetData.id);
            Dictionary<string, Dictionary<string, string>> propertySetDictionary = spreadsheetManager.GetPartsProperties();

            solidedgeManager.SetPartsProperties(propertySetDictionary, CheckboxAutoRetry.Checked);

            MessageBox.Show("Finished updating parts properties");
        }

        private void ButtonSelectDestSheet_Click(object sender, EventArgs e)
        {
            GoogleDriveForm googleDriveForm = new GoogleDriveForm();
            if (googleDriveForm.ShowDialog() == DialogResult.OK)
            {
                destSheetData = googleDriveForm.selectedSheetData;
                settings.DestSheetDataHashSet.Add(destSheetData);
                if (settings.DestSheetDataHashSet.Count != ComboBoxDestSheet.Items.Count) ComboBoxDestSheet.Items.Add(destSheetData.name);
                ComboBoxDestSheet.SelectedIndex = ComboBoxDestSheet.Items.IndexOf(destSheetData.name);
            }
            googleDriveForm.Dispose();
        }

        private void ComboBoxDestSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ComboBoxDestSheet_SelectedIndexChanged() idx:" + ComboBoxDestSheet.SelectedIndex);
            int selectedIdx = ComboBoxDestSheet.SelectedIndex;
            destSheetData = settings.DestSheetDataHashSet.ElementAt(selectedIdx);
            settings.SelectedSheetIdx = selectedIdx;
            settings.Save();
        }

        //Textをプログラムで書き換えた時はなぜか2回実行される
        private void ComboBoxDestAsm_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("ComboBoxDestAsm_TextChanged() idx:" + ComboBoxDestAsm.SelectedIndex + " text:" + ComboBoxDestAsm.Text);
            settings.SelectedAsmIdx = ComboBoxDestAsm.SelectedIndex;
            settings.Save();
        }

        //プルダウンをGUIで変更した時に実行
        private void ComboBoxDestAsm_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Console.WriteLine("ComboBoxDestAsm_SelectionChangedCommitted() idx:" + ComboBoxDestAsm.SelectedIndex);
            settings.SelectedAsmIdx = ComboBoxDestAsm.SelectedIndex;
            settings.Save();
        }

    }
}
