using Newtonsoft.Json;
using SmartMirror.Model;
using System.Net.Http;
using System.Threading.Tasks;


namespace SmartMirror.ViewModel
{
    public class WeatherHelper
    {
        private static string apiKey = "40421cbe548a05b158f1a3c1c8bb8120";
        private static string weatherApiEndpoint = $"https://api.openweathermap.org/data/2.5/weather?q=Austin,us&units=imperial&appid={apiKey}";

        public async Task<WeatherData> GetWeatherAsync()
        {
            using (var client = new HttpClient())
            {
                var weatherDataResponseJson = await client.GetStringAsync(weatherApiEndpoint);
                var weatherDataResponse = JsonConvert.DeserializeObject<WeatherData>(weatherDataResponseJson);
                return weatherDataResponse;
            }
        }
    }
}
