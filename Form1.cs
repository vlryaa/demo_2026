using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace Shoes
{
    public partial class Form1 : Form
    {
        string connection = "Server=KRN-20-202-MMP1\\MSSQLSERVER2;Database=Shoes;Integrated Security=True;TrustServerCertificate=True";
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;


            try
            {

                SqlConnection conn = new SqlConnection(connection);
                conn.Open();

                string query = "SELECT * FROM [user] WHERE login=@login AND password=@password";

                SqlCommand sqlCommand = new SqlCommand(query, conn);
                sqlCommand.Parameters.AddWithValue("@login", login);
                sqlCommand.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("Добро пожаловать", "Авторизация прошла успешно", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    int userId = Convert.ToInt32(reader[0]);
                    int roleId = Convert.ToInt32(reader[1]);
                    Form2 form21 = new Form2(userId, roleId);
                    form21.Show();
                    this.Hide();
                }
                else if (login == "" || password == "")
                {
                    MessageBox.Show("Оба поля должны быть заполнены!", "ОШИБКА", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "ОШИБКА", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных!", "ОШИБКА", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
      
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
