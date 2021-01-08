using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NET_Class
{
    public partial class FrmTool : Form
    {
        public FrmTool()
        {
            InitializeComponent();            
        }
        OpenFileDialog ofd = null;

        private void categoriesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.categoriesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.nWDataSet);

        }

        private void categoriesBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.categoriesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.nWDataSet);

        }

        private void FrmTool_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'nWDataSet.Categories' 資料表。您可以視需要進行移動或移除。
            this.categoriesTableAdapter.Fill(this.nWDataSet.Categories);

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ofd= new OpenFileDialog();
            if (this.ofd.ShowDialog() == DialogResult.OK)
            {
                this.picturePictureBox.Image = Image.FromFile(this.ofd.FileName);
            }
        }
    }
}
