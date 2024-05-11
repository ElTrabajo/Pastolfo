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
            if (textBox_nomjoueur.Text == "Veuillez saisir votre pseudo ici")
            {
                MessageBox.Show("Veuillez rentrer un nom pour le joueur", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!radioButton_classique.Checked && !radioButton_survie.Checked)
            {
                MessageBox.Show("Veuillez choisir un mode de jeu", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox_nomjoueur_Enter(object sender, EventArgs e)
        {
            if (textBox_nomjoueur.Text == "Veuillez saisir votre pseudo ici")
            {
                this.textBox_nomjoueur.Text = "";
                this.textBox_nomjoueur.ForeColor = Color.Black;
            }
        }

        private void textBox_nomjoueur_Leave(object sender, EventArgs e)
        {
            if (textBox_nomjoueur.Text == "")
            {
                this.textBox_nomjoueur.Text = "Veuillez saisir votre pseudo ici";
                this.textBox_nomjoueur.ForeColor = Color.Silver;
            }
        }

        private void button_retour_startpartie_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form mainForm = new Main();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }
    }
}
