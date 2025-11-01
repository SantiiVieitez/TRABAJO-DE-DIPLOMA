using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS.Domain;
using DAL;
using SERVICIOS;

namespace SERVICIOS.BLL
{
    public class UserBLL
    {
        UserDAL da;
        public UserBLL()
        {
            da = new UserDAL();
        }
        public void IniciarSesion(string Login,string Contraseña)
        {
            Usuarios usuariobd = da.BuscarUsuario(Login);
            if (usuariobd != null)
            {
                if (usuariobd.Bloqueo == true)
                {
                    throw new Exception("La cuenta ha sido bloqueada debido a múltiples intentos fallidos. Por favor, contacte al administrador.");
                }
                else if (usuariobd.Contraseña == Contraseña)
                {
                    SessionManager.Login(usuariobd);
                    usuariobd.Intentos = 0;
                    da.SumarIntento(usuariobd);
                }
                else
                {
                    if(usuariobd.Intentos < 3)
                    {
                        usuariobd.Intentos++;
                        da.SumarIntento(usuariobd);
                        throw new Exception("La contraseña es incorrecta.");
                    }
                    else
                    {
                        da.Bloquear(usuariobd);
                        throw new Exception("La contraseña es incorrecta o el usuario fue bloqueado");
                    }
                }
            }
        }
        public void Agregar(Usuarios pUsuario)
        {
            da.Agregar(pUsuario);
        }
        public Usuarios ValidarDNI(string DNI)
        {
            return da.BuscarDNI(DNI);
        }
        public void Borrar(Usuarios pUsuario)
        {
            da.Borrar(pUsuario);
        }
        public void Modificar(Usuarios pUsuario)
        {
            da.Modificar(pUsuario);
        }
        public Usuarios BuscarUsuario(string Nombre)
        {
            return da.BuscarUsuario(Nombre);
        }
        public List<Usuarios> RetornarListaUsuarios()
        {
            return da.RetornarListaUsuarios();
        }
        public void Desbloquear(Usuarios pUsuario)
        {
            da.Desbloquear(pUsuario);
        }
        public void CambiarIdioma(Usuarios pUsuario)
        {
            da.CambiarIdioma(pUsuario);
        }
    }
}

