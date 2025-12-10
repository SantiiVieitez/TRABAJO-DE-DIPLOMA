using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface IProveedorDAL
    {
        void Agregar(Proveedor p);
        void Modificar(Proveedor p);
        void ModificarProveedor(Proveedor p, bool a);
        void Borrar(Proveedor p);

        Proveedor BuscarProveedor(string cuit);
        List<Proveedor> ListarProveedores();

        void RegistrarProveedor(Proveedor p, bool a);
    }
}
