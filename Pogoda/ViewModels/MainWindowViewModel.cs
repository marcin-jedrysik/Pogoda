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

        public MainWindowViewModel()
        {
            Cityname = "Cityname";
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task GetWeatherAsync()
        {
            if (string.IsNullOrEmpty(Cityname))
            {
                WeatherTemp = "Prosze wpisać nazwe miasta:";
                return;
            }

            try
            {
                string apiKey = "1d4ba7b25efd6ad9bf23b0c3aaf53c0b";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Cityname}&appid={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    dynamic weatherData = JsonConvert.DeserializeObject(responseBody);
                    double temperatureInKelvin = weatherData.main.temp;
                    double temperatureInCelsius = temperatureInKelvin - 273.15;
                    WeatherTemp = $"Temperatura: {temperatureInCelsius:F2}°C";
                }
            }

            catch (HttpRequestException)
            {
                WeatherTemp = "Failed to retrieve weather data. Please check your internet connection.";
            }
            catch (Exception ex)
            {
                WeatherTemp = $"An error occurred: {ex.Message}";
            }
        }
    }

}
