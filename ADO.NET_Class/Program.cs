using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo;
using Starter;

namespace ADO.NET_Class
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new lblIndex());
            //Application.Run(new FrmSqlConnection());
            //Application.Run(new Form2());
            //Application.Run(new FrmConnected());
            //Application.Run(new FrmTransactionIsolation());
            Application.Run(new FrmDisConnected_離線DataSet());
        }
    }
}
