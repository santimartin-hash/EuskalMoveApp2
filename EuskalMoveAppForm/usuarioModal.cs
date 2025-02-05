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

            if (string.IsNullOrWhiteSpace(email.Text))
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

