using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public static class Globe
    {
        public static Config Config { get; set; }
        public static string Database
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    // Im debug auf den Desktop ablegen!
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "database.db");
                }
                else
                {
                    return @"/app/data/database.db";
                }
            }
        }
    }

    public class Config
    {
        public bool LOG_TO_FILE { get; set; }
        public bool LOG_TO_SEQ { get; set; }
        public string SEQ_SERVER_ADDRESS { get; set; }
        public string SEQ_API_KEY { get; set; }
    }
}
