using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
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
    public partial class FormConsultarProducto : Form,iObserver
    {
        ProductoBLL productoBLL;
        CarritoBLL carritoBLL;
        Carrito CarritoGeneral;
        string Idioma;
        public FormConsultarProducto(Carrito pCarrito)
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            productoBLL = new ProductoBLL();
            carritoBLL = new CarritoBLL();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productoBLL.ListaProductos();
            CarritoGeneral = pCarrito;
            
            ActualizarIdioma(SessionManager.GetInstance.idioma);
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

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = aux;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            
        }

        private void btnAgregarAlCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                Producto pAux = dataGridView1.SelectedRows[0].DataBoundItem as Producto;
                ProductoSeleccionado ps = new ProductoSeleccionado(pAux, int.Parse(numericUpDown1.Value.ToString()));
                CarritoGeneral.ListaSeleccionados = new List<ProductoSeleccionado>();
                foreach (ProductoSeleccionado ProdSelc in CarritoGeneral.ListaSeleccionados)
                {
                    if (ProdSelc.CodigoProducto == ps.CodigoProducto)
                    {
                        throw new Exception(new IdiomaBLL().Traducir(Idioma, "ElProductoYaExiste")); 
                    }
                }
                carritoBLL.AgregarProductoCarrito(CarritoGeneral.Codigo, ps);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void ActualizarIdioma(Idioma idioma)
        {
            if ( idioma.Nombre == "Español")
            {
                Idioma = "ConsultarProductoEspañol";
            }
            else
            {
                Idioma = "ConsultarProductoEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            btnAgregarAlCarrito.Text = new IdiomaBLL().Traducir(Idioma, "btnAgregarAlCarrito");
        }
    }
}
