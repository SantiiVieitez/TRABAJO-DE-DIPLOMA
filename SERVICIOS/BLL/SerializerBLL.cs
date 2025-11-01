using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BLL
{
    public class SerializerBLL
    {
        public void SerializarXML(List<Cliente> clientes, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Cliente>));
                serializer.Serialize(fs, clientes);
            }
        }
        public List<Cliente> DeserializarXML(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Cliente>));
                return (List<Cliente>)serializer.Deserialize(fs);
            }
        }
    }
}
