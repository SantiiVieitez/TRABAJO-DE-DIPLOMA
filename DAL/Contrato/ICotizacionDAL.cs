using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface ICotizacionDAL
    {
        int RetornarUltimoID();

        // CRUD heredados del genérico:
        void Agregar(Cotizacion cot);
        void Modificar(Cotizacion cot);
        void Borrar(Cotizacion cot);

        Cotizacion ObtenerPorId(object id);
        List<Cotizacion> ObtenerTodos();

        // métodos específicos
        void GuardarProductosCotizacion(Cotizacion cot);
        List<Cotizacion> RetornarCotizaciones(string cuit);
    }
}
