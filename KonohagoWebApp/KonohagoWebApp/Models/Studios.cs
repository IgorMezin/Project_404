using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KonohagoWebApp.Models
{
    public class Studios
    {
        public readonly int Id;
        public string Name { get; set; }
        public DateTime Foundation_date { get; set; }
        public Studios(int id)
        {
            Id = id;
        }
        public Studios()
        {

        }
    }
}
