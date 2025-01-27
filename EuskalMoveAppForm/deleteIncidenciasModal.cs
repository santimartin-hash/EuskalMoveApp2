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
        private Timer fadeInTimer;

        public deleteIncidenciasModal(Form parentForm, int incidenciaId)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.incidenciaId = incidenciaId;

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;
        }

        private async void deleteBtn_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var url = $"http://localhost:8080/incidencias/{incidenciaId}";
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    CloseExistingToast();
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
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Error", "Error al eliminar la incidencia.");
                    toastForm.Show();
                    this.Close();
                }
            }
        }
        private void CloseExistingToast()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is ToastForm)
                {
                    form.Close();
                    break;
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
    }
}
