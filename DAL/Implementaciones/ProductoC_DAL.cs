using BE.RFN_2;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.RFN_2
{
    public class ProductoC_DAL : BaseDAL<ProductoC>
    {
        // Singleton
        private static ProductoC_DAL _instance;
        public static ProductoC_DAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new ProductoC_DAL();
                return _instance;
            }
        }

        public ProductoC_DAL() : base("Producto_C", "ID") { }

        protected override ProductoC MapearEntidad(DataRow row)
        {
            return new ProductoC(row);
        }

        public List<ProductoC> RetonarProductoC()
        {
            return ObtenerTodos();
        }

        // Métodos obligatorios de BaseDAL (Si no se usan, pueden quedar vacíos o con excepciones)
        public override void Agregar(ProductoC entity)
        {
            // Implementar INSERT si es necesario
            throw new NotImplementedException("No implementado para ProductoC");
        }
        public override void Borrar(ProductoC entity)
        {
            string query = "DELETE FROM Producto_C WHERE ID = @ID";
            dao.ExecuteNonQuery(query, new Dictionary<string, object> { { "@ID", entity.ID } });
        }
        public override void Modificar(ProductoC entity)
        {
            // Implementar UPDATE si es necesario
            throw new NotImplementedException("No implementado para ProductoC");
        }

        // Lógica personalizada que tenías
        public void ActivarProductoC(ProductoC c)
        {
            // 1. Desactivar el producto actual activo
            string queryDesactivar = @"UPDATE Producto_C SET Activo = 0 
                                   WHERE Cod_Prod = @CodProd AND Activo = 1";
            dao.ExecuteNonQuery(queryDesactivar, new Dictionary<string, object> { { "@CodProd", c.CodigoProducto } });

            // 2. Activar el producto seleccionado por ID
            string queryActivar = @"UPDATE Producto_C SET Activo = 1 WHERE ID = @ID";
            dao.ExecuteNonQuery(queryActivar, new Dictionary<string, object> { { "@ID", c.ID } });

            // 3. Actualizar la tabla principal 'Producto' con los datos de Producto_C
            string queryActualizarProducto = @"
        UPDATE Producto
        SET 
            Nombre = (SELECT Nombre FROM Producto_C WHERE ID = @ID),
            Cantidad = (SELECT Cantidad FROM Producto_C WHERE ID = @ID),
            Descripcion = (SELECT Descripcion FROM Producto_C WHERE ID = @ID),
            Marca = (SELECT Marca FROM Producto_C WHERE ID = @ID),
            TipoDeRepuesto = (SELECT TipoDeRepuesto FROM Producto_C WHERE ID = @ID),
            TipoDeVehiculo = (SELECT TipoDeVehiculo FROM Producto_C WHERE ID = @ID),
            Material = (SELECT Material FROM Producto_C WHERE ID = @ID),
            Precio = (SELECT Precio FROM Producto_C WHERE ID = @ID),
            BorradoLogico = (SELECT BorradoLogico FROM Producto_C WHERE ID = @ID)
        WHERE Codigo = @CodProd";

            var parameters = new Dictionary<string, object>
        {
            { "@ID", c.ID },
            { "@CodProd", c.CodigoProducto }
        };

            dao.ExecuteNonQuery(queryActualizarProducto, parameters);
        }
    }
}
