using MySql.Data.MySqlClient;

namespace login
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string user;
        private string password;
        private string port;
        private string connectionString;

        public Form1()
        {
            InitializeComponent();

            //資料庫基本設定
            server = "192.168.1.60";
            database = "login";
            user = "ChuanPien";
            password = "";
            port = "3306";

            connectionString = "server=" + server + ";port=" + port + ";user id=" + user + "; password=" + password + "; database=" + database + "";
            connection = new MySqlConnection(connectionString);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime myDate = DateTime.Now;
            string myDateString = myDate.ToString("yyyy/MM/dd HH:mm:ss");
            label1.Text = myDateString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime myDate = DateTime.Now;
            string myDateString = myDate.ToString("yyyy/MM/dd HH:mm:ss");
            string time = myDateString;
            string name = textBox1.Text;
            if (name == null)
                MessageBox.Show("請輸入姓名!!");
            else
                db(name, time);
        }

        private void db(string name, string time)
        {
            bool new_member = false;
            string duty, updata;
            try
            {
                connection.Open();      //建立資料庫連線
                //檢查員工狀態
                string check = "SELECT * FROM member WHERE name = '" + name + "'";
                MySqlCommand check_command = new MySqlCommand(check, connection);
                using (MySqlDataReader isduty = check_command.ExecuteReader())
                {
                    if (isduty.Read())
                        duty = isduty["duty"].ToString();
                    else
                    {
                        new_member = true;
                        duty = "下班";
                    }
                }

                if (duty == "下班")
                    duty = "上班";
                else
                    duty = "下班";

                //更新員工狀態
                if (new_member)
                    updata = "INSERT INTO member (name, duty) VALUES ('" + name + "', '" + duty + "')";
                else
                    updata = "UPDATE member SET duty = '" + duty + "' WHERE name = '" + name + "'";
                MySqlCommand updata_command = new MySqlCommand(updata, connection);
                updata_command.ExecuteNonQuery();

                //儲存打卡log
                string ins = "INSERT INTO login_data (name, time, duty) VALUES ('" + name + "', '" + time + "', '" + duty + "')";
                MySqlCommand ins_command = new MySqlCommand(ins, connection);
                ins_command.ExecuteNonQuery();

                connection.Close();     //關閉連線
                textBox2.Text = name + "打卡成功!\r\n狀態:" + duty + "\r\n時間:" + time;
            }
            catch (MySqlException)
            {
                textBox2.Text = "打卡失敗!";
            }
        }
    }
}