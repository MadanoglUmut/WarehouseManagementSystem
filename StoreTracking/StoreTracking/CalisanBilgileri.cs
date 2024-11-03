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
    public partial class CalisanBilgileri : Form
    {

        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter da;

        void createDataTable(string query)
        {
            connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
            connection.Open();
            da = new SqlDataAdapter(query, connection);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource= table;
            connection.Close();
        }

        public CalisanBilgileri()
        {
            InitializeComponent();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "GENEL MÜDÜR BİLGİLERİ";
            string query = ("SELECT Record_Number, Employee_Name, Employee_Surname, Department.Department_Name FROM Users\r\n" +
         "INNER JOIN General_Manager on Gm_Record_Number = Record_Number" +
         "\r\nINNER JOIN Manager on Manager.Gm_Record_Number = General_Manager.Gm_Record_Number\r\n" +
         "INNER JOIN Department on Department.Department_Id = Manager.Department_Id");

            createDataTable(query);

            Product_Name_Text.Clear();
            button2.Visible=true; 


        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "MÜDÜR BİLGİLERİ";
            string query = "SELECT Record_Number, Employee_Name, Employee_Surname, Department.Department_Name FROM Users\r\n" +
        "INNER JOIN Manager on Record_Number = M_Record_Number\r\n" +
        "INNER JOIN Department on Department.Department_Id = Manager.Department_Id";

            createDataTable(query);

            Product_Name_Text.Clear();
            button2.Visible = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = "ŞEF BİLGİLERİ";
            string query = "SELECT Record_Number, Employee_Name, Employee_Surname, Department.Department_Name FROM Users\r\n" +
            "INNER JOIN Chef on Record_Number = C_Record_Number\r\n" +
            "INNER JOIN Manager on Chef.M_Record_Number = Manager.M_Record_Number\r\n" +
            "INNER JOIN Department on Department.Department_Id = Manager.Department_Id";
            createDataTable(query);

            Product_Name_Text.Clear();
            button2.Visible = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = "İŞCİ BİLGİLERİ";
            string query = "SELECT Record_Number, Employee_Name, Employee_Surname, Department.Department_Name FROM Users\r\n" +
                "INNER JOIN Employee on Record_Number = E_Record_Number\r\n" +
                "INNER JOIN Chef on Chef.C_Record_Number = Employee.C_Record_Number \r\n" +
                "INNER JOIN Manager on Chef.M_Record_Number = Manager.M_Record_Number\r\n" +
                "INNER JOIN Department on Department.Department_Id = Manager.Department_Id";
            createDataTable(query);

            Product_Name_Text.Clear();
            button2.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu viewOne = new Menu();
            viewOne.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label1.Text = "TÜM ÇALIŞAN BİLGİLERİ";
            string query = "SELECT Users.Record_Number, Users.Employee_Name, Users.Employee_Surname from Users";
            createDataTable(query);
            button2.Visible = false;
            Product_Name_Text.Clear();
            Department_Name_Text.Clear();
            


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Record_Number_Text.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            Employee_Name_Text.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            Employee_Surname_Text.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            if (dataGridView1.CurrentRow.Cells.Count >= 4)
            {
                Department_Name_Text.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                if (button2.Visible == false)
                {
                    Product_Name_Text.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
           if(label1.Text == "ŞEF BİLGİLERİ")
            {
                string query = "SELECT Users.Record_Number, Employee_Name, Employee_Surname, Department.Department_Name, COUNT(*) AS \"Number Of Products Owned\" FROM Users\r\n" +
                    "INNER JOIN Chef on Record_Number = C_Record_Number\r\n" +
                    "INNER JOIN Manager on Chef.M_Record_Number = Manager.M_Record_Number\r\n" +
                    "INNER JOIN Department on Department.Department_Id = Manager.Department_Id\r\n" +
                    "INNER JOIN Product_Activity_Date on  Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                    "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                    "GROUP BY Users.Record_Number, Users.Employee_Name, Users.Employee_Surname,Department.Department_Name,Product_Activity_Date.Active\r\n" +
                    "HAVING Product_Activity_Date.Active = 1";

                createDataTable(query);
                button2.Visible = false;
            }

           else if(label1.Text == "MÜDÜR BİLGİLERİ")
            {
                string query = "SELECT Users.Record_Number, Employee_Name, Employee_Surname, Department.Department_Name, COUNT(*) AS \"Number Of Products Owned\" FROM Users\r\n" +
                    "INNER JOIN Manager on Record_Number = M_Record_Number\r\n" +
                    "INNER JOIN Department on Department.Department_Id = Manager.Department_Id\r\n" +
                    "INNER JOIN Product_Activity_Date on  Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                    "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                    "GROUP BY Users.Record_Number, Users.Employee_Name, Users.Employee_Surname,Department.Department_Name,Product_Activity_Date.Active\r\n" +
                    "HAVING Product_Activity_Date.Active = 1";

                createDataTable(query);
                button2.Visible = false;
            }

           else if(label1.Text == "GENEL MÜDÜR BİLGİLERİ")
            {
                string query = "SELECT Users.Record_Number, Employee_Name, Employee_Surname, Department.Department_Name, COUNT(*) AS \"Number Of Products Owned\"  FROM Users\r\n" +
                    "INNER JOIN General_Manager on Gm_Record_Number = Record_Number\r\n" +
                    "INNER JOIN Manager on Manager.Gm_Record_Number = General_Manager.Gm_Record_Number\r\n" +
                    "INNER JOIN Department on Department.Department_Id = Manager.Department_Id\r\n" +
                    "INNER JOIN Product_Activity_Date on  Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                    "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                    "GROUP BY Users.Record_Number, Users.Employee_Name, Users.Employee_Surname,Department.Department_Name,Product_Activity_Date.Active\r\n" +
                    "HAVING Product_Activity_Date.Active = 1";

                createDataTable(query);
                button2.Visible = false;

            }

           else if(label1.Text == "İŞCİ BİLGİLERİ")
            {
                string query = "SELECT Users.Record_Number, Employee_Name, Employee_Surname, Department.Department_Name, COUNT(*) AS \"Number Of Products Owned\" FROM Users\r\n" +
                    "INNER JOIN Employee on Record_Number = E_Record_Number \r\n" +
                    "INNER JOIN Chef on Chef.C_Record_Number = Employee.C_Record_Number \r\n" +
                    "INNER JOIN Manager on Chef.M_Record_Number = Manager.M_Record_Number\r\n" +
                    "INNER JOIN Department on Department.Department_Id = Manager.Department_Id\r\n" +
                    "INNER JOIN Product_Activity_Date on  Product_Activity_Date.Record_Number = Users.Record_Number\r\n" +
                    "INNER JOIN Products on Products.Product_Id = Product_Activity_Date.Product_Id\r\n" +
                    "GROUP BY Users.Record_Number, Users.Employee_Name, Users.Employee_Surname,Department.Department_Name,Product_Activity_Date.Active\r\n" +
                    "HAVING Product_Activity_Date.Active = 1";

                createDataTable(query);
                button2.Visible = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
