using BE.RFN_2;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RFN_2
{
    public class ProductoC_DAL
    {
        basededatos dao;
        DataSet ds;
        public ProductoC_DAL()
        {
           dao = new basededatos();
           ds = new DataSet();
        }
        public List<ProductoC>RetonarProductoC()
        {
            List<ProductoC> lista = new List<ProductoC>();
            ds = dao.ExecuteDataSet("Select * from Producto_C");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lista.Add(new ProductoC(dr));
            }
            return lista;
        }
        public void ActivarProductoC(ProductoC c)
        {

            string queryDesactivarProductoC = $"UPDATE Producto_C SET Activo = 0 WHERE Cod_Prod = '{c.CodigoProducto}' AND Activo = 1";
            dao.ExecuteNonQuery(queryDesactivarProductoC);


            string queryActivarProductoC = $"UPDATE Producto_C SET Activo = 1 WHERE ID = {c.ID}";
            dao.ExecuteNonQuery(queryActivarProductoC);


            string queryActualizarProducto = $@"
            UPDATE Producto
            SET 
                Nombre = (SELECT Nombre FROM Producto_C WHERE ID = {c.ID}),
                Cantidad = (SELECT Existencias FROM Producto_C WHERE ID = {c.ID}),
                Descripcion = (SELECT Descripcion FROM Producto_C WHERE ID = {c.ID}),
                Marca = (SELECT Marca FROM Producto_C WHERE ID = {c.ID}),
                TipoDeRepuesto = (SELECT TipoDeRepuesto FROM Producto_C WHERE ID = {c.ID}),
                TipoDeVehiculo = (SELECT TipoDeVehiculo FROM Producto_C WHERE ID = {c.ID}),
                Material = (SELECT Material FROM Producto_C WHERE ID = {c.ID}),
                Precio = (SELECT Precio FROM Producto_C WHERE ID = {c.ID}),
                BorradoLogico = (SELECT BorradoLogico FROM Producto_C WHERE ID = {c.ID})
            WHERE Codigo = '{c.CodigoProducto}'";
            dao.ExecuteNonQuery(queryActualizarProducto);
        }
    }
}
