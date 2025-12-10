using BE;
using DAL;
using DAL.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProveedorBLL
    {
        private readonly IProveedorDAL dao;
        public ProveedorBLL()
        {
            dao = ProveedorDAL.GetInstance;
        }
        public void RegistrarProveedor(Proveedor p,bool a)
        {
            dao.RegistrarProveedor(p,a);
        }
        public void BorrarProveedor(Proveedor p)
        {
            dao.Borrar(p);
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
