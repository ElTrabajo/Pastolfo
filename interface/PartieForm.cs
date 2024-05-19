using InfoJoueurSQL;
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
    public partial class PartieForm : Form
    {
        public InfoJoueur InfoJoueur { get; set; }

        public PartieForm()
        {
            InitializeComponent();
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
