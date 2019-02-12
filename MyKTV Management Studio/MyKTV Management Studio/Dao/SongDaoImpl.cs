using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class SongDaoImpl : ISongDao
    {
        SQLServerHelper helper = new SQLServerHelper();
        public Song GetSongById(int id)
        {
            Song song = new Song();
            string sql = "SELECT song_info.song_id,song_info.song_name,song_info.song_ab,song_info.song_word_count,song_type.songtype_name,singer_info.singer_name,song_info.song_url,song_info.song_play_count FROM song_info" +
            " INNER JOIN song_type ON song_info.songtype_id = song_type.songtype_id" +
            " INNER JOIN singer_info ON song_info.singer_id = singer_info.singer_id" +
            " WHERE song_info.song_id = @song_id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@song_id",id)
            };
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql, parameters);
            if (reader.Read())
            {
                song.Id = (int)reader["song_id"];
                song.SongName = reader["song_name"].ToString();
                song.PinYin = reader["song_ab"].ToString();
                song.WordCount = (int)reader["song_word_count"];
                song.Type = reader["songtype_name"].ToString();
                song.Singer = reader["singer_name"].ToString();
                song.Url = reader["song_url"].ToString();
                song.PlayCount = (int)reader["song_play_count"];                
            }
            reader.Close();
            return song;
        }

        public List<Song> GetSongs()
        {
            List<Song> songlist = new List<Song>();
            string sql = "SELECT song_info.song_id,song_info.song_name,song_info.song_ab,song_info.song_word_count,song_type.songtype_name,singer_info.singer_name,song_info.song_url,song_info.song_play_count FROM song_info" +
            " INNER JOIN song_type ON song_info.songtype_id = song_type.songtype_id" +
            " INNER JOIN singer_info ON song_info.singer_id = singer_info.singer_id";
            DataTable dt = SQLServerBaseDao.ExecuteDataTable(sql);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Song song = new Song
                    {
                        Id = (int)dr["song_id"],
                        SongName = dr["song_name"].ToString(),
                        PinYin = dr["song_ab"].ToString(),
                        WordCount = (int)dr["song_word_count"],
                        Type = dr["songtype_name"].ToString(),
                        Singer = dr["singer_name"].ToString(),
                        Url = dr["song_url"].ToString(),
                        PlayCount = (int)dr["song_play_count"]
                    };
                    songlist.Add(song);
                }
                return songlist;
            }
            return null;
        }

        public List<Song> GetSongsByType(int typeid)
        {
            throw new NotImplementedException();
        }

        public List<Song> GetSongsByPinYin(string pinyin)
        {
            throw new NotImplementedException();
        }

        public List<Song> GetSongsByWordCount(int wordcount)
        {
            throw new NotImplementedException();
        }

        public List<Song> GetFilteredSongs(List<Song> songlist, string songtype, string pinyin, int wordcount, string singername)
        {
            var q = from u in songlist
                    select u;
            if (songtype != "全部")
            {
                q = q.Where(p => p.Type == songtype);
            }
            if (!string.IsNullOrEmpty(pinyin))
            {
                q = q.Where(p => p.PinYin == pinyin.ToUpper());
            }
            if (wordcount != 0)
            {
                q = q.Where(p => p.WordCount == wordcount);
            }
            if (!string.IsNullOrEmpty(singername))
            {
                q = q.Where(p => p.Singer.Contains(singername)
                            ||StringUtil.ConvertToFirstNamePinYin(p.Singer)==singername.ToUpper()
                            ||StringUtil.ConvertToLastNamePinYin(p.Singer)==singername.ToUpper());
            }
            return q.ToList();
        }

        public List<Song> GetDescSongsByWordCount(List<Song> list)
        {
            return list.OrderByDescending(x => x.WordCount).ThenBy(x => x.PinYin).ToList();
        }

        public List<Song> GetAscSongsByWordCount(List<Song> list)
        {
            return list.OrderBy(x => x.WordCount).ThenBy(x => x.PinYin).ToList();
        }

        public List<Song> GetDescSongsByPlayCount(List<Song> list)
        {
            return list.OrderByDescending(x => x.PlayCount).ThenBy(x => x.PinYin).ToList();
        }

        public List<Song> GetAscSongsByPlayCount(List<Song> list)
        {
            return list.OrderBy(x => x.PlayCount).ThenBy(x => x.PinYin).ToList();
        }

        public List<Song> GetAscSongs(List<Song> list)
        {
            return list.OrderBy(x => x.PinYin).ToList();
        }

        public List<Song> GetSongsByKeyWord(List<Song> list,string keyword)
        {
            return list.Where(s => s.SongName.Contains(keyword)).ToList(); 
        }

        public int AddSong(Song song,out int id)
        {
            string sql = "INSERT song_info(song_name,song_ab,song_word_count,songtype_id,singer_id,song_url,song_play_count)" +
                         " VALUES(@song_name,@song_ab,@song_word_count,@songtype_id,@singer_id,@song_url,@song_play_count)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@song_name",song.SongName),
                new SqlParameter("@song_ab",song.PinYin),
                new SqlParameter("@song_word_count",song.WordCount),
                new SqlParameter("@songtype_id",song.TypeId),
                new SqlParameter("@singer_id",song.SingerId),
                new SqlParameter("@song_url",song.Url),
                new SqlParameter("@song_play_count",0),               
            };
            int result = SQLServerBaseDao.ExecuteNonQuery(sql, parameters);
            id = Convert.ToInt32(SQLServerBaseDao.ExecuteScalar(helper.Connection, CommandType.Text, sql, parameters));
            return result;
        }
    }
}
