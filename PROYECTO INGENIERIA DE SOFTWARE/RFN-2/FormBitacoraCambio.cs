using BE;
using BE.RFN_2;
using BLL;
using BLL.RFN_2;
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
    public partial class FormBitacoraCambio : Form
    {
        ProductoC_BLL productocBLL;
        ProductoBLL productoBLL;
        private bool isUpdating = false;

        public FormBitacoraCambio()
        {
            InitializeComponent();
            productocBLL = new ProductoC_BLL();
            productoBLL = new ProductoBLL();
            CargarComboBox();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productocBLL.RetonarProductoC().OrderByDescending(p => p.Fecha).ThenByDescending(p => p.Hora).ToList();
        }
        public void CargarComboBox()
        {
            List<Producto> ListaProductos = productoBLL.ListaProductos();
            foreach (Producto p in ListaProductos)
            {
                comboBox1.Items.Add(p.Codigo);
                comboBox2.Items.Add(p.Nombre);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = dateTimePicker1.Value.Date;
                DateTime fechaFin = dateTimePicker2.Value.Date;
                string nombre = comboBox2.Text.Trim();
                string codigoProducto = comboBox1.Text.Trim();

                var listaFiltrada = productocBLL.RetonarProductoC()
                .Where(p =>
                    (string.IsNullOrEmpty(nombre) || p.Nombre.Contains(nombre)) &&
                    (string.IsNullOrEmpty(codigoProducto) || p.CodigoProducto.Contains(codigoProducto)) &&
                    (p.Fecha >= fechaInicio && p.Fecha <= fechaFin)
                )
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.Hora)
                .ToList();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = listaFiltrada;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox1.SelectedItem = null;
                comboBox2.SelectedItem = null;
                isUpdating = true;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                isUpdating = false;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = productocBLL.RetonarProductoC().OrderByDescending(p => p.Fecha).ThenByDescending(p => p.Hora).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                ProductoC aux = dataGridView1.SelectedRows[0].DataBoundItem as ProductoC;
                if (aux.Activo == true)
                {
                    throw new Exception("El producto ya se encuentra activo.");
                }
                productocBLL.ActivarProductoC(aux);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = productocBLL.RetonarProductoC().OrderByDescending(p => p.Fecha).ThenByDescending(p => p.Hora).ToList();
            }
            catch (Exception)
            { 
                throw;
            }
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            if (isUpdating) return;

            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Error de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdating = true;
                dateTimePicker1.Value = dateTimePicker2.Value;
                isUpdating = false;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;

            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("La fecha de fin no puede ser menor que la fecha de inicio.", "Error de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdating = true;
                dateTimePicker2.Value = dateTimePicker1.Value;
                isUpdating = false;
            }
        }
    }
}   
