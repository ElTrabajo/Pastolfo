﻿namespace Pacman_SAE
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start_partie = new System.Windows.Forms.Button();
            this.button_load_partie = new System.Windows.Forms.Button();
            this.button_classement = new System.Windows.Forms.Button();
            this.label_titre = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_start_partie
            // 
            this.button_start_partie.Location = new System.Drawing.Point(366, 246);
            this.button_start_partie.Name = "button_start_partie";
            this.button_start_partie.Size = new System.Drawing.Size(247, 42);
            this.button_start_partie.TabIndex = 1;
            this.button_start_partie.Text = "Commencer une partie";
            this.button_start_partie.UseVisualStyleBackColor = true;
            this.button_start_partie.Click += new System.EventHandler(this.button_start_partie_Click);
            // 
            // button_load_partie
            // 
            this.button_load_partie.Location = new System.Drawing.Point(366, 342);
            this.button_load_partie.Name = "button_load_partie";
            this.button_load_partie.Size = new System.Drawing.Size(247, 42);
            this.button_load_partie.TabIndex = 2;
            this.button_load_partie.Text = "Charger une partie";
            this.button_load_partie.UseVisualStyleBackColor = true;
            // 
            // button_classement
            // 
            this.button_classement.Location = new System.Drawing.Point(366, 438);
            this.button_classement.Name = "button_classement";
            this.button_classement.Size = new System.Drawing.Size(247, 42);
            this.button_classement.TabIndex = 3;
            this.button_classement.Text = "Classement";
            this.button_classement.UseVisualStyleBackColor = true;
            // 
            // label_titre
            // 
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(245, 52);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(489, 107);
            this.label_titre.TabIndex = 4;
            this.label_titre.Text = "Pac-Stolfo";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(979, 640);
            this.Controls.Add(this.label_titre);
            this.Controls.Add(this.button_classement);
            this.Controls.Add(this.button_load_partie);
            this.Controls.Add(this.button_start_partie);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pacman";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_start_partie;
        private System.Windows.Forms.Button button_load_partie;
        private System.Windows.Forms.Button button_classement;
        private System.Windows.Forms.Label label_titre;
    }
}

