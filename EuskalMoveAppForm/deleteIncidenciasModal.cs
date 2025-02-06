using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class deleteIncidenciasModal : Form
    {
        private Form parentForm;
        private object entity;
        private string entityType;
        private Timer fadeInTimer;
        private string loggedInUserEmail;

        public deleteIncidenciasModal(Form parentForm, object entity, string entityType, string loggedInUserEmail)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            this.entity = entity;
            this.entityType = entityType;
            this.loggedInUserEmail = loggedInUserEmail;

            // Configurar el Timer para el efecto de aparición
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Intervalo en milisegundos
            fadeInTimer.Tick += FadeInTimer_Tick;

            // Actualizar los labels según el tipo de entidad
            if (entityType == "Usuario")
            {
                label1.Text = "Eliminar usuario";
                label2.Text = "¿Estás seguro de eliminar el usuario?";
            }
            else if (entityType == "Incidencia")
            {
                label1.Text = "Eliminar incidencia";
                label2.Text = "¿Estás seguro de eliminar la incidencia?";
            }
        }

        private async void deleteBtn_Click(object sender, EventArgs e)
        {
            string apiUrl = "http://localhost:8080/";

            if (entityType == "Usuario")
            {
                var usuario = (Form2.Usuario)entity;
                if (usuario.email == loggedInUserEmail)
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Error", "No puedes eliminar el usuario con el que has iniciado sesión.", 420);
                    toastForm.Show();
                    return;
                }
                apiUrl += "usuarios/" + usuario.id;
            }
            else if (entityType == "Incidencia")
            {
                var incidencia = (Form2.Incidencia)entity;
                apiUrl += "incidencias/" + incidencia.id;
            }

            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Success", $"{entityType} eliminada correctamente.");
                    toastForm.Show();

                    // Llamar al método para recargar el DataGridView en Form2
                    if (parentForm is Form2 form2)
                    {
                        form2.ReloadDataGrid();
                    }
                    if (entityType == "Usuario")
                    {
                        var usuario = (Form2.Usuario)entity;
                        SendDeleteAccountEmail(usuario.email, usuario.nombre);
                    }
                    this.Close();
                }
                else
                {
                    CloseExistingToast();
                    ToastForm toastForm = new ToastForm(parentForm, "Error", $"Error al eliminar la {entityType.ToLower()}.");
                    toastForm.Show();
                    this.Close();
                }
            }
        }
        private void SendDeleteAccountEmail(string toEmail, string userName)
        {
            try
            {
                var fromAddress = new MailAddress("euskalmove2dam3@gmail.com", "EuskalMove");
                var toAddress = new MailAddress(toEmail, userName);
                const string fromPassword = "vths lmoi ozfu gcdr";
                const string subject = "Confirmación de eliminación de cuenta - EuskalMove";

                // HTML del correo electrónico
                string body = @"
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <title>Cuenta eliminada - EuskalMove</title>
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
            <h1>Cuenta eliminada de <span class=""highlight"">EuskalMove</span></h1>
            <p>Estimado " + userName + @",</p>
            <p>Le confirmamos que su cuenta en EuskalMove ha sido eliminada correctamente.</p>
            <p>Lamentamos verle partir y esperamos que haya disfrutado de nuestros servicios durante su tiempo con nosotros.</p>
            <p>Si ha eliminado su cuenta por error o desea volver a utilizar nuestros servicios en el futuro, no dude en crear una nueva cuenta en cualquier momento.</p>
            <p>Si tiene alguna pregunta o comentario, nuestro equipo de atención al cliente estará encantado de ayudarle.</p>
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
                CloseExistingToast();
                ToastForm toastForm = new ToastForm(parentForm, "Error", "Error al enviar el correo electrónico de confirmación de eliminación de cuenta.");
                toastForm.Show();
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
