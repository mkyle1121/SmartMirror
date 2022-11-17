using Newtonsoft.Json;
using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.ViewModel
{
    public class QuoteHelper
    {
        private static string quoteApiEndpoint = "https://type.fit/api/quotes";

        public static async Task<List<Quote>?> GetQuotesAsync()
        {
            try
            {
                using var client = new HttpClient();            
                var quoteResponseJson = await client.GetStringAsync(quoteApiEndpoint);
                var quoteResponse = JsonConvert.DeserializeObject<List<Quote>>(quoteResponseJson);
                return quoteResponse;
            }
            catch (Exception e)
            {
                var logOutputLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "smartmirror.log");
                var file = new StreamWriter(logOutputLocation, true);
                file.WriteLine(string.Concat(DateTime.Now, e.Message));
                file.Close();
                return null;
            }
            
        }
    }
}
