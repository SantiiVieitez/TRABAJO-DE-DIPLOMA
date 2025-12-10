using BE;
using dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CarritoDAL : BaseDAL<Carrito>, ICarritoDAL
    {
        // Singleton
        private static CarritoDAL _CarritoDAL;

        public static CarritoDAL GetInstance
        {
            get
            {
                if (_CarritoDAL == null) _CarritoDAL = new CarritoDAL();
                return _CarritoDAL;
            }
        }

        // Constructor
        public CarritoDAL() : base("carrito", "Codigo") { }

        protected override Carrito MapearEntidad(DataRow row)
        {
            return new Carrito(row);
        }

        public override void Agregar(Carrito pCarrito)
        {
            string query = "INSERT INTO carrito (CodigoCarrito, ClienteDNI) VALUES (@CodigoCarrito, @ClienteDNI)";
            var parameters = new Dictionary<string, object>
        {
            { "@CodigoCarrito", pCarrito.Codigo },
            { "@ClienteDNI", pCarrito.ClienteDNI }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Modificar(Carrito pCarrito)
        {
            string query = "UPDATE carrito SET ClienteDNI = @ClienteDNI WHERE CodigoCarrito = @CodigoCarrito";
            var parameters = new Dictionary<string, object>
        {
            { "@ClienteDNI", pCarrito.ClienteDNI },
            { "@CodigoCarrito", pCarrito.Codigo }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Carrito pCarrito)
        {
            string query = "DELETE FROM carrito WHERE Codigo = @CodigoCarrito";
            var parameters = new Dictionary<string, object>
        {
            { "@Codigo", pCarrito.Codigo }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        

        public void AgregarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito)
        {
            string query = "INSERT INTO ProductoCarrito (CodigoProducto, CodigoCarrito, Cantidad) VALUES (@CodProducto, @CodCarrito, @Cantidad)";
            var parameters = new Dictionary<string, object>
        {
            { "@CodProducto", pProducto.CodigoProducto },
            { "@CodCarrito", pIdCarrito },
            { "@Cantidad", pProducto.CantidadProducto }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void BorrarProductoCarrito(string pIdProducto)
        {
            string query = "DELETE FROM ProductoCarrito WHERE CodigoProducto = @CodProducto";
            var parameters = new Dictionary<string, object>
        {
            { "@CodProducto", pIdProducto }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void ModificarProductoCarrito(ProductoSeleccionado pProducto, string pIdCarrito)
        {
            string query = @"UPDATE ProductoCarrito 
                         SET Cantidad = @Cantidad 
                         WHERE CodigoProducto = @CodProducto AND CodigoCarrito = @CodCarrito";
            var parameters = new Dictionary<string, object>
        {
            { "@Cantidad", pProducto.CantidadProducto },
            { "@CodProducto", pProducto.CodigoProducto },
            { "@CodCarrito", pIdCarrito }
        };
            dao.ExecuteNonQuery(query, parameters);
        }

        // --- MÉTODOS DE BÚSQUEDA PERSONALIZADOS ---

        public Carrito ObtenerCarrito(string pDNI)
        {
            string query = "SELECT * FROM carrito WHERE ClienteDNI = @DNI";
            var parameters = new Dictionary<string, object>
        {
            { "@DNI", pDNI }
        };

            ds = dao.ExecuteDataSet(query, parameters);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return MapearEntidad(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public bool BuscarDNI(string pDNI)
        {
            string query = "SELECT 1 FROM carrito WHERE ClienteDNI = @DNI";
            var parameters = new Dictionary<string, object>
        {
            { "@DNI", pDNI }
        };

            ds = dao.ExecuteDataSet(query, parameters);

            return (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }
    }
}
