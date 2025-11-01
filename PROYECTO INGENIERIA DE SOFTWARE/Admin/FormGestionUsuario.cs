using BE;
using SERVICIOS.Domain;
using BLL;
using SERVICIOS.BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormGestionUsuario : Form, iObserver
    {
        UserBLL userBLL;
        PermisoBLL permisoBLL;
        BitacoraEventosBLL bitacoraBLL;
        Operacion op;
        string Idioma;
        public FormGestionUsuario()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            userBLL = new UserBLL();
            permisoBLL = new PermisoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = userBLL.RetornarListaUsuarios();
            List<Permiso> ListaPerfiles = permisoBLL.RetornarPerfiles();
            DropdownRol.DataSource = ListaPerfiles;
            ActualizarIdioma(SessionManager.GetInstance.idioma);
            HabilitarControles();
        }
        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = userBLL.RetornarListaUsuarios();
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            VaciarControles();
            HabilitarControles();
            Mostrar();
            label4.Text = new IdiomaBLL().Traducir(Idioma, "OperacionAgregar");
            op = Operacion.Agregar;
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            textDNI.Enabled= false;
            label4.Text = new IdiomaBLL().Traducir(Idioma, "OperacionModificar");
            CargarDatos();
            op = Operacion.Modificar;
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            Usuarios a = dataGridView1.SelectedRows[0].DataBoundItem as Usuarios;
            string Mensaje = new IdiomaBLL().Traducir(Idioma, "OperacionEliminar");
            string mensajeFinal = string.Format(Mensaje, a.Nombre, a.Apellido);
            var x = MessageBox.Show(mensajeFinal,"Borrar Usuario", MessageBoxButtons.YesNo);
            if (x == DialogResult.Yes)
            {
                userBLL.Borrar(a);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Administrador";
                bitacoraEvento.Evento = "Borrar Usuario";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (op == Operacion.Agregar)
                {
                    bool bloqueo = false;
                    bool activo = false;
                    if (rButtonBloqueadoSi.Checked)
                    {
                        bloqueo = true;
                    }
                    else
                    {
                        bloqueo = false;
                    }
                    if (rButtonActivoSi.Checked)
                    {
                        activo = true;
                    }
                    else
                    {
                        activo = false;
                    }
                    Usuarios aux = new Usuarios(
                        ValidarDNI.Validar(textDNI.Text),
                        Validar_NomApe.Validar(textApellido.Text),
                        Validar_NomApe.Validar(textNombre.Text),
                        $"{textNombre.Text}{textDNI.Text}",
                        Encriptador.GetSHA256($"{textApellido.Text}{textDNI.Text}"),
                        DropdownRol.SelectedItem.ToString(),
                        ValidarEmail.Validar(textEmail.Text),
                        bloqueo,
                        activo,
                        "Español"
                        );
                    Usuarios a = userBLL.ValidarDNI(textDNI.Text);
                    if (a != null)
                    {
                        MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "ExcepcionDNI"));
                        Mostrar();
                    }
                    else
                    {
                        userBLL.Agregar(aux);
                        BitacoraEvento bitacoraEvento = new BitacoraEvento();
                        bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                        bitacoraEvento.Fecha = DateTime.Now;
                        bitacoraEvento.Modulo = "Administrador";
                        bitacoraEvento.Evento = "Agregar Usuario";
                        bitacoraEvento.Criticidad = 3;
                        bitacoraBLL.Registrar(bitacoraEvento);
                        Mostrar();
                        VaciarControles();
                    }
                }
                else if (op == Operacion.Modificar)
                {
                    bool bloqueo = false;
                    bool activo = false;
                    if (rButtonBloqueadoSi.Checked)
                    {
                        bloqueo = true;
                    }
                    else
                    {
                        bloqueo = false;
                    }
                    if (rButtonActivoSi.Checked)
                    {
                        activo = true;
                    }
                    else
                    {
                        activo = false;
                    }
                    Usuarios aux = new Usuarios(
                        textDNI.Text,
                        Validar_NomApe.Validar(textApellido.Text),
                        Validar_NomApe.Validar(textNombre.Text),
                        $"{textNombre.Text}{textDNI.Text}",
                        Encriptador.GetSHA256($"{textApellido.Text}{textDNI.Text}"),
                        DropdownRol.SelectedItem.ToString(),
                        ValidarEmail.Validar(textEmail.Text),
                        bloqueo,
                        activo,
                        "Español"
                        );
                    userBLL.Modificar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Administrador";
                    bitacoraEvento.Evento = "Modificar Usuario";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    Mostrar();
                }
                else if (op == Operacion.Consulta)
                {
                    var aux = userBLL.RetornarListaUsuarios().Where(x =>
                    (string.IsNullOrEmpty(textDNI.Text) || x.DNI.StartsWith(textDNI.Text)) &&
                    (string.IsNullOrEmpty(textNombre.Text) || x.Nombre.StartsWith(textNombre.Text)) &&
                    (string.IsNullOrEmpty(textApellido.Text) || x.Apellido.StartsWith(textApellido.Text)) &&
                    (string.IsNullOrEmpty(textEmail.Text) || x.Email.StartsWith(textEmail.Text)) &&
                    (string.IsNullOrEmpty(DropdownRol.SelectedItem?.ToString()) || x.Rol.StartsWith(DropdownRol.SelectedItem.ToString())) &&
                    (rButtonActivoSi.Checked == true && x.Activo == true || rButtonActivoNo.Checked == true && x.Activo == false || rButtonActivoNo.Checked == false && rButtonActivoSi.Checked == false) &&
                    (rButtonBloqueadoSi.Checked == true && x.Bloqueo == true || rButtonBloqueadoNo.Checked == true && x.Bloqueo == false || rButtonBloqueadoNo.Checked == false && rButtonBloqueadoSi.Checked == false)).ToList();
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = aux;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            Usuarios aux = dataGridView1.SelectedRows[0].DataBoundItem as Usuarios;
            aux.Contraseña = Encriptador.GetSHA256( aux.Apellido + aux.DNI);
            userBLL.Desbloquear(aux);

            BitacoraEvento bitacoraEvento = new BitacoraEvento();
            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
            bitacoraEvento.Fecha = DateTime.Now;
            bitacoraEvento.Modulo = "Administrador";
            bitacoraEvento.Evento = "Desbloquear Usuario";
            bitacoraEvento.Criticidad = 3;
            bitacoraBLL.Registrar(bitacoraEvento);

            Mostrar();
        }
        public enum Operacion
        {
            Agregar,
            Borrar,
            Modificar,
            Consulta
        }
        public void CargarDatos()
        {
            Usuarios a = dataGridView1.SelectedRows[0].DataBoundItem as Usuarios;
            textDNI.Text = a.DNI;
            textApellido.Text = a.Apellido;
            textNombre.Text = a.Nombre;
            textEmail.Text = a.Email;
            DropdownRol.Text = a.Rol;
            if (a.Bloqueo == false)
            {
                rButtonBloqueadoNo.Checked = true;
            }
            else
            {
                rButtonBloqueadoSi.Checked = true;
            }
            if (a.Activo == true)
            {
                rButtonActivoSi.Checked = true;
            }
            else
            {
                rButtonActivoNo.Checked = true;
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Mostrar();
            op = Operacion.Consulta;
            label4.Text = new IdiomaBLL().Traducir(Idioma, "OperacionConsulta");
            VaciarControles();
            HabilitarControles();
        }
        private void FormGestionUsuario_Load(object sender, EventArgs e)
        {
            op = Operacion.Consulta;
            label4.Text = new IdiomaBLL().Traducir(Idioma, "OperacionConsulta");
        }
        public void VaciarControles()
        {
            foreach (Control control in groupBox1.Controls.OfType<System.Windows.Forms.TextBox>())
            {
                control.Text = string.Empty;
            }
            DropdownRol.SelectedIndex = -1;
            rButtonActivoNo.Checked = false;
            rButtonActivoSi.Checked = false;
            rButtonBloqueadoNo.Checked = false;
            rButtonBloqueadoSi.Checked=false;
        }
        public void HabilitarControles()
        {
            textApellido.Enabled = true;
            textDNI.Enabled = true;
            textNombre.Enabled = true;
            textEmail.Enabled = true;
            DropdownRol.Enabled = true;
            rButtonActivoNo.Enabled = true;
            rButtonActivoSi.Enabled = true;
            rButtonBloqueadoNo.Enabled = true;
            rButtonBloqueadoSi .Enabled = true;

        }
        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "GestionUsuarioEspañol";
            }
            else
            {
                Idioma = "GestionUsuarioEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            label5.Text = new IdiomaBLL().Traducir(Idioma, "label5");
            label6.Text = new IdiomaBLL().Traducir(Idioma, "label6");
            groupBox2.Text = new IdiomaBLL().Traducir(Idioma, "groupBox2");
            groupBox3.Text = new IdiomaBLL().Traducir(Idioma, "groupBox3");
            rButtonBloqueadoSi.Text = new IdiomaBLL().Traducir(Idioma, "rButtonBloqueadoSi");
            rButtonBloqueadoNo.Text = new IdiomaBLL().Traducir(Idioma, "rButtonBloqueadoNo");
            rButtonActivoSi.Text = new IdiomaBLL().Traducir(Idioma, "rButtonActivoSi");
            rButtonActivoNo.Text = new IdiomaBLL().Traducir(Idioma, "rButtonActivoNo");
            btnAgregar.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregar");
            btnModificar.Text = new IdiomaBLL().Traducir(Idioma, "btnModificar");
            btnBorrar.Text = new IdiomaBLL().Traducir(Idioma, "btnEliminar");
            btnDesbloquear.Text = new IdiomaBLL().Traducir(Idioma, "btnDesbloquear");
            btnGuardar.Text = new IdiomaBLL().Traducir(Idioma, "btnGuardar");
            btnCancelar.Text = new IdiomaBLL().Traducir(Idioma, "btnCancelar");
        }
    }
}
