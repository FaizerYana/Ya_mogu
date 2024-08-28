using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD=System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Я_могу
{
    public partial class Form3 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form3()
        {
            InitializeComponent();
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                myConnection = new MySqlConnection(connectString);
                myConnection.Open();
                string script = $"INSERT INTO `Занятия` (`Код_занятия`, `Название`, " +
                    $"`ФИО_педагога`, `Стоимость_1_занятия`, `Стоимость_10_занятий`) VALUES " +
                    $"(NULL, '{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}');";
                MySqlCommand com = new MySqlCommand(script, myConnection);
                com.ExecuteNonQuery();
                myConnection.Close();
                MessageBox.Show("Данные добавлены!", "Успешно");
                ActiveForm.Close();
            }
            else MessageBox.Show("Заполините все поля", "Внимание!");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
