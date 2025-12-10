using BE;
using DAL.Contrato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL : BaseDAL<Cliente>, iClienteDAL
    {
        public ClienteDAL() : base("Cliente", "DNI") { }
        protected override Cliente MapearEntidad(DataRow row)
        {
            return new Cliente(row);
        }
        private static ClienteDAL _ClienteDAL;
        public override void Agregar(Cliente pCliente)
        {
            string query = @"INSERT INTO cliente 
                         (DNI, Nombre, Apellido, Domicilio, Correo, Telefono) 
                         VALUES 
                         (@DNI, @Nombre, @Apellido, @Domicilio, @Correo, @Telefono)";

            var parameters = new Dictionary<string, object>
        {
            { "@DNI", pCliente.DNI },
            { "@Nombre", pCliente.Nombre },
            { "@Apellido", pCliente.Apellido },
            { "@Domicilio", pCliente.Domicilio },
            { "@Correo", pCliente.Correo },
            { "@Telefono", pCliente.Telefono }
        };
            dao.ExecuteNonQuery(query, parameters);
        }
        public override void Modificar(Cliente pCliente)
        {
            string query = @"UPDATE cliente SET 
                            Nombre = @Nombre, 
                            Apellido = @Apellido, 
                            Domicilio = @Domicilio, 
                            Correo = @Correo, 
                            Telefono = @Telefono 
                         WHERE DNI = @DNI";

            var parameters = new Dictionary<string, object>
        {
            { "@Nombre", pCliente.Nombre },
            { "@Apellido", pCliente.Apellido },
            { "@Domicilio", pCliente.Domicilio },
            { "@Correo", pCliente.Correo },
            { "@Telefono", pCliente.Telefono },
            { "@DNI", pCliente.DNI }
        };

            dao.ExecuteNonQuery(query, parameters);
        }
        public override void Borrar(Cliente pCliente)
        {
            string query = "DELETE FROM cliente WHERE DNI = @DNI";

            var parameters = new Dictionary<string, object>
        {
            { "@DNI", pCliente.DNI }
        };

            dao.ExecuteNonQuery(query, parameters);
        }
        public Cliente ObtenerPorId(string dni)
        {
            return base.ObtenerPorId(dni);
        }

        public static ClienteDAL GetInstance
        {
            get
            {
                if(_ClienteDAL == null) _ClienteDAL = new ClienteDAL();

                return _ClienteDAL;
            }
        }
    }
}
