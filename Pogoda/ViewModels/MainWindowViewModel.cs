using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Pogoda.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _userEnteredCity;
        private string _weatherCity;
        private string _weatherTemp;
        private string _weatherWind;
        private string _weatherDirection;
        private string _weatherRain;
        private string _weatherClouds;
        private string _weatherDescription;
        private Bitmap _weatherIcon;
        private string _currentTime;
        private string _currentDayAndDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserEnteredCity
        {
            get => _userEnteredCity;
            set
            {
                _userEnteredCity = value;
                OnPropertyChanged();
            }
        }

        public string WeatherCity
        {
            get => _weatherCity;
            set
            {
                _weatherCity = value;
                OnPropertyChanged();
            }
        }
        public string WeatherTemp
        {
            get => _weatherTemp;
            set
            {
                _weatherTemp = value;
                OnPropertyChanged();
            }
        }
        public string WeatherWind
        {
            get => _weatherWind;
            set
            {
                _weatherWind = value;
                OnPropertyChanged();
            }
        }
        public string WeatherDirection
        {
            get => _weatherDirection;
            set
            {
                _weatherDirection = value;
                OnPropertyChanged();
            }
        }
        public string WeatherRain
        {
            get => _weatherRain;
            set
            {
                _weatherRain = value;
                OnPropertyChanged();
            }
        }
        public string WeatherClouds
        {
            get => _weatherClouds;
            set
            {
                _weatherClouds = value;
                OnPropertyChanged();
            }
        }
        public string WeatherDescription
        {
            get => _weatherDescription;
            set
            {
                _weatherDescription = value;
                OnPropertyChanged();
            }
        }
        public Bitmap WeatherIcon
        {
            get => _weatherIcon;
            set
            {
                _weatherIcon = value;
                OnPropertyChanged();
            }
        }
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }
        public string CurrentDayAndDate
        {
            get => _currentDayAndDate;
            set
            {
                _currentDayAndDate = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            UserEnteredCity = "Katowice";
            GetWeatherAsync();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetWeatherAsync()
        {
            try
            {
                string apiKey = "1d4ba7b25efd6ad9bf23b0c3aaf53c0b";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={UserEnteredCity}&units=metric&appid={apiKey}&lang=pl";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic weatherData = JsonConvert.DeserializeObject(responseBody);
                    
                    int cloudiness = weatherData.clouds.all;

                    if (weatherData.rain != null && weatherData.rain["1h"] != null)
                    {
                        double rains = weatherData.rain["1h"];
                        WeatherRain = $"{rains}mm";
                    }
                    else
                    {
                        WeatherRain = "Brak";
                    }

                    WeatherDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase($"{weatherData.weather[0].description}");

                    WeatherTemp = $"{weatherData.main.temp:F2}°C";
                    WeatherWind = $"{weatherData.wind.speed}m/s";
                    WeatherDirection = $"{weatherData.wind.deg}";
                    WeatherClouds = $"{cloudiness}%";
                    WeatherIcon = GetWeatherIcon((string)weatherData.weather[0].icon);

                    long timezoneOffset = (long)weatherData["timezone"];

                    DateTimeOffset currentTimeOffset = DateTimeOffset.UtcNow
                        .ToOffset(TimeSpan.FromSeconds(timezoneOffset));
                    CurrentTime = currentTimeOffset.ToString("HH:mm - dddd, dd.MM.yyyy", CultureInfo.GetCultureInfo("pl-PL"));
                }
            }

            catch (HttpRequestException)
            {
                WeatherTemp = "Failed to retrieve weather data. Please check your internet connection.";
            }
            catch (Exception ex)
            {
                WeatherTemp = $"An error occurred: {ex.Message}";
                WeatherDirection = WeatherWind = string.Empty;
            }
        }
        private Bitmap GetWeatherIcon(string iconCode)
        {
            string basePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Pogoda/Assets");
            string iconPath = $"{iconCode}.png";

            string fullIconPath = System.IO.Path.Combine(basePath, iconPath);

            if (System.IO.File.Exists(fullIconPath))
            {
                return new Bitmap(fullIconPath);
            }
            else
            {
                return new Bitmap(System.IO.Path.Combine(basePath, "unknown.png"));
            }
        }
    }

}
