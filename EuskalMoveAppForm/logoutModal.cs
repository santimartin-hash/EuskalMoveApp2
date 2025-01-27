using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class logoutModal : Form
    {
        private Form parentForm;
        private Timer fadeInTimer;

        public logoutModal(Form parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;
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
        }

        private void logoutModal_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            fadeInTimer.Start();

            this.Location = new Point(
                parentForm.Location.X + (parentForm.Width - this.Width) / 2,
                parentForm.Location.Y + (parentForm.Height - this.Height) / 2
            );
        }

        private void FadeInTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                fadeInTimer.Stop();
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Cerrar el formulario actual (Form2)
            parentForm.Close();

            // Mostrar el formulario de inicio de sesión (Form1)
            Form1 form1 = new Form1();
            form1.Show();

            // Cerrar el modal de logout
            this.Close();
        }



        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
