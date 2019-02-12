using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    class AdministratorServiceImpl : IAdministratorService
    {
        IAdministratorDao admindao = new AdministratorDaoImpl();
        public Administrator GetAdministrator(string username, string pwd)
        {            
            return admindao.GetAdministrator(username, pwd);
        }

        public int GetAdminStatus(string username)
        {
            int result = 0;
            string sql = "SELECT admin_status FROM admin_info WHERE admin_name=@admin_name";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@admin_name",username)
            };
            result=Convert.ToInt32(SQLServerBaseDao.ExecuteScalar(sql, parameters));
            return result;
        }

        public bool SetAdminSatusByAdminName(string username, int status)
        {
            return admindao.SetAdminSatusByAdminName(username, status);
        }
    }
}
