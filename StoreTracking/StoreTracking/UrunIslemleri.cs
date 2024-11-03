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

namespace StoreTracking
{
    public partial class UrunIslemleri : Form
    {

        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        int gun = DateTime.Now.Day;
        int ay = DateTime.Now.Month;
        int yil = DateTime.Now.Year;

        public UrunIslemleri()
        {
            InitializeComponent();
        }

        void createDataTable(string sorgu)
        {
            connection.Open();
            da = new SqlDataAdapter(sorgu, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource= table;
            connection.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sorgu = "Select Products.Product_Id, Products.Product_Name, Product_Type.Category_Name, Users.Record_Number,Users.Employee_Name from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "INNER JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                "INNER JOIN Users on Product_Activity_Date.Record_Number = Users.Record_Number WHERE Active = 1";
            createDataTable(sorgu);

            button6.Visible = true;
            button7.Visible = false;
            button4.Visible = false;
            label1.Visible = true;
            textBox1.Visible = true;
            label2.Visible = true;
            textBox2.Visible = true;
            label3.Visible = true;
            textBox3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "Select Users.Record_Number,Users.Employee_Name,Products.Product_Id, Products.Product_Name, Product_Type.Category_Name, Product_Activity_Date.Request_Date, Product_Activity_Date.Approval_Date from Products\r\n" +
                "INNER JOIN Product_Type on Product_Type.Category_Id = Products.Category_Id\r\n" +
                "INNER JOIN Product_Activity_Date on Product_Activity_Date.Product_Id = Products.Product_Id\r\n" +
                " INNER JOIN Users ON Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                "WHERE    Active = 3";
            createDataTable(sorgu);
            button6.Visible = false;
            button7.Visible = true;
            button4.Visible = false;
        }

        private void UrunIslemleri_Load(object sender, EventArgs e)
        {

        }
 

        private void button1_Click(object sender, EventArgs e)
        {
            button4.Visible = true;
            button6.Visible = false;
            button7.Visible = false;
            label1.Visible=true;
            label2.Visible=true;
            textBox1.Visible = true;
            textBox2.Visible = true;

            string sorgu = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname, Product_Activity_Date.Approval_Date, Product_Activity_Date.Confirmation_Status, Products.Product_Id, Products.Product_Name, Activity_Type.Activity_Name\r\n" +
                "FROM Users\r\n" +
                "INNER JOIN Product_Activity_Date on Product_Activity_Date.Record_Number =  Users.Record_Number\r\n" +
                "INNER JOIN Products  on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                "INNER JOIN Activity_Type on Activity_Type.Activity_Category = Product_Activity_Date.Activity_Category\r\n" +
                "WHERE Confirmation_Status = 'Onaylanmadi' AND Activity_Name = 'Talep'";

            createDataTable(sorgu);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Menu viewOne = new Menu();
            viewOne.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button6.Visible = false;
            button7.Visible = false;
            int Urun_Id = int.Parse(textBox1.Text);
            int sicil_No = int.Parse(textBox2.Text);
            string tarih = yil.ToString() + "-" + ay.ToString() + "-" + gun.ToString();
            
            string sorgu = "UPDATE Product_Activity_Date SET Activity_Category = 4, Confirmation_Status = 'Onaylandi', Approval_Date = @Approval_Date,Active=1 WHERE Record_Number = @Record_Number AND Product_Id = @Product_Id";
            command = new SqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@Record_Number", sicil_No);
            command.Parameters.AddWithValue("@Product_Id", Urun_Id);
            command.Parameters.AddWithValue("@Approval_Date", tarih);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Talep Onaylanmıştır");

        }

        private void button6_Click(object sender, EventArgs e)
        {
            button4.Visible= false;
            button7.Visible= false;

            connection.Open();
            string sorgu = "INSERT INTO Swap(Old_Owner_Number, New_Owner_Number, Product_Id,Active) VALUES(@Old_Owner_Number,@New_Owner_Number,@Product_Id,1)";
            command= new SqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@Old_Owner_Number",int.Parse(textBox3.Text));
            command.Parameters.AddWithValue("@New_Owner_Number", int.Parse(textBox2.Text));
            command.Parameters.AddWithValue("@Product_Id", int.Parse(textBox1.Text));
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Devir Talebi Başarılı Şekilde Yollanmıştır");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button4.Visible= false;
            button6.Visible= false;

            string tarih = yil.ToString() + "-" + ay.ToString() + "-" + gun.ToString();
            int devirUrunId = Convert.ToInt32(label5.Text);
            connection.Open();
            string sorgu = "UPDATE Product_Activity_Date SET Active = 0, Return_Date = @Return_Date, Activity_Category = 3 where Product_Id = @Product_Id AND Active = 3";
            command = new SqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@Product_Id", devirUrunId);
            command.Parameters.AddWithValue("@Return_Date", tarih);
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("İade İşlemi Gerçekleşmiştir");
            
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label5.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }
    }
}
