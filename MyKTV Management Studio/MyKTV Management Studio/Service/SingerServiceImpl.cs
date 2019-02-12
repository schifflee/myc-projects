using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class SingerServiceImpl : ISingerService
    {
        ISingerDao singerdao = new SingerDaoImpl();
        public Singer GetSingerById(int id)
        {
            return singerdao.GetSingerById(id);
        }

        public List<Singer> GetSingers()
        {
            return singerdao.GetSingers();
        }

        public List<Singer> GetSingersByGender(string gender)
        {
            return singerdao.GetSingersByGender(gender);
        }

        public List<Singer> GetSingersByType(int typeid)
        {
            return singerdao.GetSingersByType(typeid);
        }

        public Dictionary<int, string> GetSingerTypes()
        {
            Dictionary<int, string> singertypes = new Dictionary<int, string>();
            string sql = "SELECT * FROM singer_type";
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql);
            int index;
            string type;
            while (reader.Read())
            {
                index = (int)reader["singertype_id"];
                type = reader["singertype_name"].ToString();
                singertypes.Add(index, type);
            }
            reader.Close();
            return singertypes;
        }
    }
}
