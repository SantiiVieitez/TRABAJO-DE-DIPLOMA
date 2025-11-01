using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.BLL;
using SERVICIOS.Domain;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormCobrarVenta : Form,iObserver
    {
        Carrito carrito;
        CarritoBLL CarritoBLL;
        FacturaBLL facturaBLL;
        ProductoBLL productoBLL;
        BitacoraEventosBLL bitacoraBLL;
        string Idioma;
        public FormCobrarVenta(Carrito pCarrito)
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            facturaBLL = new FacturaBLL();
            CarritoBLL = new CarritoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            productoBLL = new ProductoBLL();
            carrito = pCarrito;
            CalcularTotal();
            ManejoDeControles(false);
            Mostrar();
            
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = carrito.ListaSeleccionados;
        }
        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (ProductoSeleccionado item in carrito.ListaSeleccionados)
            {
                if(item.CantidadProducto > 1)
                {
                    total += item.PrecioProducto * item.CantidadProducto;
                }
                else
                {
                    total += item.PrecioProducto;
                }
            }
            label3.Text = $"Total = ${total}";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ManejoDeControles(true);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ManejoDeControles(false);
        }
        public void ManejoDeControles(bool Valor)
        {
            textNumeroTarjeta.Enabled = Valor;
            textCodigoSeguridad.Enabled = Valor;
            textFechaVencimiento.Enabled = Valor;
            comboTarjeta.Enabled = Valor;
            textNombreTitular.Enabled = Valor;
            textTitularDNI.Enabled = Valor;
        }
        private void btnCobrar(object sender, EventArgs e)
        {
            try
            {
                if(radioButton1.Checked)
                {
                    ValidarCampos();
                    Factura factura = new Factura();
                    factura.ID = int.Parse(facturaBLL.GenerarID());
                    factura.MetodoDePago = comboTarjeta.Text;
                    factura.DNI_Cliente = carrito.ClienteDNI;
                    factura.Fecha = DateTime.Now.Date;
                    factura.ListaSeleccionados = carrito.ListaSeleccionados;
                    facturaBLL.AgregarFactura(factura);
                    facturaBLL.AgregarProductoFactura(factura);
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "VentaRealizada"));
                    facturaBLL.GenerarFactura(factura);
                    foreach(ProductoSeleccionado ps in CarritoBLL.ObtenerCarrito(carrito.ClienteDNI).ListaSeleccionados)
                    {
                        Producto aux = productoBLL.ListaProductos().Find(prod => prod.Codigo == ps.CodigoProducto);
                        aux.Cantidad -= ps.CantidadProducto;
                        productoBLL.Modificar(aux);
                    }

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Ventas";
                    bitacoraEvento.Evento = "Generar Factura";
                    bitacoraEvento.Criticidad = 4;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    this.Close();
                }
                else
                {
                    Factura factura = new Factura();
                    factura.ID = int.Parse(facturaBLL.GenerarID());
                    factura.MetodoDePago = radioButton2.Text;
                    factura.DNI_Cliente = carrito.ClienteDNI;
                    factura.Fecha = DateTime.Now;
                    factura.ListaSeleccionados = carrito.ListaSeleccionados;
                    facturaBLL.AgregarFactura(factura);
                    facturaBLL.AgregarProductoFactura(factura);
                    MessageBox.Show(new IdiomaBLL().Traducir(Idioma, "VentaRealizada"));
                    facturaBLL.GenerarFactura(factura);
                    foreach (ProductoSeleccionado ps in CarritoBLL.ObtenerCarrito(carrito.ClienteDNI).ListaSeleccionados)
                    {
                        Producto aux = productoBLL.ListaProductos().Find(prod => prod.Codigo == ps.CodigoProducto);
                        aux.Cantidad -= ps.CantidadProducto;
                        productoBLL.Modificar(aux);
                    }

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Ventas";
                    bitacoraEvento.Evento = "Generar Factura";
                    bitacoraEvento.Criticidad = 4;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    this.Close();
                } 
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void ValidarCampos()
        {
            Regex re = new Regex(@"^[0-9]{16}$");
            textNumeroTarjeta.Text = re.IsMatch(textNumeroTarjeta.Text) ? textNumeroTarjeta.Text : throw new Exception("Formato o numero incorrecto");
            DateTime.TryParse(textFechaVencimiento.Text, out DateTime fecha);
            textFechaVencimiento.Text = fecha != DateTime.MinValue ? textFechaVencimiento.Text : throw new Exception("Formato o fecha incorrecta");
            re = new Regex(@"^[0-9]{3}$");
            textCodigoSeguridad.Text = re.IsMatch(textCodigoSeguridad.Text) ? textCodigoSeguridad.Text : throw new Exception("Formato o numero incorrecto");
            Validar_NomApe.Validar(textNombreTitular.Text);
            ValidarDNI.Validar(textTitularDNI.Text);
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            if ( idioma.Nombre == "Español")
            {
                Idioma = "CobrarVentaEspañol";
            }
            else
            {
                Idioma = "CobrarVentaEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            radioButton1.Text = new IdiomaBLL().Traducir(Idioma, "radioButton1");
            radioButton2.Text = new IdiomaBLL().Traducir(Idioma, "radioButton2");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label5.Text = new IdiomaBLL().Traducir(Idioma, "label5");
            label6.Text = new IdiomaBLL().Traducir(Idioma, "label6");
            label7.Text = new IdiomaBLL().Traducir(Idioma, "label7");
            label8.Text = new IdiomaBLL().Traducir(Idioma, "label8");
            button4.Text = new IdiomaBLL().Traducir(Idioma, "button4");
        }
    }
}
