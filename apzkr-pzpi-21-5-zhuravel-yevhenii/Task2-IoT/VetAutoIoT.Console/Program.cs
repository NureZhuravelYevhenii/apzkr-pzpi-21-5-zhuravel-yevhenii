using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using VetAutoIoT;
using VetAutoIoT.Abstractions;
using VetAutoIoT.ApiLayer;
using VetAutoIoT.ApiLayer.Abstractions;
using VetAutoIoT.Console.Commands;
using VetAutoIoT.Console.TypeRegisterers;
using VetAutoIoT.Core.Abstractions;
using VetAutoIoT.Core.Configurations;
using VetAutoIoT.Core.HttpClient;
using VetAutoIoT.Persistence;
using VetAutoIoT.Persistence.Abstractions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .Build();

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<IFeederService, FeederService>();
serviceCollection.AddSingleton<IConfigurationService, ConfigurationService>();
serviceCollection.AddSingleton<IApiService, ApiService>();
serviceCollection.AddSingleton<IApiHttpClientFactory, HttpClientFactory>();
serviceCollection.AddSingleton<IFeederIoTConfigurationManager, FeederIotConfigurationManager>();

serviceCollection.AddSingleton(serviceProvider =>
{
    var configurationManager = serviceProvider.GetRequiredService<IFeederIoTConfigurationManager>();

    configurationManager.ConfigureAsync().Wait();

    var apiConfiguration = configurationManager.GetApiConfigurationAsync().Result;

    return apiConfiguration ?? new ApiConfiguration();
});
serviceCollection.AddSingleton(serviceProvider => 
{
    var configurationManager = serviceProvider.GetRequiredService<IFeederIoTConfigurationManager>();

    configurationManager.ConfigureAsync().Wait();

    var feederConfiguration = configurationManager.GetFeederConfigurationAsync().Result;

    if (feederConfiguration is not null)
    {
        return feederConfiguration;
    }

    return new FeederConfiguration
    {
        AmountOfFood = configuration["FeederConfiguration:AmountOfFood"] is not null ? double.Parse(configuration["FeederConfiguration:AmountOfFood"]!) : 0,
        Latitude = configuration["FeederConfiguration:Latitude"] is not null ? double.Parse(configuration["FeederConfiguration:Latitude"]!) : 0,
        Longitude = configuration["FeederConfiguration:Longitude"] is not null ? double.Parse(configuration["FeederConfiguration:Longitude"]!) : 0,
    };
});

serviceCollection.AddSingleton<AllAnimalDepartCommand>();
serviceCollection.AddSingleton<AnimalApproachCommand>();
serviceCollection.AddSingleton<AnimalDepartCommand>();
serviceCollection.AddSingleton<AnimalEatCommand>();
serviceCollection.AddSingleton<ConfigureCommand>();
serviceCollection.AddSingleton<ConfigureFeederCommand>();
serviceCollection.AddSingleton<DefaultCommand>();

serviceCollection.AddDbContext<PersistenceDbContext>(builder =>
{
    builder.UseSqlite(string.Format(configuration.GetConnectionString("SqliteConnectionString")!, Environment.CurrentDirectory));
});

var typeRegistrar = new TypeRegistrar(serviceCollection);
var commandApp = new CommandApp<DefaultCommand>(typeRegistrar);
commandApp.Run(args);