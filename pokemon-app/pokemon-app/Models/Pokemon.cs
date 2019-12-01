using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pokemon_app.Models
{
    public class Pokemon
    {
        public String name { get; set; }
        public String img_url { get; set; }
        public String user { get; set; }
        public Boolean time { get; set; }
        public Int32 habilidade { get; set; }
    }
}
