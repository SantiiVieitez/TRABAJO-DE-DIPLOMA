using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RFN_2
{
    public class FacturaP_DAL
    {
        basededatos dao;
        DataSet ds;

        public FacturaP_DAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }

        public void RegistrarFacturaP(FacturaP p)
        {
            string query = $"INSERT INTO FacturaP (ID,Fecha,MetodoDePago,ID_OrdenDeCompra,NombreComprador,NombreVendedor) values ('{p.ID}','{p.Fecha:yyyy-MM-dd HH:mm:ss}'," +
                $"'{p.MetodoDePago}',{p.ID_OrdenDeCompra},'{p.NombreComprador}','{p.NombreVendedor}')";
            dao.ExecuteNonQuery(query);
            foreach(ProductoSeleccionado x in p.ListaProductos)
            {
                query = $"INSERT INTO ProductoFacturaP (ID_FacturaP,CodigoProducto,Cantidad) values ('{p.ID}','{x.CodigoProducto}',{x.CantidadProducto})";
                dao.ExecuteNonQuery(query);
            }
        }

        public List<FacturaP> RetornarFacturasP()
        {
            List<FacturaP> Lista = new List<FacturaP>();
            ds = dao.ExecuteDataSet($"select * from FacturaP");
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                FacturaP factura = new FacturaP(dr);
                DataSet dsProductosFactura = dao.ExecuteDataSet($"select CodigoProducto, Cantidad from ProductoFacturaP where ID_FacturaP = '{factura.ID}'");

                foreach (DataRow drProducto in dsProductosFactura.Tables[0].Rows)
                {
                    string codigoProducto = drProducto["CodigoProducto"].ToString();
                    int cantidad = Convert.ToInt32(drProducto["Cantidad"]);

                    DataSet dsDetalleProducto = dao.ExecuteDataSet($"select * from Producto where Codigo = '{codigoProducto}'");

                    if (dsDetalleProducto.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDetalleProducto = dsDetalleProducto.Tables[0].Rows[0];
                        Producto producto = new Producto(drDetalleProducto);
                        ProductoSeleccionado productoSeleccionado = new ProductoSeleccionado(producto, cantidad);
                        factura.ListaProductos = new List<ProductoSeleccionado>();
                        factura.ListaProductos.Add(productoSeleccionado);
                    }
                }
                Lista.Add(factura);
            }
            return Lista;
        }
        public void Modificar(FacturaP p)
        {
            string query = $"UPDATE FacturaP SET Recibido = '{p.Recibido}' WHERE ID = '{p.ID}'";
            dao.ExecuteNonQuery(query);
        }
    }
}
