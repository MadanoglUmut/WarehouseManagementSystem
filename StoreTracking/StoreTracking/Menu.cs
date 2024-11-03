using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreTracking
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            CalisanBilgileri viewOne = new CalisanBilgileri();
            viewOne.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DepoUrun viewOne = new DepoUrun();
            viewOne.Show();
            this.Hide();    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SirketElemanIslemleri viewOne = new SirketElemanIslemleri();
            viewOne.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UrunIslemleri viewOne = new UrunIslemleri();
            viewOne.Show();
            this.Hide();
        }
    }
}
