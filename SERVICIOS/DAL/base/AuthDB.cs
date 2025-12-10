using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace dao
{
    public class AuthDB
    {
        SqlConnection connection;
        public AuthDB()
        {
            string conexion = ConfigurationManager.ConnectionStrings["AuthDB"].ConnectionString;
            connection = new SqlConnection(conexion);
            //connection = new SqlConnection("Data Source=DESKTOP-RFMM2LC\\SQLEXPRESS;Initial Catalog=\"Ingenieria de Software\";Integrated Security=True");
        }
        public DataSet ExecuteDataSet(string query)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
        }
        public DataSet ExecuteDataSet(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, this.connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExecuteNonQuery(string pCommandText)
        {
            try
            {
                SqlCommand mCom = new SqlCommand(pCommandText, connection);
                connection.Open();
                return mCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
        }
        public int ExecuteNonQuery(string pCommandText, Dictionary<string, object> parameters)
        {
            try
            {
                SqlCommand mCom = new SqlCommand(pCommandText, connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        mCom.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }
                connection.Open();
                return mCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed) connection.Close();
            }
        }
    }
}

