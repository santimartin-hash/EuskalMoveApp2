namespace EuskalMoveAppForm
{
    partial class viewIncidenciasModal
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
            this.label1 = new System.Windows.Forms.Label();
            this.verBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "modal";
            // 
            // verBtn
            // 
            this.verBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.verBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.verBtn.BorderRadius = 12;
            this.verBtn.BorderThickness = 1;
            this.verBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.verBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.verBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.verBtn.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.verBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.verBtn.FillColor = System.Drawing.Color.IndianRed;
            this.verBtn.FillColor2 = System.Drawing.Color.IndianRed;
            this.verBtn.Font = new System.Drawing.Font("Leelawadee UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.verBtn.Location = new System.Drawing.Point(495, 285);
            this.verBtn.Name = "verBtn";
            this.verBtn.PressedColor = System.Drawing.Color.Transparent;
            this.verBtn.Size = new System.Drawing.Size(68, 28);
            this.verBtn.TabIndex = 17;
            this.verBtn.Text = "SALIR";
            this.verBtn.Click += new System.EventHandler(this.verBtn_Click);
            // 
            // viewIncidenciasModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 325);
            this.Controls.Add(this.verBtn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "viewIncidenciasModal";
            this.ShowInTaskbar = false;
            this.Text = "viewIncidenciasModal";
            this.Load += new System.EventHandler(this.viewIncidenciasModal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2GradientButton verBtn;
    }
}