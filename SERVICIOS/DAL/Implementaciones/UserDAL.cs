using DAL;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class UserDAL : BaseAuthDAL<Usuarios>, IUserDAL
    {
        private static UserDAL _instance;
        public static UserDAL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserDAL();
                }
                return _instance;
            }
        }

        private UserDAL() : base("Usuario", "DNI")
        {
        }

        protected override Usuarios MapearEntidad(DataRow row)
        {
            return new Usuarios(row);
        }

        public override void Agregar(Usuarios pUsuario)
        {
            string query = "insert into Usuario values (@DNI, @Apellido, @Nombre, @Login, @Contrasena, @Rol, @Email, @Bloqueo, @Activo, @Intentos, @Idioma)";
            var parameters = new Dictionary<string, object>
            {
                { "@DNI", pUsuario.DNI },
                { "@Apellido", pUsuario.Apellido },
                { "@Nombre", pUsuario.Nombre },
                { "@Login", pUsuario.Login },
                { "@Contrasena", pUsuario.Contraseña },
                { "@Rol", pUsuario.Rol },
                { "@Email", pUsuario.Email },
                { "@Bloqueo", pUsuario.Bloqueo ? 1 : 0 },
                { "@Activo", pUsuario.Activo ? 1 : 0 },
                { "@Intentos", pUsuario.Intentos },
                { "@Idioma", pUsuario.Idioma }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Modificar(Usuarios pUsuario)
        {
            string query = "update usuario set apellido = @Apellido, nombre = @Nombre, login = @Login, contraseña = @Contrasena, rol = @Rol, email = @Email, bloqueo = @Bloqueo, activo = @Activo where DNI = @DNI";
            var parameters = new Dictionary<string, object>
            {
                { "@Apellido", pUsuario.Apellido },
                { "@Nombre", pUsuario.Nombre },
                { "@Login", pUsuario.Login },
                { "@Contrasena", pUsuario.Contraseña },
                { "@Rol", pUsuario.Rol },
                { "@Email", pUsuario.Email },
                { "@Bloqueo", pUsuario.Bloqueo ? 1 : 0 },
                { "@Activo", pUsuario.Activo ? 1 : 0 },
                { "@DNI", pUsuario.DNI }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public override void Borrar(Usuarios pUsuario)
        {
            string query = "delete from usuario where DNI = @DNI";
            var parameters = new Dictionary<string, object>
            {
                { "@DNI", pUsuario.DNI }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public Usuarios BuscarUsuario(string Login)
        {
            string query = "select * from Usuario where Login = @Login";
            var parameters = new Dictionary<string, object>
            {
                { "@Login", Login }
            };
            ds = dao.ExecuteDataSet(query, parameters);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                throw new Exception("Contraseña / Password | Usuario / User Incorrecto / Wrong");
            }

            return MapearEntidad(ds.Tables[0].Rows[0]);
        }

        public Usuarios BuscarDNI(string DNI)
        {
            return ObtenerPorId(DNI);
        }

        public List<Usuarios> RetornarListaUsuarios()
        {
            return ObtenerTodos();
        }

        public void SumarIntento(Usuarios pUsuario)
        {
            string query = "update usuario set intentos = @Intentos where login = @Login";
            var parameters = new Dictionary<string, object>
            {
                { "@Intentos", pUsuario.Intentos },
                { "@Login", pUsuario.Login }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void Bloquear(Usuarios pUsuario)
        {
            string query = "update usuario set bloqueo = 1 where login = @Login";
            var parameters = new Dictionary<string, object>
            {
                { "@Login", pUsuario.Login }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void Desbloquear(Usuarios pUsuario)
        {
            string query = "update usuario set bloqueo = 0, intentos = 0, contraseña = @Contrasena where login = @Login";
            var parameters = new Dictionary<string, object>
            {
                { "@Contrasena", pUsuario.Contraseña },
                { "@Login", pUsuario.Login }
            };
            dao.ExecuteNonQuery(query, parameters);
        }

        public void CambiarIdioma(Usuarios pUsuario)
        {
            string query = "update usuario set idioma = @Idioma where DNI = @DNI";
            var parameters = new Dictionary<string, object>
            {
                { "@Idioma", pUsuario.Idioma },
                { "@DNI", pUsuario.DNI }
            };
            dao.ExecuteNonQuery(query, parameters);
        }
    }
}