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
            server = "192.168.1.60";
            database = "login";
            user = "ChuanPien";
            password = "";
            port = "3306";

            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", server, port, user, password, database);
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
            {
                MessageBox.Show("請輸入姓名!!");
            }
            else
            {
                db(name, time);
            }
        }

        private void db(string name, string time)
        {
            string duty = "";
            try
            {
                connection.Open();
                string check = "SELECT * FROM member WHERE name = '" + name + "'";
                MySqlCommand check_command = new MySqlCommand(check, connection);
                using (MySqlDataReader isduty = check_command.ExecuteReader())
                {
                    while (isduty.Read())
                    {
                        duty = isduty["duty"].ToString();
                    }
                }
                if (duty == "下班")
                {
                    duty = "上班";
                }
                else
                {
                    duty = "下班";
                }
                string ins = "INSERT INTO login_data (name, time, duty) VALUES ('" + name + "', '" + time + "', '" + duty + "')";
                MySqlCommand ins_command = new MySqlCommand(ins, connection);
                ins_command.ExecuteNonQuery();
                string updata = "UPDATE member SET duty = '" + duty + "' WHERE name = '" + name + "'";
                MySqlCommand updata_command = new MySqlCommand(updata, connection);
                updata_command.ExecuteNonQuery();
                connection.Close();
                textBox2.Text = name + "打卡成功!\r\n狀態:" + duty + "\r\n時間:" + time;
            }
            catch (MySqlException)
            {
                textBox2.Text = "打卡失敗!";
            }
        }
    }
}