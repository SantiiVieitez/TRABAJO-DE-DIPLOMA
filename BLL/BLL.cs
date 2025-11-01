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
using BE;
using DAL;

namespace BLL
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
            Usuario usuariobd = da.BuscarUsuario(Login);
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
        public void Agregar(Usuario pUsuario)
        {
            da.Agregar(pUsuario);
        }
        public Usuario ValidarDNI(string DNI)
        {
            return da.BuscarDNI(DNI);
        }
        public void Borrar(Usuario pUsuario)
        {
            da.Borrar(pUsuario);
        }
        public void Modificar(Usuario pUsuario)
        {
            da.Modificar(pUsuario);
        }
        public Usuario BuscarUsuario(string Nombre)
        {
            return da.BuscarUsuario(Nombre);
        }
        public List<Usuario> RetornarListaUsuarios()
        {
            return da.RetornarListaUsuarios();
        }
        public void Desbloquear(Usuario pUsuario)
        {
            da.Desbloquear(pUsuario);
        }
    }
    
    public class SessionManager
    {
        private static object _lock = new Object();
        private static SessionManager _session;

        public DateTime FechaInicio { get; set; }
        public Usuario Usuario{ get; set; }
        public static SessionManager GetInstance
        {
            get
            {
                if (_session == null) throw new Exception("Sesión no iniciada");

                return _session;
            }
        }
        public static void Login(Usuario usuario)
        {

            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                    _session.Usuario = usuario;
                    _session.FechaInicio = DateTime.Now;
                }
                else
                {
                    throw new Exception("Sesión ya iniciada");
                }
            }
        }
        public static void Logout()
        {
            lock (_lock)
            {
                if (_session != null)
                {
                    _session = null;
                }
                else
                {
                    throw new Exception("Sesión no iniciada");
                }
            }
        }
        

        private SessionManager()
        {
            
        }
    }
}

