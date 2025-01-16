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
    public partial class Form2 : Form
    {
        private Guna.UI2.WinForms.Guna2Button selectedButton;

        public Form2(String email, String nombre, String status, bool admin)
        {
            InitializeComponent();
            // Establecer el estado inicial de guna2Button1
            SetButtonSelected(guna2Button1);

            // Asignar eventos de hover a los botones
            guna2Button1.MouseEnter += new EventHandler(Button_MouseEnter);
            guna2Button1.MouseLeave += new EventHandler(Button_MouseLeave);
            guna2Button2.MouseEnter += new EventHandler(Button_MouseEnter);
            guna2Button2.MouseLeave += new EventHandler(Button_MouseLeave);
            guna2Button3.HoverState.FillColor = System.Drawing.Color.FromArgb(18, 16, 14);
          
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            SetButtonSelected(guna2Button1);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SetButtonSelected(guna2Button2);
        }

        private void SetButtonSelected(Guna.UI2.WinForms.Guna2Button button)
        {
            // Restablecer el estado de todos los botones
            ResetButtonState(guna2Button1);
            ResetButtonState(guna2Button2);

            // Establecer el estado del botón seleccionado
            pnlNav.Height = button.Height;
            pnlNav.Top = button.Top;
            pnlNav.Left = button.Left;
            button.ForeColor = System.Drawing.Color.FromArgb(223, 154, 87);
            pnlNav.Visible = true;

            // Ocultar el panel de hover
            pnlNav2.Visible = false;

            // Guardar el botón seleccionado
            selectedButton = button;
        }

        private void ResetButtonState(Guna.UI2.WinForms.Guna2Button button)
        {
            button.FillColor = System.Drawing.Color.FromArgb(18, 16, 14);
            button.ForeColor = System.Drawing.Color.FromArgb(218, 227, 229); // Cambia esto al color original del texto
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;

            // Mostrar pnlNav2 solo si el botón no está seleccionado
            if (button != selectedButton)
            {
                pnlNav2.Height = button.Height;
                pnlNav2.Top = button.Top;
                pnlNav2.Left = button.Left;
                pnlNav2.Visible = true;
         
            }
            button.ForeColor = System.Drawing.Color.FromArgb(132, 79, 26);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;

            // Ocultar pnlNav2 solo si el botón no está seleccionado
            if (button != selectedButton)
            {
                pnlNav2.Visible = false;
                button.ForeColor = System.Drawing.Color.FromArgb(218, 227, 229); // Cambia esto al color original del texto
            } else
            {
                button.ForeColor = System.Drawing.Color.FromArgb(223, 154, 87);
            }

            
        }

        private void guna2Button3_MouseEnter(object sender, EventArgs e)
        {
            // Cambiar el ícono del botón al hacer hover
            guna2Button3.Image = Properties.Resources.image__3_; // Reemplaza 'IconHover' con el nombre del recurso del ícono para hover
            guna2Button3.ForeColor = System.Drawing.Color.IndianRed;
            guna2Button3.Location = new System.Drawing.Point(6, 480);
        }

        private void guna2Button3_MouseLeave(object sender, EventArgs e)
        {
            // Restaurar el ícono original del botón
            guna2Button3.Image = Properties.Resources.image__2_; // Reemplaza 'IconDefault' con el nombre del recurso del ícono original
            guna2Button3.ForeColor = System.Drawing.Color.FromArgb(218, 227, 229);
            guna2Button3.Location = new System.Drawing.Point(0, 480);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            // Cerrar el formulario actual (Form2)
            this.Close();

            // Mostrar el formulario de inicio de sesión (Form1)
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}