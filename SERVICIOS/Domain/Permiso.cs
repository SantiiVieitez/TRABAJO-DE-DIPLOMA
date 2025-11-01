using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class Permiso
    {
        public string Nombre { get; set; }
        [Browsable(false)]
        public virtual bool Compuesto { get;}
        [Browsable(false)]
        public bool EsRol { get; set; }
        public Permiso() { }

        public Permiso(DataRow dr)
        {
            Nombre = dr[0].ToString();
            Compuesto = bool.Parse(dr[1].ToString());
            EsRol = bool.Parse(dr[2].ToString());
        }
    }
}
