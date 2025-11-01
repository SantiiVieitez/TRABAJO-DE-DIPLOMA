using BE;
using DAL.RFN_2;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FacturaP_BLL
    {
        FacturaP_DAL dao;
        public FacturaP_BLL() { dao = new FacturaP_DAL(); }

        public string GenerarID()
        {
            Random random = new Random();
            StringBuilder codigo = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                char letra = (char)random.Next('A', 'Z' + 1);
                codigo.Append(letra);
            }
            for (int i = 0; i < 3; i++)
            {
                int numero = random.Next(0, 10);
                codigo.Append(numero);
            }
            return codigo.ToString();
        }

        public void RegistrarFacturaP(FacturaP p)
        {
            dao.RegistrarFacturaP(p);
        }
        public List<FacturaP> RetornarFacturasP()
        {
            return dao.RetornarFacturasP();
        }
        public decimal CalcularTotal(FacturaP pFactura)
        {
            decimal total = 0;
            foreach (ProductoSeleccionado item in pFactura.ListaProductos)
            {
                if (item.CantidadProducto > 1)
                {
                    total += item.PrecioProducto * item.CantidadProducto;
                }
                else
                {
                    total += item.PrecioProducto;
                }
            }
            return total;
        }
        public void GenerarFacturaP(FacturaP pFactura)
        {
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
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Nombre");
                                    header.Cell().Element(CellStyle).Text("Marca");
                                    header.Cell().Element(CellStyle).Text("Cantidad");
                                    header.Cell().Element(CellStyle).Text("Precio");
                                });

                                foreach (ProductoSeleccionado p in pFactura.ListaProductos)
                                {
                                    table.Cell().Element(CellStyle).Text(p.NombreProducto);
                                    table.Cell().Element(CellStyle).Text(p.MarcaProducto);
                                    table.Cell().Element(CellStyle).Text(p.CantidadProducto.ToString());
                                    table.Cell().Element(CellStyle).Text(p.PrecioProducto.ToString());
                                }
                                table.Cell().ColumnSpan(3).Text("Total").Bold();
                                table.Cell().Text(CalcularTotal(pFactura).ToString());
                            });
                        });

                    page.Footer()
                        .Row(r =>
                        {
                            r.RelativeItem().Column(column =>
                            {
                                column.Item().Text($"Código de la Factura:{pFactura.ID}");
                                column.Item().Text($"Fecha: {pFactura.Fecha.ToShortDateString()}");
                                column.Item().Text($"Método de Pago:{pFactura.MetodoDePago}");
                                column.Item().Text($"Nro Orden de Compra:{pFactura.ID_OrdenDeCompra}");
                                column.Item().Text($"Nombre Comprador:{pFactura.NombreComprador}");
                                column.Item().Text($"Nombre Vendedor:{pFactura.NombreVendedor}");
                            });
                        });
                });
            }).GeneratePdf($"Facturas\\{pFactura.ID}_{DateTime.Now.ToString("yyyyMMdd")}.pdf");


            IContainer CellStyle(IContainer container)
            {
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
            }
        }
        public void Modificar(FacturaP p)
        {
            dao.Modificar(p);
        }
    }
}
