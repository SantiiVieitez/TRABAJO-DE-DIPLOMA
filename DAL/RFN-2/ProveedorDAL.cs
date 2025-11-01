using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProveedorDAL
    {
        basededatos dao;
        DataSet ds;
        public ProveedorDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public void RegistrarProveedor(Proveedor p, bool a)
        {
            if(a == true)
            {
                string query = $"insert into Proveedor (CUIT,Nombre,Email,Telefono) values ('{p.CUIT}','{p.Nombre}','{p.Email}',{p.Telefono})";
                dao.ExecuteNonQuery(query);
            }
            else
            {
                string query = $"insert into Proveedor values ('{p.CUIT}','{p.Nombre}','{p.Email}',{p.Telefono},{p.CBU},'{p.Banco}')";
                dao.ExecuteNonQuery(query);
            }
            
        }
        public void BorrarProveedor(Proveedor p)
        {
            string query = $"delete from proveedor where CUIT = '{p.CUIT}'";
            dao.ExecuteNonQuery(query);
        }
        public void ModificarProveedor(Proveedor p, bool a)
        {
            if(a == true)
            {
                string query = $"update proveedor set Nombre = '{p.Nombre}',Email = '{p.Email}',Telefono = {p.Telefono},CBU = {p.CBU},Banco = '{p.Banco}' where CUIT = '{p.CUIT}'";
                dao.ExecuteNonQuery(query);
            }
            else
            {
                string query = $"update proveedor set Nombre = '{p.Nombre}',Email = '{p.Email}',Telefono = {p.Telefono} where CUIT = '{p.CUIT}'";
                dao.ExecuteNonQuery(query);
            }
            
        }
        public Proveedor BuscarProveedor(string ID)
        {
            string query = $"select * from proveedor where CUIT = '{ID}'";
            ds = dao.ExecuteDataSet(query);
            return new Proveedor(ds.Tables[0].Rows[0]);
        }
        public List<Proveedor> ListarProveedores()
        {
            List<Proveedor> list = new List<Proveedor>();
            ds = dao.ExecuteDataSet("select * from Proveedor");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Proveedor(dr));
            }
            return list;
        }
    }
}
