using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyKTV_Management_Studio
{
    [Serializable]
    public class Administrator
    {
        public int Id{ get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }       
    }
}
