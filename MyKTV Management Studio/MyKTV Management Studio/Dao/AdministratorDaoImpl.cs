using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MyKTV_Management_Studio
{
    public class AdministratorDaoImpl : IAdministratorDao
    {
        public Administrator GetAdministratorById(int id)
        {
            Administrator administrator = new Administrator();
            string sql = "SELECT * FROM admin_info WHERE admin_id=@admin_id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@admin_id",id)
            };
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql, parameters);
            if (reader.Read())
            {
                administrator.Id = (int)reader["admin_id"];
                administrator.Username = reader["admin_name"].ToString();
                administrator.Password = reader["admin_pwd"].ToString();         
                administrator.Status = (int)reader["admin_status"];
                return administrator;
            }
            reader.Close();
            return administrator;
        }

        public Administrator GetAdministrator(string username, string pwd)
        {
            Administrator administrator = null;
            string sql = "SELECT * FROM admin_info WHERE admin_name=@admin_name AND admin_pwd=@admin_pwd";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@admin_name",username),
                new SqlParameter("@admin_pwd",pwd)
            };
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql, parameters);
            if (reader.Read())
            {
                administrator = new Administrator();
                administrator.Id = (int)reader["admin_id"];
                administrator.Username = reader["admin_name"].ToString();
                administrator.Password = reader["admin_pwd"].ToString();
                administrator.Status = (int)reader["admin_status"];
                
            }
            reader.Close();
            return administrator;
        }
        public List<Administrator> GetAdministrators()
        {
            List<Administrator> administrators = new List<Administrator>();
            string sql = "SELECT * FROM admin_info";
            DataTable dt = SQLServerBaseDao.ExecuteDataTable(sql);
            if (dt!=null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Administrator administrator = new Administrator
                    {
                        Id = (int)dr["admin_id"],
                        Username = dr["admin_name"].ToString(),
                        Password = dr["admin_pwd"].ToString(),
                        Status = (int)dr["admin_status"]
                    };
                    administrators.Add(administrator);
                }
                return administrators;
            }
            return null;
        }

        public bool SetAdminSatusByAdminName(string username,int status)
        {
            string sql = "UPDATE admin_info SET admin_status=@admin_status WHERE admin_name=@admin_name";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@admin_status",status),
                new SqlParameter("@admin_name",username)
            };
            int result = SQLServerBaseDao.ExecuteNonQuery(sql, parameters);
            if (result>0)
            {
                return true;
            }
            
            return false;
        }
    }
}
