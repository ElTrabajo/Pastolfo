using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman_SAE
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button_start_partie_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form startPartieForm = new StartPartieForm();
            startPartieForm.StartPosition = this.StartPosition;
            startPartieForm.FormClosed += (s, args) => this.Close();
            startPartieForm.Show();
        }

        private void button_load_partie_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form chargementPartieForm = new ChargementPartieForm();
            chargementPartieForm.StartPosition = this.StartPosition;
            chargementPartieForm.FormClosed += (s, args) => this.Close();
            chargementPartieForm.Show();
        }
    }
}
