using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyKTV_Management_Studio
{
    public class SongServiceImpl : ISongService
    {
        ISongDao songdao = new SongDaoImpl();
        public Song GetSong(int id)
        {
            return songdao.GetSongById(id);
        }

        public Dictionary<int, string> GetSongTypes()
        {
            Dictionary<int, string> songtypes = new Dictionary<int, string>
            {
                { 0, "全部" }
            };
            string sql = "SELECT * FROM song_type";
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql);
            int index;
            string type;
            while (reader.Read())
            {
                index = (int)reader["songtype_id"];
                type = reader["songtype_name"].ToString();
                songtypes.Add(index, type);
            }
            reader.Close();
            return songtypes;
        }

        public DateTime GetDBUpdateDate()
        {
            string sql = "SELECT TOP 1 song_update_date From song_info ORDER BY song_update_date DESC";
            object date = SQLServerBaseDao.ExecuteScalar(sql);
            return (DateTime)date;
        }

        public List<Song> GetSongsByType(int typeid)
        {
            throw new NotImplementedException();
        }

        public List<Song> GetSongsByWordCount(int wordcount)
        {
            throw new NotImplementedException();
        }

        public int GetMaxSongWordCount()
        {
            string sql = "SELECT TOP 1 song_word_count FROM song_info ORDER BY song_word_count DESC";
            object maxsongwordcount = SQLServerBaseDao.ExecuteScalar(sql);
            return Convert.ToInt32(maxsongwordcount);
        }

        public List<Song> GetSongsByPinYin(string pinyin)
        {
            throw new NotImplementedException();
        }

        public List<Song> GetSongsBySinger(string singername)
        {
            throw new NotImplementedException();
        }

        public List<Song> DescSortedByWordCount(List<Song> list)
        {
            return songdao.GetDescSongsByWordCount(list);
        }

        public List<Song> AscSortedByWordCount(List<Song> list)
        {
            return songdao.GetAscSongsByWordCount(list);
        }

        public List<Song> DescSortedByPlayCount(List<Song> list)
        {
            return songdao.GetDescSongsByPlayCount(list);
        }

        public List<Song> AscSortedByPlayCount(List<Song> list)
        {
            return songdao.GetAscSongsByPlayCount(list);
        }

        public List<Song> AscSorted(List<Song> list)
        {
            return songdao.GetAscSongs(list);
        }

        public List<Song> GetSongs(SongsListType type)
        {
            List<Song> songlist = new List<Song>();
            switch (type)
            {
                case SongsListType.SongsList:
                    songlist = songdao.GetSongs();
                    break;
                case SongsListType.SongsAscList:
                    songlist = AscSorted(songdao.GetSongs());
                    break;
                case SongsListType.SongsAscByPlayCountList:
                    songlist = AscSortedByPlayCount(songdao.GetSongs());
                    break;
                case SongsListType.SongsDescByPlayCountList:
                    songlist = DescSortedByPlayCount(songdao.GetSongs());
                    break;
                case SongsListType.SongsAscByWordCountList:
                    songlist = AscSortedByWordCount(songdao.GetSongs());
                    break;
                case SongsListType.SongsDescByWordCountList:
                    songlist = DescSortedByWordCount(songdao.GetSongs());
                    break;
            }
            return songlist;
        }

        public List<Song> GetFilteredSongs(List<Song> songlist, string songtype, string pinyin, int wordcount, string singername)
        {
            return songdao.GetFilteredSongs(songlist, songtype, pinyin, wordcount, singername);
        }

        public List<string> GetAllSortTypes()
        {
            List<string> sortbylist = new List<string>
            {
                "按热度升序",
                "按热度降序",
                "按歌名字数升序",
                "按歌名字数降序"
            };
            return sortbylist;
        }

        public List<string> GetSortListByHeat()
        {
            List<string> sortbylist = new List<string>
            {
                "按热度升序",
                "按热度降序",
            };
            return sortbylist;
        }

        public List<string> GetSingerNames()
        {
            List<string> singernamelist = new List<string>();
            string sql = "SELECT singer_info.singer_name from singer_info " +
                         " INNER JOIN song_info ON singer_info.singer_id=song_info.singer_id";
            SqlDataReader reader = SQLServerBaseDao.ExecuteReader(sql);
            while (reader.Read())
            {
                singernamelist.Add(reader["singer_name"].ToString());
            }
            reader.Close();
            return singernamelist.Distinct().ToList();
        }

        public List<Song> GetSongsByKeyWord(List<Song> list, string keyword)
        {
            return songdao.GetSongsByKeyWord(list, keyword);
        }

        public void AddSong(Song song)
        {
            int id = 0;
            int result = songdao.AddSong(song,out id);
            if (result > 0)
            {
                song.Id = id;
            }
        }
    }
}
