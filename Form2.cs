using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Shoes
{
    public partial class Form2 : Form
    {
        int userId;
        int roleId;

        string connection = "Server=KRN-20-202-MMP1\\MSSQLSERVER2;Database=Shoes;Integrated Security=True;TrustServerCertificate=True";
        public Form2(int userId = 0, int roleId = 0)
        {
            InitializeComponent();

            this.userId = userId;


            textBox1.Visible = (roleId == 1 || roleId == 2);
            comboBox2.Visible = (roleId == 1 || roleId == 2);
            button2.Visible = (roleId == 1);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            try
            {

                SqlConnection conn = new SqlConnection(connection);
                conn.Open();

                string queryfullname = "SELECT surname + ' ' + name + ' ' + patronymic FROM [user] WHERE id = @id";

                SqlCommand fullnamesql = new SqlCommand(queryfullname, conn);
                fullnamesql.Parameters.AddWithValue("id", userId);
                if (userId != 0)
                {
                    label1.Text = fullnamesql.ExecuteScalar().ToString();
                }
                else
                {
                    label1.Text = "Гость";
                }


                string query = @"SELECT * FROM Tovar 
                                LEFT JOIN categoryId ON Tovar.categoryId = categoryId.id 
                                LEFT JOIN productId ON Tovar.productId = productId.id 
                                LEFT JOIN providerId ON Tovar.providerId = providerId.id 
                                LEFT JOIN producerId ON Tovar.producerId = producerId.id 
                                LEFT JOIN unitId ON Tovar.unitId = unitId.id";

                SqlCommand sql1 = new SqlCommand(query, conn);
                SqlDataReader reader = sql1.ExecuteReader();


                while (reader.Read())
                {
                    UserControl1 userControl = new UserControl1();
                    userControl.Card(
                        reader["nameCategory"].ToString(),
                        reader["nameProduct"].ToString(),
                        reader["description"].ToString(),
                        reader["nameProvider"].ToString(),
                        reader["nameProducer"].ToString(),
                        Convert.ToDecimal(reader["price"]),
                        reader["nameUnit"].ToString(),
                        Convert.ToInt32(reader["quantity"]),
                        Convert.ToInt32(reader["discount"]),
                        reader["photo"].ToString());
                    flowLayoutPanel1.Controls.Add(userControl);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка подключения к базе данных!", "ОШИБКА", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM Tovar 
                                LEFT JOIN categoryId ON Tovar.categoryId = categoryId.id 
                                LEFT JOIN productId ON Tovar.productId = productId.id 
                                LEFT JOIN providerId ON Tovar.providerId = providerId.id 
                                LEFT JOIN producerId ON Tovar.producerId = producerId.id 
                                LEFT JOIN unitId ON Tovar.unitId = unitId.id";

            if (comboBox1.SelectedIndex == 0)
                query += " ORDER BY price ASC"; 
            else if (comboBox1.SelectedIndex == 1)
                query += " ORDER BY price DESC";
            else if (comboBox2.SelectedIndex == 2)
                query += " WHERE price > 0";
            flowLayoutPanel1.Controls.Clear();


            SqlConnection conn = new SqlConnection(connection);
            conn.Open();

            SqlCommand sql = new SqlCommand(query, conn);
            SqlDataReader reader = sql.ExecuteReader();

            while (reader.Read())
            {
                UserControl1 userControl = new UserControl1();
                userControl.Card(
                    reader["nameCategory"].ToString(),
                    reader["nameProduct"].ToString(),
                    reader["description"].ToString(),
                    reader["nameProvider"].ToString(),
                    reader["nameProducer"].ToString(),
                    Convert.ToDecimal(reader["price"]),
                    reader["nameUnit"].ToString(),
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToInt32(reader["discount"]),
                    reader["photo"].ToString());
                flowLayoutPanel1.Controls.Add(userControl);
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = @"SELECT * FROM Tovar 
                                LEFT JOIN categoryId ON Tovar.categoryId = categoryId.id 
                                LEFT JOIN productId ON Tovar.productId = productId.id 
                                LEFT JOIN providerId ON Tovar.providerId = providerId.id 
                                LEFT JOIN producerId ON Tovar.producerId = producerId.id 
                                LEFT JOIN unitId ON Tovar.unitId = unitId.id";

            if (comboBox2.SelectedIndex == 0)
                query += " WHERE nameProvider = 'Kari'"; 
            else if (comboBox2.SelectedIndex == 1)
                query += " WHERE nameProvider = 'Обувь для вас'";
            else if (comboBox2.SelectedIndex == 2)
                query += " WHERE price > 0";
            flowLayoutPanel1.Controls.Clear();


            SqlConnection conn = new SqlConnection(connection);
            conn.Open();

            SqlCommand sql = new SqlCommand(query, conn);
            SqlDataReader reader = sql.ExecuteReader();

            while (reader.Read())
            {
                UserControl1 userControl = new UserControl1();
                userControl.Card(
                    reader["nameCategory"].ToString(),
                    reader["nameProduct"].ToString(),
                    reader["description"].ToString(),
                    reader["nameProvider"].ToString(),
                    reader["nameProducer"].ToString(),
                    Convert.ToDecimal(reader["price"]),
                    reader["nameUnit"].ToString(),
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToInt32(reader["discount"]),
                    reader["photo"].ToString());
                flowLayoutPanel1.Controls.Add(userControl);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search = textBox1.Text.Trim();
            string query = @"SELECT * FROM Tovar 
                                LEFT JOIN categoryId ON Tovar.categoryId = categoryId.id 
                                LEFT JOIN productId ON Tovar.productId = productId.id 
                                LEFT JOIN providerId ON Tovar.providerId = providerId.id 
                                LEFT JOIN producerId ON Tovar.producerId = producerId.id 
                                LEFT JOIN unitId ON Tovar.unitId = unitId.id";

            if (!string.IsNullOrEmpty(search))
            {
                query += " WHERE nameProduct LIKE '%" + search + "%' " +
                    "OR categoryId.nameCategory LIKE '%" + search + "%' " +
                    "OR providerId.nameProvider LIKE '%" + search + "%' " +
                    "OR producerId.nameProducer LIKE '%" + search + "%'";
            }
            flowLayoutPanel1.Controls.Clear();

            SqlConnection conn = new SqlConnection(connection);
            conn.Open();

            SqlCommand sql = new SqlCommand(query, conn);
            SqlDataReader reader = sql.ExecuteReader();

            while (reader.Read())
            {
                UserControl1 userControl = new UserControl1();
                userControl.Card(
                    reader["nameCategory"].ToString(),
                    reader["nameProduct"].ToString(),
                    reader["description"].ToString(),
                    reader["nameProvider"].ToString(),
                    reader["nameProducer"].ToString(),
                    Convert.ToDecimal(reader["price"]),
                    reader["nameUnit"].ToString(),
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToInt32(reader["discount"]),
                    reader["photo"].ToString());
                flowLayoutPanel1.Controls.Add(userControl);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
