using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ProductoSeleccionado
    {
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string MarcaProducto { get; set; }
        public string TipoDeRepuestoProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public int CantidadProducto { get; set; }

        public ProductoSeleccionado(Producto pProducto,int pCantidad)
        {
            CodigoProducto = pProducto.Codigo;
            NombreProducto = pProducto.Nombre;
            MarcaProducto = pProducto.Marca;
            TipoDeRepuestoProducto = pProducto.TipoDeRepuesto;
            PrecioProducto = pProducto.Precio;
            CantidadProducto = pCantidad;
        }
    }
}
