using LiteDB;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class Database
    {
        public static void CheckUpdate()
        {
            Log.Information("Check database update...");
            using (var db = new LiteDatabase(Globe.Database))
            {
                // Zutaten 
                var ingredients = db.GetCollection<Ingredient>("Ingredients");
                // Masseinheiten
                var units = db.GetCollection<Units>("Units");
                // Drinks
                var drinks = db.GetCollection<Drink>("Drinks");
                // Mixture
                var mixture = db.GetCollection<Mixture>("Mixture");



                using (var client = new CocktailMakerWebClient())
                {
                    var response = client.DownloadString(@"https://api.bastelbunker.de/CocktailMakerService/GetSyncStatus");
                    var syncState = JsonConvert.DeserializeObject<SyncStatus>(response);


                    if (units.Max(x => x.ID).AsInt32 != syncState.UnitsMaxID)
                    {
                        Log.Information("Units database requiert update...");
                        db.DropCollection(units.Name);
                        response = client.DownloadString(@"https://api.bastelbunker.de/CocktailMakerService/GetAllUnits");
                        var serverUnits = JsonConvert.DeserializeObject<List<Units>>(response);
                        units.Insert(serverUnits);
                        Log.Information("Update finish.");
                    }

                    if (ingredients.Max(x => x.ID).AsInt32 != syncState.IngredientsMaxID)
                    {
                        Log.Information("Ingredients database requiert update...");
                        db.DropCollection(ingredients.Name);
                        response = client.DownloadString(@"https://api.bastelbunker.de/CocktailMakerService/GetAllIngredients");
                        var serverUnits = JsonConvert.DeserializeObject<List<Ingredient>>(response);
                        ingredients.Insert(serverUnits);
                        Log.Information("Update finish.");
                    }

                    if (drinks.Max(x => x.ID).AsInt32 != syncState.DrinksMaxID)
                    {
                        Log.Information("Drinks database requiert update...");
                        db.DropCollection(drinks.Name);
                        response = client.DownloadString(@"https://api.bastelbunker.de/CocktailMakerService/GetAllDrinks");
                        var serverDrinks = JsonConvert.DeserializeObject<List<Drink>>(response);
                        drinks.Insert(serverDrinks);
                        Log.Information("Update finish.");
                    }

                    if (mixture.Max(x => x.ID).AsInt32 != syncState.MixtureMaxID)
                    {
                        Log.Information("mixture database requiert update...");
                        db.DropCollection(mixture.Name);
                        response = client.DownloadString(@"https://api.bastelbunker.de/CocktailMakerService/GetAllMixture");
                        var serverMixtures = JsonConvert.DeserializeObject<List<Mixture>>(response);
                        mixture.Insert(serverMixtures);
                        Log.Information("Update finish.");
                    }
                }
            }
        }
    }
}
