using System.Drawing.Printing;
using System.Drawing;
using System.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Pastolfo_interface
{
    partial class FinForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblFinDePartie = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.buttonQuitter = new System.Windows.Forms.Button();
            this.buttonRecommencer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::Pastolfo_interface.Properties.Resources.astolfo_plush;
            this.pictureBox1.Location = new System.Drawing.Point(65, 285);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 93);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            // 
            // lblFinDePartie
            // 
            this.lblFinDePartie.AutoSize = true;
            this.lblFinDePartie.BackColor = System.Drawing.Color.Transparent;
            this.lblFinDePartie.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold);
            this.lblFinDePartie.ForeColor = System.Drawing.Color.Black;
            this.lblFinDePartie.Location = new System.Drawing.Point(120, 83);
            this.lblFinDePartie.Name = "lblFinDePartie";
            this.lblFinDePartie.Size = new System.Drawing.Size(516, 86);
            this.lblFinDePartie.TabIndex = 1;
            this.lblFinDePartie.Text = "Fin de la partie";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.lblScore.Location = new System.Drawing.Point(167, 222);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(125, 37);
            this.lblScore.TabIndex = 2;
            this.lblScore.Text = "Score :";
            // 
            // buttonQuitter
            // 
            this.buttonQuitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.buttonQuitter.ForeColor = System.Drawing.Color.Black;
            this.buttonQuitter.Location = new System.Drawing.Point(517, 420);
            this.buttonQuitter.Name = "buttonQuitter";
            this.buttonQuitter.Size = new System.Drawing.Size(119, 34);
            this.buttonQuitter.TabIndex = 3;
            this.buttonQuitter.Text = "Quitter";
            this.buttonQuitter.UseVisualStyleBackColor = true;
            // 
            // buttonRecommencer
            // 
            this.buttonRecommencer.BackColor = System.Drawing.Color.Black;
            this.buttonRecommencer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.buttonRecommencer.Location = new System.Drawing.Point(103, 420);
            this.buttonRecommencer.Name = "buttonRecommencer";
            this.buttonRecommencer.Size = new System.Drawing.Size(119, 34);
            this.buttonRecommencer.TabIndex = 4;
            this.buttonRecommencer.Text = "Recommencer";
            this.buttonRecommencer.UseVisualStyleBackColor = true;
            // 
            // FinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Pastolfo_interface.Properties.Resources.fin_background;
            this.ClientSize = new System.Drawing.Size(738, 524);
            this.Controls.Add(this.buttonRecommencer);
            this.Controls.Add(this.buttonQuitter);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblFinDePartie);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FinForm";
            this.Text = "Pac-Stolfo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private Label lblFinDePartie;
        private Label lblScore;
        private Button buttonQuitter;
        private Button buttonRecommencer;
    }
}
