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
    public partial class StartPartieForm : Form
    {
        public StartPartieForm()
        {
            InitializeComponent();
        }

        private void button_startgame_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_nomjoueur.Text))
            {
                MessageBox.Show("Veuillez rentrer un nom pour le joueur", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!radioButton_classique.Checked && !radioButton_survie.Checked)
            {
                MessageBox.Show("Veuillez choisir un mode de jeu", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
