using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Я_могу
{
    public partial class Form4 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form4()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ДА
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"DELETE FROM `Оплата` WHERE `Оплата`.`Код_обуч` = '{DataBank.id}'";
            MySqlCommand com = new MySqlCommand(script,myConnection);
            com.ExecuteNonQuery();
            script = $"DELETE FROM `Посещаемость` WHERE `Посещаемость`.`Код_обуч` = '{DataBank.id}'";
            com = new MySqlCommand(script, myConnection);
            com.ExecuteNonQuery();
            script = $"DELETE FROM `Обучающиеся` WHERE `Обучающиеся`.`Код_обуч` = '{DataBank.id}'";
            com = new MySqlCommand(script, myConnection);
            com.ExecuteNonQuery();
            myConnection.Close();
            MessageBox.Show("Данные удалены!", "Успешно");
            ActiveForm.Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();            
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //НЕТ
            ActiveForm.Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
