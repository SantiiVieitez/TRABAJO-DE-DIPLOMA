using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.Domain
{
    public class BitacoraEvento
    {
        [Browsable(false)]
        public int ID {  get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Evento { get; set; }
        public int Criticidad { get; set; }

        public BitacoraEvento()
        {
            ID = int.Parse(GenerarID());
        }
        public string GenerarID()
        {
            string id = "";
            Random r = new Random(Seed: DateTime.Now.Millisecond);
            for (int x = 0; x < 9; x++)
            {
                id += r.Next(0, 9);
            }
            return id;
        }
        public BitacoraEvento(DataRow dr)
        {
            ID = int.Parse(dr[0].ToString());
            Usuario = dr[1].ToString();
            Fecha = DateTime.Parse(dr[2].ToString());
            Modulo = dr[3].ToString();
            Evento = dr[4].ToString();
            Criticidad = int.Parse(dr[5].ToString());
        }
    }
}
