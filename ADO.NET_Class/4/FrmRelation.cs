using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO.NET_Class._4
{
    public partial class FrmRelation : Form
    {
        public FrmRelation()
        {
            InitializeComponent();
        }

        private void categoriesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.categoriesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.nWDataSet);

        }

        private void FrmRelation_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'nWDataSet.Products' 資料表。您可以視需要進行移動或移除。
            this.productsTableAdapter.Fill(this.nWDataSet.Products);
            // TODO: 這行程式碼會將資料載入 'nWDataSet.Categories' 資料表。您可以視需要進行移動或移除。
            this.categoriesTableAdapter.Fill(this.nWDataSet.Categories);

        }

        private void categoriesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            NWDataSet.CategoriesRow ParentRow = this.nWDataSet.Categories[this.categoriesBindingSource.Position];

            //ParentRow.GetChildRows();
            //ParentRow.GetParentRow();

            NWDataSet.ProductsRow[] childRows = ParentRow.GetProductsRows();

            this.listBox1.Items.Clear();
            foreach (NWDataSet.ProductsRow dr in childRows)
            {
                this.listBox1.Items.Add($"{dr.CategoryID} - {dr.ProductName}");
            }
        }
    }
}
