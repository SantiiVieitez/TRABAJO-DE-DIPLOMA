using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CarritoBLL
    {
        CarritoDAL da;
        public CarritoBLL()
        {
            da = new CarritoDAL();
        }
        public string GenerarCodigoCarrito()
        {
            string id = "";
            Random r = new Random(Seed: DateTime.Now.Millisecond);
            for (int x = 0; x < 2; x++)
            {
                id += (char)r.Next('A', 'Z' + 1);
            }
            for (int x = 0; x < 2; x++)
            {
                id += r.Next(0, 9);
            }
            return id;
        }

        public void Agregar(Carrito pCarrito)
        {
            pCarrito.Codigo = GenerarCodigoCarrito();
            da.Agregar(pCarrito);
        }
        public void AgregarProductoCarrito(string pCodigoCarrito, ProductoSeleccionado pProductoSeleccionado)
        {
            da.AgregarProductoCarrito(pProductoSeleccionado, pCodigoCarrito);
        }
        public void BorrarProductoCarrito(string pIdProducto)
        {
            da.BorrarProductoCarrito(pIdProducto);
        }
        public void ModificarProductoCarrito(ProductoSeleccionado ps,string idCarrito)
        {
            da.ModificarProductoCarrito(ps, idCarrito);
        }
        public Carrito ObtenerCarrito(string pDNI)
        {
            try
            {
                if (da.ObtenerCarrito(pDNI) == null)
                {
                    return null;
                }
                else
                {
                    ProductoBLL productoBLL = new ProductoBLL();
                    Carrito pCarrito = da.ObtenerCarrito(pDNI);
                    pCarrito.ListaSeleccionados = productoBLL.ListaProductosCarrito(pCarrito.Codigo);
                    return pCarrito;
                }
            }
            catch
            {
                throw new Exception("Error al obtener el carrito");
            }
        }
        public bool BuscarDNI(string pDNI)
        {
            if (da.BuscarDNI(pDNI) == true)
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
