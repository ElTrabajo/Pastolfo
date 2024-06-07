namespace Pastolfo_interface
{
    partial class GameOverForm
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
            this.button_recommencer_partie = new System.Windows.Forms.Button();
            this.button_quitter_partie = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_titre
            // 
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(234, 68);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(510, 177);
            this.label_titre.TabIndex = 7;
            this.label_titre.Text = "Game Over";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_recommencer_partie
            // 
            this.button_recommencer_partie.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_recommencer_partie.Location = new System.Drawing.Point(95, 499);
            this.button_recommencer_partie.Name = "button_recommencer_partie";
            this.button_recommencer_partie.Size = new System.Drawing.Size(151, 42);
            this.button_recommencer_partie.TabIndex = 8;
            this.button_recommencer_partie.Text = "Recommencer";
            this.button_recommencer_partie.UseVisualStyleBackColor = true;
            // 
            // button_quitter_partie
            // 
            this.button_quitter_partie.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_quitter_partie.Location = new System.Drawing.Point(716, 499);
            this.button_quitter_partie.Name = "button_quitter_partie";
            this.button_quitter_partie.Size = new System.Drawing.Size(151, 42);
            this.button_quitter_partie.TabIndex = 9;
            this.button_quitter_partie.Text = "Quitter";
            this.button_quitter_partie.UseVisualStyleBackColor = true;
            // 
            // GameOverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(979, 640);
            this.Controls.Add(this.button_quitter_partie);
            this.Controls.Add(this.button_recommencer_partie);
            this.Controls.Add(this.label_titre);
            this.Name = "GameOverForm";
            this.Text = "GameOverForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_titre;
        private System.Windows.Forms.Button button_recommencer_partie;
        private System.Windows.Forms.Button button_quitter_partie;
    }
}