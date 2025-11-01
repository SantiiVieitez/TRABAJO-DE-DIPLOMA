using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class IdiomaBLL
    {
        public string Traducir(string archivo, string clave)
        {
            string PathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Traducciones", archivo + ".json");
            string json = File.ReadAllText(PathFile);
            var traducciones = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (traducciones.TryGetValue(clave, out string traduccion))
            {
                return traduccion;
            }
            else
            {
                return null;
            }
        }
    }
}
