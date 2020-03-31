using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class Mixture
    {
        public int ID { get; set; }
        public int DrinkID { get; set; }
        public int IngredientID { get; set; }
        public int Quantity { get; set; }
        public int UnitID { get; set; }
    }
}
