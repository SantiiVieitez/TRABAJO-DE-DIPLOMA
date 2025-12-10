using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface iClienteDAL
    {
        void Agregar(Cliente cliente);
        void Modificar(Cliente cliente);
        void Borrar(Cliente cliente);
        List<Cliente> ObtenerTodos();
        Cliente ObtenerPorId(string dni);
    }
}
