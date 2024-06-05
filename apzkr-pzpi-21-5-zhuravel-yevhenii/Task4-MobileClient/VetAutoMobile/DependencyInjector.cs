using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VetAutoMobile.Abstractions.Pages;
using VetAutoMobile.ApiLayer.Entities;
using VetAutoMobile.ApiLayer.Entities.AuthorizationConfigurations;
using VetAutoMobile.Constants;
using VetAutoMobile.Pages;
using VetAutoMobile.ViewModels;
using VetAutoMobile.ViewModels.AppViewModels;
using VetAutoMobile.ViewModels.PageViewModels;

namespace VetAutoMobile
{
    public static class DependencyInjector
    {
        public static void Inject(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(typeof(MauiProgram));

            serviceCollection.AddScoped<AppShell>();

            RegisterPages(serviceCollection);
            RegisterViewModels(serviceCollection);

            serviceCollection.AddSingleton(_ =>
            {
                return new AuthorizationConfiguration
                {
                    AccessToken = Preferences.Get(PreferenceConstants.AccessToken, null)
                };
            });

            serviceCollection.Configure<DeviceConfiguration>(config =>
            {
                config.Port = int.Parse(configuration["DeviceConfiguration:Port"] ?? throw new ArgumentException("Unable to get port."));
            });

            ApiLayer.DependencyInjector.Inject(serviceCollection, configuration);
        }

        private static void RegisterPages(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEntityWithGuards, AnimalCentersPage>();
            serviceCollection.AddScoped<IEntityWithGuards, AnimalsPage>();
            serviceCollection.AddScoped<IEntityWithGuards, AnimalTypesPage>();
            serviceCollection.AddScoped<IEntityWithGuards, FeedersPage>();
            serviceCollection.AddScoped<IEntityWithGuards, SensorsPage>();
            serviceCollection.AddScoped<IEntityWithGuards, SensorTypesPage>();
            serviceCollection.AddScoped<IEntityWithGuards, LoginPage>();
            serviceCollection.AddScoped<IEntityWithGuards, RegistrationPage>();
            serviceCollection.AddScoped<IEntityWithGuards, LogoutPage>();
            serviceCollection.AddScoped<IEntityWithGuards, ConfigureDevicesPage>();
        }

        private static void RegisterViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(EntityBaseViewModel<,,,>));
            serviceCollection.AddScoped<AnimalCenterViewModel>();
            serviceCollection.AddScoped<AnimalTypeViewModel>();
            serviceCollection.AddScoped<AnimalViewModel>();
            serviceCollection.AddScoped<FeederViewModel>();
            serviceCollection.AddScoped<SensorTypeViewModel>();
            serviceCollection.AddScoped<SensorViewModel>();
            serviceCollection.AddScoped<ConfigureDevicesViewModel>();
            serviceCollection.AddScoped<AppShellViewModel>();
            serviceCollection.AddScoped<LoginViewModel>();
            serviceCollection.AddScoped<RegistrationViewModel>();
            serviceCollection.AddScoped<ConfigureDevicesViewModel>();
            serviceCollection.AddScoped<LogoutViewModel>();
        }
    }
}
