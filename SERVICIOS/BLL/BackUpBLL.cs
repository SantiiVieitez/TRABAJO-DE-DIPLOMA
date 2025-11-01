using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BackUpBLL
    {
        BackUpDAL da;
        public BackUpBLL()
        {
            da = new BackUpDAL();
        }
        public void RealizarBackUp(string ruta)
        {
           da.RealizarBackUp(ruta);
        }

        public void RestaurarBackUp(string ruta)
        {
            da.RestaurarBackUp(ruta);
        }
    }
}
