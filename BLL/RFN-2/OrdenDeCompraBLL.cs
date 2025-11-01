using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrdenDeCompraBLL
    {
        OrdenDeCompraDAL dao;
        public OrdenDeCompraBLL()
        {
            dao = new OrdenDeCompraDAL();
        }
        public void RegistrarCompra(OrdenDeCompra p)
        {
            dao.RegistrarCompra(p);
        }
        public int GenerarID()
        {
            return dao.RetornarUltimoID();
        }
    }
}
