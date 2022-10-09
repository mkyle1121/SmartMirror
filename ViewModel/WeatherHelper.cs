using Newtonsoft.Json;
using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartMirror.ViewModel
{
    public class WeatherHelper
    {
        private static string apiKey = "40421cbe548a05b158f1a3c1c8bb8120";
        private static string weatherApiEndpoint = $"https://api.openweathermap.org/data/2.5/weather?q=Austin,us&units=imperial&appid={apiKey}";
        private static string weatherIconApiEndpoint = "http://openweathermap.org/img/wn/";

        public async Task<WeatherData> GetWeatherAsync()
        {
            using (var client = new HttpClient())
            {
                var weatherDataResponseJson = await client.GetStringAsync(weatherApiEndpoint);
                var weatherDataResponse = JsonConvert.DeserializeObject<WeatherData>(weatherDataResponseJson);
                return weatherDataResponse;
            }
        }

        public async Task<Image> GetWeatherIconAsync(string iconCode)
        {
            using (var client = new HttpClient())
            {
                var weatherIconResponse = await client.GetStreamAsync($"{weatherIconApiEndpoint}{iconCode}.png");
                var img = new BitmapImage();
                return null;
            }
        }
    }
}
