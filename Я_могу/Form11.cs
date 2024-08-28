using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Я_могу
{
    public partial class Form11 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form11()
        {
            InitializeComponent();
            label1.Text = DataBank.date;
            label2.Text = DataBank.time;
            label3.Text = "Кабинет "+DataBank.kab;
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }
        private void button9_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"DELETE FROM `Расписание` WHERE `Расписание`.`Код_расписания` = '{DataBank.id}';";
            MySqlCommand command = new MySqlCommand(script, myConnection);
            command.ExecuteNonQuery();
            MessageBox.Show("Запись отменена!", "Успешно");
            ActiveForm.Close();
            myConnection.Close();
        }

        private void Form11_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
