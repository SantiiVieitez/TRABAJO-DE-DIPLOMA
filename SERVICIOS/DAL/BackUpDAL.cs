using dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BackUpDAL
    {
        AuthDB dao;
        public BackUpDAL()
        {
            dao = new AuthDB();
        }
        public void RealizarBackUp(string ruta)
        {
            string query = "BACKUP DATABASE [Ingenieria de Software] TO DISK = '" + ruta + "\\PROYECTO_INGENIERIA_DE_SOFTWARE.bak'";
            dao.ExecuteNonQuery(query);
        }
        public void RestaurarBackUp(string ruta)
        {
            string query = "USE master; " +
                           "ALTER DATABASE [Ingenieria de Software] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                           "RESTORE DATABASE [Ingenieria de Software] FROM DISK = '" + ruta + "' WITH REPLACE; " +
                           "ALTER DATABASE [Ingenieria de Software] SET MULTI_USER;";

            dao.ExecuteNonQuery(query);
        }
    }
}
