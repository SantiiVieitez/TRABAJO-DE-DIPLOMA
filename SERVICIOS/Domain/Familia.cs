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
        public List<Permiso> Permisos { get; set; } = new List<Permiso>();

        public override bool Compuesto => true;

        public Familia() : base() { }

        public Familia(DataRow dr) : base(dr)
        {
            
        }

        public override string ToString() => $"{Nombre}";
    }
}
