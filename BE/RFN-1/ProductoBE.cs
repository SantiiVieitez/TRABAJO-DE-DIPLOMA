using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string TipoDeRepuesto {  get; set; }
        public int Cantidad { get; set; }
        public string TipoDeVehiculo { get; set; }
        public string Material { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        [Browsable(false)]
        public bool BorradoLogico { get; set; }

        public Producto(DataRow dr)
        {
            Codigo = dr[0].ToString();
            Nombre = dr[1].ToString();
            Marca = dr[2].ToString();
            TipoDeRepuesto = dr[3].ToString();
            Cantidad = int.Parse(dr[4].ToString());
            TipoDeVehiculo = dr[5].ToString();
            Material = dr[6].ToString();
            Precio = decimal.Parse(dr[7].ToString());
            Descripcion = dr[8].ToString();
            BorradoLogico = bool.Parse(dr[9].ToString());

        }
        public Producto(string pId, string pNombre, string pMarca, string pModelo, int pCantidad, string pTipoDeVehiculo, string pMaterial, decimal pPrecio, string pDescripcion)
        {
            Codigo = pId;
            Nombre = pNombre;
            Marca = pMarca;
            TipoDeRepuesto = pModelo;
            Cantidad = pCantidad;
            TipoDeVehiculo = pTipoDeVehiculo;
            Material = pMaterial;
            Precio = pPrecio;
            Descripcion = pDescripcion;
            BorradoLogico = false;
        }

    }
}
