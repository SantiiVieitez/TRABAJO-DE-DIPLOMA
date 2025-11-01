using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProveedorBLL
    {
        ProveedorDAL dao;
        public ProveedorBLL()
        {
            dao = new ProveedorDAL();
        }
        public void RegistrarProveedor(Proveedor p,bool a)
        {
            dao.RegistrarProveedor(p,a);
        }
        public void BorrarProveedor(Proveedor p)
        {
            dao.BorrarProveedor(p);
        }
        public void ModificarProveedor(Proveedor p, bool a)
        {
            dao.ModificarProveedor(p,a);
        }
        public List<Proveedor> ListarProveedores()
        {
            return dao.ListarProveedores();
        }
    }
}
