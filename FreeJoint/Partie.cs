using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeJoint
{
    public class Partie
    {

        public Labyrinthe labyrinthe;

        public Label lblScore = new Label();

        public int score;
        public byte mondeActuel;
        public string NomJoueur;
        public int nbPoints;

        public List<PictureBox> points;
        public List<PictureBox> Mur;
        public List<(entite, PictureBox)> listeFruits;
        public List<(entite, PictureBox)> ListeEnnemis;
        public List<(entite, PictureBox)> ListePacGommes;
        public List<(int, int)> ListeCoordonees;

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
    }
}
