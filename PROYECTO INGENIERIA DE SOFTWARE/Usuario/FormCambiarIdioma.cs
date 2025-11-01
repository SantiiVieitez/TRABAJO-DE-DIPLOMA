using SERVICIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormCambiarIdioma : Form,iObserver
    {
        string Idioma;
        public FormCambiarIdioma()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "CambiarIdiomaEspañol";
            }
            else
            {
                Idioma = "CambiarIdiomaEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            button1.Text = new IdiomaBLL().Traducir(Idioma, "button1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem == null || comboBox1.SelectedItem.ToString() == "")
                {
                    MessageBox.Show("Por favor seleccione una opción");
                    return;
                }

                Idioma pIdioma = new Idioma(comboBox1.SelectedItem.ToString());

                SessionManager.GetInstance.Usuario.Idioma = comboBox1.SelectedItem.ToString();
                SessionManager.GetInstance.CambiarIdioma(pIdioma);
                new UserBLL().CambiarIdioma(SessionManager.GetInstance.Usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
