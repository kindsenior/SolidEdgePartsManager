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
    public partial class ProcessTypeForm : Form
    {
        public ProcessTypeForm()
        {
            InitializeComponent();
            ComboBoxProcessType.DataSource = Enum.GetValues(typeof(ProcessType));
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSkip_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
