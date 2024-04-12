using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pogoda.ViewModels;


namespace Pogoda.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void GetWeather_Button_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).GetWeather();
            
        }

    }
}