using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public static class EncriptadorReversible
    {
        public static string Encriptar(string pCadena)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(pCadena));
        }
        public static string Desencriptar(string pCadena)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(pCadena));
        }
    }
}
