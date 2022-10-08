using Newtonsoft.Json;
using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.ViewModel
{
    public class QuoteHelper
    {
        private static string quoteApiEndpoint = "https://type.fit/api/quotes";

        public async Task<List<Quote>> GetQuotesAsync()
        {
            using (var client = new HttpClient())
            {
                var quoteResponseJson = await client.GetStringAsync(quoteApiEndpoint);
                var quoteResponse = JsonConvert.DeserializeObject<List<Quote>>(quoteResponseJson);
                return quoteResponse;
            }
        }
    }
}
