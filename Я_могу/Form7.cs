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
    public partial class Form7 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form7()
        {
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"SELECT * FROM `Занятия`;";
            MySqlDataAdapter adapter = new MySqlDataAdapter(script, myConnection);
            SD.DataTable table = new SD.DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            myConnection.Close();
            comboBox1.Items.AddRange(new string[] { "Наличный расчет", "Безналичный расчет" });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //РАССЧИТАТЬ
            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string scr = $"SELECT `Стоимость_1_занятия` FROM `Занятия` WHERE `Код_занятия`={dataGridView1.CurrentRow.Cells[0].Value.ToString()}";
            MySqlCommand com = new MySqlCommand(scr,myConnection);
            int s1 = Convert.ToInt32(com.ExecuteScalar().ToString());
            string scr2 = $"SELECT `Стоимость_10_занятий` FROM `Занятия` WHERE `Код_занятия`={dataGridView1.CurrentRow.Cells[0].Value.ToString()}";
            MySqlCommand com2 = new MySqlCommand(scr2, myConnection);
            string s10 = com2.ExecuteScalar().ToString();
            myConnection.Close();
            if (Convert.ToInt32(textBox1.Text) == 10) label5.Text = s10;
            else label5.Text = (s1 * (Convert.ToInt32(textBox1.Text))).ToString();
            }
            else MessageBox.Show("Заполните все поля!", "Успешно");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ОПЛАТИТЬ
            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                button3_Click(sender, e);
                myConnection = new MySqlConnection(connectString);
                myConnection.Open();
                string script = $"INSERT INTO `Оплата` (`Код_оплаты`, `Код_обуч`, " +
                    $"`Код_занятия`, `Кол-во`, `Дата`, `Способ_оплаты`, `Сумма`) VALUES " +
                    $"(NULL, '{DataBank.id}', '{dataGridView1.CurrentRow.Cells[0].Value.ToString()}', '{textBox1.Text}'" +
                    $", '{DateTime.Now.ToString("yyyy-MM-dd")}', '{comboBox1.Text}', '{label5.Text}');";
                MySqlCommand com = new MySqlCommand(script, myConnection);
                com.ExecuteNonQuery();
                MessageBox.Show("Данные об оплате добавлены!", "Успешно");
                ActiveForm.Close();
            }
            else MessageBox.Show("Заполните все поля!", "Успешно"); 
            myConnection.Close();           
        }
        private void open(object obj)
        {
            Application.Run(new Form6());
        }
        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
