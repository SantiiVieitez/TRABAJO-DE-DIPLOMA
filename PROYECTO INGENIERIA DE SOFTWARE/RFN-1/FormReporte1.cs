using BLL;
using BE;
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
    public partial class FormReporte1 : Form
    {
        FacturaBLL facturaBLL;
        public FormReporte1()
        {
            InitializeComponent();
            facturaBLL = new FacturaBLL();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = facturaBLL.RetornarFacturas();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Factura aux = dataGridView1.SelectedRows[0].DataBoundItem as Factura;
            facturaBLL.GenerarFactura(aux);
        }
    }
}
