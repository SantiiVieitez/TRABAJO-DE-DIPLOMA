using SERVICIOS.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.BLL
{
    public class BitacoraEventosBLL
    {
        BitacoraEventosDAL da;
        public BitacoraEventosBLL() 
        {
            da = new BitacoraEventosDAL();
        }
        public void Registrar(BitacoraEvento e)
        {
            da.Registrar(e);
        }
        public List<BitacoraEvento>RetornarBitacora()
        {
            return da.RetonarBitacora();
        }
    }
}
