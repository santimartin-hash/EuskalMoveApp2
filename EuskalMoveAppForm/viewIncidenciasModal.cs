﻿using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System;

namespace EuskalMoveAppForm
{
    public partial class viewIncidenciasModal : Form
    {
        private Form parentForm;
        private Form2.Incidencia incidencia;
        private bool isReadOnly;
        private bool isCreating;
        private Timer fadeInTimer;
        private ToastForm currentToastForm; // Variable para almacenar el ToastForm actual

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

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;

            // Asignar el evento TextChanged a los TextBox relevantes
            sourceId.TextChanged += TextBox_TextChanged;
            cityTown.TextChanged += TextBox_TextChanged;
            autonomousRegion.TextChanged += TextBox_TextChanged;
            province.TextChanged += TextBox_TextChanged;
            cause.TextChanged += TextBox_TextChanged;
            latitude.TextChanged += TextBox_TextChanged;
            longitude.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.BorderColor = Color.Gray;
            }
            else
            {
                textBox.BorderColor = Color.IndianRed;
            }
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

        private void viewIncidenciasModal_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            fadeInTimer.Start();

            this.Location = new Point(
                 parentForm.Location.X + (parentForm.Width - this.Width) / 2,
                 parentForm.Location.Y + (parentForm.Height - this.Height) / 2
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
            incidenceId.ReadOnly = true;
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

        private bool ValidateFields()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(sourceId.Text))
            {
                sourceId.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                sourceId.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(incidenceName.Text))
            {
                incidenceName.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                incidenceName.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(cityTown.Text))
            {
                cityTown.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                cityTown.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(autonomousRegion.Text))
            {
                autonomousRegion.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                autonomousRegion.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(incidenceType.Text))
            {
                incidenceType.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                incidenceType.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(province.Text))
            {
                province.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                province.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(cause.Text))
            {
                cause.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                cause.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(latitude.Text))
            {
                latitude.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                latitude.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(longitude.Text))
            {
                longitude.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                longitude.BorderColor = Color.Gray;
            }

            return isValid;
        }

        private async void modificarBtn_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                // Cerrar el ToastForm actual si existe
                currentToastForm?.Close();

                // Mostrar el ToastForm con el mensaje de error
                currentToastForm = new ToastForm(this.Owner, "Error", "Complete todos los campos obligatorios.");
                currentToastForm.Show();
                return;
            }

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

                // Cerrar el ToastForm actual si existe
                currentToastForm?.Close();

                if (response.IsSuccessStatusCode)
                {
                    currentToastForm = new ToastForm(parentForm, "Success", isCreating ? "Incidencia creada correctamente." : "Incidencia modificada correctamente.");
                    currentToastForm.Show();

                    // Llamar al método para recargar el DataGridView en Form2
                    if (parentForm is Form2 form2)
                    {
                        form2.ReloadDataGrid();
                    }

                    this.Close();
                }
                else
                {
                    currentToastForm = new ToastForm(parentForm, "Error", isCreating ? "Error al crear la incidencia." : "Error al modificar la incidencia.");
                    currentToastForm.Show();
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
