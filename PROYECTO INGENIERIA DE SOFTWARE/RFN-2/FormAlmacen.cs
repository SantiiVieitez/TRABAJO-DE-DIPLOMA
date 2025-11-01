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
    public partial class FormAlmacen : Form
    {
        FacturaP_BLL facturapBLL;
        ProductoBLL productoBLL;
        BitacoraEventosBLL bitacoraBLL;
        public FormAlmacen()
        {
            InitializeComponent();
            facturapBLL = new FacturaP_BLL();
            productoBLL = new ProductoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            Mostrar();
        }
        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = facturapBLL.RetornarFacturasP();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                FacturaP p = dataGridView1.SelectedRows[0].DataBoundItem as FacturaP;
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = p.ListaProductos;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new Exception("Porfavor seleccione un Pedido");
                }
                FacturaP p = dataGridView1.SelectedRows[0].DataBoundItem as FacturaP;
                if (p.Recibido == true)
                {
                    throw new Exception("Los productos ya se recibieron");
                }
                p.Recibido = true;
                facturapBLL.Modificar(p);
                foreach (ProductoSeleccionado x in p.ListaProductos)
                {
                    Producto aux = productoBLL.BuscarProducto(x.CodigoProducto);
                    aux.Cantidad += x.CantidadProducto;
                    productoBLL.Modificar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Compras";
                    bitacoraEvento.Evento = "Actualizar Stock";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);
                }
                Mostrar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }
    }
}