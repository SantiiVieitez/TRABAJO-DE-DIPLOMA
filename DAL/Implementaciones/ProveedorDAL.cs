using BE;
using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProveedorDAL : BaseDAL<Proveedor>, IProveedorDAL
    {
        // Singleton
        private static ProveedorDAL _instance;
        public static ProveedorDAL GetInstance
        {
            get
            {
                if (_instance == null) _instance = new ProveedorDAL();
                return _instance;
            }
        }

        public ProveedorDAL() : base("Proveedor", "CUIT") { }

        protected override Proveedor MapearEntidad(DataRow row)
        {
            return new Proveedor(row);
        }

        public override void Agregar(Proveedor p)
        {
            RegistrarProveedor(p, false);
        }

        public void RegistrarProveedor(Proveedor p, bool a)
        {
            string query;
            var parameters = new Dictionary<string, object>();

            parameters["@CUIT"] = p.CUIT;
            parameters["@Nombre"] = p.Nombre;
            parameters["@Email"] = p.Email;
            parameters["@Telefono"] = p.Telefono;

            if (a)
            {
                query = @"INSERT INTO Proveedor (CUIT, Nombre, Email, Telefono)
                  VALUES (@CUIT, @Nombre, @Email, @Telefono)";
            }
            else
            {
                query = @"INSERT INTO Proveedor (CUIT, Nombre, Email, Telefono, CBU, Banco)
                  VALUES (@CUIT, @Nombre, @Email, @Telefono, @CBU, @Banco)";
                parameters["@CBU"] = p.CBU;
                parameters["@Banco"] = p.Banco;
            }

            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Modificar(Proveedor p)
        {
            // Por defecto actualiza todo
            ModificarProveedor(p, true);
        }

        public void ModificarProveedor(Proveedor p, bool a)
        {
            string query;
            var parameters = new Dictionary<string, object>();

            parameters["@Nombre"] = p.Nombre;
            parameters["@Email"] = p.Email;
            parameters["@Telefono"] = p.Telefono;
            parameters["@CUIT"] = p.CUIT;

            if (a)
            {
                query = @"UPDATE Proveedor SET 
                      Nombre = @Nombre,
                      Email = @Email,
                      Telefono = @Telefono
                   WHERE CUIT = @CUIT";
            }
            else
            {
                query = @"UPDATE Proveedor SET 
                      Nombre = @Nombre,
                      Email = @Email,
                      Telefono = @Telefono,
                      CBU = @CBU,
                      Banco = @Banco
                   WHERE CUIT = @CUIT";

                parameters["@CBU"] = p.CBU;
                parameters["@Banco"] = p.Banco;
            }

            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Proveedor p)
        {
            string query = "DELETE FROM Proveedor WHERE CUIT = @CUIT";
            var parameters = new Dictionary<string, object> { { "@CUIT", p.CUIT } };
            dao.ExecuteNonQuery(query, parameters);
        }

        public Proveedor BuscarProveedor(string id)
        {
            return ObtenerPorId(id);
        }

        public List<Proveedor> ListarProveedores()
        {
            return ObtenerTodos();
        }
    }
}
