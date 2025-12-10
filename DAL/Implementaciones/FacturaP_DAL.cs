using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RFN_2
{
    public class FacturaP_DAL : BaseDAL<FacturaP>, IFacturaP_DAL
    {
        // Singleton
        private static FacturaP_DAL _instance;
        public static FacturaP_DAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new FacturaP_DAL();
                return _instance;
            }
        }

        public FacturaP_DAL() : base("FacturaP", "ID") { }

        protected override FacturaP MapearEntidad(DataRow row)
        {
            FacturaP factura = new FacturaP(row);
            factura.ListaProductos = ObtenerProductosFactura(factura.ID);
            return factura;
        }

        public override void Agregar(FacturaP p)
        {
            string query = @"INSERT INTO FacturaP 
                     (ID, Fecha, MetodoDePago, ID_OrdenDeCompra, NombreComprador, NombreVendedor)
                     VALUES (@ID, @Fecha, @MetodoDePago, @ID_OC, @Comprador, @Vendedor)";

            var parameters = new Dictionary<string, object>
        {
            { "@ID", p.ID },
            { "@Fecha", p.Fecha },
            { "@MetodoDePago", p.MetodoDePago },
            { "@ID_OC", p.ID_OrdenDeCompra },
            { "@Comprador", p.NombreComprador },
            { "@Vendedor", p.NombreVendedor }
        };

            dao.ExecuteNonQuery(query, parameters);
            GuardarProductosFactura(p);
        }

        public override void Modificar(FacturaP p)
        {
            string query = @"UPDATE FacturaP SET
                     Recibido = @Recibido
                     WHERE ID = @ID";

            var parameters = new Dictionary<string, object>
        {
            { "@Recibido", p.Recibido },
            { "@ID", p.ID }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(FacturaP p)
        {
            string q1 = @"DELETE FROM ProductoFacturaP WHERE ID_FacturaP = @ID";
            dao.ExecuteNonQuery(q1, new Dictionary<string, object> { { "@ID", p.ID } });

            string q2 = @"DELETE FROM FacturaP WHERE ID = @ID";
            dao.ExecuteNonQuery(q2, new Dictionary<string, object> { { "@ID", p.ID } });
        }

        public List<FacturaP> RetornarFacturasP()
        {
            return ObtenerTodos();
        }

        public void RegistrarFacturaP(FacturaP p)
        {
            Agregar(p);
        }

        private void GuardarProductosFactura(FacturaP p)
        {
            foreach (ProductoSeleccionado prod in p.ListaProductos)
            {
                string query = @"INSERT INTO ProductoFacturaP 
                         (ID_FacturaP, CodigoProducto, Cantidad)
                         VALUES (@ID, @Cod, @Cant)";

                var parameters = new Dictionary<string, object>
            {
                { "@ID", p.ID },
                { "@Cod", prod.CodigoProducto },
                { "@Cant", prod.CantidadProducto }
            };
                dao.ExecuteNonQuery(query, parameters);
            }
        }

        private List<ProductoSeleccionado> ObtenerProductosFactura(string idFactura)
        {
            List<ProductoSeleccionado> lista = new List<ProductoSeleccionado>();

            DataSet dsProd = dao.ExecuteDataSet(
                "SELECT CodigoProducto, Cantidad FROM ProductoFacturaP WHERE ID_FacturaP = @ID",
                new Dictionary<string, object> { { "@ID", idFactura } }
            );

            foreach (DataRow dr in dsProd.Tables[0].Rows)
            {
                string cod = dr["CodigoProducto"].ToString();
                int cantidad = Convert.ToInt32(dr["Cantidad"]);

                DataSet dsDet = dao.ExecuteDataSet(
                    "SELECT * FROM Producto WHERE Codigo = @Cod",
                    new Dictionary<string, object> { { "@Cod", cod } }
                );

                if (dsDet.Tables[0].Rows.Count > 0)
                {
                    Producto prod = new Producto(dsDet.Tables[0].Rows[0]);
                    lista.Add(new ProductoSeleccionado(prod, cantidad));
                }
            }
            return lista;
        }
    }
}
