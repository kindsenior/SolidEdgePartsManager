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
    public partial class GoogleDriveForm : Form
    {
        public GoogleDriveForm()
        {
            InitializeComponent();

            ListViewDirectory.View = View.Details;

            UpdateListViewDirectory();
            ListViewDirectory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void UpdateListViewDirectory(String parentId = "root")
        {
            GoogleDriveManager googleDriveManager = GoogleDriveManager.Instance;

            ListViewDirectory.Items.Clear();
            ListViewItem item = null;
            ListViewItem.ListViewSubItem[] subItems;
            foreach (Google.Apis.Drive.v2.Data.File file in googleDriveManager.getChildFolder(parentId))// get and add child dirs
            {
                item = new ListViewItem(file.Title.ToString(), 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, file.OwnerNames[0].ToString()),
                     new ListViewItem.ListViewSubItem(item, file.Id.ToString())};
                item.SubItems.AddRange(subItems);
                ListViewDirectory.Items.Add(item);
            }
        }

        private void ListViewDirectory_DoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("GoogleDriveForm.ListViewDirectory_DoubleClick(" + ListViewDirectory.SelectedItems[0].Text.ToString() + ")");
            System.Windows.Forms.ListViewItem selectedItem = ListViewDirectory.SelectedItems[0];
            UpdateListViewDirectory(selectedItem.SubItems[2].Text.ToString());
        }

        private void ButtonMoveToUpDirectory_Click(object sender, EventArgs e)
        {
            UpdateListViewDirectory();
        }
    }
}
