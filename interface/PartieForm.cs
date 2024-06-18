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
        private const int colonnes = 16;
        private const int lignes = 16;
        private const int cellSize = 35;

        private static GameOverForm gameOverForm;

        public Labyrinthe labyrinthe = new Labyrinthe(colonnes, lignes);

        public string nomJoueur { get; set; }
        public bool modeSurvie { get; set; }
        public int PauseStatus { get; set; }

        Pacman Pacman = new Pacman();
        Random aleatoire = new Random();
        Partie partie;
        public InfoJoueur InfoJoueur { get; set; }

        private readonly Timer movementTimer;

        private readonly Timer movementTimerFantome;

        private SoundPlayer BackgroundMusique;

        public PartieForm()
        {

            this.partie = new Partie(labyrinthe, nomJoueur);

            InitializeComponent();
            partie.Verification(); // Vérifie les murs du labyrinthe

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
                                partie.nbPoints++;
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
                mur.BackColor = partie.mondeActuel == 2 ? Color.White : Color.Black;
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
            partie.lblScore.ForeColor = Color.White;
            partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
            partie.lblScore.Location = new Point(300, 600);
            partie.lblScore.AutoSize = true;
            partie.lblScore.BackColor = Color.Transparent;
            this.Controls.Add(partie.lblScore);
        }

        private void PartieForm_KeyDown(object sender, KeyEventArgs e)
        {
            string Current = Pacman.deplacement;
            int step = Partie.step;

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
                        partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
                        BackgroundMusique.PlayLooping();
                        RestartDeplacementFantome();
                        break;
                    case 3:
                        partie.SauvegarderPartie(nomJoueur, Pacman, InfoJoueur);
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
                        partie.nbPoints--;
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
                    if (partie.isInvincible)
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
                            partie.gameover = true;
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
            else if (partie.gameover)
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
                    partie.remainingInvincibilityTime += 5000; // Ajout de 5 secondes d'invincibilité

                    if (!partie.isInvincible)
                    {
                        partie.isInvincible = true;
                        StartInvincibilityTimer();
                    }
                }
            }
        }

        private async void StartInvincibilityTimer()
        {
            while (partie.remainingInvincibilityTime > 0)
            {
                await Task.Delay(1000); // Attendre par incréments de 1 seconde
                partie.remainingInvincibilityTime -= 1000;
            }

            EndInvincibility();
        }

        private void EndInvincibility()
        {
            Pacman.PacmanPC.BackColor = Color.Transparent;
            partie.isInvincible = false;
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
                fruitPC.BorderStyle = BorderStyle.None;
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
                PacGommePC.BorderStyle = BorderStyle.None;
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

            if (partie.mondeActuel >= 1 && partie.mondeActuel <= 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    string imageName = $"_{partie.mondeActuel - 1}_enemy_{i + 1}";
                    assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                }
            }

            return assetEnnemis;
        }

        private void iniMusique()
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
            int step = Partie.step;

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
            partie.nbPoints = 0;

            if (partie.gameover)
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
                    partie.mondeActuel++;
                }

                partie.Verification();
                afficher();
                GC.Collect();
            }
        }

        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            bool collisionDetected = false;
            int step = Partie.step;

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
                if ((partie.nbPoints == 0) && (partie.mondeActuel != 5))
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
            afficher();

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
            afficher();
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