using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductoDAL : BaseDAL<Producto>, IProductoDAL
    {
        // Singleton
        private static ProductoDAL _instance;
        public static ProductoDAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new ProductoDAL();
                return _instance;
            }
        }

        public ProductoDAL() : base("producto", "Codigo") { }

        protected override Producto MapearEntidad(DataRow row)
        {
            return new Producto(row);
        }

        public override void Agregar(Producto pProducto)
        {
            string query = @"INSERT INTO producto
                     (Codigo, Nombre, Marca, TipoDeRepuesto, Cantidad, 
                      TipoDeVehiculo, Material, Precio, Descripcion, BorradoLogico)
                     VALUES
                     (@Codigo, @Nombre, @Marca, @TipoDeRepuesto, @Cantidad,
                      @TipoDeVehiculo, @Material, @Precio, @Descripcion, @BorradoLogico)";

            var parameters = new Dictionary<string, object>
        {
            { "@Codigo",         pProducto.Codigo },
            { "@Nombre",         pProducto.Nombre },
            { "@Marca",          pProducto.Marca },
            { "@TipoDeRepuesto", pProducto.TipoDeRepuesto },
            { "@Cantidad",       pProducto.Cantidad },
            { "@TipoDeVehiculo", pProducto.TipoDeVehiculo },
            { "@Material",       pProducto.Material },
            { "@Precio",         pProducto.Precio },
            { "@Descripcion",    pProducto.Descripcion },
            { "@BorradoLogico",  pProducto.BorradoLogico }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Modificar(Producto pProducto)
        {
            string query = @"UPDATE producto SET
                         Nombre         = @Nombre,
                         Marca          = @Marca,
                         TipoDeRepuesto = @TipoDeRepuesto,
                         Cantidad       = @Cantidad,
                         TipoDeVehiculo = @TipoDeVehiculo,
                         Material       = @Material,
                         Precio         = @Precio,
                         Descripcion    = @Descripcion,
                         BorradoLogico  = @BorradoLogico
                     WHERE Codigo = @Codigo";

            var parameters = new Dictionary<string, object>
        {
            { "@Codigo",         pProducto.Codigo },
            { "@Nombre",         pProducto.Nombre },
            { "@Marca",          pProducto.Marca },
            { "@TipoDeRepuesto", pProducto.TipoDeRepuesto },
            { "@Cantidad",       pProducto.Cantidad },
            { "@TipoDeVehiculo", pProducto.TipoDeVehiculo },
            { "@Material",       pProducto.Material },
            { "@Precio",         pProducto.Precio },
            { "@Descripcion",    pProducto.Descripcion },
            { "@BorradoLogico",  pProducto.BorradoLogico }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Producto pProducto)
        {
            // Borrado lógico
            string query = @"UPDATE producto
                     SET BorradoLogico = 1
                     WHERE Codigo = @Codigo";

            var parameters = new Dictionary<string, object>
        {
            { "@Codigo", pProducto.Codigo }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public Producto ObtenerPorId(string codigo)
        {
            return base.ObtenerPorId(codigo);
        }

        public List<ProductoSeleccionado> ListaProductoCarrito(string pIdCarrito)
        {
            string query = @"SELECT * FROM ProductoCarrito
                     WHERE CodigoCarrito = @CodigoCarrito";

            var parameters = new Dictionary<string, object> { { "@CodigoCarrito", pIdCarrito } };
            var ds = dao.ExecuteDataSet(query, parameters);
            var list = new List<ProductoSeleccionado>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var producto = ObtenerPorId(dr[0].ToString());
                int cantidad = int.Parse(dr[2].ToString());
                list.Add(new ProductoSeleccionado(producto, cantidad));
            }
            return list;
        }
    }
}
