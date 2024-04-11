using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Net.Http;
using Newtonsoft.Json;


namespace Pogoda.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _cityName;
        private string _weatherInfo;

        public string Cityname
        {
            get => _cityName;
            set
            {
                _cityName = value;
            }
        }
        public string WeatherInfo
        {
            get => _weatherInfo;
            set
            {
                _weatherInfo = value;
            }
        }


        public void GetWeather()
        {
            if (string.IsNullOrEmpty(Cityname))
            {
                WeatherInfo = "Prosze wpisać nazwe miasta";
                return;
            }

            try
            {
                string apiKey = "1d4ba7b25efd6ad9bf23b0c3aaf53c0b";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Cityname}&appid={apiKey}";

                using (HttpClient client = new HttpClient())
                {

                    var response = client.GetAsync(apiUrl).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Deserialize JSON response
                    dynamic weatherData = JsonConvert.DeserializeObject(responseBody);


                    WeatherInfo = $"Temperatura: {weatherData.Main.Temp}";
                }
            }
            catch (HttpRequestException)
            {
                WeatherInfo = "Failed to retrieve weather data. Please check your internet connection.";
            }
            catch (Exception ex)
            {
                WeatherInfo = $"An error occurred: {ex.Message}";
            }
        }
    }
}
