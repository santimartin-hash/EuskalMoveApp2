using System;
using System.Drawing;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class ToastForm : Form
    {
        private Form form2;
        private Timer showTimer;
        private Timer hideTimer;
        private int targetY;
        private int recorteOffset = 2; // Ajusta este valor según sea necesario
        private Panel leftLinePanel;
        private int customWidth;
        public ToastForm(Form form2, string type, string description, int width = 0)
        {
            InitializeComponent();
            this.form2 = form2;
            this.form2.LocationChanged += Form2_LocationOrSizeChanged;
            this.form2.SizeChanged += Form2_LocationOrSizeChanged;
            this.form2.FormClosed += Form2_FormClosed; // Suscribirse al evento FormClosed

            showTimer = new Timer();
            showTimer.Interval = 10;
            showTimer.Tick += ShowTimer_Tick;

            hideTimer = new Timer();
            hideTimer.Interval = 5000; // 5 segundos
            hideTimer.Tick += HideTimer_Tick;

            // Habilitar doble memoria intermedia
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            // Configurar el formulario para que no aparezca en la barra de tareas
            this.ShowInTaskbar = false;
            this.Owner = form2;

            // Crear y configurar el panel que simula la línea de color a la izquierda
            leftLinePanel = new Panel
            {
                Width = 5, // Ajusta el ancho según sea necesario
                Dock = DockStyle.Left
            };
            this.Controls.Add(leftLinePanel);

            // Configurar los labels y el PictureBox según los parámetros
            Tipo.Text = type;
            Descripcion.Text = description;
            ConfigureToastAppearance(type);

            // Establecer el ancho personalizado si se proporciona
            if (width > 0)
            {
                this.customWidth = width;
                this.Width = width;
            }
            else
            {
                this.customWidth = this.Width; // Mantener el ancho predeterminado
            }
        }

        private void ConfigureToastAppearance(string type)
        {
            switch (type.ToLower())
            {
                case "error":
                    leftLinePanel.BackColor = Color.FromArgb(223, 87, 87);
                    pictureBox1.Image = Properties.Resources.ErrorIcon; // Asegúrate de tener esta imagen en Resources
                    break;
                case "warning":
                    leftLinePanel.BackColor = Color.FromArgb(223, 154, 87);
                    pictureBox1.Image = Properties.Resources.WarningIcon; // Asegúrate de tener esta imagen en Resources
                    break;
                case "info":
                    leftLinePanel.BackColor = Color.FromArgb(27, 151, 243);
                    pictureBox1.Image = Properties.Resources.InfoIcon; // Asegúrate de tener esta imagen en Resources
                    break;
                case "success":
                    leftLinePanel.BackColor = Color.FromArgb(67, 171, 75);
                    pictureBox1.Image = Properties.Resources.SuccessIcon; // Asegúrate de tener esta imagen en Resources
                    break;
            }
        }

        private void Form2_LocationOrSizeChanged(object sender, EventArgs e)
        {
            Position();
            UpdateRegion();
            this.BringToFront();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close(); // Cerrar el ToastForm cuando Form2 se cierra
        }

        private void form_Load(object sender, EventArgs e)
        {
            Position();
            this.Location = new Point(this.Location.X, form2.Location.Y + form2.Height); // Inicialmente debajo de Form2
            showTimer.Start();
        }

        private void Position()
        {
            int form2Right = form2.Location.X + form2.Width;
            int form2Bottom = form2.Location.Y + form2.Height;

            int toastX = form2Right - this.Width - 10;
            targetY = form2Bottom - this.Height - 10;

            this.Location = new Point(toastX, targetY);
        }

        private void ShowTimer_Tick(object sender, EventArgs e)
        {
            if (this.Location.Y > targetY)
            {
                this.Location = new Point(this.Location.X, this.Location.Y - 5);
                UpdateRegion();
            }
            else
            {
                showTimer.Stop();
                hideTimer.Start();
            }
        }

        private void HideTimer_Tick(object sender, EventArgs e)
        {
            hideTimer.Stop();
            Timer moveDownTimer = new Timer();
            moveDownTimer.Interval = 10;
            moveDownTimer.Tick += (s, args) =>
            {
                if (this.Location.Y < form2.Location.Y + form2.Height)
                {
                    this.Location = new Point(this.Location.X, this.Location.Y + 5);
                    UpdateRegion();
                }
                else
                {
                    moveDownTimer.Stop();
                    this.Close();
                }
            };
            moveDownTimer.Start();
        }

        private void UpdateRegion()
        {
            int form2Bottom = form2.Location.Y + form2.Height;
            int visibleHeight = Math.Max(0, form2Bottom - this.Location.Y - recorteOffset);
            if (visibleHeight > this.Height)
            {
                visibleHeight = this.Height;
            }

            Rectangle visibleRect = new Rectangle(0, 0, this.Width, visibleHeight);
            this.Region = new Region(visibleRect);
        }
    }
}
