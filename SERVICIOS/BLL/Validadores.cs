using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public static class ValidarDNI
    {
        private static Regex re = new Regex(@"^\d{8}$");

        public static string Validar(string dni)
        {
            if (!re.IsMatch(dni))
            {
                throw new Exception("El DNI debe tener 8 dígitos.");
            }
            return dni;
        }
    }
    public static class Validar_NomApe
    {
        private static Regex re = new Regex(@"^[a-zA-Z\s]{1,40}$");

        public static string Validar(string nomape)
        {
            if (!re.IsMatch(nomape))
            {
                throw new Exception("No puede contener números ni caracteres especiales y debe tener entre 1 y 40 caracteres.");
            }
            return nomape;
        }
    }

    public static class ValidarEmail
    {
        private static Regex re = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        public static string Validar(string email)
        {
            if (!re.IsMatch(email))
            {
                throw new Exception("El email no es válido.");
            }
            return email;
        }
    }
}
