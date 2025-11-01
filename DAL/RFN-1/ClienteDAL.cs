using BE;
using dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ClienteDAL
    {
        basededatos dao;
        DataSet ds;
        public ClienteDAL()
        {
            dao = new basededatos();
            ds = new DataSet();
        }
        public void LlenarDataSet()
        {
            string query = "select * from cliente";
            ds = dao.ExecuteDataSet(query);
        }
        public void Agregar(Cliente pCliente)
        {
            string query = $"insert into cliente values ('{pCliente.DNI}','{pCliente.Nombre}','{pCliente.Apellido}','{pCliente.Domicilio}','{pCliente.Correo}',{pCliente.Telefono})";
            dao.ExecuteNonQuery(query);
        }
        public void Borrar(Cliente pCliente)
        {
            string query = $"delete from cliente where DNI = '{pCliente.DNI}'";
            dao.ExecuteNonQuery(query);
        }
        public void Modificar(Cliente pCliente)
        {
            string query = $"update cliente set Nombre = '{pCliente.Nombre}',Apellido = '{pCliente.Apellido}',Domicilio = '{pCliente.Domicilio}',Correo= '{pCliente.Correo}',Telefono = '{pCliente.Telefono}' where DNI = {pCliente.DNI}";
            dao.ExecuteNonQuery(query);
        }
        public Cliente BuscarCliente(string pDNI)
        {
            string query = $"select * from cliente where DNI = '{pDNI}'";
            ds = dao.ExecuteDataSet(query);
            if(ds.Tables[0].Rows.Count > 0)
            {
                return new Cliente(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
            
        }
        public List<Cliente> RetornarListaClientes()
        {
            LlenarDataSet();
            List<Cliente> list = new List<Cliente>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new Cliente(dr));
            }
            return list;
        }
    }
}
