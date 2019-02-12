using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainClientSystem
{
    public class Movie
    {
        public string MovieName { get; set; }
        public string Poster { get; set; }
        public string Director { get; set; }
        public string Actor { get; set; }
        public MovieType MovieType { get; set; }
        public string Summary { get; set; }
        public double Price { get; set; }

        public Movie()
        {

        }

        public Movie(string movieName, string poster, string director, string actor, MovieType movieType, string summary, double price)
        {
            this.MovieName = movieName;
            this.Poster = poster;
            this.Director = director;
            this.Actor = actor;
            this.MovieType = movieType;
            this.Summary = summary;
            this.Price = price;
         }
    }

    public enum MovieType
    {
        [Description(DescriptionName ="喜剧")]
        Comedy,//喜剧
        [Description(DescriptionName = "战争")]
        War,//战争
        [Description(DescriptionName = "爱情")]
        Romance,//爱情
        [Description(DescriptionName = "动作")]
        Action,//动作
        [Description(DescriptionName = "卡通动漫")]
        Cartoon,//卡通动漫
        [Description(DescriptionName = "恐怖")]
        Thriller,//恐怖
        [Description(DescriptionName = "冒险")]
        Adventure//冒险
    }
}
