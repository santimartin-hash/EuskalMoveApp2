namespace EuskalMoveAppForm
{
    partial class cambiarContraseñaModal
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
            this.verBtn = new Guna.UI2.WinForms.Guna2GradientButton();
            this.guna2GradientButton1 = new Guna.UI2.WinForms.Guna2GradientButton();
            this.contraseñaLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.contraseña = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
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
            this.verBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.verBtn.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.verBtn.Font = new System.Drawing.Font("Leelawadee UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.verBtn.Location = new System.Drawing.Point(283, 102);
            this.verBtn.Name = "verBtn";
            this.verBtn.PressedColor = System.Drawing.Color.Transparent;
            this.verBtn.Size = new System.Drawing.Size(68, 28);
            this.verBtn.TabIndex = 52;
            this.verBtn.Text = "SALIR";
            this.verBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // guna2GradientButton1
            // 
            this.guna2GradientButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.guna2GradientButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.guna2GradientButton1.BorderRadius = 12;
            this.guna2GradientButton1.BorderThickness = 1;
            this.guna2GradientButton1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2GradientButton1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2GradientButton1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2GradientButton1.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2GradientButton1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2GradientButton1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.guna2GradientButton1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.guna2GradientButton1.Font = new System.Drawing.Font("Leelawadee UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GradientButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.guna2GradientButton1.Location = new System.Drawing.Point(175, 102);
            this.guna2GradientButton1.Name = "guna2GradientButton1";
            this.guna2GradientButton1.PressedColor = System.Drawing.Color.Transparent;
            this.guna2GradientButton1.Size = new System.Drawing.Size(102, 28);
            this.guna2GradientButton1.TabIndex = 53;
            this.guna2GradientButton1.Text = "MODIFICAR";
            this.guna2GradientButton1.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // contraseñaLabel
            // 
            this.contraseñaLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.contraseñaLabel.Font = new System.Drawing.Font("Leelawadee UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contraseñaLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(16)))), ((int)(((byte)(14)))));
            this.contraseñaLabel.Location = new System.Drawing.Point(29, 26);
            this.contraseñaLabel.Name = "contraseñaLabel";
            this.contraseñaLabel.Size = new System.Drawing.Size(110, 19);
            this.contraseñaLabel.TabIndex = 56;
            this.contraseñaLabel.Text = "Nueva Contraseña";
            // 
            // contraseña
            // 
            this.contraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.contraseña.BorderColor = System.Drawing.Color.Gray;
            this.contraseña.BorderRadius = 12;
            this.contraseña.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.contraseña.DefaultText = "";
            this.contraseña.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.contraseña.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.contraseña.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.contraseña.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.contraseña.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.contraseña.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.contraseña.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contraseña.ForeColor = System.Drawing.Color.Black;
            this.contraseña.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(154)))), ((int)(((byte)(87)))));
            this.contraseña.Location = new System.Drawing.Point(29, 51);
            this.contraseña.Name = "contraseña";
            this.contraseña.PasswordChar = '\0';
            this.contraseña.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.contraseña.PlaceholderText = "";
            this.contraseña.SelectedText = "";
            this.contraseña.Size = new System.Drawing.Size(322, 25);
            this.contraseña.TabIndex = 55;
            // 
            // cambiarContraseñaModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(227)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(371, 149);
            this.Controls.Add(this.contraseñaLabel);
            this.Controls.Add(this.contraseña);
            this.Controls.Add(this.guna2GradientButton1);
            this.Controls.Add(this.verBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "cambiarContraseñaModal";
            this.ShowInTaskbar = false;
            this.Text = "cambiarContraseñaModal";
            this.Load += new System.EventHandler(this.cambiarContraseñaModal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientButton verBtn;
        private Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton1;
        private Guna.UI2.WinForms.Guna2HtmlLabel contraseñaLabel;
        private Guna.UI2.WinForms.Guna2TextBox contraseña;
    }
}