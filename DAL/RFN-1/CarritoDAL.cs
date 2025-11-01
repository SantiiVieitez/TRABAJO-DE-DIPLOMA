using BE;
using dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CarritoDAL : ICarritoDAL
    {
        basededatos dao;
        DataSet ds;
        public CarritoDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public void LlenarDataSet()
        {
            string query = "select * from carrito";
            ds = dao.ExecuteDataSet(query);
        }
        public void Agregar(Carrito pCarrito)
        {
            string query = $"insert into carrito values ('{pCarrito.Codigo}','{pCarrito.ClienteDNI}')";
            dao.ExecuteNonQuery(query);
        }
        public void AgregarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito)
        {
            string query = $"insert into ProductoCarrito values ('{pProducto.CodigoProducto}','{pIdCarrito}',{pProducto.CantidadProducto})";
            dao.ExecuteNonQuery(query);
        }
        public void BorrarProductoCarrito(string pIdProducto)
        {
            string query = $"delete from ProductoCarrito where CodigoProducto = '{pIdProducto}'";
            dao.ExecuteNonQuery(query);
        }
        public void ModificarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito)
        {
            string query = $"update ProductoCarrito set Cantidad = {pProducto.CantidadProducto} where CodigoProducto = '{pProducto.CodigoProducto}' and CodigoCarrito = '{pIdCarrito}'";
            dao.ExecuteNonQuery(query);
        }
        public Carrito ObtenerCarrito(string pDNI)
        {
            string query = $"select * from carrito where ClienteDNI = '{pDNI}'";
            ds = dao.ExecuteDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Carrito aux = new Carrito(ds.Tables[0].Rows[0]);
                return aux;
            }
            else
            {
                return null;
            }
        }
        public bool BuscarDNI(string pDNI)
        {
            string query = $"select * from carrito where ClienteDNI = '{pDNI}'";
            ds = dao.ExecuteDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
