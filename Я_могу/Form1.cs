using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD=System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data.Common;
using System.Data;
using Microsoft.Office.Interop.Excel;

namespace Я_могу
{
    public partial class Form1 : Form
    {
        public MySqlConnection myConnection;
        public string connectString = "datasource=127.0.0.1;port=3306;user=root;password=;database=я_могу;";
        Thread th;
        public Form1()
        {
            excelExporter = new ExcelExporter();
            InitializeComponent();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();

            string script = "SELECT * FROM `Обучающиеся`";
            MySqlDataAdapter adapter = new MySqlDataAdapter(script, myConnection);
            SD.DataTable table = new SD.DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            string script1 = "SELECT * FROM `Занятия`";
            MySqlDataAdapter adapter1 = new MySqlDataAdapter(script1, myConnection);
            SD.DataTable table1 = new SD.DataTable();
            adapter1.Fill(table1);
            dataGridView2.DataSource = table1;

            comboBox1.Items.AddRange(new string[] { "1", "2", "3" });
            comboBox1.Text = DataBank.kab;

            dateTimePicker1.Text = DataBank.date;

            string script2 = "SELECT Concat(`Код_занятия`,' ',`Название`,' - ',`ФИО_педагога`) FROM `Занятия`;";
            MySqlCommand com = new MySqlCommand (script2, myConnection);
            MySqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetValue(0));
            }
            reader.Close();
            label_8.Text = func("8:00-8:45", myConnection,comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_9.Text = func("9:00-9:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_10.Text = func("10:00-10:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_11.Text = func("11:00-11:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_12.Text = func("12:00-12:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_13.Text = func("13:00-13:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_14.Text = func("14:00-14:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_15.Text = func("15:00-15:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_16.Text = func("16:00-16:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_17.Text = func("17:00-17:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_18.Text = func("18:00-18:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_19.Text = func("19:00-19:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));

            myConnection.Close();
        }

        static string func(string time, MySqlConnection myConnection, string kab, string date)
        {
            string inf = time;
            string scr = $"SELECT `Код_занятия` FROM `Расписание` WHERE `Время`='{time}' AND `Кабинет`='{kab}' AND `Дата`='{date}';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            if (tb.Rows.Count != 0)
            { 
                MySqlCommand com = new MySqlCommand(scr,myConnection);
                string kod = com.ExecuteScalar().ToString();
                scr = $"SELECT `ФИО_педагога` FROM `Занятия` WHERE `Код_занятия`='{kod}';";
                com = new MySqlCommand(scr, myConnection);
                inf += "\r\n\r\n" + com.ExecuteScalar().ToString();
                scr = $"SELECT `Название` FROM `Занятия` WHERE `Код_занятия`='{kod}';";
                com = new MySqlCommand(scr, myConnection);
                inf += "\r\n" + com.ExecuteScalar().ToString();
            }
            return inf;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"SELECT * FROM `Обучающиеся` WHERE `Фамилия` LIKE '%{textBox1.Text}%' OR `Имя` LIKE '%{textBox1.Text}%';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(script, myConnection);
            SD.DataTable table = new SD.DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            myConnection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //ПОИСК ЗАН
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string script = $"SELECT * FROM `Занятия` WHERE `Название` LIKE '%{textBox2.Text}%' OR `ФИО_педагога` LIKE '%{textBox2.Text}%';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(script, myConnection);
            SD.DataTable table = new SD.DataTable();
            adapter.Fill(table);
            dataGridView2.DataSource = table;
            myConnection.Close();
        }
        private void open2(object obj)
        {
            System.Windows.Forms.Application.Run(new Form2());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //ДОБ ОБУЧ
            this.Close();
            th = new Thread(open2);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        private void open3(object obj)
        {
            System.Windows.Forms.Application.Run(new Form3());
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //ДОБ ЗАН
            this.Close();
            th = new Thread(open3);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        private void open4(object obj)
        {
            System.Windows.Forms.Application.Run(new Form4());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //УД ОБУЧ
            DataBank.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string scr1 = $"SELECT `Фамилия` FROM `Обучающиеся` WHERE `Код_обуч`= {DataBank.id};";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr1, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            this.Close();
            th = new Thread(open4);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            myConnection.Close();
        }
        private void open5(object obj)
        {
            System.Windows.Forms.Application.Run(new Form5());
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //УД ЗАН
            DataBank.id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            string scr1 = $"SELECT `Название` FROM `Занятия` WHERE `Код_занятия`= {DataBank.id};";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr1, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            this.Close();
            th = new Thread(open5);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            myConnection.Close();
        }
        private void open6(object obj)
        {
            System.Windows.Forms.Application.Run(new Form6());
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DataBank.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string scr1 = $"SELECT `Фамилия` FROM `Обучающиеся` WHERE `Код_обуч`= {DataBank.id};";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr1, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            th = new Thread(open6);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            myConnection.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Поиск в расписании
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            label_8.Text = func("8:00-8:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_9.Text = func("9:00-9:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_10.Text = func("10:00-10:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_11.Text = func("11:00-11:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_12.Text = func("12:00-12:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_13.Text = func("13:00-13:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_14.Text = func("14:00-14:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_15.Text = func("15:00-15:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_16.Text = func("16:00-16:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_17.Text = func("17:00-17:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_18.Text = func("18:00-18:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            label_19.Text = func("19:00-19:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            myConnection.Close();
        }
        private void open8(object obj)
        {
            System.Windows.Forms.Application.Run(new Form8());
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //РЕД ОБУЧ
            DataBank.id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string scr1 = $"SELECT `Фамилия` FROM `Обучающиеся` WHERE `Код_обуч`= {DataBank.id};";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr1, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            this.Close();
            th = new Thread(open8);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            myConnection.Close();
        }
        private void open9(object obj)
        {
            System.Windows.Forms.Application.Run(new Form9());
        }
        private void button11_Click(object sender, EventArgs e)
        {
            //РЕД ЗАНЯТИЯ
            DataBank.id = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            string scr1 = $"SELECT `Название` FROM `Занятия` WHERE `Код_занятия`= {DataBank.id};";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr1, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            this.Close();
            th = new Thread(open9);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            myConnection.Close();
        }        

        private void button8_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string scr = $"SELECT SUM(`Сумма`) FROM `Оплата` WHERE `Способ_оплаты`='Наличный расчет' AND `Дата` BETWEEN " +
                $"'{dateTimePicker2.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePicker3.Value.ToString("yyyy-MM-dd")}';";
            MySqlCommand com = new MySqlCommand(scr, myConnection);
            N.Text=com.ExecuteScalar().ToString();
            string scr1 = $"SELECT SUM(`Сумма`) FROM `Оплата` WHERE `Способ_оплаты`='Безналичный расчет' AND `Дата` BETWEEN " +
                $"'{dateTimePicker2.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePicker3.Value.ToString("yyyy-MM-dd")}';";
            MySqlCommand com1 = new MySqlCommand(scr1, myConnection);
            BN.Text = com1.ExecuteScalar().ToString();
            myConnection.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Лист посещаемости
            dataGridView3.Rows.Clear(); 
            dataGridView3.Columns.Clear();
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string[] inf = comboBox2.Text.Split(' ');
            string kod = inf[0];

            string scr = $"SELECT COUNT(DISTINCT `Обучающиеся`.`Код_обуч`) FROM `Обучающиеся`, `Оплата` WHERE " +
                $"`Оплата`.`Код_занятия`='{kod}' AND `Оплата`.`Код_обуч`=`Обучающиеся`.`Код_обуч`;";
            MySqlCommand com2 = new MySqlCommand(scr, myConnection);
            dataGridView3.RowCount = Convert.ToInt32(com2.ExecuteScalar().ToString()) + 1;

            string script2 = $"SELECT DISTINCT Concat(`Обучающиеся`.`Код_обуч`,' ',`Обучающиеся`.`Фамилия`,' ',`Обучающиеся`.`Имя`) " +
                $"as ' ' FROM `Обучающиеся`, `Оплата` WHERE `Оплата`.`Код_занятия`='{kod}' AND `Оплата`.`Код_обуч`=`Обучающиеся`.`Код_обуч`;";
            MySqlCommand com1 = new MySqlCommand(script2, myConnection);
            MySqlDataReader reader = com1.ExecuteReader();

            int i = 1;
            while (reader.Read())
            {
            dataGridView3.Rows[i].Cells[0].Value = reader.GetValue(0);
            i++;
            }
            reader.Close();

            string scr3 = $"SELECT COUNT(DISTINCT `Дата`) FROM `Расписание` WHERE `Код_занятия`='{kod}' AND `Дата` BETWEEN " +
                $"'{dateTimePicker5.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePicker4.Value.ToString("yyyy-MM-dd")}';";
            MySqlCommand com3 = new MySqlCommand(scr3, myConnection);
            dataGridView3.ColumnCount = Convert.ToInt32(com3.ExecuteScalar().ToString()) + 1;

            string script4 = $"SELECT DISTINCT `Дата` FROM `Расписание` WHERE `Код_занятия`='{kod}' AND `Дата` BETWEEN " +
                $"'{dateTimePicker5.Value.ToString("yyyy-MM-dd")}' AND '{dateTimePicker4.Value.ToString("yyyy-MM-dd")}' ORDER BY `Дата` ASC;";
            MySqlCommand com4 = new MySqlCommand(script4, myConnection);
            MySqlDataReader reader4 = com4.ExecuteReader();

            int j = 1;
            while (reader4.Read())
            {
                dataGridView3.Rows[0].Cells[j].Value = reader4.GetValue(0);
                j++;
            }
            reader4.Close();
            int a = 0;
            int b = 0;
            for (i = 1; i < dataGridView3.Rows.Count; i++)
            {
                for (j = 1; j < dataGridView3.Columns.Count; j++)
                {
                    string scrinf = $"SELECT COUNT(*) FROM `Посещаемость` WHERE `Код_обуч`='{dataGridView3.Rows[i].Cells[0].Value.ToString().Split(' ')[0]}'" +
                        $" AND `Код_занятия`='{kod}' AND `Дата`='{dataGridView3.Rows[0].Cells[j].Value.ToString()}';";
                    MySqlCommand cominf = new MySqlCommand(scrinf, myConnection);
                    a = Convert.ToInt32(cominf.ExecuteScalar().ToString());
                    if (a != 0) dataGridView3.Rows[i].Cells[j].Value = "+";
                    else dataGridView3.Rows[i].Cells[j].Value = "";
                    dataGridView3.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            dataGridView3.ColumnCount = dataGridView3.Columns.Count + 1;
            dataGridView3.Rows[0].Cells[dataGridView3.ColumnCount-1].Value = "Посещено занятий";
            
            for (i = 1; i < dataGridView3.Rows.Count; i++)
            {    a = 0;            
                for (j = 1; j < dataGridView3.Columns.Count; j++)
                {
                    if (dataGridView3.Rows[i].Cells[j].Value == "+") a++;
                    dataGridView3.Rows[i].Cells[dataGridView3.ColumnCount-1].Value = a.ToString();
                }
            }
            dataGridView3.ColumnCount = dataGridView3.Columns.Count + 1;
            dataGridView3.Rows[0].Cells[dataGridView3.ColumnCount - 1].Value = "Остаток занятий";
            a = 0;
            for (i = 1; i < dataGridView3.Rows.Count; i++)
            {
                scr = $"SELECT SUM(`Кол-во`) FROM `Оплата` WHERE `Код_обуч`='{dataGridView3.Rows[i].Cells[0].Value.ToString().Split(' ')[0]}' AND `Код_занятия`='{kod}';";
                MySqlCommand com = new MySqlCommand(scr, myConnection);
                a = Convert.ToInt32(com.ExecuteScalar());
                scr = $"SELECT COUNT(*) FROM `Посещаемость` WHERE `Код_обуч`='{dataGridView3.Rows[i].Cells[0].Value.ToString().Split(' ')[0]}' AND `Код_занятия` = '{kod}';";
                com = new MySqlCommand(scr, myConnection);
                b = Convert.ToInt32(com.ExecuteScalar());
                dataGridView3.Rows[i].Cells[dataGridView3.ColumnCount - 1].Value = (a-b).ToString();
                if (Convert.ToInt32(dataGridView3.Rows[i].Cells[dataGridView3.ColumnCount - 1].Value)<1) dataGridView3.Rows[i].Cells[0].Style.BackColor = Color.Pink;
            }
            myConnection.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Посещаемость
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            string[] inf = comboBox2.Text.Split(' ');
            string kod = inf[0];
            for (int i = 1; i < dataGridView3.Rows.Count; i++)
            {
                for (int j = 1; j < dataGridView3.Columns.Count; j++)
                {
                    if (dataGridView3.Rows[i].Cells[j].Value.ToString() == "+")
                    {   
                         string scrinf = $"SELECT COUNT(*) FROM `Посещаемость` WHERE `Код_обуч`=" +
                            $"'{dataGridView3.Rows[i].Cells[0].Value.ToString().Split(' ')[0]}' AND " +
                            $"`Код_занятия`='{kod}' AND `Дата`='{dataGridView3.Rows[0].Cells[j].Value.ToString()}';";
                         MySqlCommand cominf = new MySqlCommand(scrinf, myConnection);
                         int a = Convert.ToInt32(cominf.ExecuteScalar().ToString());
                         if (a == 0)
                         {
                             string scr = $"INSERT INTO `Посещаемость` (`Код_посещаемости`, `Код_обуч`, `Код_занятия`, " +
                                $"`Дата`, `Присутствие`) VALUES (NULL, '{dataGridView3.Rows[i].Cells[0].Value.ToString().Split(' ')[0]}', " +
                                $"'{kod}', '{dataGridView3.Rows[0].Cells[j].Value.ToString()}', '1');";
                             MySqlCommand com = new MySqlCommand(scr, myConnection);
                             com.ExecuteNonQuery();
                         }
                    }
                }
            }
            myConnection.Close();
            button12_Click(sender, e);
            MessageBox.Show("Отметки посещаемости сохранены", "Успешно!");
        }
        static int func2(string time, MySqlConnection myConnection, string kab, string date)
        {
            int kod=0;
            string scr = $"SELECT `Код_занятия` FROM `Расписание` WHERE `Время`='{time}' AND `Кабинет`='{kab}' AND `Дата`='{date}';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(scr, myConnection);
            SD.DataTable tb = new SD.DataTable();
            adapter.Fill(tb);
            if (tb.Rows.Count != 0)
            {
                MySqlCommand com = new MySqlCommand(scr, myConnection);
                kod = Convert.ToInt32(com.ExecuteScalar());
                scr = $"SELECT `Код_расписания` FROM `Расписание` WHERE `Время`='{time}' AND `Кабинет`='{kab}' AND `Дата`='{date}' AND `Код_занятия`='{kod}';";
                com = new MySqlCommand(scr, myConnection);
                kod = Convert.ToInt32(com.ExecuteScalar());
            }
            return kod;
        }
        private void open10(object obj)
        {
            System.Windows.Forms.Application.Run(new Form10());
        }
        private void open11(object obj)
        {
            System.Windows.Forms.Application.Run(new Form11());
        }

        private void label_8_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod =func2("8:00-8:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_8.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "8:00-8:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_9_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("9:00-9:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_9.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "9:00-9:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_10_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("10:00-10:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_10.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "10:00-10:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_11_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("11:00-11:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_11.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "11:00-11:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_12_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("12:00-12:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_12.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "12:00-12:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_13_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("13:00-13:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_13.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "13:00-13:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_14_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("14:00-14:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_14.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "14:00-14:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_15_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("15:00-15:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_15.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "15:00-15:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_16_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("16:00-16:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_16.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "16:00-16:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_17_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("17:00-17:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_17.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "17:00-17:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_18_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("18:00-18:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (kod != 0)
            {
                DataBank.time = label_18.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "18:00-18:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }

        private void label_19_Click(object sender, EventArgs e)
        {
            myConnection = new MySqlConnection(connectString);
            myConnection.Open();
            int kod = func2("19:00-19:45", myConnection, comboBox1.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            DataBank.id = kod.ToString();
            DataBank.kab = comboBox1.Text;
            DataBank.date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            DataBank.time = "19:00-19:45";
            if (kod != 0)
            {
                DataBank.time = label_19.Text;
                this.Close();
                th = new Thread(open11);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                DataBank.time = "19:00-19:45";
                this.Close();
                th = new Thread(open10);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            myConnection.Close();
        }
        private ExcelExporter excelExporter;
        private void button13_Click(object sender, EventArgs e)
        {
            excelExporter.ExportExcel(dataGridView3);
        }
    }
    static class DataBank
    {
        public static string id = "";
        public static string D = "";
        public static string K = "";
        public static string T = "";
        public static string time = "";
        public static string kab = "1";
        public static string date = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
