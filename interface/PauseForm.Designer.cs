namespace Pastolfo_interface
{
    partial class PauseForm
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
            this.label_Pause = new System.Windows.Forms.Label();
            this.bouton_continuer = new System.Windows.Forms.Button();
            this.bouton_sauve_et_start = new System.Windows.Forms.Button();
            this.bouton_save_et_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_Pause
            // 
            this.label_Pause.Font = new System.Drawing.Font("Microsoft YaHei", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Pause.Location = new System.Drawing.Point(197, 48);
            this.label_Pause.Name = "label_Pause";
            this.label_Pause.Size = new System.Drawing.Size(225, 61);
            this.label_Pause.TabIndex = 0;
            this.label_Pause.Text = "Pause";
            this.label_Pause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bouton_continuer
            // 
            this.bouton_continuer.Location = new System.Drawing.Point(228, 136);
            this.bouton_continuer.Name = "bouton_continuer";
            this.bouton_continuer.Size = new System.Drawing.Size(166, 55);
            this.bouton_continuer.TabIndex = 1;
            this.bouton_continuer.Text = "Continuer";
            this.bouton_continuer.UseVisualStyleBackColor = true;
            // 
            // bouton_sauve_et_start
            // 
            this.bouton_sauve_et_start.Location = new System.Drawing.Point(228, 234);
            this.bouton_sauve_et_start.Name = "bouton_sauve_et_start";
            this.bouton_sauve_et_start.Size = new System.Drawing.Size(166, 55);
            this.bouton_sauve_et_start.TabIndex = 2;
            this.bouton_sauve_et_start.Text = "Sauvegarder et continuer";
            this.bouton_sauve_et_start.UseVisualStyleBackColor = true;
            // 
            // bouton_save_et_exit
            // 
            this.bouton_save_et_exit.Location = new System.Drawing.Point(228, 328);
            this.bouton_save_et_exit.Name = "bouton_save_et_exit";
            this.bouton_save_et_exit.Size = new System.Drawing.Size(166, 55);
            this.bouton_save_et_exit.TabIndex = 3;
            this.bouton_save_et_exit.Text = "Sauvegarder et quitter";
            this.bouton_save_et_exit.UseVisualStyleBackColor = true;
            // 
            // PauseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 450);
            this.Controls.Add(this.bouton_save_et_exit);
            this.Controls.Add(this.bouton_sauve_et_start);
            this.Controls.Add(this.bouton_continuer);
            this.Controls.Add(this.label_Pause);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PauseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PauseForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Pause;
        private System.Windows.Forms.Button bouton_continuer;
        private System.Windows.Forms.Button bouton_sauve_et_start;
        private System.Windows.Forms.Button bouton_save_et_exit;
    }
}