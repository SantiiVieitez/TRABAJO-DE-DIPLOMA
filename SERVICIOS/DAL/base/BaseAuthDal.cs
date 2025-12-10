using dao;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public abstract class BaseAuthDAL<T> where T : class
    {
        protected AuthDB dao;
        protected DataSet ds;
        protected readonly string _tableName;
        protected readonly string _primaryKeyColumn;

        public BaseAuthDAL(string tableName, string primaryKeyColumn)
        {
            dao = new AuthDB(); 
            ds = new DataSet();
            _tableName = tableName;
            _primaryKeyColumn = primaryKeyColumn;
        }

        protected abstract T MapearEntidad(DataRow row);

        public virtual List<T> ObtenerTodos()
        {
            string query = $"select * from {_tableName}";
            ds = dao.ExecuteDataSet(query);

            List<T> lista = new List<T>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lista.Add(MapearEntidad(dr));
            }
            return lista;
        }

        public virtual T ObtenerPorId(object id)
        {
            string query = $"select * from {_tableName} where {_primaryKeyColumn} = @Id";

            var parameters = new Dictionary<string, object>
            {
                { "@Id", id }
            };

            ds = dao.ExecuteDataSet(query, parameters);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return MapearEntidad(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public abstract void Agregar(T entidad);
        public abstract void Modificar(T entidad);
        public abstract void Borrar(T entidad);
    }
}
