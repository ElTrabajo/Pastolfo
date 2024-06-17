namespace Pastolfo_interface
{
    partial class ChargementPartieForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChargementPartieForm));
            this.label_titre = new System.Windows.Forms.Label();
            this.button_retour_startpartie = new System.Windows.Forms.Button();
            this.panelSauvegarde = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label_titre
            // 
            this.label_titre.BackColor = System.Drawing.Color.Transparent;
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(253, 42);
            this.label_titre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(249, 54);
            this.label_titre.TabIndex = 6;
            this.label_titre.Text = "Sauvegarde";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_retour_startpartie
            // 
            this.button_retour_startpartie.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retour_startpartie.Location = new System.Drawing.Point(650, 465);
            this.button_retour_startpartie.Margin = new System.Windows.Forms.Padding(2);
            this.button_retour_startpartie.Name = "button_retour_startpartie";
            this.button_retour_startpartie.Size = new System.Drawing.Size(56, 26);
            this.button_retour_startpartie.TabIndex = 15;
            this.button_retour_startpartie.Text = "Retour";
            this.button_retour_startpartie.UseVisualStyleBackColor = true;
            this.button_retour_startpartie.Click += new System.EventHandler(this.button_retour_startpartie_Click);
            // 
            // panelSauvegarde
            // 
            this.panelSauvegarde.AutoScroll = true;
            this.panelSauvegarde.BackColor = System.Drawing.Color.Black;
            this.panelSauvegarde.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSauvegarde.Location = new System.Drawing.Point(22, 131);
            this.panelSauvegarde.Margin = new System.Windows.Forms.Padding(2);
            this.panelSauvegarde.Name = "panelSauvegarde";
            this.panelSauvegarde.Size = new System.Drawing.Size(690, 318);
            this.panelSauvegarde.TabIndex = 17;
            // 
            // ChargementPartieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::Pastolfo_interface.Properties.Resources.save_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(734, 520);
            this.Controls.Add(this.panelSauvegarde);
            this.Controls.Add(this.button_retour_startpartie);
            this.Controls.Add(this.label_titre);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChargementPartieForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sauvegarde";
            this.Load += new System.EventHandler(this.ChargementPartieForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_titre;
        private System.Windows.Forms.Button button_retour_startpartie;
        private System.Windows.Forms.Panel panelSauvegarde;
    }
}