using System;
using InfoJoueurSQL;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoClassementSQL;

namespace FreeJoint
{
    public class Partie
    {

        public Labyrinthe labyrinthe;

        public Label lblScore = new Label();

        public int score;
        public int mondeActuel { get; set; } = 1;
        public string NomJoueur;
        public int nbPoints;

        public bool isInvincible = false, gameover = false;
        public const byte step = 5;
        public int remainingInvincibilityTime = 0;

        public List<PictureBox> points;
        public List<PictureBox> Mur;
        public List<(entite, PictureBox)> listeFruits;
        public List<(entite, PictureBox)> ListeEnnemis;
        public List<(entite, PictureBox)> ListePacGommes;
        public List<(int, int)> ListeCoordonees;

        private InfoJoueurSQLClass infoJoueurSQL;
        private InfoClassementSQLClass infoClassementSQL;

        public Partie(Labyrinthe labyrinthe1, string pseudo)
        {
            labyrinthe = labyrinthe1;
            NomJoueur = pseudo;

            score = 0;
            mondeActuel = 1;

            points = new List<PictureBox>();
            Mur = new List<PictureBox>();
            listeFruits = new List<(entite, PictureBox)>();
            ListeEnnemis = new List<(entite, PictureBox)>();
            ListePacGommes = new List<(entite, PictureBox)>();
            ListeCoordonees = new List<(int, int)>();
        }

        public void Verification()
        {
            for (int y = 0; y < labyrinthe.hauteur; y++)
            {
                for (int x = 0; x < labyrinthe.largeur; x++)
                {
                    Sommet Case = labyrinthe.grille[y, x];

                    if (x > 0 && x + 1 < labyrinthe.largeur) // On regarde si on est dans les limites en largeur
                    {
                        if (labyrinthe.grille[y, x + 1].MurGauche == false) // Si la case à droite n'a pas de mur gauche => on retire le mur droite de la case actuelle
                            Case.MurDroite = false;

                        if (labyrinthe.grille[y, x - 1].MurDroite == false) // Si la case à gauche n'a pas de mur droite => on retire le mur gauche de la case actuelle
                            Case.MurGauche = false;
                    }

                    if (y > 0 && y + 1 < labyrinthe.hauteur) // On regarde si on est dans les limites en hauteur
                    {
                        if (labyrinthe.grille[y + 1, x].MurBas == false) // Si la case au-dessus n'a pas de mur bas => on retire le mur haut de la case actuelle
                            Case.MurHaut = false;

                        if (labyrinthe.grille[y - 1, x].MurHaut == false) // Si la case en-dessous n'a pas de mur haut => on retire le mur bas de la case actuelle
                            Case.MurBas = false;
                    }
                }
            }
        }

        public void SauvegarderPartie(string JoueurNom, Pacman Pacman, InfoJoueur InfoJoueur)
        {
            int JoueurScore = score;
            int JoueurNbVies = Pacman.nbVies;
            string JoueurEtat;
            if (isInvincible == false)
            {
                JoueurEtat = "Vulnerable";
            }
            else
            {
                JoueurEtat = "Invulnerable";
            }
            int JoueurIdMonde = mondeActuel;

            if (InfoJoueur != null)
            {
                bool resultat = infoJoueurSQL.UpdateJoueur(JoueurNom, JoueurScore, JoueurNbVies, JoueurEtat, JoueurIdMonde);

                if (resultat == true)
                {
                    MessageBox.Show("Mise à jour des info du joueur avec succès!");
                    bool resultat_v2 = infoClassementSQL.UpdateClassementPoints(JoueurNom, JoueurScore);
                    if (resultat_v2 == true)
                    {
                        MessageBox.Show("Mise à jour du classement du joueur avec succès!");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la mise du classement du joueur!");
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour des infos du joueur!");
                }
            }
            else
            {
                int resultat = infoJoueurSQL.CreateJoueur(JoueurNom, JoueurScore, JoueurNbVies, JoueurEtat, JoueurIdMonde);

                if (resultat > 0)
                {
                    MessageBox.Show("Joueur créé avec succès!");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création du joueur!");
                }
            }
        }
    }
}
