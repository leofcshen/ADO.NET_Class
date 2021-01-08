using ADO.NET_Class.Properties;
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

namespace Starter
{
    public partial class FrmConnected : Form
    {
        public FrmConnected()
        {
            InitializeComponent();
            this.listView1.View = View.Details;
            LoadCountryToComboBox();
            CreateListViewColumnHeader();
        }
        private void LoadCountryToComboBox()
        {
            try
            {
                string connString = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = null;
                    command = new SqlCommand("Select distinct Country from Customers", conn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.comboBox1.Items.Clear();
                    this.comboBox1.Items.Add("All Country");
                    while (dataReader.Read())
                    {
                        this.comboBox1.Items.Add(dataReader["Country"]);
                    }
                    this.comboBox1.SelectedIndex = 0;
                } //Auton conn.close(); conn.Dispose()
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CreateListViewColumnHeader()
        {
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.LargeImageList = ImageList1;
            listView1.SmallImageList = ImageList2;
            string connString = Settings.Default.NorthwindConnectionString;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                SqlCommand comm = new SqlCommand("select * from Customers", conn);
                SqlDataReader dr = comm.ExecuteReader();
                DataTable dt = dr.GetSchemaTable();
                this.dataGridView1.DataSource = dt;

                for (int i = 0; i <= dt.Rows.Count -1; i++)
                {
                    this.listView1.Columns.Add(dt.Rows[i][0].ToString());
                }
                this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.comboBox1.SelectedIndex = 0;
            try
            {
                string connString = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = null;
                    if (comboBox1.Text == "All Country")
                        command = new SqlCommand($"Select * from Customers", conn);
                    else
                        command = new SqlCommand($"Select * from Customers where Country='{this.comboBox1.Text }'", conn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.listView1.Items.Clear();
                    Random r = new Random();
                    while (dataReader.Read())
                    {
                        ListViewItem lv = this.listView1.Items.Add(dataReader[0].ToString());                        
                        lv.ImageIndex = r.Next(0, this.ImageList1.Images.Count);
                        //ImageList1.Images.Keys.ToString();
                        //MessageBox.Show(ImageList1.Images.IndexOfKey("CTRUSA").ToString());
                        if (lv.Index % 2 == 0)
                        {
                            lv.BackColor = Color.Orange;
                        }
                        else
                        {
                            lv.BackColor = Color.LightGray;
                        }

                        for (int i = 1; i <= dataReader.FieldCount -1; i++)
                        {
                            if (dataReader.IsDBNull(i))
                            {
                                lv.SubItems.Add("Null");
                            }
                            else
                            {
                                lv.SubItems.Add(dataReader[i].ToString());
                            }
                        }
                    }
                    this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                } //Auton conn.close(); conn.Dispose()
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.LargeIcon;
        }

        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.SmallIcon;
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
        }

    }
}
