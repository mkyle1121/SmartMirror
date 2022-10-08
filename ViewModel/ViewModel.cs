using SmartMirror.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace SmartMirror.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
		public ViewModel()
		{
			CurrentDateTime = DateTime.Now.ToString("h:mm\r\ndddd, MMMM d");
			GetWeatherAsync();
            GetDogImageAsync();
            GetQuotesAsync();
            GetDaysTogether();
        }

        private List<Quote> quotes = new List<Quote>();
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




        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

		private async void GetWeatherAsync()
		{
            var weatherHelper = new WeatherHelper();
            var weatherData = await weatherHelper.GetWeatherAsync();
			Temp = (int)weatherData.main.temp;
            Description = weatherData.weather.FirstOrDefault().description;
        }

        private async void GetDogImageAsync()
        {
            var dogImageHelper = new DogImageHelper();
            var dogImageLocation = await dogImageHelper.GetDogImageAsync();

            DogImage = new BitmapImage();
            DogImage.BeginInit();
            DogImage.UriSource = new Uri(dogImageLocation);
            DogImage.EndInit();
        }

        private async void GetQuotesAsync()
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
    }
}
