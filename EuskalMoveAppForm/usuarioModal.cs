using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Mail;

namespace EuskalMoveAppForm
{
    public partial class usuarioModal : Form
    {
        private Form parentForm;
        private Form2.Usuario usuario;
        private bool isReadOnly;
        private bool isCreating;
        private Timer fadeInTimer;
        private ToastForm currentToastForm; // Variable para almacenar el ToastForm actual

        public usuarioModal(Form parentForm, Form2.Usuario usuario, bool isReadOnly, bool isCreating = false)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.usuario = usuario;
            this.isReadOnly = isReadOnly;
            this.isCreating = isCreating;

            if (isCreating)
            {
                // Limpiar los TextBox para la creación
                ClearTextBoxes();
                modificarBtn.Text = "CREAR";
                contraseña.Visible = true;
                contraseñaLabel.Visible = true;
            }
            else
            {
                modificarBtn.Text = "MODIFICAR";
      
                contraseña.Text = usuario.contraseña;
            }

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;

            // Asignar el evento TextChanged a los TextBox relevantes
            email.TextChanged += TextBox_TextChanged;
            nombre.TextChanged += TextBox_TextChanged;
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

        private void usuarioModal_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            fadeInTimer.Start();

            this.Location = new Point(
                parentForm.Location.X + (parentForm.Width - this.Width) / 2,
                parentForm.Location.Y + (parentForm.Height - this.Height) / 2
            );

            if (!isCreating)
            {
                id.Text = usuario.id.ToString();
                email.Text = usuario.email;
                nombre.Text = usuario.nombre;
                admin.Checked = usuario.isAdmin;
            }

            // Configurar los TextBox como de solo lectura o editables
            SetReadOnly(isReadOnly);
        }

        private void SetReadOnly(bool readOnly)
        {
            modificarBtn.Visible = !readOnly;
            id.ReadOnly = true;
            email.ReadOnly = readOnly;
            nombre.ReadOnly = readOnly;

            admin.Enabled = !readOnly;
        }

        private void ClearTextBoxes()
        {
            id.Text = string.Empty;
            email.Text = string.Empty;
            nombre.Text = string.Empty;
            admin.Checked = false;
        }

        private bool ValidateFields()
        {
            bool isValid = true;

            if (!IsValidEmail(email.Text))
            {
                email.BorderColor = Color.IndianRed;

                isValid = false;
            }
            else
            {
                email.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(nombre.Text))
            {
                nombre.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                nombre.BorderColor = Color.Gray;
            }

            if (string.IsNullOrWhiteSpace(contraseña.Text))
            {
                contraseña.BorderColor = Color.IndianRed;
                isValid = false;
            }
            else
            {
                contraseña.BorderColor = Color.Gray;
            }


            return isValid;
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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

            var usuarioModificado = new
            {
                email = email.Text,
                nombre = nombre.Text,
                contrasena = isCreating ? contraseña.Text : usuario.contraseña, // Usar la contraseña existente si no se está creando
                admin = admin.Checked // Enviar el valor booleano directamente
            };

            var json = JsonConvert.SerializeObject(usuarioModificado);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                if (isCreating)
                {
                    var url = "http://localhost:8080/usuarios";
                    response = await client.PostAsync(url, content);
                }
                else
                {
                    var url = $"http://localhost:8080/usuarios/{usuario.id}";
                    response = await client.PutAsync(url, content);
                }

                // Cerrar el ToastForm actual si existe
                currentToastForm?.Close();

                if (response.IsSuccessStatusCode)
                {

                    if (isCreating)
                    {
                        SendEmail(email.Text, nombre.Text);
                    }

                    currentToastForm = new ToastForm(parentForm, "Success", isCreating ? "Usuario creado correctamente." : "Usuario modificado correctamente.");
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
                    currentToastForm = new ToastForm(parentForm, "Error", isCreating ? "Error al crear el usuario." : "Error al modificar el usuario.");
                    currentToastForm.Show();
                }
            }
        }


        private void SendEmail(string toEmail, string userName)
        {
            try
            {
                var fromAddress = new MailAddress("euskalmove2dam3@gmail.com", "EuskalMove");
                var toAddress = new MailAddress(toEmail, userName);
                const string fromPassword = "vths lmoi ozfu gcdr";
                const string subject = "Bienvenido a EuskalMove";

                // HTML del correo electrónico
                string body = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <title>Bienvenido a EuskalMove</title>
    <style>
        body {
            font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 0;
            background-color: #DAE3E5;
        }
        
        .container {
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
            border-radius: 8px;
            overflow: hidden;
        }
        
        .header {
            background-color: #12100E;
            padding: 10px 5px;
            text-align: center;
        }
        
        .logo {
            max-width: 150px;
            height: auto;
        }
        
        .content {
            padding: 40px;
            color: #333333;
        }
        
        h1 {
            color: #12100E;
            margin-bottom: 20px;
            font-size: 28px;
            font-weight: 700;
            line-height: 1.2;
        }
        
        .highlight {
            color: #D69A57;
            font-weight: bold;
        }
        
        ul {
            padding-left: 20px;
            margin-bottom: 20px;
        }
        
        li {
            margin-bottom: 12px;
            position: relative;
            padding-left: 15px;
        }
        
        li::before {
            content: ""•"";
            color: #D69A57;
            font-weight: bold;
            position: absolute;
            left: -5px;
        }
        
        .signature {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #DAE3E5;
            font-style: italic;
        }
        
        .footer {
            background-color: #12100E;
            color: #ffffff;
            text-align: center;
            padding: 15px;
            font-size: 12px;
        }
        
        @media only screen and (max-width: 600px) {
            .container {
                margin: 0;
                border-radius: 0;
            }
            
            .content {
                padding: 30px 20px;
            }
        }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://i.ibb.co/FrYQCzR/Captura-modified-1.png"" width=""50""  height=""50"" alt=""EuskalMove Logo"" class=""logo"">
        </div>
        <div class=""content"">
            <h1>Bienvenido a <span class=""highlight"">EuskalMove</span></h1>
            <p>Estimado " + userName + @",</p>
            <p>Nos complace darle la bienvenida a EuskalMove, su nueva plataforma para explorar y disfrutar del País Vasco como nunca antes.</p>
            <p>Con su nueva cuenta, podrá acceder a una serie de funciones exclusivas:</p>
            <ul>
                <li>Descubrir los rincones más fascinantes del País Vasco</li>
                <li>Consultar cámaras de tráfico en tiempo real</li>
                <li>Ver incidencias actualizadas del País Vasco</li>
            </ul>
            <p>Nuestro equipo está comprometido a proporcionarle la mejor experiencia posible. Si tiene alguna pregunta o necesita asistencia, no dude en contactar con nuestro servicio de atención al cliente.</p>
            <p>Le invitamos a explorar todas las posibilidades que EuskalMove tiene para ofrecerle. ¡Esperamos que disfrute de su viaje con nosotros!</p>
            <div class=""signature"">
                <p>Atentamente,<br>El equipo de EuskalMove</p>
            </div>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 EuskalMove. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>
";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el correo electrónico: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

