using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.RFN_2
{
    public class ProductoC
    {
        public int ID { get; set; }
        public string CodigoProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Nombre { get; set; }
        public int Existencia { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string Marca { get; set; }
        public string TipoDeRepuesto { get; set; }
        public int Cantidad { get; set; }
        public string TipoDeVehiculo { get; set; }
        public string Material { get; set; }
        public decimal Precio { get; set; }
        public string BorradoLogico { get; set; }

        public ProductoC(DataRow dr)
        {
            ID = int.Parse(dr[0].ToString());
            CodigoProducto = dr[1].ToString();
            Fecha = DateTime.Parse(dr[2].ToString()).Date;
            Hora = DateTime.Parse(dr[3].ToString()).ToString("HH:mm:ss");
            Nombre = dr[4].ToString();
            Existencia = int.Parse(dr[5].ToString());
            Descripcion = dr[6].ToString();
            Activo = bool.Parse(dr[7].ToString());
            Marca = dr[8].ToString();
            TipoDeRepuesto = dr[9].ToString();
            Cantidad = int.Parse(dr[10].ToString());
            TipoDeVehiculo = dr[11].ToString();
            Material = dr[12].ToString();
            Precio = decimal.Parse(dr[13].ToString());
            BorradoLogico = dr[14].ToString();
        }
    }
}
