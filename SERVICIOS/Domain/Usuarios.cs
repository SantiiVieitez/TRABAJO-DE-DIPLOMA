using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class Usuarios
    {
        public string DNI { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Login { get; set; }
        [Browsable(false)]
        public string Contraseña { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public bool Bloqueo { get; set; }
        public bool Activo { get; set; }
        [Browsable(false)]
        public int Intentos { get; set; }
        [Browsable(false)]
        public Familia Perfil { get; set; }
        public string Idioma { get; set;}
        public Usuarios(string pDNI, string pApellido, string pNombre, string pLogin, string pContraseña, string pRol, string pEmail, bool pBloqueo, bool pActivo, string idioma)
        {
            DNI = pDNI;
            Apellido = pApellido;
            Nombre = pNombre;
            Login = pLogin;
            Contraseña = pContraseña;
            Rol = pRol;
            Email = pEmail;
            Bloqueo = pBloqueo;
            Activo = pActivo;
            Intentos = 0;
            Idioma = idioma;
        }
        public Usuarios(DataRow dr)
        {
            DNI = dr[0].ToString();
            Apellido = dr[1].ToString();
            Nombre = dr[2].ToString();
            Login = dr[3].ToString();
            Contraseña = dr[4].ToString();
            Rol = dr[5].ToString();
            Email = dr[6].ToString();
            Bloqueo = bool.Parse(dr[7].ToString());
            Activo = bool.Parse(dr[8].ToString());
            Intentos = int.Parse(dr[9].ToString());
            Idioma = dr[10].ToString();
        }
        public Usuarios() { }
    }
}
