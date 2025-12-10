using BLL;
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

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.Admin
{
    public partial class FormBackup : Form, iObserver
    {
        BackUpBLL BackupBLL;
        IdiomaBLL idiomaBLL;

        private const string DB_NAME_NEGOCIO = "Ingenieria de Software";
        private const string DB_NAME_AUTH = "AuthDB";

        string idioma;
        public FormBackup()
        {
            InitializeComponent();
            BackupBLL = new BackUpBLL();
            idiomaBLL = new IdiomaBLL();
            SessionManager.GetInstance.SuscribirObservador(this);
            ActualizarIdioma(SessionManager.GetInstance.idioma);

            rbNegocio.Checked = true;
        }

        public void ActualizarIdioma(Idioma Idioma)
        {
            if (Idioma.Nombre == "Español")
            {
                idioma = "BackupEspañol";
            }
            else
            {
                idioma = "BackupEnglish";
            }
            label1.Text = idiomaBLL.Traducir(idioma, "label1");
            label2.Text = idiomaBLL.Traducir(idioma, "label2");

            button2.Text = idiomaBLL.Traducir(idioma, "button2");
            button4.Text = idiomaBLL.Traducir(idioma, "button4");

            rbNegocio.Text = idiomaBLL.Traducir(idioma, "rbNegocio");
            rbAuth.Text = idiomaBLL.Traducir(idioma, "rbAuth");
        }

        private string ObtenerBaseSeleccionada()
        {
            if (rbAuth.Checked)
            {
                return DB_NAME_AUTH;
            }
            else
            {
                return DB_NAME_NEGOCIO;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = FolderBrowser.SelectedPath;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    string mensajeruta = idiomaBLL.Traducir(idioma, "ExcepcionRuta");
                    throw new Exception(mensajeruta);
                }

                string baseDatos = ObtenerBaseSeleccionada();

                BackupBLL.RealizarBackUp(textBox1.Text, baseDatos);
                string mensaje = idiomaBLL.Traducir(idioma, "MensajeBackup");
                string mensaje2 = idiomaBLL.Traducir(idioma, "MensajeBackup2");
                MessageBox.Show($"{mensaje} '{baseDatos}' {mensaje2}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e) 
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    throw new Exception("Por favor ingrese una ruta de archivo .bak");
                }

                string baseDatos = ObtenerBaseSeleccionada();
                DialogResult result = MessageBox.Show(
                    $"¿Está seguro que desea restaurar la base de datos '{baseDatos}'? Esto sobrescribirá los datos actuales.",
                    "Advertencia",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    BackupBLL.RestaurarBackUp(textBox2.Text, baseDatos);
                    MessageBox.Show("Restauración terminada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
