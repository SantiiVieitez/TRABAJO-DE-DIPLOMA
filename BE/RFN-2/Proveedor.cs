using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Proveedor
    {
        public string CUIT { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public long Telefono { get; set; }
        public string CBU { get; set; }
        public string Banco { get; set; }

        public Proveedor(string pCUIT, string pNombre, string pEmail, long pTelefono)
        {
            CUIT = pCUIT;
            Nombre = pNombre;
            Email = pEmail;
            Telefono = pTelefono;
        }
        public Proveedor(string pCUIT, string pNombre, string pEmail, long pTelefono, string pCBU,string pBanco)
        {
            CUIT = pCUIT;
            Nombre = pNombre;
            Email = pEmail;
            Telefono = pTelefono;
            CBU = pCBU;
            Banco = pBanco;
        }
        public Proveedor(DataRow dr)
        {
            CUIT = dr[0].ToString();
            Nombre = dr[1].ToString();
            Email = dr[2].ToString();
            Telefono = long.Parse(dr[3].ToString());
            CBU = dr[4].ToString();
            Banco = dr[5].ToString();
        }
        public override string ToString()
        {
            return Nombre;
        }
    }
}
