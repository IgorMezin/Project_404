using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonohagoWebApp.Models
{
    public class Like
    {
        public readonly int User_id;
        public int Anime_id { get; set; }
        public Like(int id)
        {
            User_id = id;
        }
    }
}
