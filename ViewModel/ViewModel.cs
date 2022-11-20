using AngleSharp;
using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AngleSharp.Html.Dom;

namespace SmartMirror.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
		public ViewModel()
		{            
            UpdateOneSecondTasks();
            UpdateTenSecondTasks();
            UpdateThirtyMinuteTasks();
        }

        private List<Quote> quotes = new List<Quote>();
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
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

		private int temp;
		public int Temp
		{
			get { return temp; }
            set
            {
                temp = value;
                OnPropertyChanged(nameof(Temp));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private int humidity;
        public int Humidity
        {
            get { return humidity; }
            set 
            { 
                humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        private Quote currentQuote;
        public Quote CurrentQuote
        {
            get { return currentQuote; }
            set 
            {
                currentQuote = value;
                OnPropertyChanged(nameof(CurrentQuote));
            }
        }

        private int daysTogether;
        public int DaysTogether
        {
            get { return daysTogether; }
            set 
            {
                daysTogether = value;
                OnPropertyChanged(nameof(DaysTogether));
            }
        }

        private BitmapImage weatherIcon;
        public BitmapImage WeatherIcon
        {
            get { return weatherIcon; }
            set 
            {
                weatherIcon = value;
                OnPropertyChanged(nameof(WeatherIcon));
            }
        }

        private BitmapImage moonPhaseImage;
        public BitmapImage  MoonPhaseImage
        {
            get { return moonPhaseImage; }
            set 
            {
                moonPhaseImage = value;
                OnPropertyChanged(nameof(MoonPhaseImage));
            }
        }

        private string moonPhaseText;
        public string MoonPhaseText
        {
            get { return moonPhaseText; }
            set 
            {
                moonPhaseText = value;
                OnPropertyChanged(nameof(MoonPhaseText));
            }
        }

        private TransitPositions transitPositions;
        public TransitPositions TransitPositions
        {
            get { return transitPositions; }
            set 
            {
                transitPositions = value;
                OnPropertyChanged(nameof(TransitPositions));
            }
        }




        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void GetCurrentTime()
        {
            CurrentDateTime = DateTime.Now.ToString("h:mm:ss\r\ndddd\r\nMMMM d");
        }

        private async void GetQuotes()
        {            
            var response = await QuoteHelper.GetQuotesAsync();
            if (response != null)
            {
                quotes = response;
                GetRandomQuote();
            }

            
        }

        private void GetRandomQuote()
        {
            if (quotes.Count > 0)
            {
                var random = new Random();
                var randomNumber = random.Next(quotes.Count);
                CurrentQuote = quotes[randomNumber];
            }
        }

        private void GetDaysTogether()
        {
            var daysTogetherTimespan = DateTime.Now - new DateTime(2018, 05, 25);
            DaysTogether = (int)daysTogetherTimespan.TotalDays;
        }        

        private async void GetCurrentMoonPhase()
        {            
            var docImages = await MoonPhaseHelper.GetMoonPhaseAsync();
            if (docImages != null)
            {
                MoonPhaseText = docImages.AlternativeText;
                var moonPhaseImageUrl = docImages.Source;      
                MoonPhaseImage = new BitmapImage();
                MoonPhaseImage.BeginInit();
                MoonPhaseImage.UriSource = new Uri(moonPhaseImageUrl);
                MoonPhaseImage.EndInit();            
            }
        }

        private async void GetTransitPositions()
        {
            TransitPositions = await TransitPositionsHelper.GetTransitPositionsAsync();
        }

        private async void GetCurrentWeather()
        {            
            var weatherData = await WeatherHelper.GetWeatherAsync();
            if (weatherData != null)
            {
                Temp = (int)weatherData.main.temp;
                Description = weatherData.weather.FirstOrDefault().description;
                Humidity = weatherData.main.humidity;
                var weatherIconUri = $"http://openweathermap.org/img/wn/{weatherData.weather.FirstOrDefault().icon}.png";
                WeatherIcon = new BitmapImage();
                WeatherIcon.BeginInit();
                WeatherIcon.UriSource = new Uri(weatherIconUri);
                WeatherIcon.EndInit();
            }
        }

        private void UpdateOneSecondTasks()
        {
            dispatcher.Invoke(async () =>
            {
                while(true)
                {
                    GetCurrentTime();
                    await Task.Delay(1000);
                }
            });            
        }

        private void UpdateTenSecondTasks()
        {
            dispatcher.Invoke(async () =>
            {
                while (true)
                {                  
                    GetRandomQuote();                 
                    await Task.Delay(10000);
                }                
            });
        }

        private void UpdateThirtyMinuteTasks()
        {
            dispatcher.Invoke(async () =>
            {
                while (true)
                {
                    GetQuotes();
                    GetDaysTogether();
                    GetCurrentWeather();
                    GetCurrentMoonPhase();
                    GetTransitPositions();
                    await Task.Delay(1800000);
                }
            });
        }

    }
}
