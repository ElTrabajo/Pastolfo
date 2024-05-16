using InfoClassementSQL;
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
    public partial class ClassementForm : Form
    {
        public ClassementForm()
        {
            InitializeComponent();
        }

        private void button_retour_startpartie_Click(object sender, EventArgs e)
        {
            this.Hide();

            Form mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }

        private void ClassementForm_Load(object sender, EventArgs e)
        {
            // Création de dataLoader par l'appel de la bibliothèque InfoJouerSQLClass
            InfoClassementSQLClass dataLoader = new InfoClassementSQLClass();

            try
            {
                // Chargement des informations des joueurs en utilisant dataLoader
                List<InfoClassement> rangs = dataLoader.ChargementInfoClassement();

                int labelXPosition = 84; // Position initial de X pour chaque joueur
                int labelYPosition = 200; // Position initial de Y pour chaque joueur

                // Création de labels pour chaque joueurs
                foreach (InfoClassement rang in rangs)
                {
                    Label LabelRangClassement = NouveauLabelJoueur($"Rang: {rang.Rang}");
                    LabelRangClassement.Location = new Point(labelXPosition, labelYPosition);
                    Label LabelNomJoueurClassement = NouveauLabelJoueur(rang.Nom);
                    LabelNomJoueurClassement.Location = new Point(LabelRangClassement.Location.X + 213, labelYPosition);
                    Label LabelPointClassement = NouveauLabelJoueur($"Points: {rang.Point}");
                    LabelPointClassement.Location = new Point(LabelNomJoueurClassement.Location.X + 213, labelYPosition);

                    // Ajout des labels sur la Windows form
                    this.Controls.Add(LabelRangClassement);
                    this.Controls.Add(LabelNomJoueurClassement);
                    this.Controls.Add(LabelPointClassement);

                    labelYPosition += LabelPointClassement.Height + 70;
                }
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur lors de problème de récupération de données
                MessageBox.Show($"Erreur pendant la récupération de données: {ex.Message}");
            }
        }

        // Méthode de type helper permettant de créer un label avec du formattage
        private Label NouveauLabelJoueur(string text)
        {
            Label label = new Label();
            label.Text = text;
            label.ForeColor = Color.White;
            label.Size = new System.Drawing.Size(44, 16);
            label.AutoSize = true;
            return label;
        }
    }
}
