using Newtonsoft.Json;
using SmartMirror.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;


namespace SmartMirror.ViewModel
{
    public class WeatherHelper
    {
        private static string apiKey = WeatherAPIKey.Key;
        private static string weatherApiEndpoint = $"https://api.openweathermap.org/data/2.5/weather?q=Austin,us&units=imperial&appid={apiKey}";

        public static async Task<WeatherData?> GetWeatherAsync()
        {
            try
            {
                using var client = new HttpClient();            
                var weatherDataResponseJson = await client.GetStringAsync(weatherApiEndpoint);
                var weatherDataResponse = JsonConvert.DeserializeObject<WeatherData>(weatherDataResponseJson);
                return weatherDataResponse;     
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
