using BE;

namespace DAL
{
    public interface ICarritoDAL
    {
        void Agregar(Carrito pCarrito);
        void AgregarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito);
        void BorrarProductoCarrito(string pIdProducto);
        bool BuscarDNI(string pDNI);
        void LlenarDataSet();
        void ModificarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito);
        Carrito ObtenerCarrito(string pDNI);
    }
}