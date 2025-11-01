using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CotizacionDAL
    {
        basededatos dao;
        DataSet ds;
        public CotizacionDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public int RetornarUltimoID()
        {
            string query = "SELECT MAX(ID) AS UltimoID FROM SolicitudCotizacion";
            DataSet ds = dao.ExecuteDataSet(query);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                object result = ds.Tables[0].Rows[0]["UltimoID"];
                if (result != DBNull.Value)
                {
                    return Convert.ToInt32(result) + 1;
                }
            }
            return 1;
        }
        public void GuardarCotizacion(Cotizacion p)
        {
            string query = $"INSERT INTO SolicitudCotizacion (ID,CUIT,NombreEmpresa,Fecha) VALUES ({p.CotizacionID},'{p.proveedor.CUIT}','{p.proveedor.Nombre}','{p.Fecha:yyyy-MM-dd HH:mm:ss}')";
            dao.ExecuteNonQuery(query);
        }
        public void GuardarProductosCotizacion(Cotizacion p)
        {
            foreach (ProductoSeleccionado a in p.Productos)
            {
                string query = $"INSERT INTO CotizacionProductos (ID_Cotizacion,CodigoProducto,Cantidad) VALUES ({p.CotizacionID},'{a.CodigoProducto}',{a.CantidadProducto})";
                dao.ExecuteNonQuery(query);
            }
        }
        public void BorrarCotizacion(Cotizacion p)
        {
            foreach (ProductoSeleccionado a in p.Productos)
            {
                string query = $"DELETE FROM CotizacionProductos WHERE ID_Cotizacion = {p.CotizacionID} AND CodigoProducto = '{a.CodigoProducto}'";
                dao.ExecuteNonQuery(query);
            }
            string query2 = $"DELETE FROM SolicitudCotizacion WHERE ID = {p.CotizacionID}";
            dao.ExecuteNonQuery(query2);
        }
        public void ActualizarCotizacion(Cotizacion p)
        {
            string query = $"UPDATE SolicitudCotizacion SET CUIT = '{p.proveedor.CUIT}', NombreEmpresa = '{p.proveedor.Nombre}', Fecha = '{p.Fecha:yyyy-MM-dd HH:mm:ss}' WHERE ID = {p.CotizacionID}";
            dao.ExecuteNonQuery(query);
        }
        public List<Cotizacion> RetornarCotizaciones()
        {
            List<Cotizacion> list = new List<Cotizacion>();
            ds = dao.ExecuteDataSet("select * from SolicitudCotizacion");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Cotizacion p = new Cotizacion(dr);
                p.proveedor = new ProveedorDAL().BuscarProveedor(dr[1].ToString());
                p.NombreProveedor = p.proveedor.Nombre;

                ds = dao.ExecuteDataSet($"select CodigoProducto, Cantidad from CotizacionProductos where ID_Cotizacion = {p.CotizacionID}");

                foreach (DataRow drProducto in ds.Tables[0].Rows)
                {
                    string codigoProducto = drProducto["CodigoProducto"].ToString();
                    int cantidad = Convert.ToInt32(drProducto["Cantidad"]);

                    DataSet dsDetalleProducto = dao.ExecuteDataSet($"select * from Producto where Codigo = '{codigoProducto}'");

                    if (dsDetalleProducto.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDetalleProducto = dsDetalleProducto.Tables[0].Rows[0];
                        Producto producto = new Producto(drDetalleProducto);
                        ProductoSeleccionado productoSeleccionado = new ProductoSeleccionado(producto, cantidad);

                        p.Productos = new List<ProductoSeleccionado>();
                        p.Productos.Add(productoSeleccionado);
                    }
                }
                list.Add(p);
            }
            return list;
        }
        public List<Cotizacion> RetornarCotizacionesProveedor(string cuit)
        {
            List<Cotizacion> list = new List<Cotizacion>();
            ds = dao.ExecuteDataSet($"select * from SolicitudCotizacion where CUIT = '{cuit}'");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Cotizacion p = new Cotizacion(dr);
                p.proveedor = new ProveedorDAL().BuscarProveedor(dr[1].ToString());
                p.NombreProveedor = p.proveedor.Nombre;
                ds = dao.ExecuteDataSet($"select CodigoProducto, Cantidad from CotizacionProductos where ID_Cotizacion = {p.CotizacionID}");

                foreach (DataRow drProducto in ds.Tables[0].Rows)
                {
                    string codigoProducto = drProducto["CodigoProducto"].ToString();
                    int cantidad = Convert.ToInt32(drProducto["Cantidad"]);
                    DataSet dsDetalleProducto = dao.ExecuteDataSet($"select * from Producto where Codigo = '{codigoProducto}'");

                    if (dsDetalleProducto.Tables[0].Rows.Count > 0)
                    {
                        DataRow drDetalleProducto = dsDetalleProducto.Tables[0].Rows[0];
                        Producto producto = new Producto(drDetalleProducto);
                        ProductoSeleccionado productoSeleccionado = new ProductoSeleccionado(producto, cantidad);

                        p.Productos = new List<ProductoSeleccionado>();
                        p.Productos.Add(productoSeleccionado);
                    }
                }
                list.Add(p);
            }
            return list;
        }
    }
}
