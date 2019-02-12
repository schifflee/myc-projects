using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public interface ISongService
    {
        Song GetSong(int id);
        List<Song> GetSongs(SongsListType type);
        List<Song> GetFilteredSongs(List<Song> songlist, string songtype, string pinyin, int wordcount, string singername);
        List<Song> GetSongsByType(int typeid);
        List<Song> GetSongsByWordCount(int wordcount);
        int GetMaxSongWordCount();
        List<Song> GetSongsByPinYin(string pinyin);        
        List<Song> GetSongsBySinger(string singername);
        List<Song> GetSongsByKeyWord(List<Song> list,string keyword);
        List<string> GetSingerNames();
        Dictionary<int, string> GetSongTypes();
        List<string> GetAllSortTypes();
        List<string> GetSortListByHeat();
        DateTime GetDBUpdateDate();
        void AddSong(Song song);
    }    
}
