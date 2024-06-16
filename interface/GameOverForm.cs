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
    public partial class GameOverForm : Form
    {
        public event EventHandler RestartGame;

        public GameOverForm()
        {
            InitializeComponent();
        }

        private void button_quitter_partie_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_recommencer_partie_Click(object sender, EventArgs e)
        {
            RestartGame?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
