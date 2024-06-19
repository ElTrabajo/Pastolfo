using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Pastolfo_interface.Properties;

namespace Pastolfo_interface
{
    public partial class FinForm : Form
    {
        private int dx = 5; // Change in x-axis
        private int dy = 5; // Change in y-axis
        private SoundPlayer simpleSound;

        public FinForm()
        {
            InitializeComponent();
            playSimpleSound();

            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen

            // Enable double buffering to reduce flicker
            this.DoubleBuffered = true;

            // Start the timer
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Make sure the PictureBox is initially brought to front
            pictureBox1.BringToFront();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Move the PictureBox
            pictureBox1.Left += dx;
            pictureBox1.Top += dy;

            // Check for collision with the form's boundaries and reverse direction if necessary
            if (pictureBox1.Left <= 0 || pictureBox1.Right >= this.ClientSize.Width)
            {
                dx = -dx;
            }
            if (pictureBox1.Top <= 0 || pictureBox1.Bottom >= this.ClientSize.Height)
            {
                dy = -dy;
            }

            // Force the form to repaint, reducing flicker
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void playSimpleSound()
        {
            simpleSound = new SoundPlayer(Resources.fin);
            simpleSound.PlayLooping();
        }

        private void buttonQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
