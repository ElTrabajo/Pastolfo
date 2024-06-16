using InfoJoueurSQL;
using InfoClassementSQL;
using Pastolfo_interface;
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

        private bool isInvincible = false;
        private const byte step = 5;
        private const int colonnes = 16;
        private const int lignes = 16;
        private const int cellSize = 35;
        private int nbPoints = 0;
        private int remainingInvincibilityTime = 0;


        public Labyrinthe labyrinthe = new Labyrinthe(colonnes, lignes);
        public int NiveauActuel { get; set; } = 1;
        public string nomJoueur { get; set; }

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

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
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

            AffichageVies();
            AffichageScore();

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

                    if (partie.points.Count() < (colonnes * lignes) - partie.ListeCoordonees.Count())
                    {
                        iniPoint(x, y);
                    }
                    else
                    {
                        if((y * 16 + x) < partie.points.Count()) {
                            if(!partie.points[(y * 16 + x)].Visible) {
                                partie.points[(y * 16 + x)].Visible = true;
                            nbPoints++;
                            }
                        }
                        
                    }
                    

                    if (Case.MurGauche)
                    {
                        PictureBox murGauche = new PictureBox();

                        if (NiveauActuel == 2)
                        {
                            murGauche.BackColor = Color.White;
                        } else
                        {
                            murGauche.BackColor = Color.Black;
                        }
                        murGauche.Width = 2;
                        murGauche.Height = cellSize;
                        murGauche.Location = new Point(x * cellSize + 210, y * cellSize + 20);
                        partie.Mur.Add(murGauche);
                        this.Controls.Add(murGauche);

                    }

                    if (Case.MurDroite)
                    {
                        PictureBox murDroite = new PictureBox();
                        if (NiveauActuel == 2)
                        {
                            murDroite.BackColor = Color.White;
                        }
                        else
                        {
                            murDroite.BackColor = Color.Black;
                        }
                        murDroite.Width = 2;
                        murDroite.Height = cellSize;
                        murDroite.Location = new Point((x + 1) * cellSize + 210, y * cellSize + 20);
                        partie.Mur.Add(murDroite);
                        this.Controls.Add(murDroite);
                    }

                    if (Case.MurHaut)
                    {
                        PictureBox murHaut = new PictureBox();
                        if (NiveauActuel == 2)
                        {
                            murHaut.BackColor = Color.White;
                        }
                        else
                        {
                            murHaut.BackColor = Color.Black;
                        }
                        murHaut.Width = cellSize;
                        murHaut.Height = 2;
                        murHaut.Location = new Point(x * cellSize + 210, y * cellSize + 20);
                        partie.Mur.Add(murHaut);
                        this.Controls.Add(murHaut);
                    }

                    if (Case.MurBas)
                    {
                        PictureBox murBas = new PictureBox();
                        if (NiveauActuel == 2)
                        {
                            murBas.BackColor = Color.White;
                        }
                        else
                        {
                            murBas.BackColor = Color.Black;
                        }
                        murBas.Width = cellSize;
                        murBas.Height = 2;
                        murBas.Location = new Point(x * cellSize + 210, (y + 1) * cellSize + 20);
                        partie.Mur.Add(murBas);
                        this.Controls.Add(murBas);
                    }

                }

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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
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
            bool touche = false, gameover = false;
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
                GameOverForm gameOverForm = new GameOverForm();
                gameOverForm.FormClosed += (s, e) => this.Close();
                gameOverForm.Show();
                this.Hide();
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
            if (!partie.ListeCoordonees.Contains((y, x)))
            {
                Image PointIcon = Properties.Resources.point;
                PictureBox point = new PictureBox();
                nbPoints++;
                point.Image = PointIcon;
                point.BorderStyle = BorderStyle.FixedSingle;
                point.BackColor = Color.Transparent;
                point.SizeMode = PictureBoxSizeMode.Zoom;
                point.Height = cellSize - 10;
                point.Width = cellSize - 10;
                point.Location = new Point(x * cellSize + 215, y * cellSize + 25);
                point.Name = Convert.ToString(nbPoints);
                partie.points.Add(point);
                this.Controls.Add(point);
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
                foreach(var fruit in partie.listeFruits)
                {
                    fruit.Item2.Visible = true;
                }
            }

        }

        private void iniPacGomme()
        {
            if(partie.ListePacGommes.Count != 4) { 
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
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
        }


        private Image[] ChoixAssetEnnemis()
        {
            Image[] assetEnnemis = new Image[4];
            switch (NiveauActuel)
            {
                case 1:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string imageName = $"_0_enemy_{i + 1}"; // Store the image name
                            assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string imageName = $"_1_enemy_{i + 1}"; // Store the image name
                            assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                        }
                        break;
                    }
                case 3:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string imageName = $"_2_enemy_{i + 1}"; // Store the image name
                            assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                        }
                        break;
                    }
                case 4:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string imageName = $"_3_enemy_{i + 1}"; // Store the image name
                            assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                        }
                        break;
                    }
                case 5:
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string imageName = $"_4_enemy_{i + 1}"; // Store the image name
                            assetEnnemis[i] = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                        }
                        break;
                    }
            }
            return assetEnnemis;
        }

        private void iniMusique()
        {
            switch (NiveauActuel)
            {
                case 1:
                    BackgroundMusique = new SoundPlayer(Properties.Resources._0_audio);
                    break;
                case 2:
                    BackgroundMusique = new SoundPlayer(Properties.Resources._1_audio);
                    break;
                case 3:
                    BackgroundMusique = new SoundPlayer(Properties.Resources._2_audio);
                    break;
                case 4:
                    BackgroundMusique = new SoundPlayer(Properties.Resources._3_audio);
                    break;
                case 5:
                    BackgroundMusique = new SoundPlayer(Properties.Resources._4_audio);
                    break;
            }
            BackgroundMusique.PlayLooping();
        }

        private Image ChoixAssetFruits()
        {
            Image assetFruit = Properties.Resources._0_object_2; // Valeur par défault
            switch (NiveauActuel)
            {
                case 1:
                    assetFruit = Properties.Resources._0_object_2;
                    break;
                case 2:
                    assetFruit = Properties.Resources._1_object_2;
                    break;
                case 3:
                    assetFruit = Properties.Resources._2_object_2;
                    break;
                case 4:
                    assetFruit = Properties.Resources._3_object_2;
                    break;
                case 5:
                    assetFruit = Properties.Resources._4_object_2;
                    break;
            }
            return assetFruit;
        }

        private Image ChoixAssetPacGomme()
        {
            Image assetPacGomme = Properties.Resources._0_object_1;
            switch (NiveauActuel)
            {
                case 1:
                    assetPacGomme = Properties.Resources._0_object_1;
                    break;
                case 2:
                    assetPacGomme = Properties.Resources._1_object_1;
                    break;
                case 3:
                    assetPacGomme = Properties.Resources._2_object_1;
                    break;
                case 4:
                    assetPacGomme = Properties.Resources._3_object_1;
                    break;
                case 5:
                    assetPacGomme = Properties.Resources._4_object_1;
                    break;
            }
            return assetPacGomme;
        }

        private Image ChoixAssetBackGround()
        {
            Image assetBackGround = Properties.Resources._0_background;
            switch (NiveauActuel)
            {
                case 1:
                    assetBackGround = Properties.Resources._0_background;
                    break;
                case 2:
                    assetBackGround = Properties.Resources._1_background;
                    break;
                case 3:
                    assetBackGround = Properties.Resources._2_background;
                    break;
                case 4:
                    assetBackGround = Properties.Resources._3_background;
                    break;
                case 5:
                    assetBackGround = Properties.Resources._4_background;
                    break;
            }
            return assetBackGround;
        }

        private void replacerFantome()
        {
            for (int i = 0; i < 4; i++)
            {
                int x = partie.ListeCoordonees[i + 1].Item1;
                int y = partie.ListeCoordonees[i + 1].Item2;
                partie.ListeEnnemis[i].Item2.Location = new Point(y * cellSize + 255, x * cellSize + 55);
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

        void PassageNiveau()
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

            NiveauActuel++;
            nbPoints = 0;
            labyrinthe.init();
            verification();
            afficher();
            GC.Collect();

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
                    PassageNiveau();
                }

            }

        }
        private void MovementTimerFantome_Tick(object sender, EventArgs e)
        {
            {
                foreach (var (fantome, picturebox) in partie.ListeEnnemis)
                {
                    fantome.timer += aleatoire.Next(0, 10);
                    if ((fantome.timer >= 100 && fantome.deplacement != "stopped") || fantome.deplacement == "stopped")
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
                partie.lblScore.Text = $"Score : {Convert.ToString(partie.score)}";
            } else
            {
                StartPartieForm parametre = (StartPartieForm)Application.OpenForms["StartPartieForm"];
                nomJoueur = parametre.nomJoueur;
            }
            afficher();
        }

        private void PartieForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string JoueurNom = nomJoueur;
            int JoueurScore = partie.score;
            int JoueurNbVies = Pacman.nbVies;
            string JoueurEtat;
            if (isInvincible == false) {
                JoueurEtat = "Vulnerable";
            } else {
                JoueurEtat = "Invulnerable";
            }
            int JoueurIdMonde = NiveauActuel;

            DialogResult sauvegarder = MessageBox.Show("Voulez-vous sauvegarder la partie ?", "Sauvegarder ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sauvegarder == DialogResult.Yes)
            {
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
            else
            {
                MessageBox.Show("Partie non sauvegardée !");
            }
        }
    }
}