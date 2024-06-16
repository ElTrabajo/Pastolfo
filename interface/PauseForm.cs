using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pastolfo_interface
{
    public partial class PauseForm : Form
    {
        public int PauseStatus { get; set; }

        public PauseForm()
        {
            InitializeComponent();
            GererClicBouton(bouton_continuer, bouton_sauve_et_start, bouton_save_et_exit);
        }

        private void GererClicBouton(Button bouton_continuer, Button bouton_sauve_et_start, Button bouton_sauve_et_exit)
        {
            // Événement Click pour chaque bouton à l'aide d'une boucle foreach
            foreach (var bouton in new Button[] { bouton_continuer, bouton_sauve_et_start, bouton_sauve_et_exit })
            {
                bouton.Click += (sender, e) =>
                {
                    Button boutonClique = sender as Button;

                    if (boutonClique != null)
                    {
                        switch (boutonClique)
                        {
                            case Button btn when btn == bouton_continuer:
                                // Définir la valeur à 1
                                PauseStatus = 1;
                                Close();
                                break;
                            case Button btn when btn == bouton_sauve_et_start:
                                // Définir la valeur à 2
                                PauseStatus = 2;
                                Close();
                                break;
                            case Button btn when btn == bouton_sauve_et_exit:
                                // Définir la valeur à 3
                                PauseStatus = 3;
                                Close();
                                break;
                        }
                    }
                };
            }
        }
    }
}
