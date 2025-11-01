using BE;
using DAL;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BLL
{
    public class FacturaBLL
    {
        FacturaDAL da;
        public FacturaBLL()
        {
            da = new FacturaDAL();
        }
        public void AgregarFactura(Factura pFactura)
        {
            da.AgregarFactura(pFactura);
        }
        public void AgregarProductoFactura(Factura pFactura)
        {
            da.GuardarProductos(pFactura);
        }
        public string GenerarID()
        {
            string id = "";
            Random r = new Random(Seed: DateTime.Now.Millisecond);
            for (int x = 0; x < 8; x++)
            {
                id += r.Next(0, 9);
            }
            return id;
        }
        public decimal CalcularTotal(Factura pFactura)
        {
            decimal total = 0;
            foreach (ProductoSeleccionado item in pFactura.ListaSeleccionados)
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
        public void GenerarFactura(Factura pFactura)
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

                                foreach(ProductoSeleccionado p in pFactura.ListaSeleccionados)
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
                                column.Item().Text($"DNI Cliente:{pFactura.DNI_Cliente}");
                            });
                        });
                });
            }).GeneratePdf($"Facturas\\{pFactura.ID}_{DateTime.Now.ToString("yyyyMMdd")}.pdf");


            IContainer CellStyle(IContainer container)
            {
                return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
            }
        }
        public List<Factura> RetornarFacturas()
        {
            return da.RetornarFacturas();
        }
    }
}
