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
        public FormBackup()
        {
            InitializeComponent();
            BackupBLL = new BackUpBLL();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    throw new Exception("Ingrese una ruta porfavor");
                }
                else
                {
                    BackupBLL.RealizarBackUp(textBox1.Text);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
            if(FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = FolderBrowser.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.SafeFileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox2.Text == "")
                {
                    throw new Exception("Porfavor ingrese una ruta");
                }
                else
                {
                    BackupBLL.RestaurarBackUp(textBox2.Text);
                    MessageBox.Show("Terminado");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
