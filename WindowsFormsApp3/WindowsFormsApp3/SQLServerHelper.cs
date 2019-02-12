using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp3
{
    public class SQLServerHelper
    {
        private string sqlconnstring="Data Source=.;Initial Catalog=MyKTV;User ID=sa;Pwd=bdqn";
        private SqlConnection connection;
        public SqlConnection Connection
        {             
            get
            {
                if (connection == null)
                {
                   connection = new SqlConnection(sqlconnstring);
                }
                return connection;
            }
        }

        public void OpenConnection()
        {
            if (Connection.State==ConnectionState.Closed)
            {
                Connection.Open();
            }
            else if (Connection.State==ConnectionState.Broken)
            {
                Connection.Close();
                Connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (Connection.State==ConnectionState.Open||Connection.State==ConnectionState.Broken)
            {
                Connection.Close();
            }
        }
    }
}
