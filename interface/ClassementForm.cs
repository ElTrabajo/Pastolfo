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

namespace Pastolfo_interface
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

                int labelYPosition = 20; // Position initial de Y pour chaque joueur

                // Création de labels pour chaque joueurs
                foreach (InfoClassement rang in rangs)
                {
                    Label LabelRangClassement = NouveauLabelJoueur($"Rang: {rang.Rang}", new Point(20, labelYPosition));
                    Label LabelNomJoueurClassement = NouveauLabelJoueur(rang.Nom, new Point(panelClassement.Width / 3 + 50, labelYPosition));
                    Label LabelPointClassement = NouveauLabelJoueur($"Points: {rang.Point}", new Point(panelClassement.Width / 2 + 250, labelYPosition));

                    // Ajout des labels sur la Windows form
                    panelClassement.Controls.Add(LabelRangClassement);
                    panelClassement.Controls.Add(LabelNomJoueurClassement);
                    panelClassement.Controls.Add(LabelPointClassement);

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
        private Label NouveauLabelJoueur(string text, Point location)
        {
            Label label = new Label
            {
                Text = text,
                ForeColor = Color.White,
                Size = new System.Drawing.Size(44, 16),
                AutoSize = true,
                Location = location
            };
            return label;
        }
    }
}
