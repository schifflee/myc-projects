using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public interface ISongDao
    {
        Song GetSongById(int id);
        List<Song> GetSongs();
        List<Song> GetFilteredSongs(List<Song> list, string songtype, string pinyin, int wordcount, string singername);
        List<Song> GetAscSongs(List<Song> list);
        List<Song> GetDescSongsByWordCount(List<Song> list);
        List<Song> GetAscSongsByWordCount(List<Song> list);
        List<Song> GetDescSongsByPlayCount(List<Song> list);
        List<Song> GetAscSongsByPlayCount(List<Song> list);
        List<Song> GetSongsByType(int typeid);
        List<Song> GetSongsByWordCount(int wordcount);
        List<Song> GetSongsByPinYin(string pinyin);
        List<Song> GetSongsByKeyWord(List<Song> list,string keyword);
        int AddSong(Song song,out int id);
    }

    public enum SongsListType
    {
        SongsList = 0,
        SongsAscList,
        SongsAscByPlayCountList,
        SongsDescByPlayCountList,
        SongsAscByWordCountList,
        SongsDescByWordCountList
    }
}
