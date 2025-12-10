using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrdenDeCompraDAL : BaseDAL<OrdenDeCompra>, IOrdenDeCompraDAL
    {
        // Singleton
        private static OrdenDeCompraDAL _instance;
        public static OrdenDeCompraDAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new OrdenDeCompraDAL();
                return _instance;
            }
        }

        public OrdenDeCompraDAL() : base("OrdenDeCompra", "ID") { }

        protected override OrdenDeCompra MapearEntidad(DataRow row)
        {
            OrdenDeCompra oc = new OrdenDeCompra(row);
            oc.ListaProductos = ObtenerProductos(oc.ID);
            return oc;
        }

        public override void Agregar(OrdenDeCompra p)
        {
            string query = @"INSERT INTO OrdenDeCompra
                     (ID, NombreEmpresa, CUIT, Fecha)
                     VALUES (@ID, @NombreEmpresa, @CUIT, @Fecha)";

            var parameters = new Dictionary<string, object>
        {
            { "@ID", p.ID },
            { "@NombreEmpresa", p.NombreEmpresa },
            { "@CUIT", p.CUIT },
            { "@Fecha", p.Fecha }
        };

            dao.ExecuteNonQuery(query, parameters);
            GuardarProductos(p);
        }

        public override void Modificar(OrdenDeCompra p)
        {
            string query = @"UPDATE OrdenDeCompra SET
                     NombreEmpresa = @NombreEmpresa,
                     CUIT = @CUIT,
                     Fecha = @Fecha
                     WHERE ID = @ID";

            var parameters = new Dictionary<string, object>
        {
            { "@NombreEmpresa", p.NombreEmpresa },
            { "@CUIT", p.CUIT },
            { "@Fecha", p.Fecha },
            { "@ID", p.ID }
        };

            dao.ExecuteNonQuery(query, parameters);

            BorrarProductos(p.ID);
            GuardarProductos(p);
        }

        public override void Borrar(OrdenDeCompra p)
        {
            BorrarProductos(p.ID);
            string query = @"DELETE FROM OrdenDeCompra WHERE ID = @ID";
            dao.ExecuteNonQuery(query, new Dictionary<string, object> { { "@ID", p.ID } });
        }

        public int RetornarUltimoID()
        {
            string query = "SELECT MAX(ID) AS UltimoID FROM OrdenDeCompra";
            DataSet ds = dao.ExecuteDataSet(query);

            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["UltimoID"] != DBNull.Value)
                return Convert.ToInt32(ds.Tables[0].Rows[0]["UltimoID"]) + 1;

            return 1;
        }

        public List<OrdenDeCompra> RetornarOrdenesDeCompra()
        {
            return ObtenerTodos();
        }

        public List<OrdenDeCompra> RetornarOrdenDeCompraID(int ID)
        {
            List<OrdenDeCompra> lista = new List<OrdenDeCompra>();
            DataSet ds = dao.ExecuteDataSet(
                "SELECT * FROM OrdenDeCompra WHERE ID = @ID",
                new Dictionary<string, object> { { "@ID", ID } });

            foreach (DataRow dr in ds.Tables[0].Rows)
                lista.Add(MapearEntidad(dr));

            return lista;
        }

        private void GuardarProductos(OrdenDeCompra p)
        {
            foreach (ProductoSeleccionado prod in p.ListaProductos)
            {
                string query = @"INSERT INTO OrdenDeCompraProducto
                         (ID_Compra, CodigoProducto, Cantidad)
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

        private void BorrarProductos(int id)
        {
            string query = @"DELETE FROM OrdenDeCompraProducto WHERE ID_Compra = @ID";
            dao.ExecuteNonQuery(query, new Dictionary<string, object> { { "@ID", id } });
        }

        private List<ProductoSeleccionado> ObtenerProductos(int idOC)
        {
            List<ProductoSeleccionado> productos = new List<ProductoSeleccionado>();

            DataSet ds = dao.ExecuteDataSet(
                "SELECT CodigoProducto, Cantidad FROM OrdenDeCompraProducto WHERE ID_Compra = @ID",
                new Dictionary<string, object> { { "@ID", idOC } });

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string codigo = dr["CodigoProducto"].ToString();
                int cantidad = Convert.ToInt32(dr["Cantidad"]);

                DataSet ds2 = dao.ExecuteDataSet(
                    "SELECT * FROM Producto WHERE Codigo = @Cod",
                    new Dictionary<string, object> { { "@Cod", codigo } });

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    Producto producto = new Producto(ds2.Tables[0].Rows[0]);
                    productos.Add(new ProductoSeleccionado(producto, cantidad));
                }
            }
            return productos;
        }
    }

}
