using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace StoreTracking
{


    public partial class LoginPage : Form
    {
        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader rd;

        public LoginPage()
        {
            InitializeComponent();
        }

        public int sicilNo;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (textBox1.Text == "admin" && textBox2.Text == "1234")
                {
                    Menu viewOne = new Menu();
                    viewOne.Show();
                    this.Hide();
                }

                else
                {
                    string sorgu = "SELECT * FROM Users WHERE Record_Number = @Record_Number AND Password = @Password";
                    command = new SqlCommand(sorgu, connection);
                    command.Parameters.AddWithValue("@Record_Number", int.Parse(textBox1.Text));
                    command.Parameters.AddWithValue("@Password", textBox2.Text);
                    connection.Open();
                    rd = command.ExecuteReader();



                    if (rd.Read())
                    {
                        connection.Close();
                        Kullanici viewOne = new Kullanici();
                        viewOne.sicilNo = int.Parse(textBox1.Text);
                        viewOne.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Bilgilerinizi Kontrol Edin");
                    }
                }
            }

            catch(System.FormatException ex)
            {
                MessageBox.Show("Kullanıcı Bulunamadı");
            }
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }

    }
}
