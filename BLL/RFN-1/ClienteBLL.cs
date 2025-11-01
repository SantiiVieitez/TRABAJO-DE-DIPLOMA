using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SERVICIOS;

namespace BLL
{
    public class ClienteBLL
    {
        ClienteDAL da;
        public ClienteBLL()
        {
            da = new ClienteDAL();
        }
        public void Agregar(Cliente pCliente)
        {
            pCliente.Domicilio = EncriptadorReversible.Encriptar(pCliente.Domicilio);
            da.Agregar(pCliente);
        }
        public void Borrar(Cliente pCliente)
        {
            da.Borrar(pCliente);
        }
        public void Modificar(Cliente pCliente)
        {
            pCliente.Domicilio = EncriptadorReversible.Encriptar(pCliente.Domicilio);
            da.Modificar(pCliente);
        }
        public Cliente BuscarCliente(string pDNI)
        {
            Cliente c = da.BuscarCliente(pDNI);
            if(c == null)
            {
                throw new Exception("Cliente no registrado");
            }
            else
            {
                c.Domicilio = EncriptadorReversible.Desencriptar(c.Domicilio);
                return c;
            }
            

        }
        public List<Cliente>ListaClientes()
        {   
            List<Cliente> list = da.RetornarListaClientes();
            foreach (Cliente c in list)
            {
                c.Domicilio = EncriptadorReversible.Desencriptar(c.Domicilio);
            }
            return list;
        }
    }
}
