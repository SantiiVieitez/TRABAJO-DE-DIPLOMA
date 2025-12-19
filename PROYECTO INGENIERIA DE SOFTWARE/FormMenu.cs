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
        string idioma;
        public FormMenu()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            ActualizarIdioma(SessionManager.GetInstance.idioma);
            permisoBLL = new PermisoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            if (SessionManager.GetInstance.Usuario != null)
            {
                ActualizarEstado();
                SessionManager.GetInstance.Usuario.Perfil = permisoBLL.RetornarPerfilUsuario(SessionManager.GetInstance.Usuario.Rol);
            }
        }

        public void ActualizarEstado()
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
            if (perfil == null || permisosAValidar == null) return false;
            if (perfil.Permisos == null) perfil.Permisos = new List<Permiso>();

            var permisosExistentes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // ✅ Agrego el nombre del perfil (familia raíz)
            if (!string.IsNullOrWhiteSpace(perfil.Nombre))
                permisosExistentes.Add(perfil.Nombre);

            AgregarPermisosRecursivos(perfil, permisosExistentes);

            foreach (var permiso in permisosAValidar)
                if (!permisosExistentes.Contains(permiso)) return false;

            return true;
        }
        private void AgregarPermisosRecursivos(Familia familia, HashSet<string> permisosExistentes)
        {
            foreach (var permiso in familia.Permisos)
            {
                permisosExistentes.Add(permiso.Nombre);
                if (permiso is Familia subFamilia)
                {
                    AgregarPermisosRecursivos(subFamilia, permisosExistentes);
                }
            }
        }
        private void gestionUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "AgregarUsuario", "BorrarUsuario", "ModificarUsuario" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if(flag == true)
            {
            FormGestionUsuario formGestionUsuario = new FormGestionUsuario();
            this.Hide();
            formGestionUsuario.ShowDialog();
                this.Show();    
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }

        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            if (SessionManager.GetInstance.Usuario != null)
            {
                if (Form.ActiveForm == this)
                {
                    ActualizarEstado();
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
            solicitudDeCotizacionToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "solicitudDeCotizacionToolStripMenuItem");
            ordenesDeCompraToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "ordenesDeCompraToolStripMenuItem");
            almacenToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "almacenToolStripMenuItem");
            productosCToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "productosCToolStripMenuItem");
            bitacoraEventosToolStripMenuItem.Text = new IdiomaBLL().Traducir(idioma, "bitacoraEventosToolStripMenuItem");
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
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
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "GestionProveedores" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormRegistrarProveedor form = new FormRegistrarProveedor();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void solicitudDeCotizacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "GestionSolicitudCotizacion", "GestionProveedores" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormSolicitudCotizacion form = new FormSolicitudCotizacion();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void ordenesDeCompraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "GestionOrdenesDeCompra" ,"GestionProveedores" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormOrdenesDeCompra form = new FormOrdenesDeCompra();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void almacenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "GestionAlmacen" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormAlmacen form = new FormAlmacen();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void reporte1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "Ventas" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormReporte1 form = new FormReporte1();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void reporte2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "Compras" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormReporte2 form = new FormReporte2();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void productosCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> permisosAValidar = new List<string> { "BitacoraDeCambios" };
            bool flag = ValidarPermisos(SessionManager.GetInstance.Usuario.Perfil, permisosAValidar);
            if (flag == true)
            {
                FormBitacoraCambio form = new FormBitacoraCambio();
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            else
            {
                string texto = new IdiomaBLL().Traducir(idioma, "FaltanPermisos");
                MessageBox.Show(texto);
            }
            
        }

        private void FormMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            string titulo = new IdiomaBLL().Traducir(idioma, "CerrarMenuTitulo");
            string texto = new IdiomaBLL().Traducir(idioma, "CerrarMenu");
            var result = MessageBox.Show(texto,
                                 titulo,
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
