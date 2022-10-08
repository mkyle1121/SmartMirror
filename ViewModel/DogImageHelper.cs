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
    public class DogImageHelper
    {        
        private static string dogImageApiEndpoint = "https://dog.ceo/api/breeds/image/random";

        public async Task<string> GetDogImageAsync()
        {
            using (var client = new HttpClient())
            {
                var dogImageResponseJson = await client.GetStringAsync(dogImageApiEndpoint);
                var dogImageResponse = JsonConvert.DeserializeObject<DogImage>(dogImageResponseJson);
                return dogImageResponse.message;
            }
        }
    }
}
