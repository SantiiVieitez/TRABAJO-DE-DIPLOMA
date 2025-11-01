using dao;
using DAL;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BitacoraEventosDAL
    {
        basededatos dao;
        DataSet ds;
        public BitacoraEventosDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }

        public void Registrar(BitacoraEvento aux)
        {
            DateTime a = aux.Fecha.Date;
            aux.ID = BuscarUltimoID();
            string query = $"insert into BitacoraEventos values ({aux.ID},'{aux.Usuario}','{aux.Fecha:yyyy-MM-dd HH:mm:ss}','{aux.Modulo}','{aux.Evento}',{aux.Criticidad})";
            dao.ExecuteNonQuery(query);
        }
        public int BuscarUltimoID()
        {
            string query = "SELECT MAX(ID) FROM BitacoraEventos";
            ds = dao.ExecuteDataSet(query);

            int valor = 1; // Valor predeterminado en caso de que la tabla esté vacía
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                valor = Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1;
            }

            return valor;
        }


        public List<BitacoraEvento> RetonarBitacora()
        {
            ds = dao.ExecuteDataSet("select * from BitacoraEventos");
            List<BitacoraEvento> Lista = new List<BitacoraEvento>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                Lista.Add(new BitacoraEvento(dr));
            }
            return Lista;
        }
    }
}
