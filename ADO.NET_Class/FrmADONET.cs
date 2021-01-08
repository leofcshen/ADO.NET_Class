using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace ADO.NET_Class
{
    public partial class lblIndex : Form
    {
        public lblIndex()
        {
            InitializeComponent();
            this.tabControl1.SelectedIndex = this.tabControl1.TabCount - 1;
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.customersTableAdapter1.Fill(this.nwDataSet1.Customers);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Products", conn);
                    SqlDataReader datareader = comm.ExecuteReader();
                    while (datareader.Read())
                    {
                        string s = $"{datareader["ProductName"],-40}-${datareader["UnitPrice"]:c2}";
                        this.listBox1.Items.Add(s);
                    }
                    //MessageBox.Show(datareader["ProductName"].ToString());

                    //BindingSource Bs = new BindingSource();
                    //Bs.DataSource = datareader;
                    //this.dataGridView1.DataSource = Bs;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
            SqlDataAdapter  adapter= new SqlDataAdapter("select * from Products", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            this.dataGridView1.DataSource = ds.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True"))
                {
                    conn.Open();
                    MessageBox.Show(conn.State.ToString());
                    SqlCommand comm = new SqlCommand("select * from Products", conn);
                    SqlDataReader datareader = comm.ExecuteReader();
                    //while (datareader.Read())
                    //{
                    //    string s = $"{datareader["ProductName"],-40}-${datareader["UnitPrice"]:c2}";
                    //    this.listBox1.Items.Add(s);
                    //}
                    //MessageBox.Show(datareader["ProductName"].ToString());

                    BindingSource Bs = new BindingSource();
                    Bs.DataSource = datareader;
                    this.dataGridView1.DataSource = Bs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
            this.dataGridView1.DataSource = this.nwDataSet1.Categories;

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.dataGridView2.DataSource = this.nwDataSet1.Products;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Northwind;Integrated Security=True");
            SqlDataAdapter adapter = new SqlDataAdapter("select * from Products where UnitPrice >30", conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            this.dataGridView1.DataSource = ds.Tables[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.FillByUnitPrice(this.nwDataSet1.Products, 30);
            this.dataGridView2.DataSource = this.nwDataSet1.Products;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.InsertProduct("xxx", true);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.customersTableAdapter1.Fill(this.nwDataSet1.Customers);
            this.dataGridView2.DataSource = this.nwDataSet1.Customers;
        }
        BindingSource bs = null;
        private void button9_Click(object sender, EventArgs e)
        {
            this.categoriesTableAdapter1.Fill(this.nwDataSet1.Categories);
            //this.dataGridView3.DataSource = this.nwDataSet1.Categories;
            //this.bindingSource1.DataSource = this.nwDataSet1.Categories;
            //this.dataGridView3.DataSource = this.bindingSource1;

            bs = new BindingSource();
            this.bs.DataSource = this.nwDataSet1.Categories;
            dataGridView3.DataSource = bs;

            currentIndex();
            this.bindingNavigator1.BindingSource = bs;

            this.lblCategoryName.DataBindings.Add("Text", this.bs, "CategoryName");
            this.pictureBox1.DataBindings.Add("Image", this.bs, "Picture", true);
        }

        private void btnPre_Click(object sender, EventArgs e)
        {            
            this.bs.Position -= 1;
            currentIndex();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.bs.Position = 0;
            currentIndex();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.bs.Position += 1;
            currentIndex();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.bs.Position = nwDataSet1.Categories.Count - 1;
            currentIndex();
        }
        private void currentIndex()
        {
            lblCurrentIndex.Text = $"{this.bs.Position+1} / {this.bs.Count}";
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            FrmTool ft = new FrmTool();
            ft.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {            
            listBox2.Items.Add(this.nwDataSet1.Tables.Count);
            for(int i = 0; i <= nwDataSet1.Tables.Count-1; i++)
            {
                DataTable dt = nwDataSet1.Tables[i];
                this.listBox2.Items.Add(dt.TableName);
                string s = "";
                for (int column = 0; column <=dt.Columns.Count-1; column++)
                {
                    s += dt.Columns[column].ColumnName + "   ";
                }
                for(int row = 0; row <=dt.Rows.Count-1; row++)
                {
                    string z = "";
                    for (int j = 0; j <= dt.Columns.Count-1; j++)
                    {
                        z += $"{dt.Rows[row][j]}   ";
                    }
                    //listBox2.Items.Add(dt.Rows[row] + "  ");

                    this.listBox2.Items.Add(z);
                }               
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DataRow dr = this.nwDataSet1.Products.Rows[0];
            MessageBox.Show(dr["ProductName"].ToString());

            MessageBox.Show(this.nwDataSet1.Products.Rows[0]["ProductName"].ToString());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.nwDataSet1.Products.Rows[0]["ProductName"] += "4444";
            this.nwDataSet1.Products.WriteXml("products.xml", XmlWriteMode.WriteSchema);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.nwDataSet1.Products.Clear();
            this.nwDataSet1.Products.ReadXml("products.xml");
            this.dataGridView4.DataSource = this.nwDataSet1.Products;
        }

        
    }
}

