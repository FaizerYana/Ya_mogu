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
    public partial class Form9 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form9()
        {
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open(); 
            string scr = $"SELECT `Название` FROM `Занятия` WHERE `Код_занятия`='{DataBank.id}';";
            MySqlCommand com = new MySqlCommand(scr, myConnection);
            textBox1.Text = com.ExecuteScalar().ToString();
            string scr1 = $"SELECT `ФИО_педагога` FROM `Занятия` WHERE `Код_занятия`='{DataBank.id}';";
            MySqlCommand com1 = new MySqlCommand(scr1, myConnection);
            textBox2.Text = com1.ExecuteScalar().ToString();
            string scr3 = $"SELECT `Стоимость_1_занятия` FROM `Занятия` WHERE `Код_занятия`='{DataBank.id}';";
            MySqlCommand com3 = new MySqlCommand(scr3, myConnection);
            textBox3.Text = com3.ExecuteScalar().ToString();
            string scr4 = $"SELECT `Стоимость_10_занятий` FROM `Занятия` WHERE `Код_занятия`='{DataBank.id}';";
            MySqlCommand com4 = new MySqlCommand(scr4, myConnection);
            textBox4.Text = com4.ExecuteScalar().ToString();
            myConnection.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"UPDATE `Занятия` SET `Название` = '{textBox1.Text}', " +
                $"`ФИО_педагога` = '{textBox2.Text}', " +
                $"`Стоимость_1_занятия` = '{textBox3.Text}', `Стоимость_10_занятий` = " +
                $"'{textBox4.Text}' WHERE `Занятия`.`Код_занятия` = '{DataBank.id}';";
            MySqlCommand com = new MySqlCommand(script, myConnection);
            com.ExecuteNonQuery();
            myConnection.Close();
            MessageBox.Show("Данные изменены!", "Успешно");
            ActiveForm.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }

        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
