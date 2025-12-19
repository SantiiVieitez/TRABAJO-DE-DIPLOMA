using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FacturaDAL : BaseDAL<Factura>, IFacturaDAL
    {
        private static FacturaDAL _instance;
        public static FacturaDAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new FacturaDAL();
                return _instance;
            }
        }

        public FacturaDAL() : base("Factura", "ID") { }

        public FacturaDAL(basededatos dao) : base("Factura", "ID", dao) { }

        protected override Factura MapearEntidad(DataRow row)
        {
            return new Factura(row);
        }

        public override void Agregar(Factura pFactura)
        {
        
            string query = @"INSERT INTO Factura
                     (ID, MetodoDePago, DNI_Cliente, Fecha, DVH)
                     VALUES
                     (@ID, @MetodoDePago, @DNI_Cliente, @Fecha, @DVH)";

            var parameters = new Dictionary<string, object>
        {
            { "@ID",           pFactura.ID },
            { "@MetodoDePago", pFactura.MetodoDePago },
            { "@DNI_Cliente",  pFactura.DNI_Cliente },
            { "@Fecha",        pFactura.Fecha },
            { "@DVH",          pFactura.DVH}
        };
            dao.ExecuteNonQuery(query, parameters);
            GuardarProductos(pFactura);
        }

        public override void Modificar(Factura pFactura)
        {
            string query = @"UPDATE Factura SET
                         MetodoDePago = @MetodoDePago,
                         DNI_Cliente  = @DNI_Cliente,
                         Fecha        = @Fecha
                     WHERE ID = @ID";

            var parameters = new Dictionary<string, object>
        {
            { "@ID",           pFactura.ID },
            { "@MetodoDePago", pFactura.MetodoDePago },
            { "@DNI_Cliente",  pFactura.DNI_Cliente },
            { "@Fecha",        pFactura.Fecha }
        };

            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Factura pFactura)
        {
            string queryDetalles = @"DELETE FROM ProductoFactura WHERE CodigoFactura = @ID";
            var parameters = new Dictionary<string, object> { { "@ID", pFactura.ID } };
            dao.ExecuteNonQuery(queryDetalles, parameters);

            string queryFactura = @"DELETE FROM Factura WHERE ID = @ID";
            dao.ExecuteNonQuery(queryFactura, parameters);
        }

        public void GuardarProductos(Factura pFactura)
        {
            foreach (ProductoSeleccionado p in pFactura.ListaSeleccionados)
            {
                string query = @"INSERT INTO ProductoFactura
                         (CodigoProducto, CodigoFactura, Cantidad)
                         VALUES
                         (@CodigoProducto, @CodigoFactura, @Cantidad)";

                var parameters = new Dictionary<string, object>
            {
                { "@CodigoProducto", p.CodigoProducto },
                { "@CodigoFactura",  pFactura.ID },
                { "@Cantidad",       p.CantidadProducto }
            };
                dao.ExecuteNonQuery(query, parameters);
            }
        }

        public Factura ObtenerPorId(string idFactura)
        {
            var factura = base.ObtenerPorId(idFactura);
            if (factura == null) return null;

            CargarProductosFactura(factura);
            return factura;
        }

        public override List<Factura> ObtenerTodos()
        {
            string query = "SELECT * FROM Factura";
            DataSet dsFacturas = dao.ExecuteDataSet(query);

            var lista = new List<Factura>();
            foreach (DataRow dr in dsFacturas.Tables[0].Rows)
            {
                Factura factura = new Factura(dr);
                factura.ListaSeleccionados = new List<ProductoSeleccionado>();
                CargarProductosFactura(factura);
                lista.Add(factura);
            }
            return lista;
        }

        private void CargarProductosFactura(Factura factura)
        {
            // CAMBIO AQUI: Usamos SUM(Cantidad) y GROUP BY para unificar duplicados
            string query = @"SELECT CodigoProducto, SUM(Cantidad) as Cantidad
                     FROM ProductoFactura
                     WHERE CodigoFactura = @CodigoFactura
                     GROUP BY CodigoProducto";

            var parameters = new Dictionary<string, object> { { "@CodigoFactura", factura.ID } };
            DataSet dsProductosFactura = dao.ExecuteDataSet(query, parameters);

            foreach (DataRow drProductoFactura in dsProductosFactura.Tables[0].Rows)
            {
                string codigoProducto = drProductoFactura["CodigoProducto"].ToString();
                int cantidad = Convert.ToInt32(drProductoFactura["Cantidad"]);

                string queryProducto = @"SELECT * FROM Producto WHERE Codigo = @Codigo";
                var paramProd = new Dictionary<string, object> { { "@Codigo", codigoProducto } };
                DataSet dsDetalleProducto = dao.ExecuteDataSet(queryProducto, paramProd);

                if (dsDetalleProducto.Tables[0].Rows.Count > 0)
                {
                    Producto producto = new Producto(dsDetalleProducto.Tables[0].Rows[0]);

                    // Aquí la cantidad ya viene sumada (ej: si había dos filas de 5, ahora vendrá un 10)
                    ProductoSeleccionado productoSeleccionado = new ProductoSeleccionado(producto, cantidad);

                    if (factura.ListaSeleccionados == null)
                        factura.ListaSeleccionados = new List<ProductoSeleccionado>();

                    factura.ListaSeleccionados.Add(productoSeleccionado);
                }
            }
        }
        public void ActualizarDVH(int id, string hash)
        {
            try
            {
                string query = "UPDATE Factura SET DVH = @DVH WHERE ID = @ID";

                var parameters = new Dictionary<string, object>
                {
                    { "@ID", id },
                    { "@DVH", hash }
                };

                dao.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el DVH de la factura {id}: " + ex.Message);
            }
        }
    }
}
