using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MyKTV_Management_Studio
{
    public class SQLServerBaseDao
    {
        private static SQLServerHelper sqlhelper = new SQLServerHelper();
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand command=sqlhelper.Connection.CreateCommand())
            {
                sqlhelper.OpenConnection();
                command.CommandText = sql;
                if (parameters!=null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand command = sqlhelper.Connection.CreateCommand())
            {
                sqlhelper.OpenConnection();
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteScalar();
            }
        }

        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            SqlCommand command = sqlhelper.Connection.CreateCommand();
            sqlhelper.OpenConnection();
            command.CommandText = sql;
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static int ExecuteNonQuery(string sql)
        {
            using (SqlCommand command = sqlhelper.Connection.CreateCommand())
            {
                sqlhelper.OpenConnection();
                command.CommandText = sql;               
                return command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string transcat_sql)
        {
            using (SqlCommand command = sqlhelper.Connection.CreateCommand())
            {
                sqlhelper.OpenConnection();
                command.CommandText = transcat_sql;               
                return command.ExecuteScalar();
            }
        }

        public static SqlDataReader ExecuteReader(string sql)
        {
            SqlCommand command = sqlhelper.Connection.CreateCommand();
            sqlhelper.OpenConnection();
            command.CommandText = sql;            
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql,sqlhelper.Connection))
            {
                DataTable datatable = new DataTable();
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                adapter.Fill(datatable);
                return datatable;
            }
        }

        public static DataSet ExecuteDataSet(string sql, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlhelper.Connection))
            {
                DataSet dataset = new DataSet();
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                adapter.Fill(dataset);
                return dataset;
            }
        }

        public static DataTable ExecuteDataTable(string sql)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlhelper.Connection))
            {
                DataTable datatable = new DataTable();
                adapter.Fill(datatable);
                return datatable;
            }
        }

        public static DataSet ExecuteDataSet(string sql)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlhelper.Connection))
            {
                DataSet dataset = new DataSet();                
                adapter.Fill(dataset);
                return dataset;
            }
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandtype, string sql, SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, null, CommandType.Text, sql,parameters);
            object obj = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj;
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParams)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = commandText;
            }            
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = CommandType.Text;
            if (commandParams != null)
            {
                foreach (SqlParameter param in commandParams)
                {
                    command.Parameters.Add(param);
                }                    
            }
        }
    }
}
