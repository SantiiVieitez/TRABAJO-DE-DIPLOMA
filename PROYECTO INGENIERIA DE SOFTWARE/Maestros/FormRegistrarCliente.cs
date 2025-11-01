using BE;
using BLL;
using SERVICIOS.BLL;
using SERVICIOS.Domain;
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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE
{
    public partial class FormRegistrarCliente : Form, iObserver
    {
        Operacion OperacionActual;
        ClienteBLL ClienteBLL;
        BitacoraEventosBLL bitacoraBLL;
        SerializerBLL SerializerBLL;
        string Idioma;
        public FormRegistrarCliente()
        {
            InitializeComponent();
            SessionManager.GetInstance.SuscribirObservador(this);
            OperacionActual = new Operacion();
            ClienteBLL = new ClienteBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            SerializerBLL = new SerializerBLL();
            Mostrar();
            ActualizarIdioma(SessionManager.GetInstance.idioma);
        }
        public void ActualizarIdioma(Idioma idioma)
        {
            if (idioma.Nombre == "Español")
            {
                Idioma = "RegistrarClienteEspañol";
            }
            else
            {
                Idioma = "RegistrarClienteEnglish";
            }
            this.Text = new IdiomaBLL().Traducir(Idioma, "FormRegistrarCliente");
            label1.Text = new IdiomaBLL().Traducir(Idioma, "label1");
            label2.Text = new IdiomaBLL().Traducir(Idioma, "label2");
            label3.Text = new IdiomaBLL().Traducir(Idioma, "label3");
            label4.Text = new IdiomaBLL().Traducir(Idioma, "label4");
            label5.Text = new IdiomaBLL().Traducir(Idioma, "label5");
            label6.Text = new IdiomaBLL().Traducir(Idioma, "label6");
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
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ClienteBLL.ListaClientes();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label10.Text = new IdiomaBLL().Traducir(Idioma, "OperacionAgregar");
                OperacionActual = Operacion.Agregar;
                VaciarControles();
                Mostrar();
                textBox3.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new Exception(new IdiomaBLL().Traducir(Idioma, "SeleccionarCliente"));
                }
                Cliente aux = dataGridView1.SelectedRows[0].DataBoundItem as Cliente;

                string Mensaje = new IdiomaBLL().Traducir(Idioma, "OperacionEliminar");
                string MensajeFinal = string.Format(Mensaje, aux.Nombre, aux.Apellido);

                var x = MessageBox.Show(MensajeFinal, new IdiomaBLL().Traducir(Idioma, "OperacionEliminarTitulo"), MessageBoxButtons.YesNo);
                if (x == DialogResult.Yes)
                {
                    ClienteBLL.Borrar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Maestros";
                    bitacoraEvento.Evento = "Borrar Cliente";
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
                    throw new Exception(new IdiomaBLL().Traducir(Idioma, "SeleccionarCliente"));
                }
                else if (dataGridView1.SelectedRows.Count > 1)
                {
                    throw new Exception("Seleccionar solo 1 Cliente");
                }
                label10.Text = new IdiomaBLL().Traducir(Idioma, "OperacionModificar");
                OperacionActual = Operacion.Modificar;
                Cliente aux = dataGridView1.SelectedRows[0].DataBoundItem as Cliente;
                textBox1.Text = aux.Nombre;
                textBox2.Text = aux.Apellido;
                textBox3.Text = aux.DNI;
                textBox4.Text = aux.Correo;
                textBox5.Text = aux.Domicilio;
                textBox6.Text = aux.Telefono.ToString();
                textBox3.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (OperacionActual == Operacion.Agregar)
                {
                    Cliente aux = new Cliente(Validar_NomApe.Validar(textBox1.Text), Validar_NomApe.Validar(textBox2.Text), ValidarDNI.Validar(textBox3.Text), textBox5.Text, ValidarEmail.Validar(textBox4.Text), int.Parse(textBox6.Text));
                    ClienteBLL.Agregar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Maestros";
                    bitacoraEvento.Evento = "Agregar Cliente";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    Mostrar();
                }
                else if (OperacionActual == Operacion.Modificar)
                {
                    Cliente aux = new Cliente(Validar_NomApe.Validar(textBox1.Text), Validar_NomApe.Validar(textBox2.Text), textBox3.Text, textBox5.Text, ValidarEmail.Validar(textBox4.Text), int.Parse(textBox6.Text));
                    ClienteBLL.Modificar(aux);

                    BitacoraEvento bitacoraEvento = new BitacoraEvento();
                    bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                    bitacoraEvento.Fecha = DateTime.Now;
                    bitacoraEvento.Modulo = "Maestros";
                    bitacoraEvento.Evento = "Modificar Cliente";
                    bitacoraEvento.Criticidad = 3;
                    bitacoraBLL.Registrar(bitacoraEvento);

                    Mostrar();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = saveFileDialog.FileName;
            }
        }
        private void MostrarArchivoSerializado(string path)
        {
            listBox1.Items.Clear();

            string[] lineas = File.ReadAllLines(path);

            foreach (string linea in lineas)
            {
                listBox1.Items.Add(linea);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                List<Cliente> ListaAux = new List<Cliente>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    Cliente cliente = (Cliente)row.DataBoundItem;
                    ListaAux.Add(cliente);
                }
                if (textBox7.Text == null)
                {
                    throw new Exception("Seleccione una ruta para guardar el archivo");
                }
                SerializerBLL.SerializarXML(ListaAux, textBox7.Text);
                MostrarArchivoSerializado(textBox7.Text);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files|*.xml";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox8.Text = openFileDialog.FileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = textBox8.Text;
                List<Cliente> clientes = SerializerBLL.DeserializarXML(filePath);
                listBox1.Items.Clear();
                foreach (Cliente p in clientes)
                {
                    listBox1.Items.Add($"Nombre: {p.Nombre}");
                    listBox1.Items.Add($"Apellido: {p.Apellido}");
                    listBox1.Items.Add($"DNI: {p.DNI}");
                    listBox1.Items.Add($"Domicilio: {p.Domicilio}");
                    listBox1.Items.Add($"Correo: {p.Correo}");
                    listBox1.Items.Add($"Teléfono: {p.Telefono}");
                    listBox1.Items.Add("--------------------------");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

