using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AideJeu
{
    public class entite
    {
        public int x;
        public int y;
        public string deplacement;
        public int timer = 0;

        public entite()
        {
            deplacement = "stopped";
        }

        public entite(int x1, int y1)
        {
            x = x1;
            y = y1;
            deplacement = "stopped";
        }

        public void SetDeplacement(string deplacement2)
        {
            deplacement = deplacement2;
        }

        public void SetCoordonees(int x1, int y1)
        {
            x = x1;
            y = y1;
        }

        ~entite()
        {

        }
    }
}
