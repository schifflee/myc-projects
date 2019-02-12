using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public interface IAdministratorService
    {
        Administrator GetAdministrator(string username,string pwd);
        int GetAdminStatus(string username);
        bool SetAdminSatusByAdminName(string username, int status);
    }
}
