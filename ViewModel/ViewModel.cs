using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        private string messageFromMichaelUri = "https://mikesmartmirror.blob.core.windows.net/messagefrommichael/MessageFromMichael.txt?sp=r&st=2022-10-09T22:16:08Z&se=2032-10-10T06:16:08Z&spr=https&sv=2021-06-08&sr=b&sig=rtOsfhHl60G8uac5hRtx4VAsJPVywD%2F9NIXZvWahhZg%3D";
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

        private BitmapImage dogImage;
        public BitmapImage DogImage
        {
            get { return dogImage; }
            set 
            {
                dogImage = value;
                OnPropertyChanged(nameof(DogImage));
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

        private BitmapImage apodImage;
        public BitmapImage ApodImage
        {
            get { return apodImage; }
            set
            {
                apodImage = value;
                OnPropertyChanged(nameof(ApodImage));
            }
        }

        private string messageFromMichael;
        public string MessageFromMichael
        {
            get { return messageFromMichael; }
            set
            {
                messageFromMichael = value;
                OnPropertyChanged(nameof(MessageFromMichael));
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



        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetCurrentTime()
        {
            CurrentDateTime = DateTime.Now.ToString("h:mm:ss\r\ndddd, MMMM d");
        }

  //      private async void GetWeather()
		//{
  //          var weatherHelper = new WeatherHelper();
  //          var weatherData = await weatherHelper.GetWeatherAsync();
		//	Temp = (int)weatherData.main.temp;
  //          Description = weatherData.weather.FirstOrDefault().description;

  //          var weatherIconUri = $"http://openweathermap.org/img/wn/{weatherData.weather.FirstOrDefault().icon}.png";

  //          WeatherIcon = new BitmapImage();
  //          WeatherIcon.BeginInit();
  //          WeatherIcon.UriSource = new Uri(weatherIconUri);
  //          WeatherIcon.EndInit();
  //      }

        //private async void GetDogImage()
        //{
        //    var dogImageHelper = new DogImageHelper();
        //    var dogImageLocation = await dogImageHelper.GetDogImageAsync();

        //    DogImage = new BitmapImage();
        //    DogImage.BeginInit();
        //    DogImage.UriSource = new Uri(dogImageLocation);
        //    DogImage.EndInit();
        //    await Task.Delay(10000);         
        //}

        private async void GetQuotes()
        {
            var quoteHelper = new QuoteHelper();
            quotes = await quoteHelper.GetQuotesAsync();
            GetRandomQuote();
        }

        private void GetRandomQuote()
        {
            var random = new Random();
            var randomNumber = random.Next(quotes.Count);
            CurrentQuote = quotes[randomNumber];
        }

        private void GetDaysTogether()
        {
            var daysTogetherTimespan = DateTime.Now - new DateTime(2018, 05, 25);
            DaysTogether = (int)daysTogetherTimespan.TotalDays;
        }

        //private async void GetApodImage()
        //{
        //    var apodHelper = new ApodHelper();
        //    var apod = await apodHelper.GetApodImageAsync();

        //    ApodImage = new BitmapImage();
        //    ApodImage.BeginInit();
        //    ApodImage.UriSource = new Uri(apod.HDurl);
        //    ApodImage.EndInit();          
        //}

        private async void GetMessageFromMichael()
        {
            using (var client = new HttpClient())
            {
                MessageFromMichael = await client.GetStringAsync(messageFromMichaelUri);
            }
        }

        private void UpdateOneSecondTasks()
        {
            Task.Run(async () =>
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
            var dogImageHelper = new DogImageHelper();

            dispatcher.Invoke(async () =>
            {
                while (true)
                {
                    var dogImageLocation = await dogImageHelper.GetDogImageAsync();

                    DogImage = new BitmapImage();
                    DogImage.BeginInit();
                    DogImage.UriSource = new Uri(dogImageLocation);
                    DogImage.EndInit();
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
                    var weatherHelper = new WeatherHelper();
                    var weatherData = await weatherHelper.GetWeatherAsync();
                    Temp = (int)weatherData.main.temp;
                    Description = weatherData.weather.FirstOrDefault().description;

                    var weatherIconUri = $"http://openweathermap.org/img/wn/{weatherData.weather.FirstOrDefault().icon}.png";

                    WeatherIcon = new BitmapImage();
                    WeatherIcon.BeginInit();
                    WeatherIcon.UriSource = new Uri(weatherIconUri);
                    WeatherIcon.EndInit();

                    GetQuotes();
                    GetDaysTogether();
                    GetMessageFromMichael();
                    await Task.Delay(1800000);
                }
            });
        }
    }
}
