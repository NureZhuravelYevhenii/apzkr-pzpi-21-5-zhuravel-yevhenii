using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;
using CommunityToolkit.Maui;

namespace VetAutoMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var configuration = new ConfigurationBuilder()
                .AddJsonStream(assembly.GetManifestResourceStream("VetAutoMobile.appsettings.json")
                    ?? throw new ArgumentException("Unable to find resource appsettings.json"))
                .Build();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp(true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Configuration.AddConfiguration(configuration);

            DependencyInjector.Inject(builder.Services, builder.Configuration);

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
