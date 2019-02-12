using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    public interface ISingerService
    {
        Singer GetSingerById(int id);
        List<Singer> GetSingers();
        List<Singer> GetSingersByType(int typeid);
        List<Singer> GetSingersByGender(string gender);
        Dictionary<int, string> GetSingerTypes();

    }
}
