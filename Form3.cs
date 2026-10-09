using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Shoes
{
    public partial class Form3 : Form
    {
        string connection = "Server=KRN-20-202-MMP1\\MSSQLSERVER2;Database=Shoes;Integrated Security=True;TrustServerCertificate=True";
        string photo = "";

        public Form3()
        {
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT id, nameCategory FROM categoryId", conn);

                DataTable categories = new DataTable();
                adapter.Fill(categories);

                comboBox1.DataSource = categories;
                comboBox1.DisplayMember = "nameCategory";
                comboBox1.ValueMember = "id";

                adapter = new SqlDataAdapter("SELECT id, nameProducer FROM producerId", conn);

                DataTable producers = new DataTable();
                adapter.Fill(producers);

                comboBox2.DataSource = producers;
                comboBox2.DisplayMember = "nameProducer";
                comboBox2.ValueMember = "id";
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string name = textBox7.Text.Trim();
            string description = textBox1.Text.Trim();
            string provider = textBox2.Text.Trim();
            string unit = textBox4.Text.Trim();

            decimal price;
            int quantity;
            int discount;

            if (name == "" || description == "" || provider == "" || unit == "" || comboBox1.SelectedValue == null || comboBox2.SelectedValue == null || !decimal.TryParse(textBox3.Text.Replace('.', ','), out price) || !int.TryParse(textBox5.Text, out quantity) || !int.TryParse(textBox6.Text, out discount))
            {
                MessageBox.Show("Заполните все поля правильно!");
                return;
            }

            if (price < 0 || quantity < 0 || discount < 0)
            {
                MessageBox.Show("Числа не могут быть отрицательными!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                // Находим поставщика
                SqlCommand sql = new SqlCommand( "SELECT id FROM providerId WHERE nameProvider = @name", conn);

                sql.Parameters.AddWithValue("@name", provider);
                object providerResult = sql.ExecuteScalar();

                if (providerResult == null)
                {
                    MessageBox.Show("Такого поставщика нет в базе!");
                    return;
                }

                int providerId = Convert.ToInt32(providerResult);

                // Находим единицу измерения
                sql = new SqlCommand("SELECT id FROM unitId WHERE nameUnit = @name", conn);

                sql.Parameters.AddWithValue("@name", unit);
                object unitResult = sql.ExecuteScalar();

                if (unitResult == null)
                {
                    MessageBox.Show("Такой единицы измерения нет в базе!");
                    return;
                }

                int unitId = Convert.ToInt32(unitResult);

                // Получаем ID категории и производителя
                int categoryId = Convert.ToInt32(comboBox1.SelectedValue);
                int producerId = Convert.ToInt32(comboBox2.SelectedValue);

                // Добавляем название товара
                sql = new SqlCommand(
                    "INSERT INTO productId (nameProduct) " +
                    "OUTPUT INSERTED.id VALUES (@name)", conn);

                sql.Parameters.AddWithValue("@name", name);
                int productId = Convert.ToInt32(sql.ExecuteScalar());

                // Добавляем товар
                sql = new SqlCommand(
                    "INSERT INTO Tovar " +
                    "(productId, categoryId, providerId, producerId, " +
                    "unitId, price, quantity, discount, description, photo) " +
                    "VALUES " +
                    "(@product, @category, @provider, @producer, " +
                    "@unit, @price, @quantity, @discount, @description, @photo)",
                    conn);

                sql.Parameters.AddWithValue("@product", productId);
                sql.Parameters.AddWithValue("@category", categoryId);
                sql.Parameters.AddWithValue("@provider", providerId);
                sql.Parameters.AddWithValue("@producer", producerId);
                sql.Parameters.AddWithValue("@unit", unitId);
                sql.Parameters.AddWithValue("@price", price);
                sql.Parameters.AddWithValue("@quantity", quantity);
                sql.Parameters.AddWithValue("@discount", discount);
                sql.Parameters.AddWithValue("@description", description);

                sql.Parameters.AddWithValue("@photo",
                    photo == "" ? (object)DBNull.Value : photo);

                sql.ExecuteNonQuery();
            }

            MessageBox.Show("Товар добавлен!");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Изображения|*.jpg;*.png;*.jpeg;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                photo = ofd.FileName;
                pictureBox1.Image = Image.FromFile(photo);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

    }
}
      


