using BE;
using BLL;
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

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.RFN_2
{
    public partial class FormOrdenesDeCompra : Form
    {
        ProveedorBLL proveedorBLL;
        CotizacionBLL cotizacionBLL;
        OrdenDeCompraBLL ordenDeCompraBLL;
        BitacoraEventosBLL bitacoraBLL;
        public FormOrdenesDeCompra()
        {
            InitializeComponent();
            proveedorBLL = new ProveedorBLL();
            cotizacionBLL = new CotizacionBLL();
            ordenDeCompraBLL = new OrdenDeCompraBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            LlenarComboBox();
        }
        public void LlenarComboBox()
        {
            List<Proveedor> listaproveedores = proveedorBLL.ListarProveedores();
            comboBox1.DataSource = listaproveedores;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Proveedor p = comboBox1.SelectedItem as Proveedor;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = cotizacionBLL.RetornarCotizacionesProveedor(p.CUIT);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new Exception("Porfavor seleccione una cotizacion");
                }
                Cotizacion cotizacion = dataGridView1.SelectedRows[0].DataBoundItem as Cotizacion;
                
                if (cotizacion.proveedor.Banco == "" && cotizacion.proveedor.CBU == "")
                {
                    throw new Exception("Porfavor realizar el registro completo del proveedor");
                }
                OrdenDeCompra orden = new OrdenDeCompra();
                orden.ID = ordenDeCompraBLL.GenerarID();
                orden.Fecha = DateTime.Now;
                orden.NombreEmpresa = cotizacion.NombreProveedor;
                orden.CUIT = cotizacion.proveedor.CUIT;
                orden.ListaProductos = cotizacion.Productos;
                ordenDeCompraBLL.RegistrarCompra(orden);
                FormPagarFactura form = new FormPagarFactura(orden);
                this.Hide();
                form.ShowDialog();

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Compras";
                bitacoraEvento.Evento = "Generar Orden de Compra";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);
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
    }
}
