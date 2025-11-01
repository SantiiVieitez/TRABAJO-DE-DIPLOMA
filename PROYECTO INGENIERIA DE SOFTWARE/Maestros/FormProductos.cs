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

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormProductos : Form,iObserver
    {
        Operacion OperacionActual;
        ProductoBLL ProductoBLL;
        BitacoraEventosBLL bitacoraBLL;
        string Idioma;
        public FormProductos()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            ProductoBLL = new ProductoBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            OperacionActual = new Operacion();
            textID.Enabled = false;
            Mostrar();
            
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "FormProductosEspañol";
            }
            else
            {
                Idioma = "FormProductosEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "titulo");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            label5.Text = new IdiomaBLL().Traducir(Idioma, "label5");
            label6.Text = new IdiomaBLL().Traducir(Idioma, "label6");
            label7.Text = new IdiomaBLL().Traducir(Idioma, "label7");
            label8.Text = new IdiomaBLL().Traducir(Idioma, "label8");
            label9.Text = new IdiomaBLL().Traducir(Idioma, "label9");
            label10.Text = new IdiomaBLL().Traducir(Idioma, "label10");
            button1.Text = new IdiomaBLL().Traducir(Idioma, "button1");
            button2.Text = new IdiomaBLL().Traducir(Idioma, "button2");
            button3.Text = new IdiomaBLL().Traducir(Idioma, "button3");
            button4.Text = new IdiomaBLL().Traducir(Idioma, "button4");
        }
        enum Operacion
        {
            Agregar,
            Modificar
        }
        public void VaciarControles()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
            }
        }
        public void Mostrar()
        {
            var productos = ProductoBLL.ListaProductos();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productos.Where(p => !p.BorradoLogico).ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OperacionActual = Operacion.Agregar;
            label10.Text = new IdiomaBLL().Traducir(Idioma, "OperacionAgregar");
            VaciarControles();
            Mostrar();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new InvalidOperationException(new IdiomaBLL().Traducir(Idioma, "ExceptionNoSeleccinado")); 
                }
                Producto aux = dataGridView1.SelectedRows[0].DataBoundItem as Producto;
                string mensaje = new IdiomaBLL().Traducir(Idioma, "OperacionEliminar");
                string mensajeFinal = string.Format(mensaje, aux.Nombre);
                var x = MessageBox.Show(mensajeFinal, new IdiomaBLL().Traducir(Idioma, "OperacionEliminarTitulo"), MessageBoxButtons.YesNo);
                if (x == DialogResult.Yes)
                {
                    ProductoBLL.Borrar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Maestros";
                    bitacoraEvento.Evento = "Borrar Producto";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    Mostrar();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new InvalidOperationException(new IdiomaBLL().Traducir(Idioma, "ExceptionNoSeleccinado"));
                }

                label10.Text = new IdiomaBLL().Traducir(Idioma, "OperacionModificar");
                OperacionActual = Operacion.Modificar;

                Producto aux = dataGridView1.SelectedRows[0].DataBoundItem as Producto;
                textNombre.Text = aux.Nombre;
                textMarca.Text = aux.Marca;
                comboRepuesto.Text = aux.TipoDeRepuesto;
                textCantidad.Text = aux.Cantidad.ToString();
                comboVehiculo.Text = aux.TipoDeVehiculo;
                textMaterial.Text = aux.Material;
                textPrecio.Text = aux.Precio.ToString();
                textDescripcion.Text = aux.Descripcion;
                textID.Text = aux.Codigo;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (OperacionActual == Operacion.Agregar)
            {
                string id = ProductoBLL.GenerarID();
                Producto aux = new Producto(id,textNombre.Text,textMarca.Text,comboRepuesto.SelectedItem.ToString(),int.Parse(textCantidad.Text),comboVehiculo.SelectedItem.ToString(),textMaterial.Text,decimal.Parse(textPrecio.Text),textDescripcion.Text);
                ProductoBLL.Agregar(aux);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Maestros";
                bitacoraEvento.Evento = "Agregar Producto";
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
            else if (OperacionActual == Operacion.Modificar)
            {
                Producto aux = new Producto(textID.Text,textNombre.Text, textMarca.Text, comboRepuesto.SelectedItem.ToString(), int.Parse(textCantidad.Text), comboVehiculo.SelectedItem.ToString(), textMaterial.Text, decimal.Parse(textPrecio.Text), textDescripcion.Text);
                ProductoBLL.Modificar(aux);

                BitacoraEvento bitacoraEvento = new BitacoraEvento();
                bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                bitacoraEvento.Fecha = DateTime.Now;
                bitacoraEvento.Modulo = "Maestros";
                bitacoraEvento.Evento = "Modificar Producto";         
                bitacoraEvento.Criticidad = 3;
                bitacoraBLL.Registrar(bitacoraEvento);

                Mostrar();
            }
        }
    }
}
