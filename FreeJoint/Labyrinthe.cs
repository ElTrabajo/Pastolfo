using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJoint
{
    public class Labyrinthe
    {
        public int largeur; // Largeur du labyrinthe en nombre de sommets
        public int hauteur; // Hauteur du labyrinthe en nombre de sommets
        public Sommet[,] grille; // Grille de sommets représentant le labyrinthe
        private Random aleatoire; // Générateur de nombres aléatoires

        public Labyrinthe(int largeur, int hauteur)
        {
            this.largeur = largeur;
            this.hauteur = hauteur;
            grille = new Sommet[hauteur, largeur]; // Crée une grille de sommets
            aleatoire = new Random(); // Initialise le générateur aléatoire

            // Initialisation de la grille avec des sommets
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    grille[y, x] = new Sommet(x, y); // Crée un sommet à chaque position de la grille
                }
            }

            // Générer le labyrinthe en utilisant l'algorithme de parcours en profondeur
            ParcoursProfondeur();

            SynchroniserMurs();
        }

        public void init()
        {
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    grille[y, x].Visite = false; // Initialise le sommet comme non visité
                    grille[y, x].MurHaut = true; // Initialise tous les murs comme présents au début
                    grille[y, x].MurBas = true;
                    grille[y, x].MurGauche = true;
                    grille[y, x].MurDroite = true;
                }
            }
            ParcoursProfondeur();
            SynchroniserMurs();
        }


        private void ParcoursProfondeur() // Méthode pour générer le labyrinthe en utilisant l'algorithme de parcours en profondeur
        {
            Stack<Sommet> pile = new Stack<Sommet>(); // Initialise une pile pour le parcours en profondeur
            Sommet sommetDepart = grille[8, 8]; // Choix aléatoire du sommet de départ
            pile.Push(sommetDepart); // Place le sommet de départ dans la pile
            sommetDepart.Visite = true; // Marque le sommet de départ comme visité

            while (pile.Count > 0) // Tant qu'il reste des sommets à visiter dans la pile
            {
                Sommet sommetCourant = pile.Peek(); // Examine le sommet au sommet de la pile
                List<Sommet> voisinsNonVisites = ObtenirVoisinsNonVisites(sommetCourant); // Obtient les voisins non visités du sommet courant

                if (voisinsNonVisites.Count > 0) // S'il y a des voisins non visités
                {
                    Sommet voisinAleatoire = voisinsNonVisites[aleatoire.Next(voisinsNonVisites.Count)]; // Choix aléatoire d'un voisin parmi les non visités
                    CasserMurEntre(sommetCourant, voisinAleatoire); // Casse le mur entre le sommet courant et le voisin choisi
                    voisinAleatoire.Visite = true; // Marque le voisin comme visité
                    pile.Push(voisinAleatoire); // Ajoute le voisin à la pile pour le visiter ensuite
                }
                else
                {
                    pile.Pop(); // Retour en arrière car tous les voisins ont été visités
                }
            }

            // Fermer les bords extérieurs du labyrinthe
            FermerBords();
        }

        private List<Sommet> ObtenirVoisinsNonVisites(Sommet sommet)
        {
            List<Sommet> voisins = new List<Sommet>();

            if (sommet.Y > 0 && !grille[sommet.Y - 1, sommet.X].Visite)
                voisins.Add(grille[sommet.Y - 1, sommet.X]); // Voisin au-dessus

            if (sommet.Y < hauteur - 1 && !grille[sommet.Y + 1, sommet.X].Visite)
                voisins.Add(grille[sommet.Y + 1, sommet.X]); // Voisin en-dessous

            if (sommet.X > 0 && !grille[sommet.Y, sommet.X - 1].Visite)
                voisins.Add(grille[sommet.Y, sommet.X - 1]); // Voisin à gauche

            if (sommet.X < largeur - 1 && !grille[sommet.Y, sommet.X + 1].Visite)
                voisins.Add(grille[sommet.Y, sommet.X + 1]); // Voisin à droite

            return voisins;
        }

        private void CasserMurEntre(Sommet sommet1, Sommet sommet2) // Méthode pour casser le mur entre deux sommets
        {
            if (sommet1.Y == sommet2.Y && sommet1.X == sommet2.X - 1)
            {
                sommet1.MurDroite = false;
                sommet2.MurGauche = false;
            }
            else if (sommet1.Y == sommet2.Y && sommet1.X == sommet2.X + 1)
            {
                sommet1.MurGauche = false;
                sommet2.MurDroite = false;
            }
            else if (sommet1.X == sommet2.X && sommet1.Y == sommet2.Y - 1)
            {
                sommet1.MurBas = false;
                sommet2.MurHaut = false;
            }
            else if (sommet1.X == sommet2.X && sommet1.Y == sommet2.Y + 1)
            {
                sommet1.MurHaut = false;
                sommet2.MurBas = false;
            }
        }

        private void SynchroniserMurs() // Méthode pour synchroniser les murs entre les sommets adjacents
        {
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    Sommet sommet = grille[y, x];

                    // Synchroniser avec le sommet au-dessus
                    if (y > 0)
                    {
                        Sommet sommetHaut = grille[y - 1, x];
                        if (sommet.MurHaut != sommetHaut.MurBas)
                        {
                            sommet.MurHaut = sommetHaut.MurBas;
                        }
                    }

                    // Synchroniser avec le sommet en dessous
                    if (y < hauteur - 1)
                    {
                        Sommet sommetBas = grille[y + 1, x];
                        if (sommet.MurBas != sommetBas.MurHaut)
                        {
                            sommet.MurBas = sommetBas.MurHaut;
                        }
                    }

                    // Synchroniser avec le sommet à gauche
                    if (x > 0)
                    {
                        Sommet sommetGauche = grille[y, x - 1];
                        if (sommet.MurGauche != sommetGauche.MurDroite)
                        {
                            sommet.MurGauche = sommetGauche.MurDroite;
                        }
                    }

                    // Synchroniser avec le sommet à droite
                    if (x < largeur - 1)
                    {
                        Sommet sommetDroite = grille[y, x + 1];
                        if (sommet.MurDroite != sommetDroite.MurGauche)
                        {
                            sommet.MurDroite = sommetDroite.MurGauche;
                        }
                    }
                }
            }
        }

        private void FermerBords() // Méthode pour fermer les bords extérieurs du labyrinthe
        {
            // Ferme les murs du bord supérieur et inférieur du labyrinthe
            for (int x = 0; x < largeur; x++)
            {
                grille[0, x].MurHaut = true;
                grille[hauteur - 1, x].MurBas = true;
            }

            // Ferme les murs du bord gauche et droit du labyrinthe
            for (int y = 0; y < hauteur; y++)
            {
                grille[y, 0].MurGauche = true;
                grille[y, largeur - 1].MurDroite = true;
            }
        }
    }
}
