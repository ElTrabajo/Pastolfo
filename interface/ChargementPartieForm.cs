using InfoJoueurSQL;
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
    public partial class ChargementPartieForm : Form
    {
        public ChargementPartieForm()
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

        private void ChargementPartieForm_Load(object sender, EventArgs e)
        {
            // Création de dataLoader par l'appel de la bibliothèque InfoJouerSQLClass
            InfoJoueurSQLClass dataLoader = new InfoJoueurSQLClass();

            try
            {
                // Chargement des informations des joueurs en utilisant dataLoader
                List<InfoJoueur> joueurs = dataLoader.ChargementInfoJoueurs();

                int labelXPosition = 84; // Position initial de X pour chaque joueur
                int labelYPosition = 200; // Position initial de Y pour chaque joueur

                // Création de labels pour chaque joueurs
                foreach (InfoJoueur joueur in joueurs)
                {
                    Label LabelNomJoueur = NouveauLabelJoueur(joueur.Nom);
                    LabelNomJoueur.Location = new Point(labelXPosition, labelYPosition);
                    Label LabelIdMonde = NouveauLabelJoueur($"Monde: {joueur.IdMonde}");
                    LabelIdMonde.Location = new Point(LabelNomJoueur.Location.X + 213, labelYPosition);
                    Label LabelViesJoueur = NouveauLabelJoueur($"Vies: {joueur.Vies}");
                    LabelViesJoueur.Location = new Point(LabelIdMonde.Location.X + 213, labelYPosition);
                    Label LabelScoreJoueur = NouveauLabelJoueur($"Score: {joueur.Score}");
                    LabelScoreJoueur.Location = new Point(LabelViesJoueur.Location.X + 213, labelYPosition);

                    // Ajout des labels sur la Windows form
                    this.Controls.Add(LabelNomJoueur);
                    this.Controls.Add(LabelIdMonde);
                    this.Controls.Add(LabelViesJoueur);
                    this.Controls.Add(LabelScoreJoueur);

                    labelYPosition += LabelScoreJoueur.Height + 70;
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
