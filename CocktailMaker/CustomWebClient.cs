using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CocktailMaker
{
    public class CocktailMakerWebClient : WebClient
    {
        public CocktailMakerWebClient() { }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            request.UserAgent = "CocktailMaker";
            return request;
        }
    }
}
