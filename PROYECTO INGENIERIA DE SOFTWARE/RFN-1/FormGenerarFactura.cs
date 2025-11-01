using BLL;
using SERVICIOS;
using System;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using BE;
using Microsoft.VisualBasic;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormGenerarFactura : Form,iObserver
    {
        CarritoBLL carritoBLL;
        FacturaBLL facturaBLL;
        ClienteBLL clienteBLL;
        ProductoBLL productoBLL;
        BitacoraEventosBLL bitacoraBLL = new BitacoraEventosBLL();
        string Idioma;
        public FormGenerarFactura()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            carritoBLL = new CarritoBLL();
            facturaBLL = new FacturaBLL();
            clienteBLL = new ClienteBLL();
            productoBLL = new ProductoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)).ListaSeleccionados;
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)) == null)
                {
                    throw new Exception(new IdiomaBLL().Traducir(Idioma, "CarritoNoEncontrado"));
                }
                if (clienteBLL.BuscarCliente(ValidarDNI.Validar(textBox1.Text)) == null)
                {
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "ClienteNoRegistrado"));
                }
                else
                {
                    Mostrar();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            FormConsultarProducto formConsultarProducto = new FormConsultarProducto(carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)));
            this.Hide();
            formConsultarProducto.ShowDialog();
            this.Show();

            BitacoraEvento bitacoraEvento = new BitacoraEvento();
            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
            bitacoraEvento.Fecha = DateTime.Now;
            bitacoraEvento.Modulo = "Ventas";
            bitacoraEvento.Evento = "Agregar Producto";
            bitacoraEvento.Criticidad = 5;
            bitacoraBLL.Registrar(bitacoraEvento);

            Mostrar();
            
        }

        private void btnGenerarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ProductoSeleccionado ps in carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)).ListaSeleccionados)
                {
                    Producto aux = productoBLL.ListaProductos().Find(prod => prod.Codigo == ps.CodigoProducto);
                    if (aux.Cantidad < ps.CantidadProducto)
                    {
                        throw new Exception(new IdiomaBLL().Traducir(Idioma, "CantidadInsuficiente"));
                    }
                }
                FormCobrarVenta form = new FormCobrarVenta(carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)));
                this.Hide();
                form.ShowDialog();
                this.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormRegistrarCliente form = new FormRegistrarCliente();
            this.Hide();
            form.ShowDialog();
            this.Show();
            BitacoraEvento bitacoraEvento = new BitacoraEvento();
            bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
            bitacoraEvento.Fecha = DateTime.Now;
            bitacoraEvento.Modulo = "Ventas";
            bitacoraEvento.Evento = "Registrar Cliente";
            bitacoraEvento.Criticidad = 5;
            bitacoraBLL.Registrar(bitacoraEvento);
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            try
            {
                ProductoSeleccionado a = dataGridView1.SelectedRows[0].DataBoundItem as ProductoSeleccionado;
                a.CantidadProducto = int.Parse(Interaction.InputBox(new IdiomaBLL().Traducir(Idioma, "ModificarCantidad"), new IdiomaBLL().Traducir(Idioma, "ModificarCantidadTitulo")));
                carritoBLL.ModificarProductoCarrito(a, carritoBLL.ObtenerCarrito(ValidarDNI.Validar(textBox1.Text)).Codigo);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Ventas";
                bitacoraEvento.Evento = "Modificar Producto";
                bitacoraEvento.Criticidad = 4;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "GenerarFacturaEspañol";
            }
            else
            {
                Idioma = "GenerarFacturaEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            btnAgregarProducto.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregarProducto");
            button2.Text = new IdiomaBLL().Traducir(Idioma, "button2");
            button1.Text = new IdiomaBLL().Traducir(Idioma, "button1");
            btnGenerarFactura.Text = new IdiomaBLL().Traducir(Idioma, "btnGenerarFactura");
        }
    }
}
