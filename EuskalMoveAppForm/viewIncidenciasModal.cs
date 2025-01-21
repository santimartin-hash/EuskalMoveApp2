using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static EuskalMoveAppForm.Form2;

namespace EuskalMoveAppForm
{
    public partial class viewIncidenciasModal : Form
    {
        private Form parentForm;
        private Form2.Incidencia incidencia;
        private bool isReadOnly;
        private bool isCreating;

        public viewIncidenciasModal(Form parentForm, Form2.Incidencia incidencia, bool isReadOnly, bool isCreating = false)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.incidencia = incidencia;
            this.isReadOnly = isReadOnly;
            this.isCreating = isCreating;

            if (isCreating)
            {
                // Limpiar los TextBox para la creación
                ClearTextBoxes();
                modificarBtn.Text = "CREAR";
            }
            else
            {
                modificarBtn.Text = "MODIFICAR";
            }
        }

        private void viewIncidenciasModal_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                parentForm.Location.X + parentForm.Width - this.Width - 60,
                parentForm.Location.Y + parentForm.Height - this.Height - 60
            );

            if (!isCreating)
            {
                // Llenar los TextBox con los valores de la incidencia
                id.Text = incidencia.id.ToString();
                incidenceId.Text = incidencia.incidenceId;
                sourceId.Text = incidencia.sourceId;
                incidenceType.Text = incidencia.incidenceType;
                autonomousRegion.Text = incidencia.autonomousRegion;
                province.Text = incidencia.province;
                cause.Text = incidencia.cause;
                cityTown.Text = incidencia.cityTown;
                startDate.Text = incidencia.startDate;
                endDate.Text = incidencia.endDate;
                pkStart.Text = incidencia.pkStart;
                pkEnd.Text = incidencia.pkEnd;
                direction.Text = incidencia.direction;
                incidenceName.Text = incidencia.incidenceName;
                latitude.Text = incidencia.latitude;
                longitude.Text = incidencia.longitude;
            }

            // Configurar los TextBox como de solo lectura o editables
            SetReadOnly(isReadOnly);
        }

        private void SetReadOnly(bool readOnly)
        {
            modificarBtn.Visible = !readOnly;
            id.ReadOnly = true;
            incidenceId.ReadOnly = readOnly;
            sourceId.ReadOnly = readOnly;
            incidenceType.ReadOnly = readOnly;
            autonomousRegion.ReadOnly = readOnly;
            province.ReadOnly = readOnly;
            cause.ReadOnly = readOnly;
            cityTown.ReadOnly = readOnly;
            startDate.ReadOnly = readOnly;
            endDate.ReadOnly = readOnly;
            pkStart.ReadOnly = readOnly;
            pkEnd.ReadOnly = readOnly;
            direction.ReadOnly = readOnly;
            incidenceName.ReadOnly = readOnly;
            latitude.ReadOnly = readOnly;
            longitude.ReadOnly = readOnly;
        }

        private void ClearTextBoxes()
        {
            id.Text = string.Empty;
            incidenceId.Text = string.Empty;
            sourceId.Text = string.Empty;
            incidenceType.Text = string.Empty;
            autonomousRegion.Text = string.Empty;
            province.Text = string.Empty;
            cause.Text = string.Empty;
            cityTown.Text = string.Empty;
            startDate.Text = string.Empty;
            endDate.Text = string.Empty;
            pkStart.Text = string.Empty;
            pkEnd.Text = string.Empty;
            direction.Text = string.Empty;
            incidenceName.Text = string.Empty;
            latitude.Text = string.Empty;
            longitude.Text = string.Empty;
        }

        private async void modificarBtn_Click(object sender, EventArgs e)
        {
            var incidenciaModificada = new
            {
                incidenceId = incidenceId.Text,
                sourceId = sourceId.Text,
                incidenceType = incidenceType.Text,
                autonomousRegion = autonomousRegion.Text,
                province = province.Text,
                cause = cause.Text,
                cityTown = cityTown.Text,
                startDate = startDate.Text,
                endDate = endDate.Text,
                pkStart = pkStart.Text,
                pkEnd = pkEnd.Text,
                direction = direction.Text,
                incidenceName = incidenceName.Text,
                latitude = latitude.Text,
                longitude = longitude.Text
            };

            var json = JsonConvert.SerializeObject(incidenciaModificada);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                if (isCreating)
                {
                    var url = "http://localhost:8080/incidencias";
                    response = await client.PostAsync(url, content);
                }
                else
                {
                    var url = $"http://localhost:8080/incidencias/{incidencia.id}";
                    response = await client.PutAsync(url, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    ToastForm toastForm = new ToastForm(parentForm, "Success", isCreating ? "Incidencia creada correctamente." : "Incidencia modificada correctamente.");
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
                    ToastForm toastForm = new ToastForm(parentForm, "Error", isCreating ? "Error al crear la incidencia." : "Error al modificar la incidencia.");
                    toastForm.Show();
                    this.Close();
                }
            }
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

