using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using Microsoft.VisualBasic.ApplicationServices;
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

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class formCambiarClave : Form, iObserver
    {
        UserBLL userBLL;
        BitacoraEventosBLL bitacoraBLL;
        string Idioma;
        public formCambiarClave()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            userBLL = new UserBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string ContraseñaVieja = textBox1.Text;
            string ContraseñaNueva = textBox2.Text;
            string ConfirmarContraseña = textBox3.Text;

            if(SessionManager.GetInstance.Usuario.Contraseña == Encriptador.GetSHA256(ContraseñaVieja))
            {
                if(ContraseñaNueva == ConfirmarContraseña)
                {
                    Usuarios aux = userBLL.BuscarUsuario(SessionManager.GetInstance.Usuario.Login);
                    aux.Contraseña = Encriptador.GetSHA256(ContraseñaNueva);
                    userBLL.Modificar(aux);
                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Usuario";
                    bitacoraEvento.Evento = "Cambiar Clave";
                    bitacoraEvento.Criticidad = 2;
                    bitacoraBLL.Registrar(bitacoraEvento);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "ContraseñasIncorrectas"));
                }
            }
            else
            {
                MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "ContraseñaActualIncorrecta"));
            }
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "CambiarClaveEspañol";
            }
            else
            {
                Idioma = "CambiarClaveEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            button1.Text = new IdiomaBLL().Traducir(Idioma, "button1");
        }
    }
}
