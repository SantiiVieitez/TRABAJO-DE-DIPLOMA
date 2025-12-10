using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface IFacturaP_DAL
    {
        void Agregar(FacturaP factura);
        void Modificar(FacturaP factura);
        void Borrar(FacturaP factura);

        FacturaP ObtenerPorId(object id);
        List<FacturaP> ObtenerTodos();

        // Específicos
        void RegistrarFacturaP(FacturaP factura);
        List<FacturaP> RetornarFacturasP();
    }
}
