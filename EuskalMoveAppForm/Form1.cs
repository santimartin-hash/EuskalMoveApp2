using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Establecer el texto de sugerencia para los Guna2TextBox
            guna2TextBox1.PlaceholderText = "example@mail.com";
            guna2TextBox2.PlaceholderText = "Contraseña";

            // Quitar la barra de título y bordes del formulario
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black; // Establecer el color de fondo del formulario a negro
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Crear una ruta con bordes redondeados
            int borderRadius = 30;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
            path.AddArc(this.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
            path.AddArc(this.Width - borderRadius, this.Height - borderRadius, borderRadius, borderRadius, 0, 90);
            path.AddArc(0, this.Height - borderRadius, borderRadius, borderRadius, 90, 90);
            path.CloseAllFigures();

            // Establecer la región del formulario a la ruta con bordes redondeados
            this.Region = new Region(path);

            // Dibujar el borde del formulario
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            //abrir nuevo form2
            Form2 form2 = new Form2();
            form2.Show();

        }
    }
}


