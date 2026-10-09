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

namespace Shoes
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public void Card(string productId, string categoryId, string description, string providerId, string producerId, decimal price, string unitId, int quantity, int discount, string photo)
        {
            label1.Text = categoryId;
            label16.Text = productId;
            label9.Text = description;
            label10.Text = providerId.ToString();
            label11.Text = producerId.ToString();
            label12.Text = $"{price.ToString()} руб.";
            label13.Text = unitId.ToString();
            label14.Text = quantity.ToString();
            label15.Text = $"{discount.ToString()} %";
            pictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, photo));

            if(discount > 15)
            {
                this.BackColor = ColorTranslator.FromHtml("#2E8B57");
            }

            if (quantity == 0)
            {
                this.BackColor = Color.LightBlue;
            }

            if (discount > 0)
            {
                decimal finalPrice = price - price * discount / 100;

                label12.Text = $"{price} руб.";
                label12.ForeColor = Color.Red;
                label12.Font = new Font(label12.Font, FontStyle.Strikeout);

                label18.Text = $"{finalPrice} руб.";
            }
            else
            {
                label12.Text = $"{price} руб.";
                label12.ForeColor = Color.Black;
                label12.Font = new Font(label12.Font, FontStyle.Regular);

                label18.Text = "";
            }

        }


    }
}
