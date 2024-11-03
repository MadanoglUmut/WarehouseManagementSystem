using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace StoreTracking
{
    public partial class elemanEkleSil : Form
    {

        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader read;

        void createDataTable(string query)
        {
            connection.Open();
            da = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
            connection.Close();
        }

        void readData(string sorgu)
        {
            connection.Open();
            command = new SqlCommand(sorgu, connection);
            read = command.ExecuteReader();

            while (read.Read())
            {
                comboBox2.Items.Add(read["Record_Number"]);
            }
            read.Close();
            connection.Close();
        }

        void goster()
        {
            string sorgu2;

            if (comboBox1.Text == "Müdür")
            {
                comboBox2.Items.Clear();
                sorgu2 = "SELECT Gm_Record_Number as Record_Number FROM General_Manager";
                readData(sorgu2);
                comboBox3.Visible = true;
                label9.Visible= true;
                
            }
            else if (comboBox1.Text == "Şef")
            {
                comboBox3.Visible = false;
                label9.Visible = false;
                comboBox2.Items.Clear();
                sorgu2 = "SELECT M_Record_Number as Record_Number FROM Manager";
                readData(sorgu2);
            }
            else if (comboBox1.Text == "İşci")
            {
                comboBox3.Visible = false;
                label9.Visible = false;
                comboBox2.Items.Clear();
                sorgu2 = "SELECT C_Record_Number as Record_Number FROM Chef";
                readData(sorgu2);
            }
        }

        public elemanEkleSil()
        {
            InitializeComponent();

            string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
            createDataTable(sorgu4);

            goster();
            comboBox1.Items.Add("Genel Müdür");
            comboBox1.Items.Add("Müdür");
            comboBox1.Items.Add("Şef");
            comboBox1.Items.Add("İşci");
            groupBox1.Hide();
            comboBox3.Visible= false;
            label9.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {



            int olusturulanId = int.Parse(label11.Text);
            int departmanId = 0;
            

            if (comboBox1.Text == "İşci")
            {
                string sorgu2 = "INSERT INTO Employee(E_Record_Number, C_Record_Number) VALUES(@E_Record_Number,@C_Record_Number)";
                command = new SqlCommand(sorgu2, connection);
                command.Parameters.AddWithValue("@E_Record_Number", olusturulanId);
                command.Parameters.AddWithValue("@C_Record_Number", int.Parse(comboBox2.Text));
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Çalışan Görevi Verilmiştir");
             }

             else if(comboBox1.Text == "Şef")
             {
                string sorgu2 = "INSERT INTO Chef(C_Record_Number, M_Record_Number) VALUES(@C_Record_Number,@M_Record_Number)";
                command= new SqlCommand(sorgu2, connection);
                command.Parameters.AddWithValue("@C_Record_Number", olusturulanId);
                command.Parameters.AddWithValue("@M_Record_Number", int.Parse(comboBox2.Text));
                connection.Open(); 
                command.ExecuteNonQuery();
                MessageBox.Show("Şef Görevi Verilmiştir");
            }

            else if(comboBox1.Text == "Müdür")
            {
                string sorgu2 = "INSERT INTO Manager(M_Record_Number, Gm_Record_Number, Department_Id) VALUES(@M_Record_Number, @Gm_Record_Number, @Department_Id) ";
                command= new SqlCommand(sorgu2, connection);
                command.Parameters.AddWithValue("@M_Record_Number",olusturulanId);
                command.Parameters.AddWithValue("@Gm_Record_Number", int.Parse(comboBox2.Text));

                if(comboBox3.Text == "Insan Kaynaklari")
                {
                    departmanId = 1;

                }
                else if(comboBox3.Text == "Bilgi Islem")
                {
                    departmanId = 2;
                }
                else if(comboBox3.Text == "Muhasebe")
                {
                    departmanId = 3;
                }
                else if(comboBox3.Text == "Teknik Servis")
                {
                    departmanId = 4;
                }
                command.Parameters.AddWithValue("@Department_Id",departmanId);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Müdür  Görevi Başarıyla Verildi");
            }  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SirketElemanIslemleri viewOne = new SirketElemanIslemleri();
            viewOne.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            goster();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("İsim Ve Soyisim Alanları Doldurulmalıdır");

            }

            else
            {
                string sorgu = "INSERT INTO Users(Employee_Name, Employee_Surname,Password)VALUES(@Employee_Name, @Employee_Surname,'1234')";
                command = new SqlCommand(sorgu, connection);
                command.Parameters.AddWithValue("@Employee_Name", textBox1.Text);
                command.Parameters.AddWithValue("@Employee_Surname", textBox2.Text);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Yeni Eleman Ekleme İşlemi Başarılı");
                connection.Close();
                groupBox1.Show();
            }

            connection.Open();
            string sorgu3 = "SELECT TOP 1 * FROM Users ORDER BY Record_Number DESC";
            command = new SqlCommand(sorgu3, connection);
            read = command.ExecuteReader();
            while (read.Read())
            {
                string sonVeri = read["Record_Number"].ToString();
                label11.Text = sonVeri;
            }
            read.Close();
            connection.Close();

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                int silinecekElemanId = int.Parse(label6.Text);
                string queryFour = "DELETE Users WHERE Record_Number = @Record_Number";
                command = new SqlCommand(queryFour, connection);
                command.Parameters.AddWithValue("@Record_Number", silinecekElemanId);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Eleman Silme Başarılı");
                connection.Close();

                string sorgu4 = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
                createDataTable(sorgu4);

            }

            catch(System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("HATA Çalışana Ait Zimmetli Ürün Ve Görev Alınmalıdır");
            }









        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
