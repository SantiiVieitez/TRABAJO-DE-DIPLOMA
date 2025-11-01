using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Cliente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Domicilio { get; set; }
        public string Correo { get; set; }
        public int Telefono { get; set; }

        public Cliente(DataRow dr)
        {
            DNI = dr[0].ToString();
            Nombre = dr[1].ToString();
            Apellido = dr[2].ToString();
            Domicilio = dr[3].ToString();
            Correo = dr[4].ToString();
            Telefono = int.Parse(dr[5].ToString());
        }
        public Cliente(string pNombre, string pApellido, string pDNI, string pDomicilio, string pCorreo, int pTelefono)
        {
            Nombre = pNombre;
            Apellido = pApellido;
            DNI = pDNI;
            Domicilio = pDomicilio;
            Correo = pCorreo;
            Telefono = pTelefono;
        }
        public Cliente()
        {

        }
    }
}
