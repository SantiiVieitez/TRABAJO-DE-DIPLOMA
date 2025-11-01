using BE;
using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using Microsoft.VisualBasic;
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
    public partial class FormCargarCarrito : Form, iObserver
    {
        CarritoBLL carritoBLL;
        BitacoraEventosBLL bitacoraBLL;
        Carrito aux;
        string Idioma;
        public FormCargarCarrito()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            carritoBLL = new CarritoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            aux = new Carrito();
            
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "CargarCarritoEspañol";
            }
            else
            {
                Idioma = "CargarCarritoEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            btnAsignar.Text = new IdiomaBLL().Traducir(Idioma, "btnAsignar");
            btnAgregarProductoCarrito.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregarProductoCarrito");
            btnEliminar.Text = new IdiomaBLL().Traducir(Idioma, "btnEliminar");
            btnModificar.Text = new IdiomaBLL().Traducir(Idioma, "btnModificar");
        }

        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = carritoBLL.ObtenerCarrito(aux.ClienteDNI).ListaSeleccionados;
        }

        private void btnAgregarProductoCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                FormConsultarProducto form = new FormConsultarProducto(aux);
                form.ShowDialog();

                var carrito = carritoBLL.ObtenerCarrito(aux.ClienteDNI);

                if (carrito != null && carrito.ListaSeleccionados != null)
                {
                    Mostrar();
                }

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Ventas";
                bitacoraEvento.Evento = "Agregar Producto Carrito";
                bitacoraEvento.Criticidad = 5;
                bitacoraBLL.Registrar(bitacoraEvento);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                if(carritoBLL.BuscarDNI(textBox1.Text) == false)
                {
                    aux.ClienteDNI = ValidarDNI.Validar(textBox1.Text);
                    carritoBLL.Agregar(aux);
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "DNIAsignadoCorrectamente"), new IdiomaBLL().Traducir(Idioma, "DNIAsignadoTitulo"), MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "DNIExistente"));
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ProductoSeleccionado ps = dataGridView1.SelectedRows[0].DataBoundItem as ProductoSeleccionado;
            carritoBLL.BorrarProductoCarrito(ps.CodigoProducto);
            Mostrar();
            BitacoraEvento bitacoraEvento = new BitacoraEvento();
            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
            bitacoraEvento.Fecha = DateTime.Now;
            bitacoraEvento.Modulo = "Ventas";
            bitacoraEvento.Evento = "Borrar Producto Carrito";
            bitacoraEvento.Criticidad = 5;
            bitacoraBLL.Registrar(bitacoraEvento);
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            ProductoSeleccionado ps = dataGridView1.SelectedRows[0].DataBoundItem as ProductoSeleccionado;
            ps.CantidadProducto = int.Parse(Interaction.InputBox(new IdiomaBLL().Traducir(Idioma, "IngresarCantidadDeseada")));
            carritoBLL.ModificarProductoCarrito(ps, aux.Codigo);
            Mostrar();
            BitacoraEvento bitacoraEvento = new BitacoraEvento();
            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
            bitacoraEvento.Fecha = DateTime.Now;
            bitacoraEvento.Modulo = "Ventas";
            bitacoraEvento.Evento = "Modificar Producto Carrito";
            bitacoraEvento.Criticidad = 5;
            bitacoraBLL.Registrar(bitacoraEvento);
        }
    }
}
