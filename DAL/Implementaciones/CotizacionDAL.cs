using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CotizacionDAL : BaseDAL<Cotizacion>, ICotizacionDAL
    {
        // Singleton
        private static CotizacionDAL _instance;
        public static CotizacionDAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new CotizacionDAL();
                return _instance;
            }
        }

        public CotizacionDAL() : base("SolicitudCotizacion", "ID") { }

        protected override Cotizacion MapearEntidad(DataRow row)
        {
            Cotizacion c = new Cotizacion(row);
            // Usamos la instancia Singleton de ProveedorDAL
            c.proveedor = ProveedorDAL.GetInstance.BuscarProveedor(row["CUIT"].ToString());
            c.NombreProveedor = c.proveedor?.Nombre;
            c.Productos = ObtenerProductosDeCotizacion(c.CotizacionID);

            return c;
        }

        public int RetornarUltimoID()
        {
            string query = "SELECT MAX(ID) AS UltimoID FROM SolicitudCotizacion";
            DataSet ds = dao.ExecuteDataSet(query);

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["UltimoID"] != DBNull.Value)
                return Convert.ToInt32(ds.Tables[0].Rows[0]["UltimoID"]) + 1;

            return 1;
        }

        public override void Agregar(Cotizacion p)
        {
            string query = @"INSERT INTO SolicitudCotizacion
                     (ID, CUIT, NombreEmpresa, Fecha)
                     VALUES (@ID, @CUIT, @NombreEmpresa, @Fecha)";

            var parameters = new Dictionary<string, object>
        {
            { "@ID", p.CotizacionID },
            { "@CUIT", p.proveedor.CUIT },
            { "@NombreEmpresa", p.proveedor.Nombre },
            { "@Fecha", p.Fecha }
        };

            dao.ExecuteNonQuery(query, parameters);
            GuardarProductosCotizacion(p);
        }

        public override void Modificar(Cotizacion p)
        {
            string query = @"UPDATE SolicitudCotizacion SET
                     CUIT = @CUIT,
                     NombreEmpresa = @NombreEmpresa,
                     Fecha = @Fecha
                     WHERE ID = @ID";

            var parameters = new Dictionary<string, object>
        {
            { "@CUIT", p.proveedor.CUIT },
            { "@NombreEmpresa", p.proveedor.Nombre },
            { "@Fecha", p.Fecha },
            { "@ID", p.CotizacionID }
        };

            dao.ExecuteNonQuery(query, parameters);

            BorrarProductosCotizacion(p.CotizacionID);
            GuardarProductosCotizacion(p);
        }

        public override void Borrar(Cotizacion p)
        {
            BorrarProductosCotizacion(p.CotizacionID);

            string query = @"DELETE FROM SolicitudCotizacion WHERE ID = @ID";
            var parameters = new Dictionary<string, object> { { "@ID", p.CotizacionID } };

            dao.ExecuteNonQuery(query, parameters);
        }

        private void BorrarProductosCotizacion(int id)
        {
            string query = @"DELETE FROM CotizacionProductos WHERE ID_Cotizacion = @ID";
            dao.ExecuteNonQuery(query, new Dictionary<string, object> { { "@ID", id } });
        }

        public void GuardarProductosCotizacion(Cotizacion p)
        {
            foreach (ProductoSeleccionado pr in p.Productos)
            {
                string query = @"INSERT INTO CotizacionProductos
                         (ID_Cotizacion, CodigoProducto, Cantidad)
                         VALUES (@ID, @Codigo, @Cantidad)";

                var parameters = new Dictionary<string, object>
            {
                { "@ID", p.CotizacionID },
                { "@Codigo", pr.CodigoProducto },
                { "@Cantidad", pr.CantidadProducto }
            };
                dao.ExecuteNonQuery(query, parameters);
            }
        }

        private List<ProductoSeleccionado> ObtenerProductosDeCotizacion(int idCot)
        {
            List<ProductoSeleccionado> lista = new List<ProductoSeleccionado>();

            DataSet dsProd = dao.ExecuteDataSet(
                @"SELECT CodigoProducto, Cantidad 
              FROM CotizacionProductos 
              WHERE ID_Cotizacion = @ID",
                new Dictionary<string, object> { { "@ID", idCot } }
            );

            foreach (DataRow dr in dsProd.Tables[0].Rows)
            {
                string codigo = dr["CodigoProducto"].ToString();
                int cantidad = Convert.ToInt32(dr["Cantidad"]);

                DataSet dsDet = dao.ExecuteDataSet(
                    "SELECT * FROM Producto WHERE Codigo = @Cod",
                    new Dictionary<string, object> { { "@Cod", codigo } }
                );

                if (dsDet.Tables[0].Rows.Count > 0)
                {
                    Producto prod = new Producto(dsDet.Tables[0].Rows[0]);
                    lista.Add(new ProductoSeleccionado(prod, cantidad));
                }
            }
            return lista;
        }

        public List<Cotizacion> RetornarCotizaciones(string cuit)
        {
            List<Cotizacion> lista = new List<Cotizacion>();
            string query = @"SELECT * FROM SolicitudCotizacion WHERE CUIT = @CUIT";

            DataSet dsCot = dao.ExecuteDataSet(query, new Dictionary<string, object> { { "@CUIT", cuit } });

            foreach (DataRow dr in dsCot.Tables[0].Rows)
            {
                Cotizacion c = MapearEntidad(dr);
                lista.Add(c);
            }
            return lista;
        }
    }

}
