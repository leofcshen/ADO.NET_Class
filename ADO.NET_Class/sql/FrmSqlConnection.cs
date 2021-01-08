using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using ADO.NET_Class.Properties;
using System.Threading;

namespace Starter

{
    public partial class FrmSqlConnection : Form
    {
        public FrmSqlConnection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = "Data Source=.;Initial Catalog=Northwind;Integrated Security=True";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                MessageBox.Show("OK");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connString = "Data Source=.;Initial Catalog=Northwind;User ID=sa;Password=a258456k";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ADO.NET_Class.Properties.Settings.NorthwindConnectionString"].ConnectionString;
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings[1].ConnectionString;
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string conn = Settings.Default.AdventureWorksConnectionString;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Github\ADO.NET_Class\ADO.NET_Class\sql\Database1.mdf;Integrated Security=True";                
                string connStr = Settings.Default.LocalDB;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Member", conn);
                    SqlDataReader da = comm.ExecuteReader();
                    this.listBox1.Items.Clear();
                    while (da.Read())
                    {
                        this.listBox1.Items.Add(da["UserName"]);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\sql\Database1.mdf;Integrated Security=True";
                //string connStr = Settings.Default.LocalDB;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Member", conn);
                    SqlDataReader da = comm.ExecuteReader();
                    this.listBox1.Items.Clear();
                    while (da.Read())
                    {
                        this.listBox1.Items.Add(da["UserName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"(LocalDB)\MSSQLLocalDB";
                builder.ApplicationName = Application.StartupPath + @"\sql\Database1.mdf";
                builder.IntegratedSecurity = true;

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = builder.ConnectionString;
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Member", conn);
                    SqlDataReader da = comm.ExecuteReader();
                    this.listBox1.Items.Clear();
                    while (da.Read())
                    {
                        this.listBox1.Items.Add(da["UserName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Connection.StateChange += new StateChangeEventHandler(Change);
            this.productsTableAdapter1.Connection.StateChange += Connection_StateChange;
            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.dataGridView1.DataSource = this.nwDataSet1.Products;
        }

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            this.statusStrip1.Items[0].Text = e.CurrentState.ToString();
            Application.DoEvents();
            Thread.Sleep(700);
        }
        private void Change(object sender, StateChangeEventArgs e)
        {
            this.statusStrip1.Items[1].Text = e.CurrentState.ToString();
            Application.DoEvents();
            Thread.Sleep(700);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {                
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.StateChange += Connection_StateChange;
                    conn.Disposed += Conn_Disposed;
                    conn.ConnectionString = connStr;
                    conn.Open();
                    SqlCommand comm = new SqlCommand("select * from Products", conn);
                    SqlDataReader da = comm.ExecuteReader();
                    this.listBox2.Items.Clear();
                    while (da.Read())
                    {
                        this.listBox2.Items.Add(da["ProductName"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Conn_Disposed(object sender, EventArgs e)
        {
            MessageBox.Show("Disposed!!");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SqlConnection[] conns = new SqlConnection[100];
            SqlDataReader[] readers = new SqlDataReader[100];
            for(int i =0; i <= conns.Length - 1; i++)
            {                
                conns[i] = new SqlConnection(Settings.Default.NorthwindConnectionString);
                conns[i].Open();
                this.label3.Text = $"{i + 1}";
                Application.DoEvents();
                SqlCommand comm = new SqlCommand("select * from products", conns[i]);
                readers[i] = comm.ExecuteReader();
                this.listBox3.Items.Clear();
                while (readers[i].Read())
                {
                    this.listBox3.Items.Add(readers[i]["ProductName"]);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            const int max =200;
            SqlConnection[] conns = new SqlConnection[max];
            SqlDataReader[] readers = new SqlDataReader[max];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                MaxPoolSize = max,
                ConnectTimeout = 1,
                InitialCatalog = "AdventureWorks",
                DataSource = ".",
                IntegratedSecurity = true,
            };

            for (int i = 0; i <= conns.Length - 1; i++)
            {
                conns[i] = new SqlConnection(builder.ConnectionString);
                conns[i].Open();
                this.label3.Text = $"{i + 1}";
                Application.DoEvents();
                SqlCommand comm = new SqlCommand("select * from Production.product", conns[i]);
                readers[i] = comm.ExecuteReader();
                this.listBox3.Items.Clear();
                while (readers[i].Read())
                {
                    this.listBox3.Items.Add(readers[i]["Name"]);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            const int max = 100;
            SqlConnection[] conns = new SqlConnection[max];
            SqlDataReader[] readers = new SqlDataReader[max];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                MaxPoolSize = max,
                ConnectTimeout = 1,
                Pooling = true,
                InitialCatalog = "AdventureWorks",
                DataSource = ".",
                IntegratedSecurity = true,
            };
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int i = 0; i <= conns.Length - 1; i++)
            {
                conns[i] = new SqlConnection(builder.ConnectionString);
                conns[i].Open();
                //this.label3.Text = $"{i + 1}";
                //Application.DoEvents();
                SqlCommand comm = new SqlCommand("select * from Production.product", conns[i]);
                readers[i] = comm.ExecuteReader();
                this.listBox3.Items.Clear();
                while (readers[i].Read())
                {
                    this.listBox3.Items.Add(readers[i]["Name"]);
                }
                conns[i].Dispose();
            }
            watch.Stop();
            this.label1.Text = watch.Elapsed.TotalSeconds.ToString();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            const int max = 100;
            SqlConnection[] conns = new SqlConnection[max];
            SqlDataReader[] readers = new SqlDataReader[max];
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                MaxPoolSize = max,
                ConnectTimeout = 1,
                Pooling = false,
                InitialCatalog = "AdventureWorks",
                DataSource = ".",
                IntegratedSecurity = true,
            };
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int i = 0; i <= conns.Length - 1; i++)
            {
                conns[i] = new SqlConnection(builder.ConnectionString);
                conns[i].Open();
                //this.label3.Text = $"{i + 1}";
                //Application.DoEvents();
                SqlCommand comm = new SqlCommand("select * from Production.product", conns[i]);
                readers[i] = comm.ExecuteReader();
                this.listBox3.Items.Clear();
                while (readers[i].Read())
                {
                    this.listBox3.Items.Add(readers[i]["Name"]);
                }
                conns[i].Dispose();
            }
            watch.Stop();
            this.label2.Text = watch.Elapsed.TotalSeconds.ToString();
        }
    }
}
