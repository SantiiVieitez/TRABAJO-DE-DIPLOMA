using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public static class Encriptador
    {
        public static string GetSHA256(string value)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding enconding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(enconding.GetBytes(value));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }

}
