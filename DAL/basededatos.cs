using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace dao
{
    public class basededatos
    {
        SqlConnection connection;
        public basededatos()
        {
            connection = new SqlConnection("Data Source=SANTII-PC\\SQLEXPRESS;Initial Catalog=\"Ingenieria de Software\";Integrated Security=True");
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
    }
}

