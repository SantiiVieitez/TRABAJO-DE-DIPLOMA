using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace dao
{
    public class basededatos
    {
        private readonly string _cs;

        private readonly SqlConnection _externalConn;
        private readonly SqlTransaction _externalTx;
        private readonly bool _useExternal;

        public basededatos()
        {
            _cs = ConfigurationManager.ConnectionStrings["MiConexionPrincipal"].ConnectionString;
            _useExternal = false;
        }

        public basededatos(SqlConnection conn, SqlTransaction tx)
        {
            _externalConn = conn;
            _externalTx = tx;
            _useExternal = true;
            _cs = conn.ConnectionString;
        }

        private SqlConnection Connection
        {
            get
            {
                if (_useExternal && _externalConn != null)
                    return _externalConn;
                else
                    return new SqlConnection(_cs);
            }
        }

        public DataSet ExecuteDataSet(string query)
        {
            var ds = new DataSet();
            var conn = Connection;
            bool shouldClose = !_useExternal;

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (_useExternal)
                        cmd.Transaction = _externalTx;

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                return ds;
            }
            finally
            {
                if (shouldClose && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public DataSet ExecuteDataSet(string query, Dictionary<string, object> parameters)
        {
            var ds = new DataSet();
            var conn = Connection;
            bool shouldClose = !_useExternal;

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (_useExternal)
                        cmd.Transaction = _externalTx;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                return ds;
            }
            finally
            {
                if (shouldClose && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            var conn = Connection;
            bool shouldClose = !_useExternal;

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new SqlCommand(commandText, conn))
                {
                    if (_useExternal)
                        cmd.Transaction = _externalTx;

                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (shouldClose && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters)
        {
            var conn = Connection;
            bool shouldClose = !_useExternal;

            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                using (var cmd = new SqlCommand(commandText, conn))
                {
                    if (_useExternal)
                        cmd.Transaction = _externalTx;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (shouldClose && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }
    } 
}
