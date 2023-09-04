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
        private string myDateString;

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
            label1.Text = myDateString;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime myDate = DateTime.Now;
            string myDateString = myDate.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime myDate = DateTime.Now;
            string myDateString = myDate.ToString("yyyy/MM/dd HH:mm:ss");
            string name = textBox1.Text;
            string time = myDateString;
            conexion(name, time);
        }

        private void conexion(string name, string time)
        {
            try
            {
                string com = "INSERT INTO login_data (name, time, type) VALUES ('" + name + "', '" + time + "', '上班')";
                MySqlCommand command = new MySqlCommand(com, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                textBox2.Text = name + "打卡成功!\r\n時間:" + time;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + connectionString);
            }
        }
    }
}