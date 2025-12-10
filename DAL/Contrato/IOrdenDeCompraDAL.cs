using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Contrato
{
    public interface IOrdenDeCompraDAL
    {
        void Agregar(OrdenDeCompra entidad);
        void Modificar(OrdenDeCompra entidad);
        void Borrar(OrdenDeCompra entidad);

        OrdenDeCompra ObtenerPorId(object id);
        List<OrdenDeCompra> ObtenerTodos();

        int RetornarUltimoID();
        List<OrdenDeCompra> RetornarOrdenesDeCompra();
        List<OrdenDeCompra> RetornarOrdenDeCompraID(int id);
    }
}
