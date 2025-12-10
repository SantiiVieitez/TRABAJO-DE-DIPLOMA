using SERVICIOS.Domain;
using System.Collections.Generic;

namespace DAL
{
    public interface IUserDAL
    {
        void Agregar(Usuarios pUsuario);
        void Modificar(Usuarios pUsuario);
        void Borrar(Usuarios pUsuario);     
        Usuarios BuscarUsuario(string Login);
        Usuarios BuscarDNI(string DNI);
        List<Usuarios> RetornarListaUsuarios();
        void SumarIntento(Usuarios pUsuario);
        void Bloquear(Usuarios pUsuario);
        void Desbloquear(Usuarios pUsuario);
        void CambiarIdioma(Usuarios pUsuario);
    }
}