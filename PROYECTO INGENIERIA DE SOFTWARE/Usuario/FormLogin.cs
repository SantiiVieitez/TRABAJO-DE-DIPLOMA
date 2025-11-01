using SERVICIOS.BLL;
using SERVICIOS.Domain;
using BLL;
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
    public partial class FormLogin : Form, iObserver
    {
        UserBLL userBLL;
        BitacoraEventosBLL bitacoraBLL;
        public FormLogin()
        {
            InitializeComponent();
            
            userBLL = new UserBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            textBox1.Text = "santiv";
            textBox2.Text = "12345";
            button1_Click(null, null);
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                userBLL.IniciarSesion(textBox1.Text,Encriptador.GetSHA256(textBox2.Text));
                if (SessionManager.GetInstance.Usuario != null)
                {
                    //MessageBox.Show("Sesion iniciada con exito");
                    SessionManager.GetInstance.Usuario.Perfil = new PermisoBLL().RetornarPerfilUsuario(SessionManager.GetInstance.Usuario.Rol);
                    this.Hide();
                    FormMenu fm = new FormMenu();
                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Usuario";
                    bitacoraEvento.Evento = "Login";
                    bitacoraEvento.Criticidad = 1;
                    bitacoraBLL.Registrar(bitacoraEvento);
                    fm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Contraseña o usuario incorrecto");
                }
                textBox1.Text = null;
                textBox2.Text = null;
                this.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
