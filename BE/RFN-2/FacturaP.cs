using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BE
{
    public class FacturaP
    {
        public string ID { get; set; }
        public DateTime Fecha { get; set; }
        public string MetodoDePago { get; set; }
        public int ID_OrdenDeCompra { get; set; }
        public string NombreComprador { get; set; }
        public string NombreVendedor { get; set; }
        public List<ProductoSeleccionado> ListaProductos {  get; set; }
        public bool Recibido { get; set; }

        public FacturaP() { }
        public FacturaP(DataRow dr)
        {
            ID = dr[0].ToString();
            Fecha = DateTime.Parse(dr[1].ToString());
            MetodoDePago = dr[2].ToString();
            ID_OrdenDeCompra = int.Parse(dr[3].ToString());
            NombreComprador = dr[4].ToString();
            NombreVendedor = dr[5].ToString();
            Recibido = bool.Parse(dr[6].ToString());
        }
    }

}
