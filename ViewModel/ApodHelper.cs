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
    public class ApodHelper
    {
        private static string apiKey = "wkyRrSkkIa00TeJCEIJnocKyrXYDkJPioO8D0ITE";
        private static string apodApiEndpoint = $"https://api.nasa.gov/planetary/apod?api_key={apiKey}";       

        public async Task<Apod> GetApodImageAsync()
        {
            var apod = new Apod();

            using (var client = new HttpClient())
            {
                var apodResponseJson = await client.GetStringAsync(apodApiEndpoint);
                apod = JsonConvert.DeserializeObject<Apod>(apodResponseJson);
            }

            return apod;
        }
    }
}
