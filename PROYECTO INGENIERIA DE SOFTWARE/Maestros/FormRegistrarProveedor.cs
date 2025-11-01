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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.RFN_2
{
    public partial class FormRegistrarProveedor : Form
    {
        ProveedorBLL proveedorBLL;
        BitacoraEventosBLL bitacoraBLL;
        Opcion op;
        public FormRegistrarProveedor()
        {
            InitializeComponent();
            proveedorBLL = new ProveedorBLL();
            bitacoraBLL = new BitacoraEventosBLL();
            Mostrar();
        }
        public enum Opcion
        {
            agregar,
            modificar,
            nada
        }
        public void Mostrar()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = proveedorBLL.ListarProveedores();
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
        }
        public void VaciarControles()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = "";
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            op = Opcion.agregar;
            label7.Text = "Opcion: Registrar Proveedor";
            textBox2.Enabled = true;
            VaciarControles();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    throw new Exception("Por favor seleccione un Proveedor.");
                }
                Proveedor aux = dataGridView1.SelectedRows[0].DataBoundItem as Proveedor;
                DialogResult a = MessageBox.Show($"Proveedor seleccionado: {aux.Nombre} ¿Desea Eliminarlo?", "Eliminar Proveedor", MessageBoxButtons.YesNo);
                if (a == DialogResult.Yes)
                {
                    proveedorBLL.BorrarProveedor(aux);
                    MessageBox.Show("Proveedor eliminado con exito");
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
                    throw new Exception("Por favor seleccione un Proveedor.");
                }
                op = Opcion.modificar;
                label7.Text = "Opcion: Modificar Proveedor";
                Proveedor aux = dataGridView1.SelectedRows[0].DataBoundItem as Proveedor;
                textBox1.Text = aux.Nombre;
                textBox2.Text = aux.CUIT;
                textBox3.Text = aux.Telefono.ToString();
                textBox4.Text = aux.Email;
                textBox5.Text = aux.CBU.ToString();
                textBox6.Text = aux.Banco;
                textBox2.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();
                if (radioButton1.Checked)
                {
                    if (op == Opcion.agregar)
                    {
                        Proveedor aux = new Proveedor(textBox2.Text, textBox1.Text, textBox4.Text, long.Parse(textBox3.Text)
                                                     , textBox5.Text, textBox6.Text);
                        proveedorBLL.RegistrarProveedor(aux, false);
                        MessageBox.Show("Proveedor registrado con exito");
                        VaciarControles();
                        textBox2.Enabled = true;
                        op = Opcion.nada;

                        BitacoraEvento bitacoraEvento = new BitacoraEvento();
                        bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                        bitacoraEvento.Fecha = DateTime.Now;
                        bitacoraEvento.Modulo = "Compras";
                        bitacoraEvento.Evento = "Registrar Proveedor";
                        bitacoraEvento.Criticidad = 5;
                        bitacoraBLL.Registrar(bitacoraEvento);

                        Mostrar();


                    }
                    else if (op == Opcion.modificar)
                    {
                        Proveedor aux = new Proveedor(textBox2.Text, textBox1.Text, textBox4.Text, long.Parse(textBox3.Text)
                                                     , textBox5.Text, textBox6.Text);
                        proveedorBLL.ModificarProveedor(aux, false);
                        MessageBox.Show("Proveedor modificado con exito");
                        VaciarControles();
                        textBox2.Enabled = true;
                        op = Opcion.nada;

                        BitacoraEvento bitacoraEvento = new BitacoraEvento();
                        bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                        bitacoraEvento.Fecha = DateTime.Now;
                        bitacoraEvento.Modulo = "Compras";
                        bitacoraEvento.Evento = "Modificar Proveedor";
                        bitacoraEvento.Criticidad = 5;
                        bitacoraBLL.Registrar(bitacoraEvento);

                        Mostrar();
                    }
                }
                else
                {
                    if (op == Opcion.agregar)
                    {
                        Proveedor aux = new Proveedor(textBox2.Text, textBox1.Text, textBox4.Text, long.Parse(textBox3.Text));
                        proveedorBLL.RegistrarProveedor(aux, true);
                        MessageBox.Show("Proveedor registrado con exito");
                        VaciarControles();
                        textBox2.Enabled = true;
                        op = Opcion.nada;

                        BitacoraEvento bitacoraEvento = new BitacoraEvento();
                        bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                        bitacoraEvento.Fecha = DateTime.Now;
                        bitacoraEvento.Modulo = "Compras";
                        bitacoraEvento.Evento = "Registrar Proveedor";
                        bitacoraEvento.Criticidad = 5;
                        bitacoraBLL.Registrar(bitacoraEvento);

                        Mostrar();
                    }
                    else if (op == Opcion.modificar)
                    {
                        Proveedor aux = new Proveedor(textBox2.Text, textBox1.Text, textBox4.Text, long.Parse(textBox3.Text));
                        proveedorBLL.ModificarProveedor(aux, true);
                        MessageBox.Show("Proveedor modificado con exito");
                        VaciarControles();
                        textBox2.Enabled = true;
                        op = Opcion.nada;

                        BitacoraEvento bitacoraEvento = new BitacoraEvento();
                        bitacoraEvento.Usuario = SessionManager.GetInstance.Usuario.Login;
                        bitacoraEvento.Fecha = DateTime.Now;
                        bitacoraEvento.Modulo = "Compras";
                        bitacoraEvento.Evento = "Modificar Proveedor";
                        bitacoraEvento.Criticidad = 5;
                        bitacoraBLL.Registrar(bitacoraEvento);

                        Mostrar();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                throw new Exception("Por favor complete todos los campos obligatorios.");
            }
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                throw new Exception("Por favor seleccione una opción.");
            }
        }
    }
}
