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
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            string pathExe = Path.Combine(basePath, "Traducciones", archivo + ".json");
            string pathDev = Path.GetFullPath(Path.Combine(basePath, @"..\..\Traducciones", archivo + ".json"));

            string pathFile = File.Exists(pathExe) ? pathExe : pathDev;

            if (!File.Exists(pathFile))
                throw new FileNotFoundException($"No se encontró el archivo de traducción", pathFile);

            string json = File.ReadAllText(pathFile);
            var traducciones = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return traducciones != null && traducciones.TryGetValue(clave, out string traduccion)
                ? traduccion
                : null;
        }
    }
}
