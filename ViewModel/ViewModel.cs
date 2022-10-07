using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartMirror.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
		public ViewModel()
		{
			currentDateTime = DateTime.Now.ToString("H:mm\r\ndddd, MMMM d");
			GetWeatherAsync();	
		}

        public event PropertyChangedEventHandler? PropertyChanged;

		private string currentDateTime;

		public string CurrentDateTime
		{
			get { return currentDateTime; }
			set
			{ 
				currentDateTime = value;
				OnPropertyChanged(nameof(CurrentDateTime));
			}
		}

		private WeatherData weatherData;

		public WeatherData WeatherData
		{
			get { return weatherData; }
            set
            {
                weatherData = value;
                OnPropertyChanged(nameof(WeatherData));
            }
        }



		private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

		private async void GetWeatherAsync()
		{
            var weatherHelper = new WeatherHelper();
            weatherData = await weatherHelper.GetWeatherAsync();
			Console.WriteLine();
        }
    }
}
