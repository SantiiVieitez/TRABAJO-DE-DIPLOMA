using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS.Domain;
using dao;

namespace DAL
{
    public class UserDAL
    {
        AuthDB daoUser;
        DataSet ds;
        public UserDAL()
        {
            daoUser = new AuthDB();
            ds = new DataSet();
        }
        public void LlenarDataSet()
        {
            string query = "select * from usuario";
            ds = daoUser.ExecuteDataSet(query);
        }
        public Usuarios BuscarUsuario(string Login)
        {
            ds = daoUser.ExecuteDataSet($"select * from Usuario where Login = '{Login}'");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                throw new Exception("el usuario o la contraseña son incorrectos bd");
            }
            Usuarios aux = new Usuarios(ds.Tables[0].Rows[0]);
            return aux;
        }
        public Usuarios BuscarDNI(string DNI)
        {
            ds = daoUser.ExecuteDataSet($"select * from Usuario where DNI = '{DNI}'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Usuarios aux = new Usuarios(ds.Tables[0].Rows[0]);
                return aux;
            }
            else
            {
                return null;
            }
            
        }
        public void SumarIntento(Usuarios pUsuario)
        {
            string query = $"update usuario set intentos = {pUsuario.Intentos} where login = '{pUsuario.Login}'";
            daoUser.ExecuteNonQuery(query);
        }
        public void Bloquear(Usuarios pUsuario)
        {
            string query = $"update usuario set bloqueo = 1 where login = '{pUsuario.Login}'";
            daoUser.ExecuteNonQuery(query);
        }
        public void Desbloquear(Usuarios pUsuario)
        {
            string query = $"update usuario set bloqueo = 0, intentos = 0, contraseña = '{pUsuario.Contraseña}' where login = '{pUsuario.Login}'";
            daoUser.ExecuteNonQuery(query);
        }
        public void Agregar(Usuarios pUsuario)
        {
            string query = $"insert into Usuario values ('{pUsuario.DNI}','{pUsuario.Apellido}','{pUsuario.Nombre}','{pUsuario.Login}','{pUsuario.Contraseña}','{pUsuario.Rol}','{pUsuario.Email}',{(pUsuario.Bloqueo ? 1 : 0)},{(pUsuario.Activo ? 1 : 0)},{pUsuario.Intentos},'{pUsuario.Idioma}')";
            daoUser.ExecuteNonQuery(query);
        }
        public void Borrar(Usuarios pUsuario)
        {
            string query = $"delete from usuario where DNI = '{pUsuario.DNI}'";
            daoUser.ExecuteNonQuery(query);
        }
        public void Modificar(Usuarios pUsuario)
        {
            string query = $"update usuario set apellido = '{pUsuario.Apellido}',nombre = '{pUsuario.Nombre}',login = '{pUsuario.Login}'," +
                $"contraseña = '{pUsuario.Contraseña}',rol = '{pUsuario.Rol}',email = '{pUsuario.Email}',bloqueo = '{(pUsuario.Bloqueo ? 1 : 0)}'," +
                $"activo = '{(pUsuario.Activo ? 1 : 0)}' where DNI = '{pUsuario.DNI}'";
            daoUser.ExecuteNonQuery(query);
        }
        public List<Usuarios> RetornarListaUsuarios()
        {
            LlenarDataSet();
            List<Usuarios> list = new List<Usuarios>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Usuarios(dr));
            }
            return list;
        }
        public void CambiarIdioma(Usuarios pUsuario)
        {
            string query = $"update usuario set idioma = '{pUsuario.Idioma}' where DNI = '{pUsuario.DNI}'";
            daoUser.ExecuteNonQuery(query);
        }
    }
}
