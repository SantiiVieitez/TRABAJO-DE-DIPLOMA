using BE;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.RFN_2
{
    public partial class FormPagarFactura : Form
    {
        OrdenDeCompra OrdenDeCompra;
        FacturaP_BLL facturapBLL;
        BitacoraEventosBLL bitacoraBLL;
        public FormPagarFactura(OrdenDeCompra p)
        {
            InitializeComponent();
            facturapBLL = new FacturaP_BLL();
            bitacoraBLL = new BitacoraEventosBLL();
            OrdenDeCompra = p;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = p.ListaProductos;
            decimal total = 0;
            foreach (ProductoSeleccionado x in p.ListaProductos)
            {
                total = x.PrecioProducto * x.CantidadProducto;
            }
            label4.Text = $"Total = ${total}";
        }
        public void ValidarCampos()
        {
            Regex re = new Regex(@"^[0-9]{16}$");
            if (!re.IsMatch(textNumeroTarjeta.Text))
            {
                throw new Exception("El número de tarjeta debe tener exactamente 16 dígitos.");
            }
            if (!DateTime.TryParse(textFechaVencimiento.Text, out DateTime fecha))
            {
                throw new Exception("La fecha de vencimiento no tiene un formato válido.");
            }
            if (comboTarjeta.SelectedItem == null ||
            (comboTarjeta.SelectedItem.ToString() != "Tarjeta Crédito" && comboTarjeta.SelectedItem.ToString() != "Tarjeta Débito"))
            {
                throw new Exception("Por favor, seleccione un tipo de tarjeta válido (Crédito o Débito).");
            }
            re = new Regex(@"^[0-9]{3}$");
            if (!re.IsMatch(textCodigoSeguridad.Text))
            {
                throw new Exception("El código de seguridad debe tener exactamente 3 dígitos.");
            }
            Validar_NomApe.Validar(textNombreTitular.Text);
        }
        public void ManejoDeControles(bool Valor)
        {
            textNumeroTarjeta.Enabled = Valor;
            textCodigoSeguridad.Enabled = Valor;
            textFechaVencimiento.Enabled = Valor;
            comboTarjeta.Enabled = Valor;
            textNombreTitular.Enabled = Valor;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ManejoDeControles(true);
            comboBox1.Enabled = false;
            textTitularDNI.Enabled = false;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ManejoDeControles(false);
            comboBox1.Enabled = true;
            textTitularDNI.Enabled = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    ValidarCampos();
                    FacturaP factura = new FacturaP();
                    factura.MetodoDePago = comboTarjeta.SelectedItem.ToString();
                    factura.NombreComprador = "RepuestoMaster";
                    factura.Fecha = DateTime.Now;
                    factura.ListaProductos = OrdenDeCompra.ListaProductos;
                    factura.ID_OrdenDeCompra = OrdenDeCompra.ID;
                    factura.NombreVendedor = OrdenDeCompra.NombreEmpresa;
                    factura.ID = facturapBLL.GenerarID();
                    facturapBLL.RegistrarFacturaP(factura);
                    MessageBox.Show("Pago realizado con Exito");

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Compras";
                    bitacoraEvento.Evento = "Pagar Orden de Compra";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    this.Close();

                    
                }
                else
                {
                    Regex re = new Regex(@"^\d{22}$");
                    if (!re.IsMatch(textTitularDNI.Text))
                    {
                        throw new Exception("El CBU debe tener exactamente 22 dígitos.");
                    }
                    if (comboBox1.SelectedItem == null)
                    {
                        throw new Exception("Por favor, seleccione un Banco primero.");
                    }
                    FacturaP factura = new FacturaP();
                    factura.MetodoDePago = "Transferencia Bancaria";
                    factura.NombreComprador = "RepuestoMaster";
                    factura.Fecha = DateTime.Now;
                    factura.ID_OrdenDeCompra = OrdenDeCompra.ID;
                    factura.ListaProductos = OrdenDeCompra.ListaProductos;
                    factura.NombreVendedor = OrdenDeCompra.NombreEmpresa;
                    factura.ID = facturapBLL.GenerarID();
                    facturapBLL.RegistrarFacturaP(factura);
                    MessageBox.Show("Transferencia realizada con Exito");

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Compras";
                    bitacoraEvento.Evento = "Pagar Orden de Compra";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    this.Close();
                }
            }
            catch (Exception){ throw;}
            
        }
    }
}
