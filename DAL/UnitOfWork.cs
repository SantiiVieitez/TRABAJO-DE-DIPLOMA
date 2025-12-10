using DAL.Contrato;
using dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly SqlConnection _conn;
        private SqlTransaction _tx;
        private basededatos _dao;

        public IFacturaDAL Facturas { get; private set; }

        public UnitOfWork()
        {
            // CORRECCIÓN: Leemos directamente del App.config igual que en basededatos
            string connectionString = ConfigurationManager.ConnectionStrings["MiConexionPrincipal"].ConnectionString;
            _conn = new SqlConnection(connectionString);
        }

        public void Begin()
        {
            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            _tx = _conn.BeginTransaction();
            _dao = new basededatos(_conn, _tx);

            Facturas = new FacturaDAL(_dao);
        }

        public void Commit()
        {
            _tx?.Commit();
            Cleanup();
        }

        public void Rollback()
        {
            _tx?.Rollback();
            Cleanup();
        }

        private void Cleanup()
        {
            _tx?.Dispose();
            _tx = null;
            _dao = null;

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
        }

        public void Dispose()
        {
            Cleanup();
            _conn.Dispose();
        }
    }
}
