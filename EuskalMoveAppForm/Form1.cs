using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuskalMoveAppForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Establecer el texto de sugerencia para los Guna2TextBox
            guna2TextBox1.PlaceholderText = "example@mail.com";
            guna2TextBox2.PlaceholderText = "Contraseña";
        }
    }
}
