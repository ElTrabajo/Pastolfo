namespace Pastolfo_interface
{
    partial class PartieForm
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
            this.labelViesJoueur = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelViesJoueur
            // 
            this.labelViesJoueur.Location = new System.Drawing.Point(65, 598);
            this.labelViesJoueur.Name = "labelViesJoueur";
            this.labelViesJoueur.Size = new System.Drawing.Size(100, 23);
            this.labelViesJoueur.TabIndex = 0;
            this.labelViesJoueur.Text = "Vies :";
            // 
            // PartieForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 640);
            this.Controls.Add(this.labelViesJoueur);
            this.Name = "PartieForm";
            this.Text = "PartieForm";
            this.Load += new System.EventHandler(this.PartieForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelViesJoueur;
    }
}