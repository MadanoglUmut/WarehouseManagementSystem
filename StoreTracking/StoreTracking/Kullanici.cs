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
using System.Windows.Forms.VisualStyles;

namespace StoreTracking
{
    public partial class Kullanici : Form
    {

        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader dr;

        int gun = DateTime.Now.Day;
        int ay = DateTime.Now.Month;
        int yil = DateTime.Now.Year;
        


        public Kullanici()
        {
            InitializeComponent();
            

        }

        public int sicilNo;

        void veriOku(string sorgu)
        {
            connection.Open();
            using (command = new SqlCommand(sorgu, connection))
            {
                using (dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        label1.Text = dr["Employee_Name"].ToString();
                    }
                }

            }
        }

        void createDataTable(string sorgu, int sicilNo)
        {
            connection.Open();
            da = new SqlDataAdapter(sorgu, connection);
            da.SelectCommand.Parameters.AddWithValue("@sicilNo", sicilNo);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource= table;
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //KullaniciTalep viewOne = new KullaniciTalep();
            //viewOne.Show();

            label3.Text = "TALEP EKRANI";
            button2.Visible = false;
            button4.Visible = true;
            connection.Open();
            string sorgu2 = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "LEFT JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                "WHERE Product_Activity_Date.Activity_Id IS NULL OR Product_Activity_Date.Activity_Category = 3";
            da = new SqlDataAdapter(sorgu2, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string tarih = yil.ToString() + "-" + ay.ToString() + "-" + gun.ToString();
            int talepEdilecekId = int.Parse(label5.Text);
            string sorgu = "INSERT INTO Product_Activity_Date(Product_Id, Record_Number,Activity_Category,Confirmation_Status,Request_Date,Active) VALUES(@Product_Id, @sicilNo, 1, 'Onaylanmadi', @Request_Date,0)";
            command = new SqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@Product_Id", talepEdilecekId);
            command.Parameters.AddWithValue("@Request_Date", tarih);
            command.Parameters.AddWithValue("@sicilNo", sicilNo);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Talebiniz Başarılı Şekilde Gönderilmiştir");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sorgu1 = "UPDATE Product_Activity_Date SET Active = 3 where Product_Id = @Product_Id AND Record_Number=@Record_Number";
            command = new SqlCommand(sorgu1, connection);
            int iadeEdilecekId = int.Parse(label5.Text);
            command.Parameters.AddWithValue("@Product_Id", iadeEdilecekId);
            command.Parameters.AddWithValue("@Record_Number", sicilNo);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("İade Talebiniz Alınmıştır.");
        }



        private void button3_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button4.Visible = false;

            string sorgu = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name, Product_Activity_Date.Request_Date, Product_Activity_Date.Approval_Date from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "INNER JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                "INNER JOIN Users ON Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                "WHERE Users.Record_Number = @sicilNo AND (Active = 1 or Active = 3)";
            createDataTable(sorgu,sicilNo);
            label3.Text = "ZİMMETİMDEKİ ÜRÜNLER";
        }

        private void Kullanici_Load(object sender, EventArgs e)
        {
            button4.Visible = false;

            string sorgu = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname AS \"Number Of Products Owned\"FROM Users \r\n" +
                "GROUP BY Users.Record_Number, Users.Employee_Name, Users.Employee_Surname\r\n" +
                "HAVING Users.Record_Number = @sicilNo";
            createDataTable(sorgu, sicilNo);

            string isim = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            label4.Text = isim;

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           label5.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button6.Visible = true;
            string sorgu = "select Swap_Id,Products.Product_Id ,Products.Product_Name,Swap.New_Owner_Number ,Users.Employee_Name  from swap \r\n" +
                "INNER JOIN Users ON Users.Record_Number = Swap.New_Owner_Number\r\n" +
                "INNER JOIN Products ON Products.Product_Id = Swap.Product_Id\r\n" +
                "WHERE Swap.Old_Owner_Number = @sicilNo AND Active = 1";
            createDataTable(sorgu, sicilNo);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string tarih = yil.ToString() + "-" + ay.ToString() + "-" + gun.ToString();
            int yeniSahipId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[3].Value);
            int urunId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[1].Value);

           //label6.Text = yeniSahipId.ToString();

            string query = "UPDATE Swap SET Active = 0 WHERE Product_Id = @Product_Id";
            command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Product_Id", urunId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            string query2 = "UPDATE Product_Activity_Date SET Active = 0 WHERE Product_Id = @Product_Id";
            command = new SqlCommand(query2, connection);
            command.Parameters.AddWithValue("@Product_Id", urunId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            string query3 = "INSERT INTO Product_Activity_Date(Product_Id, Record_Number, Activity_Category , Old_Owner_Number, Confirmation_Status , Request_Date, Approval_Date, Active)\r\n" +
                "VALUES (@Product_Id,@Record_Number,2,@Old_Owner_Number,'Onaylandi',@Request_Date,@Approval_Date,1 )";
            command = new SqlCommand(query3, connection);
            command.Parameters.AddWithValue("@Product_Id", urunId);
            command.Parameters.AddWithValue("@Record_Number", yeniSahipId);
            command.Parameters.AddWithValue("@Old_Owner_Number", sicilNo);
            command.Parameters.AddWithValue("@Request_Date", tarih);
            command.Parameters.AddWithValue("@Approval_Date", tarih);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Devir İşlemi Başarılı Şekilde Gerçekleşmiştir");

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
