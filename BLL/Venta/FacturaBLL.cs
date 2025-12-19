using BE;
using DAL;
using DAL.Contrato;
using dao;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.IO;

namespace BLL
{
    public class FacturaBLL
    {
        public FacturaBLL()
        {
        }

        public void AgregarFactura(Factura pFactura)
        {
            using (var uow = new UnitOfWork())
            {
                try
                {
                    uow.Begin();
                    string fechaFormateada = pFactura.Fecha.ToString("yyyy-MM-dd");
                    string cadenaConcatenada =
                        pFactura.ID.ToString() +
                        pFactura.MetodoDePago.Trim() +
                        pFactura.DNI_Cliente.ToString() +
                        fechaFormateada;

                    pFactura.DVH = Encriptador.GetSHA256(cadenaConcatenada);

                    uow.Facturas.Agregar(pFactura);

                    uow.Commit();
                }
                catch (Exception)
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public void AgregarProductoFactura(Factura pFactura)
        {
            using (var uow = new UnitOfWork())
            {
                try
                {
                    uow.Begin();
                    uow.Facturas.GuardarProductos(pFactura);
                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public void RepararDigitosVerificadores()
        {
            using (var uow = new UnitOfWork())
            {
                try
                {
                    uow.Begin();

                    List<Factura> facturas = uow.Facturas.ObtenerTodos();

                    foreach (var fac in facturas)
                    {
                        string hashNuevo = Seguridad.CalcularDVH(fac);
                        uow.Facturas.ActualizarDVH(fac.ID, hashNuevo);
                    }

                    uow.Commit();
                }
                catch
                {
                    uow.Rollback();
                    throw;
                }
            }
        }

        public List<Factura> RetornarFacturas()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Begin();

                return uow.Facturas.ObtenerTodos();
            }
        }

        public void VerificarDVH()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Begin(); 

                List<Factura> Facturas = uow.Facturas.ObtenerTodos();

                foreach (var x in Facturas)
                {
                    string fechaFormateada = x.Fecha.ToString("yyyy-MM-dd");
                    string cadenaConcatenada =
                        x.ID.ToString() +
                        x.MetodoDePago.Trim() +
                        x.DNI_Cliente.ToString() +
                        fechaFormateada;

                    string aux = Encriptador.GetSHA256(cadenaConcatenada);

                    if (!string.Equals(aux, x.DVH, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Integridad comprometida en Factura ID {x.ID}");
                    }
                }
            }
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
            if (pFactura.ListaSeleccionados != null)
            {
                foreach (ProductoSeleccionado item in pFactura.ListaSeleccionados)
                {
                    if (item.CantidadProducto > 1)
                        total += item.PrecioProducto * item.CantidadProducto;
                    else
                        total += item.PrecioProducto;
                }
            }
            return total;
        }

        public void GenerarFactura(Factura pFactura)
        {
            EjecutarQuestPDF(pFactura, $"Facturas\\{pFactura.ID}_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public void GenerarFactura(Factura pFactura, string directorio)
        {
            EjecutarQuestPDF(pFactura, directorio);
        }

        private void EjecutarQuestPDF(Factura pFactura, string ruta)
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string pathImgExe = Path.Combine(basePath, "Imagenes", "Logo250.jpg");
            string pathImgDev = Path.GetFullPath(Path.Combine(basePath, @"..\..\Imagenes", "Logo250.jpg"));
            string rutaLogo = File.Exists(pathImgExe) ? pathImgExe : pathImgDev;

            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(100).Image(rutaLogo);
                        row.RelativeItem().AlignRight().Text(t => t.Span("RepuestoMaster").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium));
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
                    {
                        col.Spacing(20);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c => { c.RelativeColumn(); c.RelativeColumn(); c.RelativeColumn(); c.RelativeColumn(); });
                            table.Header(h =>
                            {
                                h.Cell().Element(CellStyle).Text("Nombre");
                                h.Cell().Element(CellStyle).Text("Marca");
                                h.Cell().Element(CellStyle).Text("Cantidad");
                                h.Cell().Element(CellStyle).Text("Precio");
                            });

                            if (pFactura.ListaSeleccionados != null)
                            {
                                foreach (ProductoSeleccionado p in pFactura.ListaSeleccionados)
                                {
                                    table.Cell().Element(CellStyle).Text(p.NombreProducto);
                                    table.Cell().Element(CellStyle).Text(p.MarcaProducto);
                                    table.Cell().Element(CellStyle).Text(p.CantidadProducto.ToString());
                                    table.Cell().Element(CellStyle).Text(p.PrecioProducto.ToString());
                                }
                            }
                            table.Cell().ColumnSpan(3).Text("Total").Bold();
                            table.Cell().Text(CalcularTotal(pFactura).ToString());
                        });
                    });

                    page.Footer().Row(r =>
                    {
                        r.RelativeItem().Column(c =>
                        {
                            c.Item().Text($"Código: {pFactura.ID}");
                            c.Item().Text($"Fecha: {pFactura.Fecha:d}");
                            c.Item().Text($"Pago: {pFactura.MetodoDePago}");
                            c.Item().Text($"DNI: {pFactura.DNI_Cliente}");
                        });
                    });
                });
            }).GeneratePdf(ruta);
        }

        IContainer CellStyle(IContainer container)
        {
            return container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
        }
    }
}