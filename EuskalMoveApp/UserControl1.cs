﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EuskalMoveApp
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            // Asociar el evento MouseDown al método UserControl1_MouseDown
            this.MouseDown += new MouseEventHandler(UserControl1_MouseDown);
        }

        // Importar funciones de la API de Windows para mover la ventana
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Minimizar la ventana
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.WindowState = FormWindowState.Minimized;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Maximizar o restaurar la ventana
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                if (parentForm.WindowState == FormWindowState.Maximized)
                {
                    parentForm.WindowState = FormWindowState.Normal;
                }
                else
                {
                    parentForm.WindowState = FormWindowState.Maximized;
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // Cerrar la ventana
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Close();
            }
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            // Mover la ventana
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.FindForm().Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
    }
}
