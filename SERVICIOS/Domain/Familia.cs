using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class Familia : Permiso
    {
        public Familia(DataRow dr) : base(dr)
        {
        }
        public Familia()
        {

        }

        public List<Permiso> Permisos { get; set; }
        public override bool Compuesto => true;

        public override string ToString()
        {
            return $"{Nombre}";
        }
    }
}
