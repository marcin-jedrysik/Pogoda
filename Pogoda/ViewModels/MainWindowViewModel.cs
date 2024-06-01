using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;


namespace Pogoda.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _cityName;
        private string _weatherTemp;
        private string _weatherWind;
        private string _weatherDirection;
        private string _weatherRain;
        private string _weatherClouds;
        private string _weatherIcon;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Cityname
        {
            get => _cityName;
            set
            {
                _cityName = value;
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
        public string WeatherIcon
        {
            get => _weatherIcon;
            set
            {
                _weatherIcon = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Cityname = "Katowice";
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetWeatherAsync()
        {
            if (string.IsNullOrEmpty(Cityname))
            {
                WeatherTemp = "Podaj";
                return;
            }

            try
            {
                string apiKey = "1d4ba7b25efd6ad9bf23b0c3aaf53c0b";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Cityname}&units=metric&appid={apiKey}";

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
                        WeatherRain = $"Planowany opad {rains} mm";
                    }
                    else
                    {
                        WeatherRain = "Brak opadów";
                    }


                    WeatherTemp = $"Temperatura: {weatherData.main.temp:F2}°C";
                    WeatherWind = $"Prędkość wiatru {weatherData.wind.speed}m/s";
                    WeatherDirection = $"Kierunek wiatru {weatherData.wind.deg}";
                    WeatherClouds = $"Zachmurzenie {cloudiness}%";

                    WeatherIcon = GetWeatherIcon(cloudiness);
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
        private string GetWeatherIcon(int cloudiness)
        {
            if (cloudiness > 50)
            {
                return "Pogoda/Assets/pzach.png";
            }
            else
            {
                return "Pogoda/Assets/slon.png";
            }
        }
    }

}
