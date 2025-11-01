using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FacturaDAL
    {
        basededatos dao;
        DataSet ds;
        public FacturaDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public void LlenarDataSet()
        {
            string query = "select * from Factura";
            ds = dao.ExecuteDataSet(query);
        }
        public void AgregarFactura(Factura pFactura)
        {
            string query = $"insert into Factura values ({pFactura.ID},'{pFactura.MetodoDePago}', '{pFactura.DNI_Cliente}', '{pFactura.Fecha.ToString("yyyy-MM-dd")}')";
            dao.ExecuteNonQuery(query);
        }
        public void GuardarProductos(Factura pFactura)
        {
            foreach (ProductoSeleccionado p in pFactura.ListaSeleccionados)
            {
                string query = $"insert into ProductoFactura values ('{p.CodigoProducto}',{pFactura.ID},'{p.CantidadProducto}')";
                dao.ExecuteNonQuery(query);
            }
        }
        public List<Factura> RetornarFacturas()
        {
            List<Factura> Lista = new List<Factura>();
            string query = "select * from Factura";
            ds = dao.ExecuteDataSet(query);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Factura factura = new Factura(dr);
                factura.ListaSeleccionados = new List<ProductoSeleccionado>();

                query = $"select CodigoProducto, Cantidad from ProductoFactura where CodigoFactura = '{factura.ID}'";
                DataSet dsProductosFactura = dao.ExecuteDataSet(query);

                foreach (DataRow drProductoFactura in dsProductosFactura.Tables[0].Rows)
                {
                    string codigoProducto = drProductoFactura["CodigoProducto"].ToString();
                    int cantidad = Convert.ToInt32(drProductoFactura["Cantidad"]);

                    DataSet dsDetalleProducto = dao.ExecuteDataSet($"select * from Producto where Codigo = '{codigoProducto}'");

                    if (dsDetalleProducto.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDetalleProducto = dsDetalleProducto.Tables[0].Rows[0];
                        Producto producto = new Producto(drDetalleProducto);
                        ProductoSeleccionado productoSeleccionado = new ProductoSeleccionado(producto, cantidad);
                        //inicializar antes del foreach.
                        factura.ListaSeleccionados.Add(productoSeleccionado);
                    }
                }
                Lista.Add(factura);
            }
            return Lista;
        }
    }
}
