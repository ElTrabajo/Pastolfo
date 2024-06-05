using InfoJoueurSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pastolfo_interface
{
    public partial class PartieForm : Form
    {

        private int step = 5;
        bool appui = false;
        static int colonnes = 16;
        static int lignes = 16;
        public int nbPoints = 0;
        public int nbVies = 3;
        int timerFantome = 10;
        private int remainingInvincibilityTime = 0;
        private bool isInvincible = false;
        int score = 0;
        int cellSize = 30;

        public Label lblScore = new Label();
        public Label lblVies = new Label();
        Label remplissage = new Label();
        public GenerateurDeLabyrinthe labyrinthe = new GenerateurDeLabyrinthe(colonnes, lignes);
        Pacman Pacman = new Pacman();
        Random aleatoire = new Random();

        PictureBox PacmanPC = new PictureBox();


        private readonly Timer movementTimer;

        private readonly Timer movementTimerFantome;

        private List<PictureBox> points = new List<PictureBox>();

        private List<PictureBox> MurVer = new List<PictureBox>();

        private List<PictureBox> MurHor = new List<PictureBox>();

        private List<(entite, PictureBox)> listeFruits = new List<(entite, PictureBox)>();

        private List<(entite, PictureBox)> ListeEnnemis = new List<(entite, PictureBox)>();

        private List<(entite, PictureBox)> ListePacGommes = new List<(entite, PictureBox)>();

        private List<(int, int)> ListeCoordonees = new List<(int, int)>();
        public InfoJoueur InfoJoueur { get; set; }

        public PartieForm()
        {
            InitializeComponent();

            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            verification();
            afficher();

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


            remplissage.Font = new Font("Arial", 14);
            remplissage.Text = "Score :";
            remplissage.AutoSize = true;
            remplissage.Location = new Point(500, 600);

            lblScore.Font = new Font("Arial", 14); // Font and size
            lblScore.Text = Convert.ToString(score);
            lblScore.Location = new Point(570, 600);
            lblScore.AutoSize = true;
            this.Controls.Add(remplissage);
            this.Controls.Add(lblScore);

            lblVies.Font = new Font("Arial", 14);
            lblVies.Text = Convert.ToString(nbVies);
            lblVies.Location = new Point(700, 600);
            lblVies.AutoSize = true;
            this.Controls.Add(lblVies);

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

            for (int i = 0; i < 4; i++)
            {
                if (ListeEnnemis.Count < 4)
                {
                    iniFantome();
                }
                else
                {
                    replacerFantome();
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if(listeFruits.Count < 4) { 
                iniFruits();
                }
                else
                {
                    listeFruits[i].Item2.Visible = true;
                }

                if(ListePacGommes.Count < 4)
                {
                    iniPacGomme();
                }
                else
                {
                    ListePacGommes[i].Item2.Visible = false;
                }
                
            }

            for (int y = 0; y < labyrinthe.hauteur; y++)
            {
                for (int x = 0; x < labyrinthe.largeur; x++)
                {

                    Sommet Case = labyrinthe.grille[y, x];

                    /*PictureBox truc = new PictureBox();
                    truc.Name = $"pictureboxcase{y}{x}";
                    truc.Visible = false;
                    truc.Height = cellSize ;
                    truc.Width = cellSize;
                    truc.Location = new Point(x * cellSize + 400, y * cellSize + 100);
                    var coordonées = new Tuple<PictureBox, int, int>(truc, y, x);
                    trigger.Add(coordonées);
                    this.Controls.Add(truc);*/

                    if (points.Count < colonnes * lignes)
                    {
                        iniPoint(x, y);
                    }
                    else
                    {
                        foreach (PictureBox p in points)
                        {
                            p.Visible = true;
                        }
                    }

                    if (Case.MurGauche)
                    {
                        PictureBox murGauche = new PictureBox();

                        murGauche.BackColor = Color.Black;
                        murGauche.Width = 2;
                        murGauche.Height = cellSize;
                        murGauche.Location = new Point(x * cellSize + 400, y * cellSize + 100);
                        MurVer.Add(murGauche);
                        this.Controls.Add(murGauche);

                    }

                    if (Case.MurDroite)
                    {
                        PictureBox murDroite = new PictureBox();
                        murDroite.BackColor = Color.Black;
                        murDroite.Width = 2;
                        murDroite.Height = cellSize;
                        murDroite.Location = new Point((x + 1) * cellSize + 400, y * cellSize + 100);
                        MurVer.Add(murDroite);
                        this.Controls.Add(murDroite);
                    }

                    if (Case.MurHaut)
                    {
                        PictureBox murHaut = new PictureBox();
                        murHaut.BackColor = Color.Black;
                        murHaut.Width = cellSize;
                        murHaut.Height = 2;
                        murHaut.Location = new Point(x * cellSize + 400, y * cellSize + 100);
                        MurHor.Add(murHaut);
                        this.Controls.Add(murHaut);
                    }

                    if (Case.MurBas)
                    {
                        PictureBox murBas = new PictureBox();
                        murBas.BackColor = Color.Black;
                        murBas.Width = cellSize;
                        murBas.Height = 2;
                        murBas.Location = new Point(x * cellSize + 400, (y + 1) * cellSize + 100);
                        MurHor.Add(murBas);
                        this.Controls.Add(murBas);
                    }
                }
            }
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
            foreach (var point in points)
            {
                if (PacmanPC.Bounds.IntersectsWith(point.Bounds))
                {
                    if (point.Visible == true)
                    {
                        point.Visible = false;
                        nbPoints--;
                        score += 100;
                        lblScore.Text = Convert.ToString(score);
                    }


                    break;
                }
            }
        }

        private bool CheckCollisionWithMurVer(int offsetX)
        {
            Rectangle futurePosition = new Rectangle(PacmanPC.Left + offsetX, PacmanPC.Top, PacmanPC.Width, PacmanPC.Height);
            foreach (var mur in MurVer)
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
            foreach (var mur in MurVer)
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
            Rectangle futurePosition = new Rectangle(PacmanPC.Left, PacmanPC.Top + offsetY, PacmanPC.Width, PacmanPC.Height);
            foreach (var mur in MurHor)
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
            foreach (var mur in MurHor)
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

            foreach (var (fantome, pictureboxfantome) in ListeEnnemis)
            {
                if (PacmanPC.Bounds.IntersectsWith(pictureboxfantome.Bounds))
                {
                    if (isInvincible)
                    {
                        temp = (fantome, pictureboxfantome);
                        break;
                    }
                    else
                    {
                        touche = true;
                        nbVies--;
                        score = 0;
                        lblVies.Text = nbVies.ToString();
                        lblScore.Text = score.ToString();
                        break;
                    }
                }
            }

            if (touche)
            {
                afficher();
            }
            else if (temp.pictureboxfantome != null)
            {
                ListeEnnemis.Remove(temp);

                // Make the ghost invisible for 3 seconds
                temp.pictureboxfantome.Visible = false;
                await Task.Delay(3000);

                // Make the ghost visible again and re-add to the list
                temp.pictureboxfantome.Visible = true;
                ListeEnnemis.Add((temp.fantome, temp.pictureboxfantome));
            }
        }

        private void CheckCollisionFruit()
        {
            foreach (var (fruit, pictureboxfruit) in listeFruits)
            {
                if (pictureboxfruit.Visible==true && PacmanPC.Bounds.IntersectsWith(pictureboxfruit.Bounds))
                {
                    score += 1000;
                    pictureboxfruit.Visible = false;
                    lblScore.Text = Convert.ToString(score);
                }
            }
        }

        private void CheckCollisionPacGomme()
        {
            foreach (var (pacGomme, pictureboxPacGomme) in ListePacGommes)
            {
                if (pictureboxPacGomme.Visible && PacmanPC.Bounds.IntersectsWith(pictureboxPacGomme.Bounds))
                {
                    pictureboxPacGomme.Visible = false;
                    PacmanPC.BackColor = Color.Blue;
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
            PacmanPC.BackColor = Color.Transparent;
            isInvincible = false;
        }

        private void iniPacMan()
        {
            string locationPac = @"C:\Users\UTILISATEUR\Downloads\pngimg.com - pacman_PNG52.png";
            PacmanPC.ImageLocation = locationPac;
            PacmanPC.Location = new Point(8 * cellSize + 405, 8 * cellSize + 105);
            ListeCoordonees.Add((8, 8));
            PacmanPC.SizeMode = PictureBoxSizeMode.StretchImage;
            PacmanPC.Size = new Size(cellSize - 8, cellSize - 8);
            PacmanPC.BackColor = Color.Transparent;
            PacmanPC.BorderStyle = BorderStyle.FixedSingle;
            PacmanPC.BringToFront();
            this.Controls.Add(PacmanPC);
        }

        private void iniFantome()
        {
            entite fantome1 = new entite();
            int x = aleatoire.Next(0, colonnes);
            int y = aleatoire.Next(0, lignes);
            string locationFantome = @"C:\Users\UTILISATEUR\Downloads\png-clipart-ms-pac-man-pac-man-games-casper-ghosts-pink-ghost-s-blue-text.png";
            fantome1.SetCoordonees(x, y);
            ListeCoordonees.Add((y,x));

            PictureBox fantome = new PictureBox();
            ListeEnnemis.Add((fantome1, fantome));

            fantome.Location = new Point(x * cellSize + 405, y * cellSize + 105);
            fantome.ImageLocation = locationFantome;
            fantome.SizeMode = PictureBoxSizeMode.StretchImage;
            fantome.BackColor = Color.Transparent;
            fantome.Size = new Size(cellSize - 8, cellSize - 8);
            fantome.BringToFront();

            this.Controls.Add(fantome);
        }

        private void iniPoint(int x, int y)
        {
            if (!ListeCoordonees.Contains((y, x)))
            {
            string locationPoint = @"C:\Users\UTILISATEUR\Downloads\pngimg.com - coin_PNG36871.png";
            PictureBox point = new PictureBox();
            nbPoints++;
            point.ImageLocation = locationPoint;
            point.SizeMode = PictureBoxSizeMode.Zoom;
            point.Height = cellSize - 10;
            point.Width = cellSize - 10;
            point.Location = new Point(x * cellSize + 405, y * cellSize + 105);
            point.Name = Convert.ToString(nbPoints);
            points.Add(point);
            this.Controls.Add(point);
            }
        }

        private void iniFruits()
        {
            int x, y;
            do
            {
                x = aleatoire.Next(0, colonnes);
                y = aleatoire.Next(0, lignes);
            } 
            while (ListeCoordonees.Contains((y,x)));

            string locationFruit = @"C:\Users\UTILISATEUR\Downloads\Fresh_Strawberry_Fruit_PNG_Clipart.png";
            PictureBox fruitPC = new PictureBox();
            entite fruit = new entite(x,y);
            listeFruits.Add((fruit, fruitPC));
            ListeCoordonees.Add((y,x));
            nbPoints++;
            fruitPC.Visible = true;
            fruitPC.ImageLocation = locationFruit;
            fruitPC.SizeMode = PictureBoxSizeMode.Zoom;
            fruitPC.Height = cellSize - 10;
            fruitPC.Width = cellSize - 10;
            fruitPC.Location = new Point(x * cellSize + 405, y * cellSize + 105);
            points.Add(fruitPC);
            this.Controls.Add(fruitPC);

        }

        private void iniPacGomme()
        {
            int x, y;
            do
            {
                x = aleatoire.Next(0, colonnes);
                y = aleatoire.Next(0, lignes);
            }
            while (ListeCoordonees.Contains((y, x)));

            string locationPacGomme = @"C:\Users\UTILISATEUR\Downloads\1685850.png";
            PictureBox PacGommePC = new PictureBox();
            entite PacGomme = new entite(x, y);
            ListeCoordonees.Add((y, x));
            nbPoints++;
            ListePacGommes.Add((PacGomme, PacGommePC));
            PacGommePC.Visible = true;
            PacGommePC.ImageLocation = locationPacGomme;
            PacGommePC.SizeMode = PictureBoxSizeMode.Zoom;
            PacGommePC.Height = cellSize - 10;
            PacGommePC.Width = cellSize - 10;
            PacGommePC.Location = new Point(x * cellSize + 405, y * cellSize + 105);
            points.Add(PacGommePC);
            this.Controls.Add(PacGommePC);

        }

        private void replacerFantome()
        {
            for (int i = 0; i < 4; i++)
            {
                int x = ListeCoordonees[i + 1].Item1;
                int y = ListeCoordonees[i + 1].Item2;
                ListeEnnemis[i].Item2.Location = new Point(y * cellSize + 405, x * cellSize + 105);
            }
        }


        /*private void MettreAJourPosition()
        {
            foreach(var teub in trigger)
            {
                if (PacmanPC.Bounds.IntersectsWith(teub.Item1.Bounds))
                {
                    Pacman.x = teub.Item3;
                    Pacman.y = teub.Item2;
                    //Console.WriteLine(teub.Item1.Name);
                }
            }
        }*/


        /*private void MovementTimer_Tick(object sender, EventArgs e)
        {
            MettreAJourPosition();
            switch (Pacman.deplacement)
            {
                case "Gauche":
                    if (!labyrinthe.grille[Pacman.y,Pacman.x].MurGauche) {
                        PacmanPC.Left -= step;
                    }
                    break;

                case "Droite":
                    if (!labyrinthe.grille[Pacman.y, Pacman.x].MurDroite)
                    {
                        PacmanPC.Left += step;
                    }
                    break;

                case "Haut":
                    if (!labyrinthe.grille[Pacman.y, Pacman.x].MurHaut)
                    {
                        PacmanPC.Top -= step;
                    }
                    break;

                case "Bas":
                    if (!labyrinthe.grille[Pacman.y, Pacman.x].MurBas)
                    {
                        PacmanPC.Top += step;
                    }
                    break;
            }
        }*/
        private void DeplacementFantomeAlea()
        {
            foreach ((entite, PictureBox) ennemi in ListeEnnemis)
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

        private void MovementTimer_Tick(object sender, EventArgs e)
        {
            bool collisionDetected = false;
            switch (Pacman.deplacement)
            {
                case "Gauche":
                    if (!CheckCollisionWithMurVer(-step))
                    {
                        PacmanPC.Left -= step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Droite":
                    if (!CheckCollisionWithMurVer(step))
                    {
                        PacmanPC.Left += step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Haut":
                    if (!CheckCollisionWithMurHor(-step))
                    {
                        PacmanPC.Top -= step;
                    }
                    else
                    {
                        collisionDetected = true;
                    }
                    break;
                case "Bas":
                    if (!CheckCollisionWithMurHor(step))
                    {
                        PacmanPC.Top += step;
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
                
            }

        }
        private void MovementTimerFantome_Tick(object sender, EventArgs e)
        {
            {
                foreach (var (fantome, picturebox) in ListeEnnemis)
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
                // Use PlayerData to initialize the form controls
                //labelName.Text = PlayerData.Nom;
                labelViesJoueur.Text += $" {InfoJoueur.Vies}";
                //labelCase.Text = $"Case ID: {PlayerData.IdCase}";
                //labelScore.Text = $"Score: {PlayerData.Score}";
                //labelState.Text = $"État: {PlayerData.Etat}";
                //labelWorld.Text = $"Monde: {PlayerData.IdMonde}";
            }
        }
    }
}
