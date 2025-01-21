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
    public partial class viewIncidenciasModal : Form
    {
        public viewIncidenciasModal()
        {
            InitializeComponent();
        }

        private void viewIncidenciasModal_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Width - 990,
                Screen.PrimaryScreen.WorkingArea.Height - 600
            );
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void verBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
