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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Я_могу
{
    public partial class Form10 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;

        public Form10()
        {
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();

            label1.Text = DataBank.date;
            label2.Text = DataBank.time;
            label3.Text = "Кабинет " + DataBank.kab;
            label4.Text = "Занятие:";

            string script2 = "SELECT Concat(`Код_занятия`,' ',`Название`,' - ',`ФИО_педагога`) FROM `Занятия`;";
            MySqlCommand com = new MySqlCommand(script2, myConnection);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetValue(0));
            }
            reader.Close();
            myConnection.Close();
        }
        private void open(object obj)
        {
            Application.Run(new Form1());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            if (comboBox2.Text != "")
            {
                string[] inf = comboBox2.Text.Split(' ');
                string kod = inf[0];
                string script = $"INSERT INTO `Расписание` (`Код_расписания`, `Время`, `Кабинет`, `Код_занятия`, `Дата`) " +
                    $"VALUES (NULL, '{DataBank.time}', '{DataBank.kab}', '{kod}', '{DataBank.date}');";
                MySqlCommand command = new MySqlCommand(script, myConnection);
                command.ExecuteNonQuery();
                MessageBox.Show("Данные добавлены!", "Успешно");
                ActiveForm.Close();
            }
            else MessageBox.Show("Укажите занятие!", "Внимание");
            myConnection.Close();
        }

        private void Form10_FormClosing(object sender, FormClosingEventArgs e)
        {
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
