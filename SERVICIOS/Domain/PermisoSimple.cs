using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class PermisoSimple : Permiso
    {
        public override bool Compuesto => false;

        public PermisoSimple(DataRow dr)
        {
            Nombre = dr[0].ToString();
        }
    }
}
