using Guna.UI2.WinForms;
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
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EuskalMoveAppForm
{
    public partial class Form1 : Form
    {
        private string userEmail;
        private string userName;
        private string userStatus;
        private bool userIsAdmin;
        private ToastForm currentToast; // Variable para almacenar la referencia al ToastForm
        //constante para api 
        private const string apiPort = "10.10.13.169";


        public Form1()
        {
            InitializeComponent();
            // Establecer el texto de sugerencia para los Guna2TextBox
            guna2TextBox1.PlaceholderText = "example@mail.com";
            guna2TextBox2.PlaceholderText = "Contraseña";

            // Quitar la barra de título y bordes del formulario
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black; // Establecer el color de fondo del formulario a negro
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

            // Dibujar el borde del formulario
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }

        private async void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text) || string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                ShowToast("Warning", "Por favor, complete todos los campos.");
                return;
            }

            string apiUrl = "http://"+apiPort+":8080/usuarios/login";
            var loginData = new
            {
                email = guna2TextBox1.Text,
                contrasena = guna2TextBox2.Text // Cambiado a "contrasena"
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(apiUrl, content);
                }
                catch (HttpRequestException ex)
                {
                    ShowToast("Error", "Error de conexión: " + ex.Message);
                    return;
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<UserData>(responseBody);

                    // Guardar los datos del usuario
                    userEmail = userData.email;
                    userName = userData.nombre;
                    userStatus = userData.status;
                    userIsAdmin = userData.admin;

                    if (userStatus == "OK")
                    {
                        // Mostrar mensaje de éxito y abrir Form2
                        Form2 form2 = new Form2(userEmail, userName, userStatus, userIsAdmin);
                        form2.Show();

                        // Cerrar el ToastForm actual si existe
                        currentToast?.Close();

                        this.Hide();

                        ToastForm toastForm = new ToastForm(form2, "Success", "Inicio de sesión exitoso.");
                        toastForm.Show();
                    }
                    else
                    {
                        // Mostrar mensaje de error si el status no es "OK"
                        ShowToast("Error", "Error en el inicio de sesión.");
                    }
                }
                else
                {
                    ShowToast("Error", "Error en el inicio de sesión.");
                }
            }
        }

        private void ShowToast(string type, string message)
        {
            // Cerrar el ToastForm actual si existe
            currentToast?.Close();

            // Crear y mostrar un nuevo ToastForm
            currentToast = new ToastForm(this, type, message);
            currentToast.Show();
        }

        private void guna2GradientButton1_MouseEnter(object sender, EventArgs e)
        {
            guna2GradientButton1.HoverState.FillColor = System.Drawing.Color.FromArgb(18, 16, 14);
            guna2GradientButton1.HoverState.ForeColor = System.Drawing.Color.FromArgb(223, 154, 87);
            guna2GradientButton1.HoverState.FillColor2 = System.Drawing.Color.FromArgb(18, 16, 14);
        }

        private void guna2GradientButton1_MouseLeave(object sender, EventArgs e)
        {
            guna2GradientButton1.HoverState.FillColor = System.Drawing.Color.FromArgb(218, 227, 229);
            guna2GradientButton1.HoverState.ForeColor = System.Drawing.Color.FromArgb(223, 154, 87);
            guna2GradientButton1.HoverState.FillColor2 = System.Drawing.Color.FromArgb(218, 227, 229);
        }
    }

    public class UserData
    {
        public string email { get; set; }
        public string nombre { get; set; }
        public string status { get; set; }
        public bool admin { get; set; }
    }
}
