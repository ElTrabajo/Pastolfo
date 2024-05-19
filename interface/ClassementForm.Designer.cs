namespace Pastolfo_interface
{
    partial class ClassementForm
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
            this.button_retour_startpartie = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_titre
            // 
            this.label_titre.Font = new System.Drawing.Font("Microsoft YaHei UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_titre.ForeColor = System.Drawing.SystemColors.Control;
            this.label_titre.Location = new System.Drawing.Point(337, 52);
            this.label_titre.Name = "label_titre";
            this.label_titre.Size = new System.Drawing.Size(324, 66);
            this.label_titre.TabIndex = 6;
            this.label_titre.Text = "Classement";
            this.label_titre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_retour_startpartie
            // 
            this.button_retour_startpartie.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retour_startpartie.Location = new System.Drawing.Point(867, 572);
            this.button_retour_startpartie.Name = "button_retour_startpartie";
            this.button_retour_startpartie.Size = new System.Drawing.Size(75, 32);
            this.button_retour_startpartie.TabIndex = 15;
            this.button_retour_startpartie.Text = "Retour";
            this.button_retour_startpartie.UseVisualStyleBackColor = true;
            this.button_retour_startpartie.Click += new System.EventHandler(this.button_retour_startpartie_Click);
            // 
            // ClassementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(979, 640);
            this.Controls.Add(this.button_retour_startpartie);
            this.Controls.Add(this.label_titre);
            this.Name = "ClassementForm";
            this.Text = "ChargementPartieForm";
            this.Load += new System.EventHandler(this.ClassementForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_titre;
        private System.Windows.Forms.Button button_retour_startpartie;
    }
}