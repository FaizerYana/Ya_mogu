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

namespace Я_могу
{
    public partial class Form6 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form6()
        {
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string scr1 = $"SELECT `Фамилия` FROM `Обучающиеся` WHERE `Код_обуч`= {DataBank.id};";
            MySqlCommand com1 = new MySqlCommand(scr1,myConnection);
            label1.Text = com1.ExecuteScalar().ToString();
            string scr2 = $"SELECT `Имя` FROM `Обучающиеся` WHERE `Код_обуч`= {DataBank.id};";
            MySqlCommand com2 = new MySqlCommand(scr2, myConnection);
            label2.Text = com2.ExecuteScalar().ToString();
            string script = $"SELECT `Занятия`.`Название`,`Оплата`.`Кол-во` as 'Кол-во оплаченных занятий',`Оплата`.`Дата` FROM `Занятия`,`Оплата` WHERE " +
                $"`Оплата`.`Код_занятия` = `Занятия`.`Код_занятия` AND `Оплата`.`Код_обуч` = {DataBank.id} ORDER BY `Оплата`.`Дата` DESC;";
            MySqlDataAdapter adapter = new MySqlDataAdapter(script, myConnection);
            SD.DataTable table = new SD.DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;     
            myConnection.Close();
        }
        private void open(object obj)
        {
            Application.Run(new Form7());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
