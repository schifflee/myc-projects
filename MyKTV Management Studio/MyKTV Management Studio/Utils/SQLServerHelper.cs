using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MyKTV_Management_Studio
{
    public class SQLServerHelper
    {
        public static readonly string DataSource = Properties.Resources.DataSource;
        public static readonly string InitialCatalog = Properties.Resources.InitialCatalog;
        public static readonly string UserID = Properties.Resources.UserID;
        public static readonly string Pwd = Properties.Resources.Pwd;
        public static readonly string sqlconnstring = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Pwd={3}", DataSource, InitialCatalog, UserID, Pwd);
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
