using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreTracking
{

    
    public partial class KullaniciTalep : Form
    {
        SqlConnection connection = new SqlConnection("server=.; Initial Catalog=StoreTrackingTwo;Integrated Security=SSPI");
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader dr;

        public KullaniciTalep()
        {
            InitializeComponent();
        }

        private void KullaniciTalep_Load(object sender, EventArgs e)
        {
            connection.Open();
            string sorgu = "SELECT Users.Employee_Name FROM Users where Users.Record_Number = 4";
            using (command = new SqlCommand(sorgu, connection))
            {
                using (dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        label4.Text = dr["Employee_Name"].ToString();
                    }
                }

            }
        }
    }
}
