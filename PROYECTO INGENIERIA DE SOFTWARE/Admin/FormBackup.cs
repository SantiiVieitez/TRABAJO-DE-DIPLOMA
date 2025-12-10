using BLL;
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
    public partial class FormBackup : Form
    {
        BackUpBLL BackupBLL;

        private const string DB_NAME_NEGOCIO = "Ingenieria de Software";
        private const string DB_NAME_AUTH = "AuthDB";
        public FormBackup()
        {
            InitializeComponent();
            BackupBLL = new BackUpBLL();

            rbNegocio.Checked = true;
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
        private void button2_Click(object sender, EventArgs e) // REALIZAR BACKUP
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    throw new Exception("Ingrese una ruta por favor");
                }

                string baseDatos = ObtenerBaseSeleccionada();

                // Pasamos la ruta y el nombre de la base de datos a la BLL
                BackupBLL.RealizarBackUp(textBox1.Text, baseDatos);

                MessageBox.Show($"Backup de la base '{baseDatos}' realizado con éxito.");
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

        private void button4_Click(object sender, EventArgs e) // REALIZAR RESTORE
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    throw new Exception("Por favor ingrese una ruta de archivo .bak");
                }

                string baseDatos = ObtenerBaseSeleccionada();

                // Confirmación de seguridad (Opcional pero recomendado)
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
