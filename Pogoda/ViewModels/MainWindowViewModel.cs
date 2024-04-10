namespace Pogoda.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Cityname = "Katowice";
        public string apiKey = "1d4ba7b25efd6ad9bf23b0c3aaf53c0b";

        

        string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Cityname}&appid={apiKey}";
    }
}
