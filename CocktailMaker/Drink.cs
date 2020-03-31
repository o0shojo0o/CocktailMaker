using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class Drink
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Alcoholic { get; set; }
        public string DrinkThumb { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
