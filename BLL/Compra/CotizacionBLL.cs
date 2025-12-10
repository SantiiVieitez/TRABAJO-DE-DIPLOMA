using BE;
using DAL;
using DAL.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CotizacionBLL
    {
        private readonly ICotizacionDAL dao;
        public CotizacionBLL()
        {
            dao = CotizacionDAL.GetInstance;
        }
        public int CalcularID()
        {
            return dao.RetornarUltimoID();
        }
        public void Registrar(Cotizacion p)
        {
            dao.Agregar(p);
        }
        public void RegistrarProductosCotizacion(Cotizacion p)
        {
            dao.GuardarProductosCotizacion(p);
        }
        public void BorrarCotizacion(Cotizacion p)
        {
            dao.Borrar(p);
        }
        public void ActualizarCotizacion(Cotizacion p)
        {
            dao.Modificar(p);
        }
        public List<Cotizacion> RetornarCotizaciones()
        {
            return dao.ObtenerTodos();
        }
        public List<Cotizacion> RetornarCotizacionesProveedor(string cuit)
        {
            return dao.RetornarCotizacionesProveedor(cuit);
        }
    }
}
