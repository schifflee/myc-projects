using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MyKTV_Management_Studio
{
    public class SingerDaoImpl : ISingerDao
    {
        public Singer GetSingerById(int id)
        {
            Singer singer = new Singer();
            string sql = "SELECT singer_info.singer_id,singer_info.singer_name,singer_type.singertype_name,singer_info.singer_gender,singer_info.singer_photo_url,singer_info.singer_description FROM singer_info" +
            " INNER JOIN singer_type ON singer_info.singertype_id=singer_type.singertype_id"+
            " WHERE singer_id=@singer_id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@singer_id",id)
            };
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql, parameters);
            if (reader.Read())
            {
                singer.Id = (int)reader["singer_id"];
                singer.SingerName = reader["singer_name"].ToString();
                singer.Type = reader["singertype_name"].ToString();
                singer.Gender = reader["singer_gender"].ToString();
                singer.PhotoUrl = reader["singer_photo_url"].ToString();
                singer.Description = reader["singer_description"].ToString();
            }
            reader.Close();
            return singer;
        }

        public List<Singer> GetSingers()
        {
            List<Singer> singerlist = new List<Singer>();
            string sql = "SELECT singer_info.singer_id,singer_info.singer_name,singer_type.singertype_name,singer_info.singer_gender,singer_info.singer_photo_url,singer_info.singer_description FROM singer_info" +
            " INNER JOIN singer_type ON singer_info.singertype_id=singer_type.singertype_id";
            DataTable dt = SQLServerBaseDao.ExecuteDataTable(sql);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Singer singer = new Singer
                    {
                        Id = (int)dr["singer_id"],
                        SingerName = dr["singer_name"].ToString(),
                        Type=dr["singertype_name"].ToString(),
                        Gender=dr["singer_gender"].ToString(),
                        PhotoUrl=dr["singer_photo_url"].ToString(),
                        Description=dr["singer_Description"].ToString()                      
                    };
                    singerlist.Add(singer);
                }
                return singerlist;
            }
            return null;
        }        

        public List<Singer> GetSingersByType(int typeid)
        {
            List<Singer> singerlist = new List<Singer>();
            string sql = "SELECT singer_info.singer_id,singer_info.singer_name,singer_type.singertype_name,singer_info.singer_gender,singer_info.singer_photo_url,singer_info.singer_description FROM singer_info" +
            " INNER JOIN singer_type ON singer_info.singertype_id=singer_type.singertype_id" +
            " WHERE singertype_id=@singertype_id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@singer_id",typeid)
            };
            DataTable dt = SQLServerBaseDao.ExecuteDataTable(sql,parameters);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Singer singer = new Singer
                    {
                        Id = (int)dr["singer_id"],
                        SingerName = dr["singer_name"].ToString(),
                        Type = dr["singertype_name"].ToString(),
                        Gender = dr["singer_gender"].ToString(),
                        PhotoUrl = dr["singer_photo_url"].ToString(),
                        Description = dr["singer_Description"].ToString()
                    };
                    singerlist.Add(singer);
                }
                return singerlist;
            }
            return null;
        }

        public List<Singer> GetSingersByGender(string gender)
        {
            List<Singer> singerlist = new List<Singer>();
            string sql = "SELECT singer_info.singer_id,singer_info.singer_name,singer_type.singertype_name,singer_info.singer_gender,singer_info.singer_photo_url,singer_info.singer_description FROM singer_info" +
            " INNER JOIN singer_type ON singer_info.singertype_id=singer_type.singertype_id" +
            " WHERE singer_gender=@singer_gender";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@singer_id",gender)
            };
            DataTable dt = SQLServerBaseDao.ExecuteDataTable(sql, parameters);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Singer singer = new Singer
                    {
                        Id = (int)dr["singer_id"],
                        SingerName = dr["singer_name"].ToString(),
                        Type = dr["singertype_name"].ToString(),
                        Gender = dr["singer_gender"].ToString(),
                        PhotoUrl = dr["singer_photo_url"].ToString(),
                        Description = dr["singer_Description"].ToString()
                    };
                    singerlist.Add(singer);
                }
                return singerlist;
            }
            return null;
        }
    }
}
