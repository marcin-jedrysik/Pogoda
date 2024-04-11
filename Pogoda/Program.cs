using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;

namespace Pogoda
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();


    }
    public class WeatherData
    {
        public MainData Main { get; set; }
        public List<WeatherDescription> Weather { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; }
    }

    public class WeatherDescription
    {
        //public string Description { get; set; }
    }

}
