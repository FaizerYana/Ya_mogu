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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Я_могу
{
    public partial class Form8 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form8()
        {
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open(); 
            string scr = $"SELECT `Фамилия` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
            MySqlCommand com = new MySqlCommand(scr, myConnection);
            textBox1.Text = com.ExecuteScalar().ToString();
            string scr2 = $"SELECT `Имя` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
            MySqlCommand com2 = new MySqlCommand(scr2, myConnection);
            textBox2.Text = com2.ExecuteScalar().ToString();
            string scr3 = $"SELECT `Основной_контакт` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
            MySqlCommand com3 = new MySqlCommand(scr3, myConnection);
            textBox3.Text = com3.ExecuteScalar().ToString();
            string scr4 = $"SELECT `Основной_номер_тел` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
            MySqlCommand com4 = new MySqlCommand(scr4, myConnection);
            maskedTextBox1.Text = com4.ExecuteScalar().ToString();
            
            string scr5 = $"SELECT `Доп_контакт` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr5, myConnection);
            DataTable tb = new DataTable();
            adapter.Fill(tb);
            if (tb.Rows.Count != 0)
            {
                MySqlCommand com5 = new MySqlCommand(scr5, myConnection);
                textBox5.Text = com5.ExecuteScalar().ToString();
                string scr6 = $"SELECT `Доп_номер_тел` FROM `Обучающиеся` WHERE `Код_обуч`='{DataBank.id}';";
                MySqlCommand com6 = new MySqlCommand(scr6, myConnection);
                maskedTextBox2.Text = com6.ExecuteScalar().ToString();
            }            
            myConnection.Close();
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }
        private void button8_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"UPDATE `Обучающиеся` SET `Фамилия` = '{textBox1.Text}', `Имя` = '{textBox2.Text}', " +
                $"`Основной_контакт` = '{textBox3.Text}', `Основной_номер_тел` = '{maskedTextBox1.Text}', " +
                $"`Доп_контакт` = '{textBox5.Text}', `Доп_номер_тел` = '{maskedTextBox2.Text}' WHERE `Обучающиеся`.`Код_обуч` = '{DataBank.id}';";
            MySqlCommand com = new MySqlCommand(script, myConnection);
            com.ExecuteNonQuery();
            myConnection.Close();
            MessageBox.Show("Данные изменены!", "Успешно");
            ActiveForm.Close();
        }
        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
