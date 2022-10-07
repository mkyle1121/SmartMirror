using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartMirror.ViewModel
{
    public class WeatherHelper
    {
        private static string latitude = "30.266666";
        private static string longitude = "-97.733330";
        private static string apiKey = "40421cbe548a05b158f1a3c1c8bb8120";
        private static string weatherApiEndpoint = $"https://api.openweathermap.org/data/2.5/weather?q=Austin,us&appid={apiKey}";

        public async Task<WeatherData> GetWeatherAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(weatherApiEndpoint);                
                return JsonConvert.DeserializeObject<WeatherData>(response);
            }
        }
    }
}
