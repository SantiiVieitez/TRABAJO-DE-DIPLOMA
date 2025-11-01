using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Carrito
    {
        public string Codigo { get; set; }
        public string ClienteDNI { get; set; }
        public List<ProductoSeleccionado> ListaSeleccionados { get; set; }

        public Carrito(DataRow dr)
        {
            Codigo = dr[0].ToString();
            ClienteDNI = dr[1].ToString();
        }
        public Carrito()
        {

        }
    }
}
