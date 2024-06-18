using FreeJoint;
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

namespace Pastolfo_interface
{
    public partial class ChargementPartieForm : Form
    {
        public bool modeSurvie {  get; set; }

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

                int labelYPosition = 20; // Position initial de Y pour chaque joueur

                // Création de labels pour chaque joueurs
                foreach (InfoJoueur joueur in joueurs)
                {
                    // Création des labels spécifique à chaque joueurs
                    Label LabelNomJoueur = NouveauLabelJoueur(joueur.Nom, new Point(20, labelYPosition));
                    Label LabelIdMonde = NouveauLabelJoueur($"Monde: {joueur.IdMonde}", new Point(panelSauvegarde.Width / 3 - 50, labelYPosition));
                    Label LabelViesJoueur = NouveauLabelJoueur($"Vies: {joueur.Vies}", new Point(panelSauvegarde.Width / 2 + 40, labelYPosition));
                    Label LabelScoreJoueur = NouveauLabelJoueur($"Score: {joueur.Score}", new Point(panelSauvegarde.Width / 2 + 300, labelYPosition));

                    // Création de la MessageBox pour la confirmation de chargement de sauvegarde spécifique à chaque joueur
                    LabelNomJoueur.Click += (labelSender, clickEventArgs) =>
                    {
                        DialogResult confirmResult = MessageBox.Show($"Voulez vous charger la sauvegarde de {joueur.Nom} ?", "Confirmation", MessageBoxButtons.OKCancel);

                        if (confirmResult == DialogResult.OK)
                        {
                            // Création de la MessageBox pour le choix du mode de jeu
                            DialogResult modeJeu = MessageBox.Show("Voulez vous jouer en mode survie ?", "Confirmation", MessageBoxButtons.YesNo);
                            if (modeJeu == DialogResult.Yes)
                            {
                                modeSurvie = true;
                            }
                            else
                            {
                                modeSurvie = false;
                            }
                            // Exportation des données du joueur depuis la base de données vers le jeu
                            ExportDonnéeJoueurForm(joueur);

                            this.Hide();
                        }
                    };

                    // Ajout des labels sur la Windows form
                    panelSauvegarde.Controls.Add(LabelNomJoueur);
                    panelSauvegarde.Controls.Add(LabelIdMonde);
                    panelSauvegarde.Controls.Add(LabelViesJoueur);
                    panelSauvegarde.Controls.Add(LabelScoreJoueur);

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

        // Méthode permettant d'exporter les données du joueur choisi dans une nouvelle WinForm
        private void ExportDonnéeJoueurForm(InfoJoueur joueur)
        {
            PartieForm partieForm = new PartieForm
            {
                InfoJoueur = joueur // Ajout des données du joueur dans la propriété InfoJoueur
            };
            partieForm.FormClosed += (s, args) => this.Close();
            partieForm.Show();
        }
    }
}
