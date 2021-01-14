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
            this.tabControl1.SelectedIndex = 3;
            this.tabControl2.SelectedIndex = 1;

            this.pictureBox1.AllowDrop = true;
            this.pictureBox1.DragEnter += PictureBox1_DragEnter;
            this.pictureBox1.DragDrop += PictureBox1_DragDrop;

            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.DragEnter += FlowLayoutPanel1_DragEnter;
            this.flowLayoutPanel1.DragDrop += FlowLayoutPanel1_DragDrop;
        }

        private void FlowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            for (int i = 0; i <= filenames.Length - 1; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Image = Image.FromFile(filenames[i]);
                pic.SizeMode = PictureBoxSizeMode.StretchImage;

                this.flowLayoutPanel1.Controls.Add(pic);
            }
        }

        private void FlowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void PictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show("DragDrop");
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);

            this.pictureBox1.Image = Image.FromFile(filenames[0]);
        }

        private void PictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
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

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
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

                        for (int i = 1; i <= dataReader.FieldCount - 1; i++)
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
            catch (Exception ex)
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
                    MessageBox.Show((dr.HasRows) ? "Login Successfully!!!" : "Login Failed!!!");
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
            catch (Exception ex)
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
            catch (Exception ex)
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
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = DDLCommandText;
                    comm.Connection = conn;

                    conn.Open();
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Create ImageTable successfully");
                }
            }
            catch (Exception ex)
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

        private void button25_Click(object sender, EventArgs e)
        {
            //insert
            try
            {

                string connString = Settings.Default.NorthwindConnectionString;

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;


                    SqlCommand command = new SqlCommand();

                    command.CommandText = $"Insert into MyImageTable (Description, Image) values (@Desc, @Image)";
                    command.Connection = conn;

                    byte[] bytes = { 1, 3 };//this.Picturebox1.Image=>bytes[]

                    //=============================
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bytes = ms.GetBuffer();

                    //=============================
                    command.Parameters.Add("@Desc", SqlDbType.Text).Value = this.textBox4.Text;
                    command.Parameters.Add("@Image", SqlDbType.Image).Value = bytes;

                    conn.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Insert image successfully");

                } //Auton conn.close(); conn.Dispose()

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //select
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    conn.Open();
                    command.CommandText = "select *  from MyImageTable";
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.listBox3.Items.Clear();
                    this.listBox4.Items.Clear();
                    while (dataReader.Read())
                    {
                        this.listBox3.Items.Add(dataReader["Description"]);
                        this.listBox4.Items.Add(dataReader["ImageID"]);
                    }
                } //Auto conn.close();conn.dispose(0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ImageID = (int)this.listBox4.Items[this.listBox3.SelectedIndex];
            ShowImage(ImageID);
        }

        private void ShowImage(int imageID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    conn.Open();
                    command.CommandText = "select * from MyImageTable where ImageID=" + imageID;
                    SqlDataReader dr = command.ExecuteReader();
                    dr.Read();
                    byte[] bytes = (byte[])dr["Image"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    this.pictureBox2.Image = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    conn.Open();
                    command.CommandText = "select *  from MyImageTable";
                    SqlDataReader dataReader = command.ExecuteReader();
                    this.listBox5.Items.Clear();
                    while (dataReader.Read())
                    {
                        MyImage myImage = new MyImage();
                        myImage.ImageID = (int)dataReader["ImageID"];
                        myImage.Description = dataReader["Description"].ToString();
                        this.listBox5.Items.Add(myImage);
                    }
                } //Auto conn.close();conn.dispose(0;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                this.pictureBox2.Image = this.pictureBox2.ErrorImage;
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            MyImage myImage = (MyImage)this.listBox5.SelectedItem;
            //MyImage myImage = (MyImage)this.listBox5.Items[this.listBox5.SelectedIndex];
            ShowImage(myImage.ImageID);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {                    
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;
                    
                    conn.Open();
                    comm.CommandText = "Insert into region (RegionID, RegionDescription) values (100, 'xxx')";
                    comm.ExecuteNonQuery();
                    comm.CommandText = "Insert into region (RegionID, RegionDescription) values (100, 'xxxxxx')";
                    comm.ExecuteNonQuery();
                    comm.CommandText = "Insert into region (RegionID, RegionDescription) values (101, 'xxxx')";
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Insert Region Successfully!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            SqlTransaction txn = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(Settings.Default.NorthwindConnectionString);
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                conn.Open();
                txn = conn.BeginTransaction();

                comm.Transaction = txn;
                comm.CommandText = "Insert into region (RegionID, RegionDescription) values (100, 'xxx')";
                comm.ExecuteNonQuery();
                comm.CommandText = "Insert into region (RegionID, RegionDescription) values (100, 'xxxxxx')";
                comm.ExecuteNonQuery();
                comm.CommandText = "Insert into region (RegionID, RegionDescription) values (101, 'xxxx')";
                comm.ExecuteNonQuery();
                txn.Commit();

                MessageBox.Show("Insert Region Successfully!!");
            }
            catch (Exception ex)
            {
                txn.Rollback();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
    class MyImage
    {
        public int ImageID { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return $"{this.ImageID} - {this.Description}";
        }
    }
}
