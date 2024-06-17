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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            GererClicBouton(button_start_partie, button_load_partie, button_classement);
        }

        private void GererClicBouton(Button button_start_partie, Button button_load_partie, Button button_classement)
        {
            // Événement Click pour chaque bouton à l'aide d'une boucle foreach
            foreach (var bouton in new Button[] { button_start_partie, button_load_partie, button_classement })
            {
                bouton.Click += (sender, e) =>
                {
                    Button boutonClique = sender as Button;

                    if (boutonClique != null)
                    {
                        switch (boutonClique)
                        {
                            case Button btn when btn == button_start_partie:
                                Form startPartieForm = new StartPartieForm();
                                startPartieForm.FormClosed += (s, args) => this.Close();
                                startPartieForm.Show();
                                this.Hide();
                                break;
                            case Button btn when btn == button_load_partie:
                                Form chargementPartieForm = new ChargementPartieForm();
                                chargementPartieForm.FormClosed += (s, args) => this.Close();
                                chargementPartieForm.Show();
                                this.Hide();
                                break;
                            case Button btn when btn == button_classement:
                                Form classementForm = new ClassementForm();
                                classementForm.FormClosed += (s, args) => this.Close();
                                classementForm.Show();
                                this.Hide();
                                break;
                        }
                    }
                };
            }
        }
    }
}
