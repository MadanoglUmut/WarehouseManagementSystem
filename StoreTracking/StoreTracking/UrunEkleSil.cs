using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StoreTracking
{
    public partial class UrunEkleSil : Form
    {

        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;

        public UrunEkleSil()
        {
            InitializeComponent();
            string sorgu = "SELECT Product_Id, Product_Name, Category_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id";
            createDataTable(sorgu);
            comboBox1.Items.Add("Elektronik");
            comboBox1.Items.Add("Mobilya");
            comboBox1.Items.Add("Beyaz Esya");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DepoUrun viewOne = new DepoUrun();
            viewOne.Show();
            this.Hide();
        }

        void createDataTable(string query)
        {
            connection.Open();
            da = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource= table;
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                if(string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Ürün İsmi Boş Bırakılamaz");
                }
                else
                {
                    string sorgu = "INSERT INTO Products(Product_Name, Category_Id)VALUES(@Product_Name,@Category_Id)";
                    command = new SqlCommand(sorgu, connection);
                    command.Parameters.AddWithValue("@Product_Name", textBox1.Text);


                    int valueCatagory = 0;

                    if (comboBox1.Text == "Elektronik")
                    {
                        valueCatagory = 1;
                    }

                    else if (comboBox1.Text == "Mobilya")
                    {
                        valueCatagory = 2;
                    }

                    else if (comboBox1.Text == "Beyaz Esya")
                    {
                        valueCatagory = 3;
                    }
                    command.Parameters.AddWithValue("@Category_Id", valueCatagory);
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Ürün Ekleme İşlemi Başarılı");
                    textBox1.Clear();
                    connection.Close();

                    string sorgu2 = "SELECT Product_Id, Product_Name, Category_Name from Products\r\n" +
                    "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id";
                    createDataTable(sorgu2);
                }
            }


            catch(System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("İlgili Alanlar Boş Bırakılarak Eleman Eklenemez");
                connection.Close();
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                int silinecekUrunId = int.Parse(label6.Text);
                string sorgu = "DELETE Products Where Product_Id = @Product_Id";
                command = new SqlCommand(sorgu, connection);
                command.Parameters.AddWithValue("@Product_Id", silinecekUrunId);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Ürün Silme Başarılı");
                connection.Close();

                string sorgu2 = "SELECT Product_Id, Product_Name, Category_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id";
                createDataTable(sorgu2);
            }

            catch(System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Zimmetli Ürün Kaydı Depodan Silinemez");
                connection.Close();
            }





        }
         
        private void button4_Click(object sender, EventArgs e)
        {
            DepoUrun viewOne  = new DepoUrun();
            viewOne.Show();
            this.Hide();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

     
    }
}
