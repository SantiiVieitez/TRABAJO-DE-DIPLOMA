using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrdenDeCompraDAL
    {
        basededatos dao;
        DataSet ds;

        public OrdenDeCompraDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public void RegistrarCompra(OrdenDeCompra p)
        {
            string query = $"INSERT INTO OrdenDeCompra (ID,NombreEmpresa,CUIT,Fecha) values({p.ID},'{p.NombreEmpresa}','{p.CUIT}','{p.Fecha:yyyy-MM-dd HH:mm:ss}')";
            dao.ExecuteNonQuery(query);
            foreach(ProductoSeleccionado x in p.ListaProductos)
            {
               query = $"INSERT INTO OrdenDeCompraProducto (ID_Compra,CodigoProducto,Cantidad) values({p.ID},'{x.CodigoProducto}',{x.CantidadProducto})";
               dao.ExecuteNonQuery(query);
            }
        }
        public int RetornarUltimoID()
        {
            string query = "SELECT MAX(ID) AS UltimoID FROM OrdenDeCompra";
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

        public List<OrdenDeCompra> RetornarOrdenesDeCompra()
        {
            List<OrdenDeCompra> list = new List<OrdenDeCompra>();
            ds = dao.ExecuteDataSet("select * from OrdenDeCompra");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OrdenDeCompra p = new OrdenDeCompra(dr);

                ds = dao.ExecuteDataSet($"select CodigoProducto, Cantidad from OrdenDeCompraProducto where ID_Compra = {p.ID}");

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

                        p.ListaProductos = new List<ProductoSeleccionado>();
                        p.ListaProductos.Add(productoSeleccionado);
                    }
                }
                list.Add(p);
            }
            return list;
        }
        public List<OrdenDeCompra> RetornarOrdenDeCompraID(int ID)
        {
            List<OrdenDeCompra> list = new List<OrdenDeCompra>();
            ds = dao.ExecuteDataSet($"select * from OrdenDeCompra where ID = {ID}");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                OrdenDeCompra p = new OrdenDeCompra(dr);

                ds = dao.ExecuteDataSet($"select CodigoProducto, Cantidad from OrdenDeCompraProducto where ID_Compra = {p.ID}");

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

                        p.ListaProductos = new List<ProductoSeleccionado>();
                        p.ListaProductos.Add(productoSeleccionado);
                    }
                }
                list.Add(p);
            }
            return list;
        }
    }
}
