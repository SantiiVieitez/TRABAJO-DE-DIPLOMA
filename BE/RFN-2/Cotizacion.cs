using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Cotizacion
    {
        public List<ProductoSeleccionado> Productos { get; set; }
        public int CotizacionID { get; set; }
        [Browsable(false)]
        public Proveedor proveedor { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime Fecha { get; set; }

        public Cotizacion() { }

        public Cotizacion(DataRow dr)
        {
            CotizacionID = int.Parse(dr[0].ToString());
            Fecha = DateTime.Parse(dr[3].ToString());
        }
    }
}
