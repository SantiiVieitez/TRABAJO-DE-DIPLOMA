using BE;
using BLL;
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
    public partial class FormReporte2 : Form
    {
        FacturaP_BLL facturaP_BLL;
        public FormReporte2()
        {
            InitializeComponent();
            facturaP_BLL = new FacturaP_BLL();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = facturaP_BLL.RetornarFacturasP();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FacturaP aux = dataGridView1.SelectedRows[0].DataBoundItem as FacturaP;

            SaveFileDialog guardarDialogo = new SaveFileDialog();
            string nombreSugerido = $"{aux.ID}_{DateTime.Now.ToString("yyyyMMdd")}.pdf";
            guardarDialogo.FileName = nombreSugerido;
            guardarDialogo.Filter = "Archivos PDF (*.pdf)|*.pdf";
            guardarDialogo.Title = "Guardar Factura";

            if (guardarDialogo.ShowDialog() == DialogResult.OK)
            {
                facturaP_BLL.GenerarFacturaP(aux, guardarDialogo.FileName);

                MessageBox.Show("Factura generada con éxito.");
            }
        }
    }
}
