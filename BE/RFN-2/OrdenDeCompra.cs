using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class OrdenDeCompra
    {
        public int ID { get; set; }
        public string NombreEmpresa { get; set; }
        public string CUIT {  get; set; }
        public DateTime Fecha { get; set; }
        public List<ProductoSeleccionado> ListaProductos { get; set; }

        public OrdenDeCompra() { }
        public OrdenDeCompra(DataRow dr)
        {
            ID = int.Parse(dr[0].ToString());
            NombreEmpresa = dr[1].ToString();
            CUIT = dr[2].ToString();
            Fecha = DateTime.Parse(dr[3].ToString());
        }
    }
}
