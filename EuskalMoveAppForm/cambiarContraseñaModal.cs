﻿using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EuskalMoveAppForm.Form2;

namespace EuskalMoveAppForm
{
    public partial class cambiarContraseñaModal : Form
    {
        private Form2 parentForm;
        private Usuario usuario;
        private Timer fadeInTimer;

        public cambiarContraseñaModal(Form2 parentForm, Usuario usuario)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.usuario = usuario;

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;

            // Manejar el evento FormClosing del formulario principal
            parentForm.FormClosing += ParentForm_FormClosing;
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cancelar el cierre del formulario principal si el modal está abierto
            if (this.Visible)
            {
                e.Cancel = true;
                this.BringToFront();
            }
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            string nuevaContraseña = contraseña.Text;

            if (string.IsNullOrEmpty(nuevaContraseña))
            {
                MessageBox.Show("La nueva contraseña no puede estar vacía.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            usuario.contraseña = nuevaContraseña;

            string apiUrl = "http://localhost:8080/usuarios/" + usuario.id;

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Success", "Contraseña actualizada correctamente.");
                    toastForm.Show();

                    // Llamar al método para recargar el DataGridView en Form2
                    parentForm.ReloadDataGrid();

                    this.Close();
                }
                else
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Error", "Error al actualizar la contraseña.");
                    toastForm.Show();
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

        private void cambiarContraseñaModal_Load(object sender, EventArgs e)
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
