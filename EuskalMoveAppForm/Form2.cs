using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class Form2 : Form
    {
        private Guna.UI2.WinForms.Guna2Button selectedButton;

        private string userEmail;
        private string userName;
        private string userStatus;
        private bool userIsAdmin;

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
            // Llamar al método para cargar las incidencias
           
       

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

        private void ConfigureDataGridView()
        {
            LoadIncidencias();
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 16, 14);
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewIncidencias.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
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

            // Añadir columna de botones al final
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Ver";
            buttonColumn.Name = "Ver";
            buttonColumn.Text = "Ver";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridViewIncidencias.Columns.Add(buttonColumn);

            buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Modificar";
            buttonColumn.Name = "Modificar";
            buttonColumn.Text = "Modificar";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridViewIncidencias.Columns.Add(buttonColumn);

            buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Eliminar";
            buttonColumn.Name = "Eliminar";
            buttonColumn.Text = "Eliminar";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridViewIncidencias.Columns.Add(buttonColumn);

            // Manejar el evento CellContentClick
            dataGridViewIncidencias.CellContentClick += DataGridViewIncidencias_CellContentClick;
        }

        private void DataGridViewIncidencias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dataGridViewIncidencias.Columns[e.ColumnIndex].Name == "Ver")
                {
                    // Acción para el botón "Ver"
                    VerIncidencia(e.RowIndex);
                }
                else if (dataGridViewIncidencias.Columns[e.ColumnIndex].Name == "Modificar")
                {
                    // Acción para el botón "Modificar"
                    ModificarIncidencia(e.RowIndex);
                }
                else if (dataGridViewIncidencias.Columns[e.ColumnIndex].Name == "Eliminar")
                {
                    // Acción para el botón "Eliminar"
                    EliminarIncidencia(e.RowIndex);
                }
            }
        }

        private void VerIncidencia(int rowIndex)
        {
            // Lógica para ver la incidencia
            var incidencia = (Incidencia)dataGridViewIncidencias.Rows[rowIndex].DataBoundItem;
            MessageBox.Show($"Ver incidencia: {incidencia.incidenceName}");
        }

        private void ModificarIncidencia(int rowIndex)
        {
            // Lógica para modificar la incidencia
            var incidencia = (Incidencia)dataGridViewIncidencias.Rows[rowIndex].DataBoundItem;
            MessageBox.Show($"Modificar incidencia: {incidencia.incidenceName}");
        }

        private void EliminarIncidencia(int rowIndex)
        {
            // Lógica para eliminar la incidencia
            var incidencia = (Incidencia)dataGridViewIncidencias.Rows[rowIndex].DataBoundItem;
            MessageBox.Show($"Eliminar incidencia: {incidencia.incidenceName}");
        }






        private async void LoadIncidencias()
        {
            string apiUrl = "http://10.10.13.169:8080/incidencias";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(apiUrl);
                }
                catch (HttpRequestException ex)
                {
                    ToastForm toastForm = new ToastForm(this, "Error", "Error de conexión: " + ex.Message);
                    return;
                }
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var incidencias = JsonConvert.DeserializeObject<List<Incidencia>>(responseBody);

                    // Filtrar los campos necesarios y mostrar en el DataGridView
                    dataGridViewIncidencias.DataSource = incidencias;
                }
                else
                {
                    ToastForm toastForm = new ToastForm(this, "Error", "Error al cargar las incidencias.");
                }
            }
        }
    }

    public class Incidencia
    {
        public int id { get; set; }
        public string province { get; set; }
        public string cause { get; set; }
        public string cityTown { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string incidenceName { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
