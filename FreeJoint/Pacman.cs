using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeJoint
{
    public class Pacman
    {
        public int x;
        public int y;
        public string deplacement;
        public string etat;
        public int nbVies;
        public PictureBox PacmanPC;

        public Pacman()
        {
            x = 8;
            y = 8;
            deplacement = "stopped";
            nbVies = 3;
            PacmanPC = new PictureBox();
        }

        public void SetDeplacement(string deplacement2)
        {
            deplacement = deplacement2;
        }
    }
}
