using AideJeu;
using InfoJoueurSQL;
using InfoClassementSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pastolfo_interface
{
    public partial class PartieForm : Form
    {
        //Creation des données du labyrinthe
        public Labyrinthe labyrinthe = new Labyrinthe(colonnes, lignes);
        private const int colonnes = 16;
        private const int lignes = 16;
        private const int cellSize = 35;

        //Creation des données du personnage principal
        Pacman Pacman = new Pacman();
        private int remainingInvincibilityTime = 0;

        private static GameOverForm gameOverForm;
        private static FinForm finForm;

        //Creation des données du joueur et de la partie
        public InfoJoueur InfoJoueur { get; set; }
        
        public string nomJoueur { get; set; }
        Partie partie;
        public bool modeSurvie { get; set; }
        public int PauseStatus { get; set; }
        Random aleatoire = new Random();
        private readonly Timer movementTimer;
        private readonly Timer movementTimerFantome;
        private SoundPlayer BackgroundMusique;

        public PartieForm()
        {

            this.partie = new Partie(labyrinthe, nomJoueur); //Initialisation de la partie avec le labyrinthe crée et le pseudo du joueur

            InitializeComponent();
            partie.Verification(); // Vérifie les murs du labyrinthe

            movementTimer = new Timer
            {
                Interval = 45 // Timer pour le déplacement du protagoniste qui s'actualise tout les 45 millisecondes
            };

            movementTimer.Tick += new EventHandler(MovementTimer_Tick);
            movementTimer.Start();

            movementTimerFantome = new Timer
            {
                Interval = 50 // Timer pour le déplacement des fantômes qui s'actualise tout les 50 millisecondes
            };
            movementTimerFantome.Tick += new EventHandler(MovementTimerFantome_Tick);
            movementTimerFantome.Start();
        }

        public void InitialisationPartie() //Procédure qui gère la création et l'affichage des éléments nécessaires au jeu (labyrinthe,joueur,bonus,ennemis)
        {
            //Appel des méthodes pour crée les différents élements nécessaires
            iniPacMan();
            iniBackground();
            iniFantome();
            iniMusique();

            for (int i = 0; i <= 4; i++) //Boucle pour pouvoir créer plusieurs bonus dans le labyrinthe
            {
                iniFruits();
                iniPacGomme();
            }

            for (int y = 0; y < colonnes; y++) 
            {
                for (int x = 0; x < lignes; x++) //Double boucle pour représenter les cases du labyrinthe
                {
                    Sommet Case = labyrinthe.grille[y, x]; //Creation d'un sommet pour chaque case du labyrinthe

                    if (partie.points.Count < (colonnes * lignes) - partie.ListeCoordonees.Count) //Condition pour éviter de recréer les objets points à chaque mort 
                    {
                        iniPoint(y,x); //Creation d'un objet point dans la case actuel
                    }
                    else
                    {
                        if ((y * 16 + x) < partie.points.Count)
                        {
                            if (!partie.points[(y * 16 + x)].Visible) //Si le point de la case actuel à été touché par le joueur
                            {
                                partie.points[(y * 16 + x)].Visible = true; //Alors on remet visible le point dans le labyrinthe
                                partie.nbPoints++; //Et on augmente le nombre de points pour le bon fonctionnement du passage à niveau
                            }
                        }
                    }

                    //Appel des procédurs AjoutMur pour générer les murs du labyrinthes dans les quatres directions
                    AjoutMur(Case.MurGauche, x, y, 2, cellSize);
                    AjoutMur(Case.MurDroite, x + 1, y, 2, cellSize);
                    AjoutMur(Case.MurHaut, x, y, cellSize, 2);
                    AjoutMur(Case.MurBas, x, y + 1, cellSize, 2);
                }
            }
        }

        private void AjoutMur(bool possedeMur, int x, int y, int largeur, int hauteur)
        {
            if (possedeMur) 
            {
                PictureBox mur = new PictureBox();
                mur.BackColor = partie.mondeActuel == 2 || partie.mondeActuel == 4 || partie.mondeActuel == 5 ? Color.White : Color.Black; //Le niveau 2 possédant un fond noir, les murs seront blancs pour une question de visibilité
                mur.Width = largeur;
                mur.Height = hauteur;
                mur.Location = new Point(x * cellSize + 210, y * cellSize + 20);
                partie.Mur.Add(mur); //Ajout du mur dans la liste des murs du labyrinthe
                this.Controls.Add(mur); //Ajout de la picturebox dans la collection du form
            }
        }

        private void AffichageVies() //Gère l'affichage des vies dans le form
        {
            for(int i = 0;i<Pacman.nbVies;i++) { //Boucle selon le nombre de vies restantes du joueur
                PictureBox iconeVie = new PictureBox();
                //Règlages des paramètres de la picturebox
                iconeVie.Name = Convert.ToString($"Vie{i+1}"); 
                iconeVie.Height = cellSize;
                iconeVie.Width = cellSize;
                iconeVie.Image = Properties.Resources.vie;
                iconeVie.BackColor = Color.Transparent;
                iconeVie.SizeMode = PictureBoxSizeMode.StretchImage;
                iconeVie.Location = new Point(600+(i*50), 595);
                this.Controls.Add(iconeVie); //Ajout de la picturebox dans la collection du form
            }
        }

        private void AffichageScore() //Gère l'affichage du score dans le form
        {
            //Règlages des paramètres du label
            partie.lblScore.Font = new Font("Arial", 14); // Font and size
            partie.lblScore.ForeColor = Color.White;
            partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
            partie.lblScore.Location = new Point(300, 600);
            partie.lblScore.AutoSize = true;
            partie.lblScore.BackColor = Color.Transparent;
            this.Controls.Add(partie.lblScore); //Ajout du label dans la collection du form
        }

        private void iniPacMan() //Permet d'initialiser le joueur et son modèle
        {
            Pacman.PacmanPC.Image = Properties.Resources.astolfo; //Creation de la picturebox représentant le joueur
            //Règlage des paramètre de la picturebox
            Pacman.PacmanPC.Location = new Point(8 * cellSize + 215, 8 * cellSize + 25);
            partie.ListeCoordonees.Add((8, 8));
            Pacman.PacmanPC.SizeMode = PictureBoxSizeMode.Zoom;
            Pacman.PacmanPC.Size = new Size(cellSize - 5, cellSize - 5);
            Pacman.PacmanPC.BackColor = Color.Transparent;
            Pacman.PacmanPC.BringToFront();
            this.Controls.Add(Pacman.PacmanPC); //Ajout de la picturebox dans la collection du form
        }

        private void iniFantome() //Permet d'initialiser les ennemis et leurs modèles
        {
            // Si les 4 ennemis ne sont pas encore crées
            if (partie.ListeEnnemis.Count != 4)
            {
                //Liste des ressources des ennemis (change selon le niveau)
                Image[] asset = ChoixAssetEnnemis();

                // Pour chaque ennemi du niveau
                foreach (Image skin in asset)
                {
                    // Crée une nouvelle entité pour le fantôme
                    entite fantome1 = new entite();

                    int x;
                    int y;

                    // Génère des coordonnées aléatoires pour le fantôme
                    do
                    {
                        x = aleatoire.Next(0, colonnes);
                        y = aleatoire.Next(0, lignes);
                    }
                    // S'assure que les coordonnées ne sont pas déjà occupées (joueur,bonus)
                    while (partie.ListeCoordonees.Contains((y, x)));

                    fantome1.SetCoordonees(x, y);

                    // Ajoute les coordonnées à la liste des coordonnées utilisées
                    partie.ListeCoordonees.Add((y, x));

                    // Crée un nouveau PictureBox pour le fantôme
                    PictureBox fantome = new PictureBox();

                    // Ajoute le fantôme à la liste des ennemis
                    partie.ListeEnnemis.Add((fantome1, fantome));

                    //Gestion du paramètrage de la picturebox
                    fantome.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                    fantome.Image = skin;
                    fantome.SizeMode = PictureBoxSizeMode.StretchImage;
                    fantome.BackColor = Color.Transparent;
                    fantome.Size = new Size(cellSize - 8, cellSize - 8);
                    fantome.BringToFront();

                    //Ajout de la picturebox dans la collection du form
                    this.Controls.Add(fantome);
                }
            }
        }

        private void iniPoint(int x, int y)
        {
            var coordinates = (y, x);

            // Vérifier si les coordonnées sont déjà utilisées
            if (!partie.ListeCoordonees.Contains(coordinates))
            {
                // Charger l'icône de point depuis les ressources (mise en cache si possible)
                Image pointIcon = Properties.Resources.point;

                // Créer une nouvelle PictureBox pour le point
                PictureBox point = new PictureBox
                {
                    Image = pointIcon,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Height = cellSize - 10,
                    Width = cellSize - 10,
                    Location = new Point(x * cellSize + 215, y * cellSize + 25),
                    Name = partie.nbPoints.ToString()
                };

                // Ajouter la PictureBox à la liste des points et au formulaire
                partie.points.Add(point);
                this.Controls.Add(point);

                // Incrémenter le nombre de points
                partie.nbPoints++;
            }
        }

        private void iniFruits() //Permet d'initialiser les "fruits" et leurs modèles
        {
            if (partie.listeFruits.Count != 4) //Si les quatres fruits ne sont pas initialisés
            {
                int x;
                int y;
                do
                { // Génère des coordonnées aléatoires pour le fruit
                    x = aleatoire.Next(0, colonnes);
                    y = aleatoire.Next(0, lignes);
                }
                while (partie.ListeCoordonees.Contains((y, x))); // S'assure que les coordonnées ne sont pas déjà occupées (joueur,ennemis,bonus)
 
                PictureBox fruitPC = new PictureBox(); //Création de la picturebox du fruit
                entite fruit = new entite(x, y);
                partie.listeFruits.Add((fruit, fruitPC)); //Ajout du fruit dans la liste des fruits
                partie.ListeCoordonees.Add((y, x)); // Ajoute les coordonnées à la liste des coordonnées utilisées

                //Paramètrage de la picturebox
                fruitPC.Image = ChoixAssetFruits();
                fruitPC.SizeMode = PictureBoxSizeMode.StretchImage;
                fruitPC.BackColor = Color.Transparent;
                fruitPC.BorderStyle = BorderStyle.None;
                fruitPC.Height = cellSize - 10;
                fruitPC.Width = cellSize - 10;
                fruitPC.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                fruitPC.BringToFront();
                this.Controls.Add(fruitPC); //Ajout de la picturebox dans la collection du form
            }
            else
            {
                // Rendre tous les fruits visibles
                foreach (var fruit in partie.listeFruits)
                {
                    fruit.Item2.Visible = true;
                }
            }
        }

        private void iniPacGomme() //Permet d'initialiser les "pac-gommes" et leurs modèles
        {
            if (partie.ListePacGommes.Count != 4) //Si les quatres pac-gommes ne sont pas initialisés
            {
                int x, y;
                do
                { // Génère des coordonnées aléatoires pour la pac-gomme
                    x = aleatoire.Next(0, colonnes);
                    y = aleatoire.Next(0, lignes);
                }
                while (partie.ListeCoordonees.Contains((y, x)));  // S'assure que les coordonnées ne sont pas déjà occupées (joueur,ennemis,bonus)

                PictureBox PacGommePC = new PictureBox(); //Création de la picturebox de la pac-gomme
                entite PacGomme = new entite(x, y);
                partie.ListeCoordonees.Add((y, x)); // Ajoute les coordonnées à la liste des coordonnées utilisées
                partie.ListePacGommes.Add((PacGomme, PacGommePC));

                //Paramètrage de la picturebox
                PacGommePC.Visible = true;
                PacGommePC.Image = ChoixAssetPacGomme();
                PacGommePC.SizeMode = PictureBoxSizeMode.StretchImage;
                PacGommePC.BackColor = Color.Transparent;
                PacGommePC.BorderStyle = BorderStyle.None;
                PacGommePC.Height = cellSize - 10;
                PacGommePC.Width = cellSize - 10;
                PacGommePC.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                this.Controls.Add(PacGommePC); //Ajout de la picturebox dans la collection du form
            }
            else
            {
                // Rendre toutes les pac-gommes visibles
                foreach (var pacGomme in partie.ListePacGommes)
                {
                    pacGomme.Item2.Visible = true;
                }
            }
        }

        private void iniBackground() //Initialise l'image de fond du niveau 
        {
            this.DoubleBuffered = true;
            this.BackgroundImage = ChoixAssetBackGround();
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private void iniMusique() //Initialise la musique de fond
        {
            string resourceName = $"_{partie.mondeActuel - 1}_audio";

            try
            {
                BackgroundMusique = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject(resourceName));
                BackgroundMusique.PlayLooping();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement de la musique pour le niveau {partie.mondeActuel}: {ex.Message}");
                // Gérer l'erreur de chargement de la musique (ex: jouer une autre musique par défaut)
            }
        }

        private Image[] ChoixAssetEnnemis() //Choisi les bonnes images pour les ennemis en fonction du niveau
        {
            Image[] assetEnnemis = new Image[4];

            if (partie.mondeActuel >= 1 && partie.mondeActuel <= 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    string imageName = $"_{partie.mondeActuel - 1}_enemy_{i + 1}";
                    assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                }
            }

            return assetEnnemis; //Retourne la liste des ressources pour l'apparence des ennemis
        }

        private Image ChoixAssetFruits() //Choisi les bonnes images pour les fruits en fonction du niveau
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_object_2,
                Properties.Resources._1_object_2,
                Properties.Resources._2_object_2,
                Properties.Resources._3_object_2,
                Properties.Resources._4_object_2
            };

            // Valider que partie.mondeActuel est dans la plage des indices du tableau
            int index = partie.mondeActuel - 1;
            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où partie.mondeActuel est hors de la plage attendue
                Console.WriteLine($"partie.mondeActuel {partie.mondeActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_object_2; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private Image ChoixAssetPacGomme() //Choisi les bonnes images pour les pac-gommes en fonction du niveau
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_object_1,
                Properties.Resources._1_object_1,
                Properties.Resources._2_object_1,
                Properties.Resources._3_object_1,
                Properties.Resources._4_object_1
            };

            // Valider que partie.mondeActuel est dans la plage des indices du tableau
            int index = partie.mondeActuel - 1;
            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où partie.mondeActuel est hors de la plage attendue
                Console.WriteLine($"partie.mondeActuel {partie.mondeActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_object_1; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private Image ChoixAssetBackGround() //Choisi la bonne image de fond pour chaque niveau
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_background,
                Properties.Resources._1_background,
                Properties.Resources._2_background,
                Properties.Resources._3_background,
                Properties.Resources._4_background
            };

            // Valider que partie.mondeActuel est dans la plage des indices du tableau
            int index = partie.mondeActuel - 1;

            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où partie.mondeActuel est hors de la plage attendue
                Console.WriteLine($"partie.mondeActuel {partie.mondeActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_background; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private void StopDeplacementFantome() //Permet d'arrêter le déplacement des ennemis
        {
            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis) 
            {
                ennemi.Item1.SetDeplacement("Pause"); //Pour chaque ennemi, on met sa direction en "pause"
            }
        }

        private void PartieForm_KeyDown(object sender, KeyEventArgs e) //Gestion des appuis de touche de l'utilisateur
        {
            int step = Partie.step;

            if (e.KeyCode == Keys.Left && !CheckCollisionWithMurVer(-step)) //Si le joueur appui sur la flèche gauche du clavier, et qu'il n'y a aucun obstacle à sa gauche
            {
                Pacman.SetDeplacement("Gauche"); 
            }
            else
            {
                if (e.KeyCode == Keys.Right && !CheckCollisionWithMurVer(step)) //Si le joueur appui sur la flèche droite du clavier, et qu'il n'y a aucun obstacle à sa droite
                {
                    Pacman.SetDeplacement("Droite");
                }
                else
                {
                    if (e.KeyCode == Keys.Down && !CheckCollisionWithMurHor(step)) //Si le joueur appui sur la flèche du bas du clavier, et qu'il n'y a aucun obstacle en dessous
                    {
                        Pacman.SetDeplacement("Bas");
                    }
                    else
                    {
                        if (e.KeyCode == Keys.Up && !CheckCollisionWithMurHor(-step)) //Si le joueur appui sur la flèche du haut du clavier, et qu'il n'y a aucun obstacle au dessus
                        {
                            Pacman.SetDeplacement("Haut");
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.P) //Si le bouton de pause est pressé 
            {
                //On met en pause la musique,le déplacement des ennemis et du joueur
                BackgroundMusique.Stop();
                StopDeplacementFantome();

                PauseForm pauseForm = new PauseForm();
                pauseForm.ShowDialog(); //Affichage du form de pause

                PauseStatus = pauseForm.PauseStatus;
                switch (PauseStatus) //Choix de l'utilisateur
                {
                    case 1: //Continuer sans sauvegarder
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                    case 2: //Sauvegarder et continuer
                        partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                    case 3: //Sauvegarder et quitter
                        partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
                        Application.Exit();
                        break;
                    default: //Fermeture du form de pause avec la croix
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                }
            }
        }

        private void CheckCollisionWithPoints() //Verification si le joueur est entrée en collision avec un point
        {
            foreach (var point in partie.points) //Pour tout les points du labyrinthe
            {
                if (Pacman.PacmanPC.Bounds.IntersectsWith(point.Bounds)) //Si le joueur entre en contact avec le point
                {
                    if (point.Visible == true)//Et que le point n'as pas déjà été pris
                    {
                        //Mise a jour du point
                        point.Visible = false; 
                        partie.nbPoints--; 
                        //Mise à jour du score
                        partie.score += 100; 
                        partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}"; 
                    }
                    break;
                }
            }
        }

        private bool CheckCollisionWithMurVer(int offsetX) //Verification d'une collision entre le joueur et un mur "vertical" 
        {
            //Pour éviter que la collision soit détectée quand le joueur sera déjà dans le mur, on va vouloir vérifier la futur position du joueur
            Rectangle futurePosition = new Rectangle(Pacman.PacmanPC.Left + offsetX, Pacman.PacmanPC.Top, Pacman.PacmanPC.Width, Pacman.PacmanPC.Height);
            foreach (var mur in partie.Mur) //Pour tout les murs du labyrinthe
            {
                if (futurePosition.IntersectsWith(mur.Bounds)) //Si la future position du joueur est dans le mur
                {
                    return true; //On retourne true pour signifier la collision
                }
            }
            return false; //Sinon on retourne false pour signifier aucune collision
        }

        private bool CheckCollisionWithMurVerF(int offsetX, PictureBox Ennemi) //Même principe mais pour les ennemis, la picturebox passé en paramètre seras celle de l'ennemi
        {

            Rectangle futurePosition = new Rectangle(Ennemi.Left + offsetX, Ennemi.Top, Ennemi.Width, Ennemi.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; 
                }
            }
            return false; 
        }

        private bool CheckCollisionWithMurHor(int offsetY) //Verification d'une collision entre le joueur et un mur "horizontal", le principe est le même que pour les murs verticals
        {
            Rectangle futurePosition = new Rectangle(Pacman.PacmanPC.Left, Pacman.PacmanPC.Top + offsetY, Pacman.PacmanPC.Width, Pacman.PacmanPC.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; 
                }
            }
            return false; 
        }

        private bool CheckCollisionWithMurHorF(int offsetY, PictureBox Ennemi) //Même principe mais pour les ennemis, la picturebox passé en paramètre seras celle de l'ennemi
        {
            Rectangle futurePosition = new Rectangle(Ennemi.Left, Ennemi.Top + offsetY, Ennemi.Width, Ennemi.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        private async void CheckCollisionFantome() //Verification des collisions entre le joueur et les ennemis
        {
            bool touche = false; // Variable pour vérifier si le joueur a été touché
            var EnnemiAMasquer = (fantome: (entite)null, pictureboxfantome: (PictureBox)null); //Variable servant pour masquer l'ennemi touché si le joueur est invincible

            foreach (var (fantome, pictureboxfantome) in partie.ListeEnnemis) //Pour tout les ennemis sur le labyrinthe
            {
                if (Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxfantome.Bounds)) //Si le joueur rentre en collision avec un ennemi
                {
                    if (partie.isInvincible) //Si le joueur est invincible après la consomation d'une pac-gomme
                    {
                        EnnemiAMasquer = (fantome, pictureboxfantome); //Garde en mémoire l'ennemi touché si le joueur est invincible 
                        break;
                    }
                    else //Si le joueur n'est pas sous l'effet d'une pac-gomme, alors le niveau se remet à zéro et il perd une vie
                    {
                        touche = true;
                        //Mise à jour des vies
                        this.Controls.RemoveByKey($"Vie{Pacman.nbVies}");
                        Pacman.nbVies--;
                        //Mise à jour du score
                        partie.score = 0;
                        partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
                        if (Pacman.nbVies == 0) { // Si Pacman n'a plus de vies
                            partie.gameover = true; //La partie est finie
                            touche = false;
                        }
                        break;
                    }
                }
            }

            if (touche) // Si Pacman a été touché
            {
                InitialisationPartie(); // Réinitialiser la partie à zéro
            }
            else if (partie.gameover) // Si la partie est terminée
            {
                ReInitLabyrintheOrGameOverOrEnd(); //Appel de la procédure de gestion de fin de partie
            }
            else if (EnnemiAMasquer.pictureboxfantome != null) // Si un ennemi a été touché pendant que le joueur était invincible
            {
                partie.ListeEnnemis.Remove(EnnemiAMasquer); // Enlever temporairement l'ennemi de la liste des ennemis
                //Rend l'ennemi invisible pendant 3 secondes
                EnnemiAMasquer.pictureboxfantome.Visible = false;
                await Task.Delay(3000);

                //Au bout de 3 secondes,  l'ennemi est de nouveau visible et re-ajouté à la liste
                EnnemiAMasquer.pictureboxfantome.Visible = true;
                partie.ListeEnnemis.Add((EnnemiAMasquer.fantome, EnnemiAMasquer.pictureboxfantome));
            }
        }

        private void CheckCollisionFruit() //Verification des collisions entre le joueur et les "fruits" (bonus qui octroie +1000 de score)
        {
            foreach (var (fruit, pictureboxfruit) in partie.listeFruits) //Pour tout les fruits du labyrinthe
            {
                if (pictureboxfruit.Visible == true && Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxfruit.Bounds)) //Si le joueur entre en collision avec un fruit
                {
                    //Mise à jour du score
                    partie.score += 1000;
                    partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";

                    pictureboxfruit.Visible = false; //Rendre invisible le fruit
                    
                }
            }
        }

        private void CheckCollisionPacGomme() //Verification des collisions entre le joueur et les "pac-gommes" (bonus qui rend le joueur temporairement invincible)
        {
            foreach (var (pacGomme, pictureboxPacGomme) in partie.ListePacGommes) //Pour toutes les pac-gommes du labyrinthe
            {
                if (pictureboxPacGomme.Visible && Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxPacGomme.Bounds)) //Si le joueur entre en collision avec une pac-gomme
                {
                    pictureboxPacGomme.Visible = false;
                    Pacman.PacmanPC.BackColor = Color.Blue; 
                    remainingInvincibilityTime += 5000; // Ajout de 5 secondes d'invincibilité (permet le cumul des pac-gommes)

                    if (!partie.isInvincible) //Si le joueur n'était pas déjà invincible, on le rend invincible
                    {
                        partie.isInvincible = true;
                        StartInvincibilityTimer();
                    }
                }
            }
        }

        private async void StartInvincibilityTimer() //Gère le timer pour l'invincibilité 
        {
            while (remainingInvincibilityTime > 0) //Tant qu'il reste du temps d'invincibilité
            {
                await Task.Delay(1000); // Attendre par incréments de une seconde
                remainingInvincibilityTime -= 1000; //Retirer une seconde au temps d'invincibilité
            }

            Pacman.PacmanPC.BackColor = Color.Transparent;
            partie.isInvincible = false; //Retirer l'état d'invincibilité
        }

        private void RestartDeplacementFantome() //Permet de recommencer le mouveau des fantômes après la fermeture du menu de pause
        {
            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis)
            {
                ennemi.Item1.SetDeplacement("Stopped");
            }
        }

        private void DeplacementFantomeAlea() //Gère le déplacement aléatoire des ennemis
        {
            int step = Partie.step;

            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis)
            {
                switch (ennemi.Item1.deplacement) //Regarde la direction de chaque ennemi dans la liste 
                {
                    case "Gauche":
                        if (!CheckCollisionWithMurVerF(-step, ennemi.Item2)) //Si l'ennemi va à gauche et n'as pas de mur dans sa direction, il peut se déplacer
                        {
                            ennemi.Item2.Left -= step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped"); //Sinon l'on stoppe l'ennemi
                        }
                        break;
                    case "Droite":
                        if (!CheckCollisionWithMurVerF(step, ennemi.Item2)) //Si l'ennemi va à droite et n'as pas de mur dans sa direction, il peut se déplacer
                        {
                            ennemi.Item2.Left += step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped"); //Sinon l'on stoppe l'ennemi
                        }
                        break;
                    case "Haut":
                        if (!CheckCollisionWithMurHorF(-step, ennemi.Item2)) //Si l'ennemi va vers le haut et n'as pas de mur dans sa direction, il peut se déplacer
                        {
                            ennemi.Item2.Top -= step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped"); //Sinon l'on stoppe l'ennemi
                        }
                        break;
                    case "Bas":
                        if (!CheckCollisionWithMurHorF(step, ennemi.Item2)) //Si l'ennemi va vers le bas et n'as pas de mur dans sa direction, il peut se déplacer
                        {
                            ennemi.Item2.Top += step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped"); //Sinon l'on stoppe l'ennemi
                        }
                        break;
                }
            }
        }

        void ReInitLabyrintheOrGameOverOrEnd() //Permet de réinitialiser le labyrinthe en cas de changement de niveau ou si le joueur recommence après un game over
        {
            foreach(PictureBox mur in partie.Mur)
            {
                this.Controls.Remove(mur); // Enlevez le PictureBox du formulaire
                mur.Dispose();
            }
            partie.Mur.Clear();

            for (int i = 0; i < partie.ListePacGommes.Count; i++)
            {
                var pacgomme = partie.ListePacGommes[i];

                pacgomme.Item1 = null; // Mettre l'entité à null pour éligibilité à la collecte des ordures
                this.Controls.Remove(pacgomme.Item2); // Enlever le PictureBox du formulaire

                if (pacgomme.Item2 != null)
                {
                    pacgomme.Item2.Dispose(); // Libérer les ressources du PictureBox
                }

            }

            // Vider la liste après avoir supprimé les PictureBox
            partie.ListePacGommes.Clear();

            for (int i = 0; i < partie.listeFruits.Count; i++)
            {
                var fruit = partie.listeFruits[i];

                fruit.Item1 = null; // Mettre l'entité à null pour éligibilité à la collecte des ordures
                this.Controls.Remove(fruit.Item2); // Enlever le PictureBox du formulaire

                if (fruit.Item2 != null)
                {
                    fruit.Item2.Dispose(); // Libérer les ressources du PictureBox
                }
            }

            // Vider la liste après avoir supprimé les PictureBox
            partie.listeFruits.Clear();

            for (int i = 0; i < partie.ListeEnnemis.Count; i++)
            {
                var ennemi = partie.ListeEnnemis[i];

                ennemi.Item1 = null; // Mettre l'entité à null pour éligibilité à la collecte des ordures
                this.Controls.Remove(ennemi.Item2); // Enlever le PictureBox du formulaire

                if (ennemi.Item2 != null)
                {
                    ennemi.Item2.Dispose(); // Libérer les ressources du PictureBox
                }

            }

            // Vider la liste après avoir supprimé les PictureBox
            partie.ListeEnnemis.Clear();

            partie.ListeCoordonees.Clear();

            for (int i = 0; i < partie.points.Count; i++)
            {
                var point = partie.points[i];

                this.Controls.Remove(point); // Enlever le PictureBox du formulaire
            }

            partie.points.Clear();
            partie.nbPoints = 0;

            if (partie.gameover)
            {
                BackgroundMusique.Stop();

                // Créer et afficher un nouveau formulaire Game Over
                if (gameOverForm == null)
                {
                    gameOverForm = new GameOverForm();
                    gameOverForm.FormClosed += (s, args) => this.Close();
                    gameOverForm.StartPosition = this.StartPosition;
                    gameOverForm.RestartGame += OnRestartGame;
                }

                gameOverForm.Show();
                this.Hide(); // Cacher la forme principale
            } else if ((partie.mondeActuel == 5) && (!modeSurvie)) {
                BackgroundMusique.Stop();

                DialogResult fin = MessageBox.Show("Félicitation ! Vous avez terminer le mode classique ! Voulez-vous sauvegarder votre partie ?", "Sauvegarder ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (fin == DialogResult.Yes)
                {
                    partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
                }
                else
                {
                    MessageBox.Show("Partie non sauvegardée !");
                }

                if (finForm == null)
                {
                    finForm = new FinForm();
                    finForm.FormClosed += (s, args) => this.Close();
                    finForm.StartPosition = this.StartPosition;
                }
                finForm.Show();
                this.Hide();
            } else {
                if (!modeSurvie) {
                    partie.mondeActuel++;
                }
                partie.Verification();
                InitialisationPartie();
            }
        }

        private void MovementTimer_Tick(object sender, EventArgs e) //Permet de gérer l'avencement automatique du joueur après l'appui sur une touche
        {
            bool collisionDetected = false;
            int step = Partie.step;

            switch (Pacman.deplacement)
            {
                case "Gauche":
                    if (!CheckCollisionWithMurVer(-step))
                    {
                        Pacman.PacmanPC.Left -= step; //Si le joueur souhaite aller à gauche et qu'il n'y a pas de mur, alors l'on bouge la picturebox du joueur vers la gauche
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Droite":
                    if (!CheckCollisionWithMurVer(step))
                    {
                        Pacman.PacmanPC.Left += step; //Si le joueur souhaite aller à droite et qu'il n'y a pas de mur, alors l'on bouge la picturebox du joueur vers la gauche
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Haut":
                    if (!CheckCollisionWithMurHor(-step))
                    {
                        Pacman.PacmanPC.Top -= step; //Si le joueur souhaite aller vers le haut et qu'il n'y a pas de mur, alors l'on bouge la picturebox du joueur vers la gauche
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Bas":
                    if (!CheckCollisionWithMurHor(step))
                    {
                        Pacman.PacmanPC.Top += step; //Si le joueur souhaite aller vers le bas et qu'il n'y a pas de mur, alors l'on bouge la picturebox du joueur vers la gauche
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Stopped":
                    collisionDetected = true;
                    break;
            }

            if (!collisionDetected)
            {
                //Si le joueur avance, on vérifie si il entre en collision avec un élément du jeu
                CheckCollisionFruit();
                CheckCollisionPacGomme();
                CheckCollisionWithPoints();
                CheckCollisionFantome();
                if (partie.nbPoints == 0)
                {
                    ReInitLabyrintheOrGameOverOrEnd(); //Passage au niveau suivant si le joueur a récupérer le dernier point d'un niveau
                }
            }
        }
        private void MovementTimerFantome_Tick(object sender, EventArgs e) //Permet de gérer le changement de direction des ennemis aléatoirement
        {
            {
                foreach (var (fantome, picturebox) in partie.ListeEnnemis) //Pour tout les ennemis de la liste des ennemis
                {
                    fantome.timer += aleatoire.Next(0, 10);
                    if ((fantome.timer >= 100 && fantome.deplacement != "stopped" && fantome.deplacement != "Pause") || fantome.deplacement == "stopped") //Si le timer à dépassé 100 et que l'ennemi n'est pas en pause ou si l'ennemi est stoppé alors on lui attribue une nouvelle direction
                    {
                        string[] directions = { "Gauche", "Droite", "Haut", "Bas" };
                        int indexAleatoire = aleatoire.Next(0, directions.Length);
                        fantome.SetDeplacement(directions[indexAleatoire]); //Selection aléatoire de la prochaine direction du fantôme
                        fantome.timer = 0;
                    }
                }
                DeplacementFantomeAlea(); //Fait se déplacer l'ennemi
                CheckCollisionFantome(); //Regarde si l'ennemi est entré en collision avec le joueur
            }
        }

        private void PartieForm_Load(object sender, EventArgs e)
        {
            if (InfoJoueur != null)
            {
                Pacman.nbVies = InfoJoueur.Vies;
                nomJoueur = InfoJoueur.Nom;
                partie.score = InfoJoueur.Score;
                partie.mondeActuel = InfoJoueur.IdMonde;

                if (InfoJoueur.Etat == "Vulnerable")
                {
                    partie.isInvincible = false;
                }
                else if (InfoJoueur.Etat == "Invulnerable")
                {
                    partie.isInvincible = true;
                }

                ChargementPartieForm saveloader = (ChargementPartieForm)Application.OpenForms["ChargementPartieForm"];
                modeSurvie = saveloader.modeSurvie;
            } else
            {
                StartPartieForm parametre = (StartPartieForm)Application.OpenForms["StartPartieForm"];
                nomJoueur = parametre.nomJoueur;
                modeSurvie = parametre.modeSurvie;
            }

            AffichageVies();
            AffichageScore();
            InitialisationPartie();
            if (partie.isInvincible)
            {
                Pacman.PacmanPC.BackColor = Color.Blue;
                partie.remainingInvincibilityTime += 5000; // Ajout de 5 secondes d'invincibilité
                StartInvincibilityTimer();
            }
        }

        private void OnRestartGame(object sender, EventArgs e)
        {
            partie.gameover = false;
            Pacman.nbVies = 3;
            partie.score = 0;
            if (!modeSurvie)
                partie.mondeActuel = 1;

            Show();
            gameOverForm.Hide();
            partie.Verification();
            AffichageVies();
            AffichageScore();
            InitialisationPartie();
        }

        private void AskSauvegarderPartie() {

            DialogResult sauvegarder = MessageBox.Show("Voulez-vous sauvegarder la partie ?", "Sauvegarder ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sauvegarder == DialogResult.Yes)
            {
                partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
            }
            else
            {
                MessageBox.Show("Partie non sauvegardée !");
            }
        }

        private void PartieForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((!partie.gameover) && (PauseStatus != 3))
            {
                AskSauvegarderPartie();
            }
        }
    }
}