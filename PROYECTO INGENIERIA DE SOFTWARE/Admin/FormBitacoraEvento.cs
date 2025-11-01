using SERVICIOS.Domain;
using SERVICIOS.BLL;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;

namespace PROYECTO_INGENIERIA_DE_SOFTWARE.Admin
{
    public partial class FormBitacoraEvento : Form
    {
        UserBLL userBLL;
        BitacoraEventosBLL bitacoraEventosBLL;
        List<BitacoraEvento> resultados;
        private bool isUpdating = false;
        public FormBitacoraEvento()
        {
            InitializeComponent();
            bitacoraEventosBLL = new BitacoraEventosBLL();
            userBLL = new UserBLL();
            List<Usuarios> ListaUsuarios = new List<Usuarios>();
            List<BitacoraEvento> resultados = new List<BitacoraEvento>();
            ListaUsuarios = userBLL.RetornarListaUsuarios();
            foreach(Usuarios x in ListaUsuarios)
            {
                comboBox1.Items.Add(x.Login);
            }
            Mostrar();
        }
        public void Mostrar()
        {
            var bitacoraEventos = bitacoraEventosBLL.RetornarBitacora();
            var bitacoraEventosOrdenados = bitacoraEventos.OrderBy(x => x.Fecha).ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bitacoraEventosOrdenados;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<BitacoraEvento> ListaBitacora = bitacoraEventosBLL.RetornarBitacora();

                resultados = ListaBitacora.Where(x =>
                (string.IsNullOrEmpty(comboBox1.Text) || x.Usuario == comboBox1.Text) &&
                (x.Fecha >= dateTimePicker1.Value && x.Fecha <= dateTimePicker2.Value) &&
                (string.IsNullOrEmpty(comboBox2.Text) || x.Modulo == comboBox2.Text) &&
                (string.IsNullOrEmpty(comboBox3.Text) || x.Evento == comboBox3.Text) &&
                (comboBox4.SelectedIndex == -1 || (int.TryParse(comboBox4.Text, out int criticidad) && x.Criticidad == criticidad))
                ).ToList();

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = resultados;

                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var bitacoraEventos = bitacoraEventosBLL.RetornarBitacora();
            var bitacoraEventosOrdenados = bitacoraEventos.OrderBy(x => x.Fecha).ToList();
            resultados = bitacoraEventos.OrderBy(x => x.Fecha).ToList();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bitacoraEventosOrdenados;
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            comboBox4.SelectedItem = null;
            isUpdating = true;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            isUpdating = false;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string NombreArchivo = $"Bitacora{DateTime.Now.ToString("dd-MM-yyyy-HH-mm")}";
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header()
                        .Row(row =>
                        {
                            row.ConstantItem(100).Image("Imagenes\\Logo250.jpg");
                            row.RelativeItem().AlignRight().Text(text =>
                            {
                                text.Span("RepuestoMaster").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                            });
                        });
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Login");
                                    header.Cell().Element(CellStyle).Text("Fecha");
                                    header.Cell().Element(CellStyle).Text("Modulo");
                                    header.Cell().Element(CellStyle).Text("Evento");
                                    header.Cell().Element(CellStyle).Text("Criticidad");
                                });

                                foreach (BitacoraEvento p in resultados)
                                {
                                    table.Cell().Element(CellStyle).Text(p.Usuario);
                                    table.Cell().Element(CellStyle).Text(p.Fecha);
                                    table.Cell().Element(CellStyle).Text(p.Modulo);
                                    table.Cell().Element(CellStyle).Text(p.Evento);
                                    table.Cell().Element(CellStyle).Text(p.Criticidad);
                                }
                            });
                        });
                });
            }).GeneratePdf($"Bitacoras\\{NombreArchivo}.pdf");


            QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
            {
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
            }
        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string Usuario = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                Usuarios aux = userBLL.BuscarUsuario(Usuario);
                textBox1.Text = aux.Nombre;
                textBox2.Text = aux.Apellido;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombre = comboBox2.SelectedItem.ToString();

            if (nombre == "Administrador")
            {
                comboBox3.Items.Clear();
                string[] items = {
        "Login",
        "Logout",
        "Crear Usuario",
        "Borrar Usuario",
        "Modificar Usuario",
        "Asignar Perfil",
        "Quitar Perfil",
        "Registrar Cliente",
        "Borrar Cliente",
        "Modificar Cliente",
        "Cambiar Clave",
        "Cambiar Idioma",
        "Cobrar",
        "Generar Carrito",
        "Generar Factura",
        "Agregar Producto",
        "Borrar Producto",
        "Modificar Producto",
        "Agregar Proveedor",
        "Borrar Proveedor",
        "Modificar Proveedor",
        "Generar Reporte"
        };
                comboBox3.Items.AddRange(items);
            }
            else if (nombre == "Maestros")
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Registrar Cliente");
                comboBox3.Items.Add("Borrar Cliente");
                comboBox3.Items.Add("Modificar Cliente");
                comboBox3.Items.Add("Agregar Producto");
                comboBox3.Items.Add("Borrar Producto");
                comboBox3.Items.Add("Modificar Producto");
                comboBox3.Items.Add("Agregar Proveedor");
                comboBox3.Items.Add("Borrar Proveedor");
                comboBox3.Items.Add("Modificar Proveedor");
            }
            else if (nombre == "Usuario")
            {
                comboBox3.Items.Clear();
                string[] items = {
        "Login",
        "Logout",
        "Cambiar Clave",
        "Cambiar Idioma"
        };
                comboBox3.Items.AddRange(items);
            }
            else if (nombre == "Ventas")
            {
                comboBox3.Items.Clear();
                string[] items = {
        "Cobrar",
        "Generar Carrito",
        "Generar Factura",
        };
                comboBox3.Items.AddRange(items);
            }
            else if (nombre == "Compras")
            {
                comboBox3.Items.Clear();
                string[] items = {
            "Generar Solicitud",
            "Borrar Solicitud",
            "Actualizar Solicitud",
            "Generar Orden de Compra",
            "Registrar Proveedor",
            "Modificar Proveedor",
            "Pagar Orden de Compra",
            "Actualizar Stock"
        };
                comboBox3.Items.AddRange(items);
            }
            else if (nombre == "Reportes")
            {
                comboBox3.Items.Clear();
                string[] items = {
        "Generar Reporte 1"
        };
                comboBox3.Items.AddRange(items);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaInicio = dateTimePicker1.Value;
            DateTime fechaFin = dateTimePicker2.Value;

            if (isUpdating) return;

            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Error de fecha", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isUpdating = true;
                dateTimePicker1.Value = dateTimePicker2.Value;
                isUpdating = false;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
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
