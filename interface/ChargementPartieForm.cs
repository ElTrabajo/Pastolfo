using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
            MySql.Data.MySqlClient.MySqlConnection connection;
            string myConnectionString = $"server=10.1.139.236;user=d4;pwd=mdp;Database=based4";

            string query = "SELECT nom_joueur, nbVies_joueur, score_joueur, id_monde FROM Joueur";
            int labelYPosition = 200;

            try
            {
                connection = new MySql.Data.MySqlClient.MySqlConnection();
                connection.ConnectionString = myConnectionString;
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) // Faire une boucle pour tout les joueurs trouvées
                {
                    string playerName = reader.GetString("nom_joueur");
                    int nbVies_joueur = reader.GetInt32("nbVies_joueur");
                    int score_joueur = reader.GetInt32("score_joueur");
                    int worldId = reader.GetInt32("id_monde");

                    string formattedText = playerName;
                    formattedText += $"Monde: {worldId}";
                    formattedText += $"Vies: {nbVies_joueur}";
                    formattedText += $"Score: {score_joueur}";

                    Label playerNameLabel = new Label();
                    playerNameLabel.ForeColor = Color.White;
                    playerNameLabel.Text = playerName;
                    playerNameLabel.Location = new Point(84, labelYPosition); // Position du nom du joueur
                    playerNameLabel.AutoSize = true;
                    playerNameLabel.Size = new System.Drawing.Size(44, 16);

                    Label worldIdLabel = new Label();
                    worldIdLabel.ForeColor = Color.White;
                    worldIdLabel.Text = $"Monde: {worldId}";
                    worldIdLabel.Location = new Point(297, labelYPosition); // Position du monde
                    worldIdLabel.AutoSize = true;
                    worldIdLabel.Size = new System.Drawing.Size(44, 16);

                    Label livesLabel = new Label();
                    livesLabel.ForeColor = Color.White;
                    livesLabel.Text = $"Vies: {nbVies_joueur}";
                    livesLabel.Location = new Point(510, labelYPosition); // Position des vies
                    livesLabel.AutoSize = true;
                    livesLabel.Size = new System.Drawing.Size(44, 16);

                    Label scoreLabel = new Label();
                    scoreLabel.ForeColor = Color.White;
                    scoreLabel.Text = $"Score: {score_joueur}";
                    scoreLabel.Location = new Point(723, labelYPosition); // Position du score
                    scoreLabel.AutoSize = true;
                    scoreLabel.Size = new System.Drawing.Size(44, 16);

                    // Ajouts des labels sur la form
                    this.Controls.Add(playerNameLabel);
                    this.Controls.Add(worldIdLabel);
                    this.Controls.Add(livesLabel);
                    this.Controls.Add(scoreLabel);

                    labelYPosition += playerNameLabel.Height + 70; // Ajuste la position Y pour le prochain joueur
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // Affichage d'un message d'erreur lors de problème de récupération de données
                MessageBox.Show($"Erreur pendant la récupération de données: {ex.Message}");
            }
        }
    }
}
