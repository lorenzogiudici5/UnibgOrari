using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg
{
    public class Rootobject
    {
        public Earthquake[] earthquakes { get; set; }
    }

    public class Earthquake
    {
        public string datetime { get; set; }
        public float depth { get; set; }
        public float lng { get; set; }
        public string src { get; set; }
        public string eqid { get; set; }
        public float magnitude { get; set; }
        public float lat { get; set; }
    }

}
