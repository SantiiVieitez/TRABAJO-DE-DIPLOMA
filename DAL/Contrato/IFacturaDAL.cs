using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface IFacturaDAL
    {
        void Agregar(Factura factura);
        void Modificar(Factura factura);
        void Borrar(Factura factura);

        Factura ObtenerPorId(string idFactura);
        List<Factura> ObtenerTodos();
        void GuardarProductos(Factura factura);
        void ActualizarDVH(int id, string hash);
    }
}
