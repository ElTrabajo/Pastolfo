﻿using System;
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
    public partial class Main : Form
    {
        public Main()
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
    }
}
