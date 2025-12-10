using BE;
using DAL;
using DAL.Contrato;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductoBLL
    {
         private readonly IProductoDAL da;

        public ProductoBLL()
        {
            da = ProductoDAL.GetInstance;
        }
        public void Agregar(Producto pProducto)
        {
            da.Agregar(pProducto);
        }
        public void Borrar(Producto pProducto)
        {
            da.Borrar(pProducto);
        }
        public void Modificar(Producto pProducto)
        {
            da.Modificar(pProducto);
        }
        public List<Producto> ListaProductos()
        {
            var productos = da.ObtenerTodos();


            return productos;
        }
        public string GenerarID()
        {
            string id = "";
            Random r = new Random(Seed: DateTime.Now.Millisecond);
            for (int x = 0; x < 3; x++)
            {
                id += (char)r.Next('A', 'Z' + 1);
            }
            for (int x = 0; x < 3; x++)
            {
                id += r.Next(0, 9);
            }
            return id;
        }
        public Producto BuscarProducto(string pIdProducto)
        {
            return da.ObtenerPorId(pIdProducto);
        }
        public List<ProductoSeleccionado> ListaProductosCarrito(string id)
        {
            return da.ListaProductoCarrito(id);
        }
    }
}
