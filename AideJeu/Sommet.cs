using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AideJeu
{
    public class Sommet // Cette classe représente un sommet du graphe
    {
        public int X { get; set; } // Coordonnée X du sommet
        public int Y { get; set; } // Coordonnée Y du sommet
        public bool Visite { get; set; } // Indique si le sommet a été visité lors de la génération du labyrinthe
        public bool MurHaut { get; set; } // Indique s'il y a un mur en haut du sommet
        public bool MurBas { get; set; } // Indique s'il y a un mur en bas du sommet
        public bool MurGauche { get; set; } // Indique s'il y a un mur à gauche du sommet
        public bool MurDroite { get; set; } // Indique s'il y a un mur à droite du sommet

        public Sommet(int x, int y)
        {
            X = x;
            Y = y;
            Visite = false; // Initialise le sommet comme non visité
            MurHaut = true; // Initialise tous les murs comme présents au début
            MurBas = true;
            MurGauche = true;
            MurDroite = true;
        }
    }
}
