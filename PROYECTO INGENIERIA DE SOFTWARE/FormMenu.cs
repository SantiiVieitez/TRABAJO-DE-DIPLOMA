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
using Microsoft.VisualBasic;
using SERVICIOS;
using PROYECTO_INGENIERIA_DE_SOFTWARE.Admin;
using PROYECTO_INGENIERIA_DE_SOFTWARE.RFN_2;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormMenu : Form, iObserver
    {
        PermisoBLL permisoBLL;
        BitacoraEventosBLL bitacoraBLL;
        public FormMenu()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            ActualizarIdioma(SessionManager.GetInstance.idioma);
            permisoBLL = new PermisoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            if (SessionManager.GetInstance.Usuario != null)
            {
                ActulizarEstado();
                SessionManager.GetInstance.Usuario.Perfil = permisoBLL.RetornarPerfilUsuario(SessionManager.GetInstance.Usuario.Rol);
            }
        }

        public void ActulizarEstado()
        {
            if(SessionManager.GetInstance.Usuario.Login != null)
            {
                aToolStripMenuItem.Text = SessionManager.GetInstance.Usuario.Login;
            }
            else
            {
                aToolStripMenuItem.Text = "";
            }
            
        }
        public bool ValidarPermisos(Familia perfil, List<string> permisosAValidar)
        {
            if (perfil == null || perfil.Permisos == null || permisosAValidar == null)
            {
                return false;
            }

            // Crear un HashSet para los permisos encontrados
            var permisosExistentes = new HashSet<string>();
            AgregarPermisosRecursivos(perfil, permisosExistentes);

            foreach (var permiso in permisosAValidar)
            {
                if (!permisosExistentes.Contains(permiso))
                {
                    return false; // Si falta uno, devuelve false
                }
            }

            return true; // Si todos están presentes, devuelve true
        }
        private void AgregarPermisosRecursivos(Familia familia, HashSet<string> permisosExistentes)
        {
            foreach (var permiso in familia.Permisos)
            {
                permisosExistentes.Add(permiso.Nombre);
                if (permiso is Familia subFamilia)
                {
                    AgregarPermisosRecursivos(subFamilia, permisosExistentes); // Llamada recursiva para permisos compuestos
                }
            }
        }
        private void gestionUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGestionUsuario formGestionUsuario = new FormGestionUsuario();
            this.Hide();
            formGestionUsuario.ShowDialog();
            this.Show();
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            if (SessionManager.GetInstance.Usuario != null)
            {
                if (Form.ActiveForm == this)
                {
                    ActulizarEstado();
                }
            }
            else
            {
                login.ShowDialog();
            }
        }
        private void OpenLoginForm()
        {
            FormLogin login = new FormLogin();
            login.FormClosed += new FormClosedEventHandler(LoginForm_FormClosed);
            login.Show();
        }
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("Desea cerrar sesion / Close Session?", "Cerrar Sesion / Close Session", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(r == DialogResult.Yes)
            {
                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Usuario";
                bitacoraEvento.Evento = "Logout";
                bitacoraEvento.Criticidad = 1;
                bitacoraBLL.Registrar(bitacoraEvento);
                SessionManager.Logout();
                this.Close();
            }
        }
        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formCambiarClave formCambiarClave = new formCambiarClave();
            this.Hide();
            formCambiarClave.ShowDialog();
            this.Show();
        }
        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> {"AgregarCliente", "BorrarCliente", "ModificarCliente"};
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if(flag == true)
            {
                FormRegistrarCliente form = new FormRegistrarCliente();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
            
        }
        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "AgregarProducto", "BorrarProducto", "ModificarProducto" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if(flag == true)
            {
                FormProductos form = new FormProductos();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
            
        }
        private void carritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "AgregarProductoCarrito", "BorrarProductoCarrito", "ModificarProductoCarrito", "ConsultarProducto"};
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormCargarCarrito form = new FormCargarCarrito();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
            
        }
        private void facturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "GenerarFactura", "CobrarVenta" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormGenerarFactura form = new FormGenerarFactura();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
            
        }
        private void perfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "AgregarFamilia", "AgregarPerfil", "BorrarFamilia", "BorrarPerfil", "AsignarPermiso", "QuitarPermiso" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            //bool flag = true;
            if (flag == true)
            {
                FormPerfiles form = new FormPerfiles();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
        }
        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCambiarIdioma form = new FormCambiarIdioma();
            this.Hide();
            form.ShowDialog();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
            this.Show();
        }
        public void ActualizarIdioma(Idioma Idioma)
        {
            string idioma;
            if(Idioma.Nombre == "Español")
            {
                idioma = "FormMenuEspañol";
            }
            else
            {
                idioma = "FormMenuEnglish";
            }

            this.Text = new IdiomaBLL().Traducir(idioma, "titulo");
            sesionToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "sesionToolStripMenuItem");
            adminToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "adminToolStripMenuItem");
            gestionUsuarioToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "gestionUsuariosToolStripMenuItem");
            perfilesToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "perfilesToolStripMenuItem");
            maestrosToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "maestrosToolStripMenuItem");
            productosToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "productosToolStripMenuItem");
            clientesToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "clientesToolStripMenuItem");
            proveedoresToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "proveedoresToolStripMenuItem");
            usuarioToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "usuarioToolStripMenuItem");
            loginToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "loginToolStripMenuItem");
            logoutToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "logoutToolStripMenuItem");
            cambiarClaveToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "cambiarClaveToolStripMenuItem");
            cambiarIdiomaToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "cambiarIdiomarToolStripMenuItem");
            ventasToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "ventasToolStripMenuItem");
            carritoToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "carritoToolStripMenuItem");
            facturaToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "facturaToolStripMenuItem");
            comprasToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "comprasToolStripMenuItem");
            reportesToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "reportesToolStripMenuItem");
            reporte1ToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "reporte1ToolStripMenuItem");
            reporte2ToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "reporte2ToolStripMenuItem");

        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "BitacoraEventos" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormBitacoraEvento form = new FormBitacoraEvento();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
        }
        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "Backup" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormBackup form = new FormBackup();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Faltan uno o mas Permisos en el perfil");
            }
            
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRegistrarProveedor form = new FormRegistrarProveedor();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void solicitudDeCotizacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSolicitudCotizacion form = new FormSolicitudCotizacion();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void ordenesDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOrdenesDeCompra form = new FormOrdenesDeCompra();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void almacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAlmacen form = new FormAlmacen();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void reporte1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReporte1 form = new FormReporte1();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void reporte2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReporte2 form = new FormReporte2();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void productosCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBitacoraCambio form = new FormBitacoraCambio();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void FormMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea cerrar la sesión actual?",
                                 "Confirmación de cierre de sesión",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                SessionManager.Logout();
                
            }
        }
    }
}
