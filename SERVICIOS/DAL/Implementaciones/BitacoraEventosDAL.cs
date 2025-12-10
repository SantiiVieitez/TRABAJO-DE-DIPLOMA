using dao;
using SERVICIOS.Domain;
using System;
using System.Collections.Generic;
using System.Data;

public class BitacoraEventosDAL
{
    private AuthDB dao;
    private DataSet ds;

    private static BitacoraEventosDAL _instance;
    public static BitacoraEventosDAL Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BitacoraEventosDAL();
            }
            return _instance;
        }
    }

    private BitacoraEventosDAL()
    {
        dao = new AuthDB();
        ds = new DataSet();
    }

    public void Registrar(BitacoraEvento aux)
    {
        aux.ID = BuscarUltimoID();

        string query = "insert into BitacoraEventos values (@ID, @Usuario, @Fecha, @Modulo, @Evento, @Criticidad)";

        var parameters = new Dictionary<string, object>
        {
            { "@ID", aux.ID },
            { "@Usuario", aux.Usuario },
            { "@Fecha", aux.Fecha }, 
            { "@Modulo", aux.Modulo },
            { "@Evento", aux.Evento },
            { "@Criticidad", aux.Criticidad }
        };

        dao.ExecuteNonQuery(query, parameters);
    }

    public int BuscarUltimoID()
    {
        string query = "SELECT MAX(ID) FROM BitacoraEventos";
        ds = dao.ExecuteDataSet(query);

        int valor = 1;
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
        {
            valor = Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1;
        }

        return valor;
    }

    public List<BitacoraEvento> RetonarBitacora()
    {
        string query = "select * from BitacoraEventos";
        ds = dao.ExecuteDataSet(query);

        List<BitacoraEvento> Lista = new List<BitacoraEvento>();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Lista.Add(new BitacoraEvento(dr));
        }
        return Lista;
    }
}