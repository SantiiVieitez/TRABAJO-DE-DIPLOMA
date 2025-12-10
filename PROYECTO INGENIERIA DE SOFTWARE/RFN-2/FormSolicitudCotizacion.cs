using BE;
using BLL;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.RFN_2
{
    public partial class FormSolicitudCotizacion : Form, iObserver
    {
        ProveedorBLL proveedorBLL;
        CotizacionBLL cotizacionBLL;
        ProductoBLL productoBLL;
        BitacoraEventosBLL bitacoraBLL;
        List<ProductoSeleccionado> Productos;
        string idioma;
        IdiomaBLL idiomaBLL;

        public FormSolicitudCotizacion()
        {
            InitializeComponent();
            idiomaBLL = new IdiomaBLL();
            productoBLL = new ProductoBLL();
            proveedorBLL = new ProveedorBLL();
            cotizacionBLL = new CotizacionBLL();
            List<Producto> ListaProductos = new List<Producto>();
            bitacoraBLL = new BitacoraEventosBLL();
            ListaProductos = productoBLL.ListaProductos();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
            SessionManager.GetInstance.SuscribirObservador(this);
            Productos = new List<ProductoSeleccionado>();
            foreach (Producto p in ListaProductos)
            {
                p.Precio = p.Precio * 0.8m;
            }

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ListaProductos;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = proveedorBLL.ListarProveedores();
            dataGridView3.DataSource = null;
            dataGridView3.DataSource = cotizacionBLL.RetornarCotizaciones();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var aux = productoBLL.ListaProductos().Where(x =>
                (string.IsNullOrEmpty(textBox1.Text)) ||
                (x.Nombre != null && x.Nombre.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (x.Marca != null && x.Marca.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (x.TipoDeRepuesto != null && x.TipoDeRepuesto.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (x.TipoDeVehiculo != null && x.TipoDeVehiculo.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (x.Material != null && x.Material.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (x.Descripcion != null && x.Descripcion.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

                dataGridView2.DataSource = null;
                dataGridView2.DataSource = aux;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormRegistrarProveedor form = new FormRegistrarProveedor();
            this.Hide();
            form.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione un proveedor.");
                }
                Cotizacion cotizacion = new Cotizacion();
                cotizacion.CotizacionID = cotizacionBLL.CalcularID();
                cotizacion.proveedor = dataGridView1.SelectedRows[0].DataBoundItem as Proveedor;
                cotizacion.NombreProveedor = cotizacion.proveedor.Nombre;
                
                if (Productos == null)
                {
                    throw new Exception("Porfavor seleccione productos");
                }
                cotizacion.Productos = Productos;
                cotizacion.Fecha = DateTime.Now;
                cotizacionBLL.Registrar(cotizacion);
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = cotizacionBLL.RetornarCotizaciones();
                Productos.Clear();
                dataGridView4.DataSource = null;

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Compras";
                bitacoraEvento.Evento = "Generar Solicitud";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }   
        }

        private void btnAgregarAlCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    Producto pAux = dataGridView2.SelectedRows[0].DataBoundItem as Producto;
                    ProductoSeleccionado ps = new ProductoSeleccionado(pAux, int.Parse(numericUpDown1.Value.ToString()));
                    Productos.Add(ps);
                    dataGridView4.DataSource = null;
                    dataGridView4.DataSource = Productos;
                }
                else
                {
                    throw new Exception("Seleccione una fila porfavor");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView4.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione un Producto.");
                }
                ProductoSeleccionado aux = dataGridView4.SelectedRows[0].DataBoundItem as ProductoSeleccionado;
                Productos.Remove(aux);
                dataGridView4.DataSource = null;
                dataGridView4.DataSource = Productos;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView4.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione un Producto.");
                }
                ProductoSeleccionado aux = dataGridView4.SelectedRows[0].DataBoundItem as ProductoSeleccionado;
                Productos.Find(p => p.CodigoProducto == aux.CodigoProducto).CantidadProducto = int.Parse(Interaction.InputBox("Ingrese la nueva Cantidad"));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione una cotizacion.");
                }

                Cotizacion a = dataGridView3.SelectedRows[0].DataBoundItem as Cotizacion;
                cotizacionBLL.BorrarCotizacion(a);
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = cotizacionBLL.RetornarCotizaciones();

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Compras";
                bitacoraEvento.Evento = "Borrar Solicitud";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione una cotizacion.");
                }
                Cotizacion a = dataGridView3.SelectedRows[0].DataBoundItem as Cotizacion;
                a.proveedor = dataGridView1.SelectedRows[0].DataBoundItem as Proveedor;
                a.NombreProveedor = a.proveedor.Nombre;
                a.Productos = Productos;
                a.Fecha = DateTime.Now;
                cotizacionBLL.ActualizarCotizacion(a);
                dataGridView3.DataSource = null;
                dataGridView3.DataSource = cotizacionBLL.RetornarCotizaciones();

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Compras";
                bitacoraEvento.Evento = "Actualizar Solicitud";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var aux = dataGridView3.SelectedRows[0].DataBoundItem as Cotizacion;
            if (aux != null) 
            {
                dataGridView5.DataSource = null;
                dataGridView5.DataSource = aux.Productos;
            }
        }

        public void ActualizarIdioma(Idioma Idioma)
        {
            if (Idioma.Nombre == "Español")
            {
                idioma = "SolicitudDeCotizacionEspañol";
            }
            else
            {
                idioma = "SolicitudDeCotizacionEnglish";
            }

            label1.Text = idiomaBLL.Traducir(idioma, "label1");
            label2.Text = idiomaBLL.Traducir(idioma, "label2");
            label3.Text = idiomaBLL.Traducir(idioma, "label3");
            label4.Text = idiomaBLL.Traducir(idioma, "label4");
            label5.Text = idiomaBLL.Traducir(idioma, "label5");
            label6.Text = idiomaBLL.Traducir(idioma, "label6");
            label7.Text = idiomaBLL.Traducir(idioma, "label7");
            button1.Text = idiomaBLL.Traducir(idioma, "button1");
            button2.Text = idiomaBLL.Traducir(idioma, "button2");
            button3.Text = idiomaBLL.Traducir(idioma, "button3");
            button4.Text = idiomaBLL.Traducir(idioma, "button4");
            button5.Text = idiomaBLL.Traducir(idioma, "button5");
            button6.Text = idiomaBLL.Traducir(idioma, "button6");
            btnAgregarAlCarrito.Text = idiomaBLL.Traducir(idioma, "btnAgregarAlCarrito");
        }
    }
}
