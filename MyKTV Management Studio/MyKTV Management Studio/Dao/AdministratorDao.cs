using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public interface IAdministratorDao
    {
        Administrator GetAdministratorById(int id);
        Administrator GetAdministrator(string username, string pwd);
        List<Administrator> GetAdministrators();
        bool SetAdminSatusByAdminName(string username,int status);
    }
}
