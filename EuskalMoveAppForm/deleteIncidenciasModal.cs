using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class deleteIncidenciasModal : Form
    {
        private Form parentForm;
        private int incidenciaId;

        public deleteIncidenciasModal(Form parentForm, int incidenciaId)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.incidenciaId = incidenciaId;
        }

        private async void deleteBtn_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var url = $"http://localhost:8080/incidencias/{incidenciaId}";
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    ToastForm toastForm = new ToastForm(parentForm, "Success", "Incidencia eliminada correctamente.");
                    toastForm.Show();

                    // Llamar al método para recargar el DataGridView en Form2
                    if (parentForm is Form2 form2)
                    {
                        form2.ReloadDataGrid();
                    }

                    this.Close();
                }
                else
                {
                    ToastForm toastForm = new ToastForm(parentForm, "Error", "Error al eliminar la incidencia.");
                    toastForm.Show();
                    this.Close();
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
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

        private void deleteIncidenciasModal_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                parentForm.Location.X + (parentForm.Width - this.Width) / 2,
                parentForm.Location.Y + (parentForm.Height - this.Height) / 2
            );
        }
    }
}

