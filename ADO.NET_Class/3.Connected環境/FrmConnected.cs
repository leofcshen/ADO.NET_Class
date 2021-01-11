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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();
                    //comm.CommandText = "Insert into Member (UserName, Password) values ('ccc', 'ccc')";
                    comm.CommandText = $"Insert into Member (UserName, Password) values ('{UserName}', '{Password}')";
                    comm.Connection = conn;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Insert Member Successfully!!!");
                }
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
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();                    
                    comm.CommandText = $"Select * From Member Where UserName ='{UserName}' and password ='{Password}'";
                    comm.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = comm.ExecuteReader();
                    MessageBox.Show((dr.HasRows)? "Login Successfully!!!" : "Login Failed!!!");
                }
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
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();
                    //comm.CommandText = "Insert into Member (UserName, Password) values ('ccc', 'ccc')";
                    comm.CommandText = $"Insert into Member (UserName, Password) values (@UserName, @Password)";
                    comm.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = UserName;
                    //comm.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value = Password;
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@Password";
                    p1.SqlDbType = SqlDbType.NVarChar;
                    p1.Size = 40;
                    p1.Value = Password;
                    comm.Parameters.Add(p1);

                    comm.Connection = conn;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Insert Member Successfully!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = $"Select * From Member Where UserName = @UserName and password = @Password";
                    comm.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = UserName;
                    //way1
                    //comm.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value = Password; ;
                    //way2
                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "Password";
                    p1.SqlDbType = SqlDbType.NVarChar;
                    p1.Size = 40;
                    comm.Parameters.Add(p1);

                    comm.Connection = conn;
                    conn.Open();
                    SqlDataReader dr = comm.ExecuteReader();
                    MessageBox.Show((dr.HasRows) ? "Login Successfully!!!" : "Login Failed!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "InsertMember";
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = UserName;
                    comm.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value = Password;

                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@return_value";
                    p1.Direction = ParameterDirection.ReturnValue;
                    comm.Parameters.Add(p1);

                    comm.Connection = conn;
                    conn.Open();
                    //string s = comm.ExecuteScalar().ToString();
                    int MemberID = (int)p1.Value;
                    MessageBox.Show("Insert Member Successfully!!!" + MemberID);
                    //MessageBox.Show(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;
                    string UserName = this.textBox1.Text;
                    string Password = this.textBox2.Text;

                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "InsertMember";
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = UserName;
                    comm.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value =
                        System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.textBox2.Text, "sha1");


                    SqlParameter p1 = new SqlParameter();
                    p1.ParameterName = "@return_value";
                    p1.Direction = ParameterDirection.ReturnValue;
                    comm.Parameters.Add(p1);

                    comm.Connection = conn;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    int MemberID = (int)p1.Value;
                    MessageBox.Show("Insert Member Successfully!!!" + MemberID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.memberTableAdapter1.Insert(this.textBox1.Text, this.textBox2.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                string connStr = Settings.Default.NorthwindConnectionString;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connStr;                    
                    
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    comm.CommandText = "select Max(UnitPrice) From Products";                    
                    this.listBox1.Items.Add("Max =" + comm.ExecuteScalar());
                    comm.CommandText = "select Min(UnitPrice) From Products";
                    this.listBox1.Items.Add("Min =" + comm.ExecuteScalar());
                    comm.CommandText = "select Avg(UnitPrice) From Products";
                    this.listBox1.Items.Add("Avg =" + comm.ExecuteScalar());
                    comm.CommandText = "select Count(*) From Products";
                    this.listBox1.Items.Add("count =" + comm.ExecuteScalar());
                    comm.CommandText = "select Sum(UnitsInStock) From Products";
                    this.listBox1.Items.Add("Sum UnitsInStock =" + comm.ExecuteScalar());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                conn.Open();

                comm.CommandText = "select * from categories; select * from products";
                SqlDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    this.listBox1.Items.Add(dr["CategoryName"]);
                }
                dr.NextResult();
                while (dr.Read())
                {
                    this.listBox2.Items.Add(dr["ProductName"]);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    conn.Open();

                    comm.CommandText = "select * from categories";
                    SqlDataReader dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        this.listBox1.Items.Add(dr["CategoryName"]);
                    }                    
                    comm.CommandText = "select * from products";
                    dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        this.listBox2.Items.Add(dr["ProductName"]);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string DDLCommandText =
                "CREATE TABLE[dbo].[MyImageTable](" +
                "[ImageID][int] IDENTITY(1, 1) NOT NULL," +
                "[Description] [text] NULL," +
                "[Image] [image] NULL," +
                "CONSTRAINT[PK_MyImageTable] PRIMARY KEY CLUSTERED" +
                "(" +
                "[ImageID] ASC" +
                ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
                ")ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]";
            try
            {
                using(SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = DDLCommandText;
                    comm.Connection = conn;

                    conn.Open();
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Create ImageTable successfully");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {

                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;

                    conn.Open();

                    command.CommandText = "select * from categories";

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            this.listBox1.Items.Add(dataReader["CategoryName"]);
                        }
                    }//Auto dataReader.Close();

                    //===================================
                    //"已經開啟一個與這個 Command 相關的 DataReader，必須先將它關閉。

                    command.CommandText = "select * from Products";

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {
                            this.listBox2.Items.Add(dataReader["ProductName"]);
                        }
                    }


                } //Auto conn.close();conn.dispose(0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {

                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;

                    conn.Open();

                    command.CommandText = "select * from categories";

                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        this.listBox1.Items.Add(dataReader["CategoryName"]);
                    }

                    //===================================
                    //"已經開啟一個與這個 Command 相關的 DataReader，必須先將它關閉。
                    dataReader.Close();

                    command.CommandText = "select * from Products";

                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        this.listBox2.Items.Add(dataReader["ProductName"]);
                    }


                } //Auto conn.close();conn.dispose(0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }
    }
}
