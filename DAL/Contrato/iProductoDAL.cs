using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface IProductoDAL
    {
        void Agregar(Producto producto);
        void Modificar(Producto producto);
        void Borrar(Producto producto);
        Producto ObtenerPorId(string codigo);
        List<Producto> ObtenerTodos();
        List<ProductoSeleccionado> ListaProductoCarrito(string idCarrito);
    }
}
