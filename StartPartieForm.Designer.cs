namespace Pacman_SAE
{
    partial class StartPartieForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_titre = new System.Windows.Forms.Label();
            this.label_nomjoueur = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_gamemode = new System.Windows.Forms.Label();
            this.button_startgame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_titre
            // 
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(259, 52);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(489, 107);
            this.label_titre.TabIndex = 5;
            this.label_titre.Text = "Pac-Stolfo";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_nomjoueur
            // 
            this.label_nomjoueur.AutoSize = true;
            this.label_nomjoueur.ForeColor = System.Drawing.SystemColors.Control;
            this.label_nomjoueur.Location = new System.Drawing.Point(428, 223);
            this.label_nomjoueur.Name = "label_nomjoueur";
            this.label_nomjoueur.Size = new System.Drawing.Size(120, 16);
            this.label_nomjoueur.TabIndex = 6;
            this.label_nomjoueur.Text = "NOM DU JOUEUR";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(323, 260);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(321, 22);
            this.textBox1.TabIndex = 7;
            // 
            // label_gamemode
            // 
            this.label_gamemode.AutoSize = true;
            this.label_gamemode.ForeColor = System.Drawing.SystemColors.Control;
            this.label_gamemode.Location = new System.Drawing.Point(429, 339);
            this.label_gamemode.Name = "label_gamemode";
            this.label_gamemode.Size = new System.Drawing.Size(98, 16);
            this.label_gamemode.TabIndex = 8;
            this.label_gamemode.Text = "MODE DE JEU";
            // 
            // button_startgame
            // 
            this.button_startgame.Location = new System.Drawing.Point(383, 567);
            this.button_startgame.Name = "button_startgame";
            this.button_startgame.Size = new System.Drawing.Size(184, 32);
            this.button_startgame.TabIndex = 11;
            this.button_startgame.Text = "Commencer";
            this.button_startgame.UseVisualStyleBackColor = true;
            // 
            // StartPartieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(979, 640);
            this.Controls.Add(this.button_startgame);
            this.Controls.Add(this.label_gamemode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label_nomjoueur);
            this.Controls.Add(this.label_titre);
            this.Name = "StartPartieForm";
            this.Text = "Pacman";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_titre;
        private System.Windows.Forms.Label label_nomjoueur;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_gamemode;
        private System.Windows.Forms.Button button_startgame;
    }
}