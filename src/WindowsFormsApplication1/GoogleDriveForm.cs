using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public struct SheetData
    {
        public String name, id, parentId;
    }

    public partial class GoogleDriveForm : Form
    {
        private List<String> directoryPathList;// 過去のディレクトリの移動履歴リスト
        private const int idIdx = 2, typeIdx = 3, parentIdIdx = 4;// ListViewFile.Items.SubItems内のインデクス
        public SheetData selectedSheetData;// 選択中のSpreadsheetのデータ (Form1クラスからはこの変数にアクセスする)

        public GoogleDriveForm()
        {
            InitializeComponent();

            ListViewFile.View = View.Details;

            directoryPathList = new List<String>() {"root"};
            UpdateListViewFile();
            ListViewFile.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void UpdateListViewFile(String parentId = "root")
        {
            GoogleDriveManager googleDriveManager = GoogleDriveManager.Instance;

            ListViewFile.Items.Clear();
            ListViewItem item = null;
            ListViewItem.ListViewSubItem[] subItems;
            foreach (Google.Apis.Drive.v2.Data.File file in googleDriveManager.getChildFolder(parentId))// get and add child dirs
            {
                item = new ListViewItem(file.Title.ToString(), 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, file.OwnerNames[0].ToString()),// owner name
                     new ListViewItem.ListViewSubItem(item, file.Id.ToString()),// id
                     new ListViewItem.ListViewSubItem(item, file.MimeType.ToString()),// mime type
                     new ListViewItem.ListViewSubItem(item, file.Parents[0].Id.ToString())};// parent id
                item.SubItems.AddRange(subItems);
                ListViewFile.Items.Add(item);
            }
        }

        private void ListViewFile_DoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("GoogleDriveForm.ListViewDirectory_DoubleClick(" + ListViewFile.SelectedItems[0].Text.ToString() + ")");
            System.Windows.Forms.ListViewItem selectedItem = ListViewFile.SelectedItems[0];
            if (selectedItem.SubItems[typeIdx].Text == "application/vnd.google-apps.folder")// ディレクトリがクリックされた時だけ
            {
                String id = selectedItem.SubItems[idIdx].Text.ToString();
                directoryPathList.Add(id);
                UpdateListViewFile(id);
            }
        }

        private void ButtonMoveToUpDirectory_Click(object sender, EventArgs e)
        {
            int count = directoryPathList.Count;
            if (count > 1)
            {
                directoryPathList.RemoveAt(directoryPathList.Count - 1);
                UpdateListViewFile(directoryPathList.Last());
            }
        }

        private void ListViewFile_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ListViewFile.SelectedItems.Count > 0)
            {
                System.Windows.Forms.ListViewItem selectedItem = ListViewFile.SelectedItems[0];
                if (selectedItem.SubItems[typeIdx].Text == "application/vnd.google-apps.spreadsheet")
                {
                    selectedSheetData.name = selectedItem.Text.ToString();
                    selectedSheetData.id = selectedItem.SubItems[idIdx].Text.ToString();
                    selectedSheetData.parentId = selectedItem.SubItems[parentIdIdx].Text.ToString();
                    TextBoxSelectedSpreadsheet.Text = selectedSheetData.name;
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
