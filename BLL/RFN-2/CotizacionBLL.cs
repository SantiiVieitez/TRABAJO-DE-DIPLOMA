using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CotizacionBLL
    {
        CotizacionDAL dao;
        public CotizacionBLL()
        {
            dao = new CotizacionDAL();
        }
        public int CalcularID()
        {
            return dao.RetornarUltimoID();
        }
        public void Registrar(Cotizacion p)
        {
            dao.GuardarCotizacion(p);
        }
        public void RegistrarProductosCotizacion(Cotizacion p)
        {
            dao.GuardarProductosCotizacion(p);
        }
        public void BorrarCotizacion(Cotizacion p)
        {
            dao.BorrarCotizacion(p);
        }
        public void ActualizarCotizacion(Cotizacion p)
        {
            dao.ActualizarCotizacion(p);
        }
        public List<Cotizacion> RetornarCotizaciones()
        {
            return dao.RetornarCotizaciones();
        }
        public List<Cotizacion> RetornarCotizacionesProveedor(string cuit)
        {
            return dao.RetornarCotizacionesProveedor(cuit);
        }
    }
}
