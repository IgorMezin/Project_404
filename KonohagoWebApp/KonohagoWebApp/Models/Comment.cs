using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonohagoWebApp.Models
{
    public class Comment
    {
        public string User_img { get; set; }
        public string Username { get; set; }

        public readonly int User_id;

        public int Anime_id;
        public string Text { get; set; }
        public Comment(int id)
        {
            User_id = id;
        }
        public Comment()
        {

        }
    }
}
