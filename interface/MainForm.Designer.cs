namespace Pastolfo_interface
{
    partial class MainForm
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
            this.button_start_partie.Location = new System.Drawing.Point(274, 200);
            this.button_start_partie.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_start_partie.Name = "button_start_partie";
            this.button_start_partie.Size = new System.Drawing.Size(185, 34);
            this.button_start_partie.TabIndex = 1;
            this.button_start_partie.Text = "Commencer une partie";
            this.button_start_partie.UseVisualStyleBackColor = true;
            this.button_start_partie.Click += new System.EventHandler(this.button_start_partie_Click);
            // 
            // button_load_partie
            // 
            this.button_load_partie.Location = new System.Drawing.Point(274, 278);
            this.button_load_partie.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_load_partie.Name = "button_load_partie";
            this.button_load_partie.Size = new System.Drawing.Size(185, 34);
            this.button_load_partie.TabIndex = 2;
            this.button_load_partie.Text = "Charger une partie";
            this.button_load_partie.UseVisualStyleBackColor = true;
            this.button_load_partie.Click += new System.EventHandler(this.button_load_partie_Click);
            // 
            // button_classement
            // 
            this.button_classement.Location = new System.Drawing.Point(274, 356);
            this.button_classement.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_classement.Name = "button_classement";
            this.button_classement.Size = new System.Drawing.Size(185, 34);
            this.button_classement.TabIndex = 3;
            this.button_classement.Text = "Classement";
            this.button_classement.UseVisualStyleBackColor = true;
            this.button_classement.Click += new System.EventHandler(this.button_classement_Click);
            // 
            // label_titre
            // 
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(172, 41);
            this.label_titre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(381, 87);
            this.label_titre.TabIndex = 4;
            this.label_titre.Text = "Pac-Stolfo";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(734, 520);
            this.Controls.Add(this.label_titre);
            this.Controls.Add(this.button_classement);
            this.Controls.Add(this.button_load_partie);
            this.Controls.Add(this.button_start_partie);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
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

