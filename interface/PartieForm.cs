using FreeJoint;
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

        private bool isInvincible = false, gameover = false;
        private const byte step = 5;
        private const int colonnes = 16;
        private const int lignes = 16;
        private const int cellSize = 35;
        private int nbPoints = 0;
        private int remainingInvincibilityTime = 0;
        private static GameOverForm gameOverForm;

        public Labyrinthe labyrinthe = new Labyrinthe(colonnes, lignes);
        public int NiveauActuel { get; set; } = 1;
        public string nomJoueur { get; set; }
        public bool modeSurvie { get; set; }
        public int PauseStatus { get; set; }

        Pacman Pacman = new Pacman();
        Random aleatoire = new Random();
        Partie partie;

        private readonly Timer movementTimer;

        private readonly Timer movementTimerFantome;

        public InfoJoueur InfoJoueur { get; set; }

        private InfoJoueurSQLClass infoJoueurSQL;
        private InfoClassementSQLClass infoClassementSQL;

        private SoundPlayer BackgroundMusique;

        public PartieForm()
        {

            this.partie = new Partie(labyrinthe, nomJoueur);

            InitializeComponent();
            infoJoueurSQL = new InfoJoueurSQLClass();
            infoClassementSQL = new InfoClassementSQLClass();
            verification();

            movementTimer = new Timer
            {
                Interval = 45 // Adjust for desired speed
            };
            movementTimer.Tick += new EventHandler(MovementTimer_Tick);
            movementTimer.Start();

            movementTimerFantome = new Timer
            {
                Interval = 50 // Adjust for desired speed
            };
            movementTimerFantome.Tick += new EventHandler(MovementTimerFantome_Tick);
            movementTimerFantome.Start();
        }

        private void verification()
        {
            for (int y = 0; y < labyrinthe.hauteur; y++)
            {
                for (int x = 0; x < labyrinthe.largeur; x++)
                {
                    Sommet Case = labyrinthe.grille[y, x];

                    if (x > 0 && x + 1 < labyrinthe.largeur) // On regarde si on est dans les limites en largeur
                    {
                        if (labyrinthe.grille[y, x - 1].MurGauche == false) // Si la case d'avant n'a pas de mur gauche
                        {
                            Case.MurDroite = false;
                        }
                        else
                        {
                            if (labyrinthe.grille[y, x - 1].MurDroite == false)
                            {
                                Case.MurGauche = false;
                            }
                            else
                            {
                                if (labyrinthe.grille[y, x + 1].MurGauche == false)
                                {
                                    Case.MurDroite = false;
                                }
                                else
                                {
                                    if (labyrinthe.grille[y, x + 1].MurDroite == false)
                                    {
                                        Case.MurGauche = false;
                                    }
                                }
                            }

                            if (y > 0 && y + 1 < labyrinthe.hauteur) // On regarde si on est dans les limites en hauteur
                            {
                                if (labyrinthe.grille[y - 1, x].MurHaut == false)
                                {
                                    Case.MurBas = false;
                                }
                                else
                                {
                                    if (labyrinthe.grille[y - 1, x].MurBas == false)
                                    {
                                        Case.MurHaut = false;
                                    }
                                    else
                                    {
                                        if (labyrinthe.grille[y + 1, x].MurHaut == false)
                                        {
                                            Case.MurBas = false;
                                        }
                                        else
                                        {
                                            if (labyrinthe.grille[y + 1, x].MurBas == false)
                                            {
                                                Case.MurHaut = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (y > 0 && y + 1 < labyrinthe.hauteur) // On regarde si on est dans les limites en hauteur
                    {
                        if (labyrinthe.grille[y - 1, x].MurHaut == false)
                        {
                            Case.MurBas = false;
                        }
                        else
                        {
                            if (labyrinthe.grille[y - 1, x].MurBas == false)
                            {
                                Case.MurHaut = false;
                            }
                            else
                            {
                                if (labyrinthe.grille[y + 1, x].MurHaut == false)
                                {
                                    Case.MurBas = false;
                                }
                                else
                                {
                                    if (labyrinthe.grille[y + 1, x].MurBas == false)
                                    {
                                        Case.MurHaut = false;
                                    }
                                }
                            }
                        }

                        if (x > 0 && x + 1 < labyrinthe.largeur) // On regarde si on est dans les limites en largeur
                        {
                            if (labyrinthe.grille[y, x - 1].MurGauche == false) // Si la case d'avant n'a pas de mur gauche
                            {
                                Case.MurDroite = false;
                            }
                            else
                            {
                                if (labyrinthe.grille[y, x - 1].MurDroite == false)
                                {
                                    Case.MurGauche = false;
                                }
                                else
                                {
                                    if (labyrinthe.grille[y, x + 1].MurGauche == false)
                                    {
                                        Case.MurDroite = false;
                                    }
                                    else
                                    {
                                        if (labyrinthe.grille[y, x + 1].MurDroite == false)
                                        {
                                            Case.MurGauche = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void afficher()
        {
            iniPacMan();
            iniBackground();
            iniFantome();
            iniMusique();

            for (int i = 0; i <= 4; i++)
            {
                iniFruits();
                iniPacGomme();
            }

            for (int y = 0; y < labyrinthe.hauteur; y++)
            {
                for (int x = 0; x < labyrinthe.largeur; x++)
                {
                    Sommet Case = labyrinthe.grille[y, x];

                    if (partie.points.Count < (colonnes * lignes) - partie.ListeCoordonees.Count)
                    {
                        iniPoint(x, y);
                    }
                    else
                    {
                        if ((y * 16 + x) < partie.points.Count)
                        {
                            if (!partie.points[(y * 16 + x)].Visible)
                            {
                                partie.points[(y * 16 + x)].Visible = true;
                                nbPoints++;
                            }
                        }
                    }

                    AjoutMur(Case.MurGauche, x, y, 2, cellSize, true);
                    AjoutMur(Case.MurDroite, x + 1, y, 2, cellSize, true);
                    AjoutMur(Case.MurHaut, x, y, cellSize, 2, false);
                    AjoutMur(Case.MurBas, x, y + 1, cellSize, 2, false);
                }
            }
        }

        private void AjoutMur(bool possedeMur, int x, int y, int largeur, int hauteur, bool estVertical)
        {
            if (possedeMur)
            {
                PictureBox mur = new PictureBox();
                mur.BackColor = NiveauActuel == 2 ? Color.White : Color.Black;
                mur.Width = largeur;
                mur.Height = hauteur;
                mur.Location = estVertical ? new Point(x * cellSize + 210, y * cellSize + 20)
                                           : new Point(x * cellSize + 210, y * cellSize + 20);
                partie.Mur.Add(mur);
                this.Controls.Add(mur);
            }
        }

        private void AffichageVies()
        {
            for(int i = 0;i<Pacman.nbVies;i++) { 
                PictureBox iconeVie = new PictureBox();
                iconeVie.Name = Convert.ToString($"Vie{i+1}"); ;
                iconeVie.Height = cellSize;
                iconeVie.Width = cellSize;
                iconeVie.Image = Properties.Resources.vie;
                iconeVie.BackColor = Color.Transparent;
                iconeVie.BringToFront();
                iconeVie.SizeMode = PictureBoxSizeMode.StretchImage;
                iconeVie.Location = new Point(600+(i*50), 595);
                this.Controls.Add(iconeVie);
            }
        }

        private void AffichageScore()
        {
            partie.lblScore.Font = new Font("Arial", 14); // Font and size
            partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
            partie.lblScore.Location = new Point(300, 600);
            partie.lblScore.AutoSize = true;
            partie.lblScore.BackColor = Color.Transparent;
            this.Controls.Add(partie.lblScore);
        }

        private void PartieForm_KeyDown(object sender, KeyEventArgs e)
        {
            string Current = Pacman.deplacement;
            if (e.KeyCode == Keys.Left && !CheckCollisionWithMurVer(-step))
            {
                Pacman.SetDeplacement("Gauche");
            }
            else
            {
                if (e.KeyCode == Keys.Right && !CheckCollisionWithMurVer(step))
                {
                    Pacman.SetDeplacement("Droite");
                }
                else
                {
                    if (e.KeyCode == Keys.Down && !CheckCollisionWithMurHor(step))
                    {
                        Pacman.SetDeplacement("Bas");
                    }
                    else
                    {
                        if (e.KeyCode == Keys.Up && !CheckCollisionWithMurHor(-step))
                        {
                            Pacman.SetDeplacement("Haut");
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.P)
            {
                BackgroundMusique.Stop();
                StopDeplacementFantome();

                PauseForm pauseForm = new PauseForm();
                pauseForm.ShowDialog();

                PauseStatus = pauseForm.PauseStatus;
                switch (PauseStatus)
                {
                    case 1:
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                    case 2:
                        SauvegarderPartie();
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                    case 3:
                        SauvegarderPartie();
                        Application.Exit();
                        break;
                    default:
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                }
            }
        }

        private void CheckCollisionWithPoints()
        {
            foreach (var point in partie.points)
            {
                if (Pacman.PacmanPC.Bounds.IntersectsWith(point.Bounds))
                {
                    if (point.Visible == true)
                    {
                        point.Visible = false;
                        nbPoints--;
                        partie.score += 100;
                        partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
                    }
                    break;
                }
            }
        }

        private bool CheckCollisionWithMurVer(int offsetX)
        {
            Rectangle futurePosition = new Rectangle(Pacman.PacmanPC.Left + offsetX, Pacman.PacmanPC.Top, Pacman.PacmanPC.Width, Pacman.PacmanPC.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; // Collision detected
                }
            }
            return false; // No collision
        }

        private bool CheckCollisionWithMurVerF(int offsetX, PictureBox Ennemi)
        {

            Rectangle futurePosition = new Rectangle(Ennemi.Left + offsetX, Ennemi.Top, Ennemi.Width, Ennemi.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; // Collision detected
                }
            }
            return false; // No collision
        }

        private bool CheckCollisionWithMurHor(int offsetY)
        {
            Rectangle futurePosition = new Rectangle(Pacman.PacmanPC.Left, Pacman.PacmanPC.Top + offsetY, Pacman.PacmanPC.Width, Pacman.PacmanPC.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; // Collision detected
                }
            }
            return false; // No collision
        }

        private bool CheckCollisionWithMurHorF(int offsetY, PictureBox Ennemi)
        {
            Rectangle futurePosition = new Rectangle(Ennemi.Left, Ennemi.Top + offsetY, Ennemi.Width, Ennemi.Height);
            foreach (var mur in partie.Mur)
            {
                if (futurePosition.IntersectsWith(mur.Bounds))
                {
                    return true; // Collision detected
                }
            }
            return false; // No collision
        }

        private async void CheckCollisionFantome()
        {
            bool touche = false;
            var temp = (fantome: (entite)null, pictureboxfantome: (PictureBox)null); // Initialize temp

            foreach (var (fantome, pictureboxfantome) in partie.ListeEnnemis)
            {
                if (Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxfantome.Bounds))
                {
                    if (isInvincible)
                    {
                        temp = (fantome, pictureboxfantome);
                        break;
                    }
                    else
                    {
                        touche = true;
                        this.Controls.RemoveByKey($"Vie{Pacman.nbVies}");
                        Pacman.nbVies--;
                        partie.score = 0;
                        partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
                        if (Pacman.nbVies == 0) {
                            gameover = true;
                            touche = false;
                        }
                        break;
                    }
                }
            }

            if (touche)
            {
                afficher();
            }
            else if (gameover)
            {
                ReInitLabyrintheOrGameOver();
            }
            else if (temp.pictureboxfantome != null)
            {
                partie.ListeEnnemis.Remove(temp);

                // Make the ghost invisible for 3 seconds
                temp.pictureboxfantome.Visible = false;
                await Task.Delay(3000);

                // Make the ghost visible again and re-add to the list
                temp.pictureboxfantome.Visible = true;
                partie.ListeEnnemis.Add((temp.fantome, temp.pictureboxfantome));
            }
        }

        private void CheckCollisionFruit()
        {
            foreach (var (fruit, pictureboxfruit) in partie.listeFruits)
            {
                if (pictureboxfruit.Visible == true && Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxfruit.Bounds))
                {
                    partie.score += 1000;
                    pictureboxfruit.Visible = false;
                    partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
                }
            }
        }

        private void CheckCollisionPacGomme()
        {
            foreach (var (pacGomme, pictureboxPacGomme) in partie.ListePacGommes)
            {
                if (pictureboxPacGomme.Visible && Pacman.PacmanPC.Bounds.IntersectsWith(pictureboxPacGomme.Bounds))
                {
                    pictureboxPacGomme.Visible = false;
                    Pacman.PacmanPC.BackColor = Color.Blue;
                    remainingInvincibilityTime += 5000; // Ajout de 5 secondes d'invincibilité

                    if (!isInvincible)
                    {
                        isInvincible = true;
                        StartInvincibilityTimer();
                    }
                }
            }
        }

        private async void StartInvincibilityTimer()
        {
            while (remainingInvincibilityTime > 0)
            {
                await Task.Delay(1000); // Attendre par incréments de 1 seconde
                remainingInvincibilityTime -= 1000;
            }
            EndInvincibility();
        }

        private void EndInvincibility()
        {
            Pacman.PacmanPC.BackColor = Color.Transparent;
            isInvincible = false;
        }

        private void iniPacMan()
        {
            Pacman.PacmanPC.Image = Properties.Resources.astolfo;
            Pacman.PacmanPC.Location = new Point(8 * cellSize + 215, 8 * cellSize + 25);
            partie.ListeCoordonees.Add((8, 8));
            Pacman.PacmanPC.SizeMode = PictureBoxSizeMode.Zoom;
            Pacman.PacmanPC.Size = new Size(cellSize - 5, cellSize - 5);
            Pacman.PacmanPC.BackColor = Color.Transparent;
            Pacman.PacmanPC.BringToFront();
            this.Controls.Add(Pacman.PacmanPC);
        }

        private void iniFantome()
        {
            if (partie.ListeEnnemis.Count != 4) { 
            Image[] asset = ChoixAssetEnnemis();
            foreach (Image skin in asset) {
                entite fantome1 = new entite();

                    int x;
                    int y;
                    do
                    {
                        x = aleatoire.Next(0, colonnes);
                        y = aleatoire.Next(0, lignes);
                    }
                    while (partie.ListeCoordonees.Contains((y, x)));
                    fantome1.SetCoordonees(x, y);

                    partie.ListeCoordonees.Add((y,x));

                PictureBox fantome = new PictureBox();
                partie.ListeEnnemis.Add((fantome1, fantome));

                fantome.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                fantome.Image = skin;
                fantome.SizeMode = PictureBoxSizeMode.StretchImage;
                fantome.BackColor = Color.Transparent;
                fantome.Size = new Size(cellSize - 8, cellSize - 8);
                fantome.BringToFront();

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
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Height = cellSize - 10,
                    Width = cellSize - 10,
                    Location = new Point(x * cellSize + 215, y * cellSize + 25),
                    Name = nbPoints.ToString()
                };

                // Ajouter la PictureBox à la liste des points et au formulaire
                partie.points.Add(point);
                this.Controls.Add(point);

                // Incrémenter le nombre de points
                nbPoints++;
            }
        }

        private void iniFruits()
        {
            if (partie.listeFruits.Count != 4)
            {
                int x;
                int y;
                do
                {
                    x = aleatoire.Next(0, colonnes);
                    y = aleatoire.Next(0, lignes);
                }
                while (partie.ListeCoordonees.Contains((y, x)));

                PictureBox fruitPC = new PictureBox();
                entite fruit = new entite(x, y);
                partie.listeFruits.Add((fruit, fruitPC));
                partie.ListeCoordonees.Add((y, x));

                fruitPC.Image = ChoixAssetFruits();
                fruitPC.SizeMode = PictureBoxSizeMode.StretchImage;
                fruitPC.BackColor = Color.Transparent;
                fruitPC.Height = cellSize - 10;
                fruitPC.Width = cellSize - 10;
                fruitPC.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                fruitPC.BringToFront();
                this.Controls.Add(fruitPC);
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

        private void iniPacGomme()
        {
            if (partie.ListePacGommes.Count != 4)
            {
                int x, y;
                do
                {
                    x = aleatoire.Next(0, colonnes);
                    y = aleatoire.Next(0, lignes);
                }
                while (partie.ListeCoordonees.Contains((y, x)));

                PictureBox PacGommePC = new PictureBox();
                entite PacGomme = new entite(x, y);
                partie.ListeCoordonees.Add((y, x));
                partie.ListePacGommes.Add((PacGomme, PacGommePC));

                PacGommePC.Visible = true;
                PacGommePC.Image = ChoixAssetPacGomme();
                PacGommePC.SizeMode = PictureBoxSizeMode.StretchImage;
                PacGommePC.BackColor = Color.Transparent;
                PacGommePC.Height = cellSize - 10;
                PacGommePC.Width = cellSize - 10;
                PacGommePC.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                this.Controls.Add(PacGommePC);
            }
            else
            {
                // Rendre tous les PacGommes visibles
                foreach (var pacGomme in partie.ListePacGommes)
                {
                    pacGomme.Item2.Visible = true;
                }
            }
        }

        private void iniBackground()
        {
            this.DoubleBuffered = true;
            this.BackgroundImage = ChoixAssetBackGround();
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        }

        private Image[] ChoixAssetEnnemis()
        {
            Image[] assetEnnemis = new Image[4];

            if (NiveauActuel >= 1 && NiveauActuel <= 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    string imageName = $"_{NiveauActuel - 1}_enemy_{i + 1}";
                    assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                }
            }

            return assetEnnemis;
        }

        private void iniMusique()
        {
            string resourceName = $"_{NiveauActuel - 1}_audio";

            try
            {
                BackgroundMusique = new SoundPlayer((Stream)Properties.Resources.ResourceManager.GetObject(resourceName));
                BackgroundMusique.PlayLooping();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement de la musique pour le niveau {NiveauActuel}: {ex.Message}");
                // Gérer l'erreur de chargement de la musique (ex: jouer une autre musique par défaut)
            }
        }

        private Image ChoixAssetFruits()
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_object_2,
                Properties.Resources._1_object_2,
                Properties.Resources._2_object_2,
                Properties.Resources._3_object_2,
                Properties.Resources._4_object_2
            };

            // Valider que NiveauActuel est dans la plage des indices du tableau
            int index = NiveauActuel - 1;
            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où NiveauActuel est hors de la plage attendue
                Console.WriteLine($"NiveauActuel {NiveauActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_object_2; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private Image ChoixAssetPacGomme()
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_object_1,
                Properties.Resources._1_object_1,
                Properties.Resources._2_object_1,
                Properties.Resources._3_object_1,
                Properties.Resources._4_object_1
            };

            // Valider que NiveauActuel est dans la plage des indices du tableau
            int index = NiveauActuel - 1;
            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où NiveauActuel est hors de la plage attendue
                Console.WriteLine($"NiveauActuel {NiveauActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_object_1; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private Image ChoixAssetBackGround()
        {
            Image[] assets = new Image[]
            {
                Properties.Resources._0_background,
                Properties.Resources._1_background,
                Properties.Resources._2_background,
                Properties.Resources._3_background,
                Properties.Resources._4_background
            };

            // Valider que NiveauActuel est dans la plage des indices du tableau
            int index = NiveauActuel - 1;
            if (index >= 0 && index < assets.Length)
            {
                return assets[index];
            }
            else
            {
                // Gérer le cas où NiveauActuel est hors de la plage attendue
                Console.WriteLine($"NiveauActuel {NiveauActuel} n'est pas géré. Retour de l'image par défaut.");
                return Properties.Resources._0_background; // Retourner une image par défaut ou gérer autrement l'erreur
            }
        }

        private void StopDeplacementFantome()
        {
            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis)
            {
                ennemi.Item1.SetDeplacement("Pause");
            }
        }

        private void RestartDeplacementFantome()
        {
            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis)
            {
                ennemi.Item1.SetDeplacement("Stopped");
            }
        }

        private void DeplacementFantomeAlea()
        {
            foreach ((entite, PictureBox) ennemi in partie.ListeEnnemis)
            {
                switch (ennemi.Item1.deplacement)
                {
                    case "Gauche":
                        if (!CheckCollisionWithMurVerF(-step, ennemi.Item2))
                        {
                            ennemi.Item2.Left -= step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped");
                        }
                        break;
                    case "Droite":
                        if (!CheckCollisionWithMurVerF(step, ennemi.Item2))
                        {
                            ennemi.Item2.Left += step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped");
                        }
                        break;
                    case "Haut":
                        if (!CheckCollisionWithMurHorF(-step, ennemi.Item2))
                        {
                            ennemi.Item2.Top -= step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped");
                        }
                        break;
                    case "Bas":
                        if (!CheckCollisionWithMurHorF(step, ennemi.Item2))
                        {
                            ennemi.Item2.Top += step;
                            ennemi.Item2.BringToFront();
                        }
                        else
                        {
                            ennemi.Item1.SetDeplacement("Stopped");
                        }
                        break;
                }
            }
        }

        void ReInitLabyrintheOrGameOver()
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
            nbPoints = 0;

            if (gameover)
            {
                BackgroundMusique.Stop();

                // Créer et afficher un nouveau formulaire Game Over
                if (gameOverForm == null)
                {
                    gameOverForm = new GameOverForm();
                    gameOverForm.StartPosition = this.StartPosition;
                    gameOverForm.RestartGame += OnRestartGame;
                }
                gameOverForm.Show();
                this.Hide(); // Cacher la forme principale
            } else {
                if (!modeSurvie) {
                    NiveauActuel++;
                }
                verification();
                afficher();
                GC.Collect();
            }
        }

        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            bool collisionDetected = false;
            switch (Pacman.deplacement)
            {
                case "Gauche":
                    if (!CheckCollisionWithMurVer(-step))
                    {
                        Pacman.PacmanPC.Left -= step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Droite":
                    if (!CheckCollisionWithMurVer(step))
                    {
                        Pacman.PacmanPC.Left += step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Haut":
                    if (!CheckCollisionWithMurHor(-step))
                    {
                        Pacman.PacmanPC.Top -= step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Bas":
                    if (!CheckCollisionWithMurHor(step))
                    {
                        Pacman.PacmanPC.Top += step;
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
                CheckCollisionFruit();
                CheckCollisionPacGomme();
                CheckCollisionWithPoints();
                CheckCollisionFantome();
                if ((nbPoints == 0) && (NiveauActuel != 5))
                {
                    ReInitLabyrintheOrGameOver();
                }
            }
        }
        private void MovementTimerFantome_Tick(object sender, EventArgs e)
        {
            {
                foreach (var (fantome, picturebox) in partie.ListeEnnemis)
                {
                    fantome.timer += aleatoire.Next(0, 10);
                    if ((fantome.timer >= 100 && fantome.deplacement != "stopped" && fantome.deplacement != "Pause") || fantome.deplacement == "stopped")
                    {
                        string[] directions = { "Gauche", "Droite", "Haut", "Bas" };
                        int indexAleatoire = aleatoire.Next(0, directions.Length);
                        fantome.SetDeplacement(directions[indexAleatoire]);
                        fantome.timer = 0;
                    }
                }
                DeplacementFantomeAlea();
                CheckCollisionFantome();
            }
        }

        private void PartieForm_Load(object sender, EventArgs e)
        {
            if (InfoJoueur != null)
            {
                Pacman.nbVies = InfoJoueur.Vies;
                nomJoueur = InfoJoueur.Nom;
                partie.score = InfoJoueur.Score;
                NiveauActuel = InfoJoueur.IdMonde;
                if (InfoJoueur.Etat == "Vulnerable")
                {
                    isInvincible = false;
                }
                else if (InfoJoueur.Etat == "Invulnerable")
                {
                    isInvincible = true;
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
            afficher();
            if (isInvincible)
            {
                Pacman.PacmanPC.BackColor = Color.Blue;
                remainingInvincibilityTime += 5000; // Ajout de 5 secondes d'invincibilité
                StartInvincibilityTimer();
            }
        }

        private void OnRestartGame(object sender, EventArgs e)
        {
            gameover = false;
            Pacman.nbVies = 3;
            partie.score = 0;
            if (!modeSurvie)
                NiveauActuel = 1;

            Show();
            gameOverForm.Hide();
            verification();
            AffichageVies();
            AffichageScore();
            afficher();
        }

        private void SauvegarderPartie()
        {
            string JoueurNom = nomJoueur;
            int JoueurScore = partie.score;
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
            int JoueurIdMonde = NiveauActuel;

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

        private void AskSauvegarderPartie() {

            DialogResult sauvegarder = MessageBox.Show("Voulez-vous sauvegarder la partie ?", "Sauvegarder ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sauvegarder == DialogResult.Yes)
            {
                SauvegarderPartie();
            }
            else
            {
                MessageBox.Show("Partie non sauvegardée !");
            }
        }

        private void PartieForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((!gameover) && (PauseStatus != 3))
            {
                AskSauvegarderPartie();
            }
        }
    }
}