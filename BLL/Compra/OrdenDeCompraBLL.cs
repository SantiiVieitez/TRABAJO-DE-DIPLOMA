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
    public class OrdenDeCompraBLL
    {
        IOrdenDeCompraDAL dao;
        public OrdenDeCompraBLL()
        {
            dao = OrdenDeCompraDAL.GetInstance;
        }
        public void RegistrarCompra(OrdenDeCompra p)
        {
            dao.Agregar(p);
        }
        public int GenerarID()
        {
            return dao.RetornarUltimoID();
        }
    }
}
