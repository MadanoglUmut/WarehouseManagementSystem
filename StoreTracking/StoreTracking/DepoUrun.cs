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
    public partial class DepoUrun : Form
    {

        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter da;

        void createDataTable(string query)
        {
            connection =new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
            connection.Open();
            da = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource =table;
            connection.Close();
        }


        public DepoUrun()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name, Users.Employee_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "INNER JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                "INNER JOIN Users on Product_Activity_Date.Record_Number = Users.Record_Number WHERE Active = 1";

            createDataTable(query);
            label2.Text = "ZİMMET BİLGİSİ";
            dataGridView1.Size = new System.Drawing.Size(450, 214);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id";
           
            createDataTable(query);

            label2.Text = "DEPO ÜRÜN BİLGİLERİ";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu viewOne = new Menu();
            viewOne.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "LEFT JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                "WHERE Product_Activity_Date.Activity_Id IS NULL OR Product_Activity_Date.Activity_Category = 3";

            createDataTable(query);
            label2.Text = "BOŞTAKİ ÜRÜNLER";



        }

        private void button5_Click(object sender, EventArgs e)
        {
            UrunEkleSil viewOne = new UrunEkleSil();  
            viewOne.Show();
            this.Hide();
        }
    }
}
