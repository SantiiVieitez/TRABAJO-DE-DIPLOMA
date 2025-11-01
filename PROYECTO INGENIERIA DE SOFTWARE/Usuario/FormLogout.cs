using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormLogout : Form
    {
        public FormLogout()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SessionManager.Logout();
            MessageBox.Show("Sesion cerrada");
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
