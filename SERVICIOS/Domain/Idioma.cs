using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class Idioma
    {
        public string Nombre { get; set; }
        public Idioma(string pNombre)
        {
            Nombre = pNombre;
        }
    }
}
