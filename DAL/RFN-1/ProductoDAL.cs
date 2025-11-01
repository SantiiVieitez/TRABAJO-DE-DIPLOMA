using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductoDAL
    {
        basededatos dao;
        DataSet ds;
        public ProductoDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }

        public void LlenarDataSet()
        {
            string query = "select * from producto";
            ds = dao.ExecuteDataSet(query);
        }
        public void Agregar(Producto pProducto)
        {
            string query = $"insert into producto values ('{pProducto.Codigo}','{pProducto.Nombre}','{pProducto.Marca}','{pProducto.TipoDeRepuesto}',{pProducto.Cantidad},'{pProducto.TipoDeVehiculo}','{pProducto.Material}',{pProducto.Precio},'{pProducto.Descripcion}','{pProducto.BorradoLogico}')";
            dao.ExecuteNonQuery(query);
        }
        public void Borrar(Producto pProducto)
        {
            string query = $"UPDATE producto SET BorradoLogico = 1 WHERE Codigo = '{pProducto.Codigo}'";
            dao.ExecuteNonQuery(query);
        }
        public void Modificar(Producto pProducto)
        {
            string query = $"update producto set Nombre = '{pProducto.Nombre}',Marca = '{pProducto.Marca}',TipoDeRepuesto = '{pProducto.TipoDeRepuesto}',Cantidad= {pProducto.Cantidad},TipoDeVehiculo = '{pProducto.TipoDeVehiculo}',Material = '{pProducto.Material}',Precio = {pProducto.Precio},Descripcion = '{pProducto.Descripcion}',BorradoLogico = '{pProducto.BorradoLogico}' where Codigo = '{pProducto.Codigo}'";
            dao.ExecuteNonQuery(query);
        }
        public List<Producto> RetornarListaProductos()
        {
            LlenarDataSet();
            List<Producto> list = new List<Producto>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Producto(dr));
            }
            return list;
        }
        public Producto BuscarProducto(string pIdProducto)
        {
            string query = $"select * from producto where Codigo = '{pIdProducto}'";
            ds = dao.ExecuteDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return new Producto(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public List<ProductoSeleccionado>ListaProductoCarrito(string pIdCarrito)
        {
            string query = $"select * from ProductoCarrito where CodigoCarrito = '{pIdCarrito}'";
            ds = dao.ExecuteDataSet(query);
            List<ProductoSeleccionado> list = new List<ProductoSeleccionado>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var producto = BuscarProducto(dr[0].ToString());
                list.Add(new ProductoSeleccionado(producto, int.Parse(dr[2].ToString())));

            }
            return list;
        }
    }
}
