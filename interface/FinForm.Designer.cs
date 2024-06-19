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
            this.buttonQuitter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::Pastolfo_interface.Properties.Resources.astolfo_plush;
            this.pictureBox1.Location = new System.Drawing.Point(87, 351);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.lblFinDePartie.Location = new System.Drawing.Point(160, 102);
            this.lblFinDePartie.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFinDePartie.Name = "lblFinDePartie";
            this.lblFinDePartie.Size = new System.Drawing.Size(645, 106);
            this.lblFinDePartie.TabIndex = 1;
            this.lblFinDePartie.Text = "Fin de la partie";
            // 
            // buttonQuitter
            // 
            this.buttonQuitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.buttonQuitter.ForeColor = System.Drawing.Color.Black;
            this.buttonQuitter.Location = new System.Drawing.Point(413, 517);
            this.buttonQuitter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonQuitter.Name = "buttonQuitter";
            this.buttonQuitter.Size = new System.Drawing.Size(159, 42);
            this.buttonQuitter.TabIndex = 3;
            this.buttonQuitter.Text = "Quitter";
            this.buttonQuitter.UseVisualStyleBackColor = true;
            this.buttonQuitter.Click += new System.EventHandler(this.buttonQuitter_Click);
            // 
            // FinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Pastolfo_interface.Properties.Resources.fin_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 645);
            this.Controls.Add(this.buttonQuitter);
            this.Controls.Add(this.lblFinDePartie);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FinForm";
            this.Text = "Pac-Stolfo";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        private Label lblFinDePartie;
        private Button buttonQuitter;
    }
}
