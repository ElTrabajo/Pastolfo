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
            this.label_titre.BackColor = System.Drawing.Color.Transparent;
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(176, 55);
            this.label_titre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(395, 144);
            this.label_titre.TabIndex = 7;
            this.label_titre.Text = "Game Over";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_recommencer_partie
            // 
            this.button_recommencer_partie.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_recommencer_partie.Location = new System.Drawing.Point(71, 405);
            this.button_recommencer_partie.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_recommencer_partie.Name = "button_recommencer_partie";
            this.button_recommencer_partie.Size = new System.Drawing.Size(113, 34);
            this.button_recommencer_partie.TabIndex = 8;
            this.button_recommencer_partie.Text = "Recommencer";
            this.button_recommencer_partie.UseVisualStyleBackColor = true;
            // 
            // button_quitter_partie
            // 
            this.button_quitter_partie.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_quitter_partie.Location = new System.Drawing.Point(537, 405);
            this.button_quitter_partie.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_quitter_partie.Name = "button_quitter_partie";
            this.button_quitter_partie.Size = new System.Drawing.Size(113, 34);
            this.button_quitter_partie.TabIndex = 9;
            this.button_quitter_partie.Text = "Quitter";
            this.button_quitter_partie.UseVisualStyleBackColor = true;
            // 
            // GameOverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Pastolfo_interface.Properties.Resources.main_menu_background;
            this.ClientSize = new System.Drawing.Size(734, 520);
            this.Controls.Add(this.button_quitter_partie);
            this.Controls.Add(this.button_recommencer_partie);
            this.Controls.Add(this.label_titre);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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