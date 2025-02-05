using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ScottPlot;
using System.Linq;
using static EuskalMoveAppForm.Form2;

namespace EuskalMoveAppForm
{
    public partial class Form2 : Form
    {
        private Guna.UI2.WinForms.Guna2Button selectedButton;

        private string userEmail;
        private string userName;
        private string userStatus;
        private bool userIsAdmin;
        private List<Incidencia> incidencias; // Almacenar la lista completa de incidencias
        private List<Usuario> usuarios;
        int parentX, parentY;
        private const string apiPort = "localhost";
        public Form2(String email, String nombre, String status, bool admin)
        {
            InitializeComponent();
            userEmail = email;
            userName = nombre;
            userStatus = status;
            userIsAdmin = admin;

            label1.Text = "Bienvenido, " + userName;
            // Establecer el estado inicial de guna2Button1
            SetButtonSelected(guna2Button1);

            // Asignar eventos de hover a los botones
            guna2Button1.MouseEnter += new EventHandler(Button_MouseEnter);
            guna2Button1.MouseLeave += new EventHandler(Button_MouseLeave);
            guna2Button2.MouseEnter += new EventHandler(Button_MouseEnter);
            guna2Button2.MouseLeave += new EventHandler(Button_MouseLeave);
            guna2Button3.HoverState.FillColor = System.Drawing.Color.FromArgb(18, 16, 14);

            ConfigureDataGridView();
            ConfigureDataGridViewUsuarios();
            // Llamar al método para cargar las incidencias y usuarios
            LoadIncidencias();
            LoadUsuarios();
            // Asignar el evento SelectionChanged al DataGridView
            dataGridViewIncidencias.SelectionChanged += DataGridViewIncidencias_SelectionChanged;
            dataGridViewUsuarios.SelectionChanged += DataGridViewUsuarios_SelectionChanged;
            
            // Asignar el evento Click al panelIncidencias
            panelIncidencias.Click += PanelIncidencias_Click;
            panelUsuarios.Click += PanelIncidencias_Click;

            // Asignar el evento Click al botón "ver"
            verBtn.Click += verBtn_Click;
            modificarBtn.Click += modificarBtn_Click;

            this.FormClosing += Form2_FormClosing;
          
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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
        private void DataGridViewIncidencias_SelectionChanged(object sender, EventArgs e)
        {
            // Habilitar los botones si hay una fila seleccionada
            if (dataGridViewIncidencias.SelectedRows.Count > 0)
            {
                verBtn.Enabled = true;
                modificarBtn.Enabled = true;
                eliminarBtn.Enabled = true;
                crearBtn.Enabled = false;
            }
            else
            {
                verBtn.Enabled = false;
                modificarBtn.Enabled = false;
                eliminarBtn.Enabled = false;
                crearBtn.Enabled = true;
            }
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
        private void PanelIncidencias_Click(object sender, EventArgs e)
        {
            // Deseleccionar la fila seleccionada en el DataGridView
            dataGridViewIncidencias.ClearSelection();
            dataGridViewUsuarios.ClearSelection();
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

            //si es guna2Button1 panelIncidencias visible si es guna2Button2 panelUsuarios visible
            if (button == guna2Button1)
            {
                panelIncidencias.Visible = true;
                panelUsuarios.Visible = false;
                LoadIncidencias();
            }
            else
            {
                panelIncidencias.Visible = false;
                panelUsuarios.Visible = true;
                LoadUsuarios();
            }

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
        private void verBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewIncidencias.SelectedRows.Count > 0)
            {
                // Obtener la incidencia seleccionada
                var selectedRow = dataGridViewIncidencias.SelectedRows[0];
                var incidenciaView = (IncidenciaView)selectedRow.DataBoundItem;

                // Buscar la incidencia completa en la lista original
                var incidencia = incidencias.FirstOrDefault(i => i.id == incidenciaView.id);

                if (incidencia != null)
                {
                    Form modalbackground = new Form();
                    //bordes redondeados al background
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (viewIncidenciasModal modal = new viewIncidenciasModal(this, incidencia, true)) // Pasar true para solo lectura
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }

        private void modificarBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewIncidencias.SelectedRows.Count > 0)
            {
                // Obtener la incidencia seleccionada
                var selectedRow = dataGridViewIncidencias.SelectedRows[0];
                var incidenciaView = (IncidenciaView)selectedRow.DataBoundItem;

                // Buscar la incidencia completa en la lista original
                var incidencia = incidencias.FirstOrDefault(i => i.id == incidenciaView.id);

                if (incidencia != null)
                {
                    Form modalbackground = new Form();
                    //bordes redondeados al background
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (viewIncidenciasModal modal = new viewIncidenciasModal(this, incidencia, false)) // Pasar false para editable
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button button = sender as Guna.UI2.WinForms.Guna2Button;

            // Ocultar pnlNav2 solo si el botón no está seleccionado
            if (button != selectedButton)
            {
                pnlNav2.Visible = false;
                button.ForeColor = System.Drawing.Color.FromArgb(218, 227, 229); // Cambia esto al color original del texto
            }
            else
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
            Form modalbackground = new Form();
            //bordes redondeados al background
            modalbackground.Paint += (s, pe) =>
            {
                int borderRadius = 30;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                path.CloseAllFigures();
                modalbackground.Region = new Region(path);
            };

            using (logoutModal modal = new logoutModal(this))
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.FormBorderStyle = FormBorderStyle.None;
                modalbackground.Opacity = .70d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;
                modal.StartPosition = FormStartPosition.CenterScreen;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 16, 14);
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(18, 16, 14); // Cambia el color de fondo de la celda del encabezado seleccionada
            dataGridViewIncidencias.EnableHeadersVisualStyles = false;

            dataGridViewIncidencias.GridColor = Color.FromArgb(18, 16, 14);
            dataGridViewIncidencias.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewIncidencias.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewIncidencias.RowHeadersVisible = false;

            dataGridViewIncidencias.DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 154, 87);
            dataGridViewIncidencias.DefaultCellStyle.SelectionForeColor = Color.Black; // Cambia el color del texto de la celda seleccionada a negro

            dataGridViewIncidencias.BackgroundColor = Color.FromArgb(218, 227, 229);
            dataGridViewIncidencias.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridViewIncidencias.DefaultCellStyle.BackColor = Color.FromArgb(218, 227, 229);
            dataGridViewIncidencias.DefaultCellStyle.ForeColor = Color.FromArgb(18, 16, 14);

            // Ajustar el ancho de las columnas al tamaño del DataGridView
            dataGridViewIncidencias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Deshabilitar el desplazamiento horizontal
            dataGridViewIncidencias.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // Seleccionar toda la fila al seleccionar una celda
            dataGridViewIncidencias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Establecer todas las columnas como de solo lectura
            dataGridViewIncidencias.ReadOnly = true;


            dataGridViewIncidencias.RowTemplate.Height = 40;
            // Configurar el ancho de las columnas

        }

        private async void LoadIncidencias()
        {
            string apiUrl = "http://" + apiPort + ":8080/incidencias";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(apiUrl);
                }
                catch (HttpRequestException ex)
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(this, "Error", "Error de conexión: " + ex.Message);
                    toastForm.Show();
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    incidencias = JsonConvert.DeserializeObject<List<Incidencia>>(responseBody); // Almacenar la lista completa de incidencias

                    // Convertir a IncidenciaView para mostrar en el DataGridView
                    var incidenciasView = incidencias.Select(i => new IncidenciaView
                    {
                        id = i.id,
                        province = i.province,
                        cause = i.cause,
                        cityTown = i.cityTown,
                        incidenceName = i.incidenceName,
                        OriginalIncidencia = i
                    }).ToList();

                    dataGridViewIncidencias.DataSource = incidenciasView;

                    // Configurar el ancho de las columnas
                    dataGridViewIncidencias.Columns["id"].Width = 30; // Ajusta este valor según tus necesidades
                    dataGridViewIncidencias.Columns["province"].Width = 70; // Ajusta este valor según tus necesidades
                    dataGridViewIncidencias.Columns["cause"].Width = 160; // Ajusta este valor según tus necesidades
                    dataGridViewIncidencias.Columns["cityTown"].Width = 120; // Ajusta este valor según tus necesidades
                    dataGridViewIncidencias.Columns["incidenceName"].Width = 200;
                    dataGridViewIncidencias.Columns["OriginalIncidencia"].Visible = false;
                    // Deseleccionar cualquier fila al inicio
                    dataGridViewIncidencias.ClearSelection();
                }
                else
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(this, "Error", "Error al cargar las incidencias.");
                    toastForm.Show();
                }
            }
        }

        public void ReloadDataGrid()
        {
            LoadIncidencias();
            LoadUsuarios();
        }
        public class Incidencia
        {
            public int id { get; set; }
            public string incidenceId { get; set; }
            public string sourceId { get; set; }
            public string incidenceType { get; set; }
            public string autonomousRegion { get; set; }
            public string province { get; set; }
            public string cause { get; set; }
            public string cityTown { get; set; }
            public string startDate { get; set; }
            public string endDate { get; set; }
            public string pkStart { get; set; }
            public string pkEnd { get; set; }
            public string direction { get; set; }
            public string incidenceName { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        private void crearBtn_Click(object sender, EventArgs e)
        {
            Form modalbackground = new Form();
            //bordes redondeados al background
            modalbackground.Paint += (s, pe) =>
            {
                int borderRadius = 30;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                path.CloseAllFigures();
                modalbackground.Region = new Region(path);
            };

            using (viewIncidenciasModal modal = new viewIncidenciasModal(this, new Incidencia(), false, true)) // Pasar true para creación
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.FormBorderStyle = FormBorderStyle.None;
                modalbackground.Opacity = .70d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();
            }
        }

        private void eliminarBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewIncidencias.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewIncidencias.SelectedRows[0];
                var incidenciaView = (IncidenciaView)selectedRow.DataBoundItem;
                var incidencia = incidencias.FirstOrDefault(i => i.id == incidenciaView.id);

                if (incidencia != null)
                {
                    Form modalbackground = new Form();
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (deleteIncidenciasModal modal = new deleteIncidenciasModal(this, incidencia, "Incidencia", userEmail))
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }

        private void generarInformeBtn_Click(object sender, EventArgs e)
        {
            // Verificar si hay datos en el DataGridView
            if (dataGridViewIncidencias.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para generar el informe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear listas para almacenar los datos
            var ciudades = new Dictionary<string, int>();
            var tiposIncidencia = new Dictionary<string, int>();

            foreach (DataGridViewRow row in dataGridViewIncidencias.Rows)
            {
                // Acceder al objeto de vista y luego al objeto original
                var incidenciaView = (IncidenciaView)row.DataBoundItem;
                var incidencia = incidenciaView.OriginalIncidencia;

                // Contar incidencias por ciudad
                if (ciudades.ContainsKey(incidencia.cityTown))
                {
                    ciudades[incidencia.cityTown]++;
                }
                else
                {
                    ciudades[incidencia.cityTown] = 1;
                }

                // Contar incidencias por tipo
                if (tiposIncidencia.ContainsKey(incidencia.incidenceType))
                {
                    tiposIncidencia[incidencia.incidenceType]++;
                }
                else
                {
                    tiposIncidencia[incidencia.incidenceType] = 1;
                }
            }

            // Generar gráfico de incidencias por ciudad
            var plt1 = new ScottPlot.Plot(600, 400);
            double[] valoresCiudades = ciudades.Values.Select(x => (double)x).ToArray();
            plt1.AddBar(valoresCiudades);
            plt1.XTicks(ciudades.Keys.Select((x, i) => (double)i).ToArray(), ciudades.Keys.ToArray());
            plt1.Title("Ciudades con más incidencias");
            plt1.YLabel("Número de incidencias");

            // Generar gráfico circular para tipos de incidencia
            var plt3 = new ScottPlot.Plot(600, 400);
            double[] valoresTipos = tiposIncidencia.Values.Select(x => (double)x).ToArray();
            string[] etiquetasTipos = tiposIncidencia.Keys.ToArray();

            plt3.PlotPie(valoresTipos, etiquetasTipos);
            plt3.Title("Distribución de Tipos de Incidencias");

            // Guardar gráficos como imágenes
            string path1 = "ciudades_incidencias.png";
            string path3 = "tipos_incidencias.png";

            plt1.SaveFig(path1);
            plt3.SaveFig(path3);

            // Crear documento PDF
            Document document = new Document();
            string pdfPath = "informe_incidencias.pdf";

            PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
            document.Open();

            document.Add(new Paragraph("Informe de Incidencias"));

            AddImageToPdf(document, path1);
            AddImageToPdf(document, path3);

            document.Close(); // Cerrar el documento PDF

            // Abrir el PDF generado
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(pdfPath) { UseShellExecute = true });

            MessageBox.Show("Informe generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddImageToPdf(Document document, string imagePath)
        {
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);

            image.ScaleToFit(document.PageSize.Width - 40, document.PageSize.Height - 40);
            image.Alignment = Element.ALIGN_CENTER;

            document.Add(image);
        }

        private void ConfigureDataGridViewUsuarios()
        {
            dataGridViewUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 16, 14);
            dataGridViewUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewUsuarios.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
            dataGridViewUsuarios.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(18, 16, 14);
            dataGridViewUsuarios.EnableHeadersVisualStyles = false;

            dataGridViewUsuarios.GridColor = Color.FromArgb(18, 16, 14);
            dataGridViewUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewUsuarios.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewUsuarios.RowHeadersVisible = false;

            dataGridViewUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 154, 87);
            dataGridViewUsuarios.DefaultCellStyle.SelectionForeColor = Color.Black;

            dataGridViewUsuarios.BackgroundColor = Color.FromArgb(218, 227, 229);
            dataGridViewUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridViewUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(218, 227, 229);
            dataGridViewUsuarios.DefaultCellStyle.ForeColor = Color.FromArgb(18, 16, 14);

            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.ReadOnly = true;
            dataGridViewUsuarios.RowTemplate.Height = 40;
        
            // Deseleccionar cualquier fila al inicio
            dataGridViewUsuarios.ClearSelection();
        }

        private async void LoadUsuarios()
        {
            string apiUrl = "http://" + apiPort + ":8080/usuarios";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(apiUrl);
                }
                catch (HttpRequestException ex)
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(this, "Error", "Error de conexión: " + ex.Message);
                    toastForm.Show();
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);

                    // Configurar el DataGridView
                    dataGridViewUsuarios.DataSource = null; // Limpiar el DataSource primero
                    dataGridViewUsuarios.DataSource = usuarios;

                    // Configurar las columnas
                    if (dataGridViewUsuarios.Columns["contraseña"] != null)
                    {
                        dataGridViewUsuarios.Columns["contraseña"].Visible = false;
                    }
                   

                    if (dataGridViewUsuarios.Columns["admin"] != null)
                    {
                        dataGridViewUsuarios.Columns["admin"].ValueType = typeof(bool);
                        dataGridViewUsuarios.Columns["admin"].DefaultCellStyle.NullValue = false;
                    }


                    // Deseleccionar todas las filas
                    dataGridViewUsuarios.ClearSelection();

                    // Asegurarse de que no hay fila actual
                    dataGridViewUsuarios.CurrentCell = null;

                 
                }
                else
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(this, "Error", "Error al cargar los usuarios.");
                    toastForm.Show();
                }
            }
        }

       

        private void DataGridViewUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            // Habilitar los botones si hay una fila seleccionada
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                verUsuarioBtn.Enabled = true;
                modificarUsuarioBtn.Enabled = true;
                eliminarUsuarioBtn.Enabled = true;
                añadirUsuarioBtn.Enabled = false;
                cambiarContraseñaBtn.Enabled = true;
            }
            else
            {
                verUsuarioBtn.Enabled = false;
                modificarUsuarioBtn.Enabled = false;
                eliminarUsuarioBtn.Enabled = false;
                añadirUsuarioBtn.Enabled = true;
                cambiarContraseñaBtn.Enabled = false;
            }
        }

        public class Usuario
        {
            public int id { get; set; }
            public string email { get; set; }
            public string nombre { get; set; }
            [JsonProperty("contrasena")]
            public string contraseña { get; set; }
            [JsonProperty("admin")]
            public bool isAdmin { get; set; }
        }

        private void verUsuarioBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                // Obtener el usuario seleccionado
                var selectedRow = dataGridViewUsuarios.SelectedRows[0];
                var usuario = (Usuario)selectedRow.DataBoundItem;

                if (usuario != null)
                {
                    Form modalbackground = new Form();
                    //bordes redondeados al background
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (usuarioModal modal = new usuarioModal(this, usuario, true)) // Pasar true para solo lectura
                    {
                        modalbackground.StartPosition = FormStartPosition.CenterScreen;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }
        private void modificarUsuarioBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                // Obtener el usuario seleccionado
                var selectedRow = dataGridViewUsuarios.SelectedRows[0];
                var usuario = (Usuario)selectedRow.DataBoundItem;

                if (usuario != null)
                {
                    Form modalbackground = new Form();
                    //bordes redondeados al background
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (usuarioModal modal = new usuarioModal(this, usuario, false)) // Pasar false para editable
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }

        private void añadirUsuarioBtn_Click(object sender, EventArgs e)
        {
            Form modalbackground = new Form();
            //bordes redondeados al background
            modalbackground.Paint += (s, pe) =>
            {
                int borderRadius = 30;
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                path.CloseAllFigures();
                modalbackground.Region = new Region(path);
            };

            using (usuarioModal modal = new usuarioModal(this, new Usuario(), false, true)) // Pasar true para creación
            {
                modalbackground.StartPosition = FormStartPosition.Manual;
                modalbackground.FormBorderStyle = FormBorderStyle.None;
                modalbackground.Opacity = .70d;
                modalbackground.BackColor = Color.Black;
                modalbackground.Size = this.Size;
                modalbackground.Location = this.Location;
                modalbackground.ShowInTaskbar = false;
                modalbackground.Show();
                modal.Owner = modalbackground;

                parentX = this.Location.X;
                parentY = this.Location.Y;

                modal.ShowDialog();
                modalbackground.Dispose();
            }
        }
        private void eliminarUsuarioBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewUsuarios.SelectedRows[0];
                var usuario = (Usuario)selectedRow.DataBoundItem;

                if (usuario != null)
                {
                    Form modalbackground = new Form();
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (deleteIncidenciasModal modal = new deleteIncidenciasModal(this, usuario, "Usuario", userEmail))
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }
        private void cambiarContraseñaBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewUsuarios.SelectedRows[0];
                var usuario = (Usuario)selectedRow.DataBoundItem;

                if (usuario != null)
                {
                    Form modalbackground = new Form();
                    //bordes redondeados al background
                    modalbackground.Paint += (s, pe) =>
                    {
                        int borderRadius = 30;
                        GraphicsPath path = new GraphicsPath();
                        path.AddArc(0, 0, borderRadius, borderRadius, 180, 90);
                        path.AddArc(modalbackground.Width - borderRadius, 0, borderRadius, borderRadius, 270, 90);
                        path.AddArc(modalbackground.Width - borderRadius, modalbackground.Height - borderRadius, borderRadius, borderRadius, 0, 90);
                        path.AddArc(0, modalbackground.Height - borderRadius, borderRadius, borderRadius, 90, 90);
                        path.CloseAllFigures();
                        modalbackground.Region = new Region(path);
                    };

                    using (cambiarContraseñaModal modal = new cambiarContraseñaModal(this, usuario))
                    {
                        modalbackground.StartPosition = FormStartPosition.Manual;
                        modalbackground.FormBorderStyle = FormBorderStyle.None;
                        modalbackground.Opacity = .70d;
                        modalbackground.BackColor = Color.Black;
                        modalbackground.Size = this.Size;
                        modalbackground.Location = this.Location;
                        modalbackground.ShowInTaskbar = false;
                        modalbackground.Show();
                        modal.Owner = modalbackground;

                        parentX = this.Location.X;
                        parentY = this.Location.Y;

                        modal.ShowDialog();
                        modalbackground.Dispose();
                    }
                }
            }
        }


        public class IncidenciaView
        {
            public int id { get; set; }
            public string province { get; set; }
            public string cause { get; set; }
            public string cityTown { get; set; }
            public string incidenceName { get; set; }

            public Incidencia OriginalIncidencia { get; set; }
        }
     
    }
}

