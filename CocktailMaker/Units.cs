using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class Units
    {
        public int ID { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public double InMililiter { get; set; }
    }
}
