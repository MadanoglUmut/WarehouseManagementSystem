using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StoreTracking
{
    public partial class SirketElemanIslemleri : Form
    {

        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;

        public SirketElemanIslemleri()
        {
            InitializeComponent();
        }

        void createDataTable(string sorgu,int sicil_No)
        {
            connection.Open();
            da = new SqlDataAdapter(sorgu, connection);
            da.SelectCommand.Parameters.AddWithValue("@sicil_No", sicil_No);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource= table;
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Menu viewOne = new Menu();
            viewOne.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            elemanEkleSil viewOne = new elemanEkleSil();
            viewOne.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = "Elemanın Zimmetindeki Urunleri Goruntule";
           // label2.Location = new Point(180, 200);
            label3.Visible= true;
            textBox1.Visible= true;
            button5.Visible= true;
            button6.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label2.Text = "Eleman Talepleri Goruntule";
            //label2.Location = new Point(210, 200);
            label3.Visible = true;
            textBox1.Visible = true;
            button5.Visible= false;
            button6.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Lütfen İlgili Alanı Doldurunuz");
                }
                else
                {
                    int sicil_No = int.Parse(textBox1.Text);
                    string sorgu = "SELECT Product_Activity_Date.Request_Date,Product_Activity_Date.Activity_Id,Users.Record_Number, Users.Employee_Name, Users.Employee_Surname, Products.Product_Id,Product_Activity_Date.Confirmation_Status\r\nFROM Users\r\n" +
                        "INNER JOIN Product_Activity_Date on Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                        "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                        "WHERE Confirmation_Status = 'Onaylanmadi' and Users.Record_Number = @sicil_No ";
                    createDataTable(sorgu, sicil_No);
                }
            }

            catch(System.FormatException ex)
            {
                MessageBox.Show("Sicil Numarasını Doğru Formatta Giriniz");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Lütfen İlgili Alanı Doldurunuz");
                }
                else
                {
                    int sicil_No = int.Parse(textBox1.Text);
                    string sorgu = "SELECT Product_Activity_Date.Request_Date, Activity_Type.Activity_Name, Users.Record_Number, Users.Employee_Name, Users.Employee_Surname, Products.Product_Id, Products.Product_Name from Users\r\n" +
                        "INNER JOIN Product_Activity_Date on Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                        "INNER JOIN Activity_Type on Activity_Type.Activity_Category = Product_Activity_Date.Activity_Category\r\n" +
                        "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                        "WHERE Users.Record_Number = @sicil_No and Activity_Name!='Talep' and Active = 1";
                    createDataTable(sorgu, sicil_No);
                }
            }

            catch(System.FormatException ex)
            {
                MessageBox.Show("Sicil Numarasını Doğru Formatta Giriniz");
            }
        }
    }
}
