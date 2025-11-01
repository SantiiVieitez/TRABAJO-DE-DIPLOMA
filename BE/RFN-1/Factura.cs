using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Factura
    {
        public int ID { get; set; }
        public string MetodoDePago { get; set; }
        public string DNI_Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public List<ProductoSeleccionado> ListaSeleccionados { get; set; }

        public Factura(DataRow dr)
        {
            ID = int.Parse(dr[0].ToString());
            MetodoDePago = dr[1].ToString();
            DNI_Cliente = dr[2].ToString();
            Fecha = DateTime.Parse(dr[3].ToString());
        }
        public Factura()
        {

        }
    }
}
