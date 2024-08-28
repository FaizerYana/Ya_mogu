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
    public partial class Form2 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form2()
        {
            InitializeComponent();
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && maskedTextBox1.Text != "+7(   )   -  -  ")
            {
                myConnection = new MySqlConnection(connectString);
                myConnection.Open();
                string script = $"INSERT INTO `Обучающиеся` (`Код_обуч`, `Фамилия`, `Имя`, `Основной_контакт`, `Основной_номер_тел`, " +
                    $"`Доп_контакт`, `Доп_номер_тел`) VALUES (NULL, '{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', " +
                    $"'{maskedTextBox1.Text}', '{textBox5.Text}', '{maskedTextBox2.Text}');";
                MySqlCommand com = new MySqlCommand(script, myConnection);
                com.ExecuteNonQuery();
                myConnection.Close();
                MessageBox.Show("Данные добавлены!", "Успешно");
                ActiveForm.Close();
            }
            else MessageBox.Show("Заполните все поля", "Внимание!");
        }
    }
}
