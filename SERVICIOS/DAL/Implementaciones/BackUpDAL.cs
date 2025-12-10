using dao;
using System;

public class BackUpDAL
{
    private basededatos dao;

    private static BackUpDAL _instance;

    public static BackUpDAL Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackUpDAL();
            }
            return _instance;
        }
    }
    private BackUpDAL()
    {
        dao = new basededatos();
    }

    public void RealizarBackUp(string ruta, string nombreBase)
    {
        string nombreArchivo = $"{nombreBase}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
        string query = $"BACKUP DATABASE [{nombreBase}] TO DISK = '{ruta}\\{nombreArchivo}'";
        dao.ExecuteNonQuery(query);
    }

    public void RestaurarBackUp(string ruta, string nombreBase)
    {
        string query = $"USE master; " +
                       $"ALTER DATABASE [{nombreBase}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                       $"RESTORE DATABASE [{nombreBase}] FROM DISK = '{ruta}' WITH REPLACE; " +
                       $"ALTER DATABASE [{nombreBase}] SET MULTI_USER;";

        dao.ExecuteNonQuery(query);
    }
}