using VetAutoIoT.Abstractions;
using VetAutoIoT.ApiLayer.Abstractions;
using VetAutoIoT.ApiLayer.Entities;
using VetAutoIoT.Core.Configurations;
using VetAutoIoT.Entities.EventArguments;

namespace VetAutoIoT
{
    public class FeederService : IFeederService
    {
        private readonly IApiService _apiService;
        private readonly FeederConfiguration _feederConfiguration;

        public IDictionary<Guid, double> Animals { get; set; } = new Dictionary<Guid, double>();

        public FeederService(IApiService apiService, FeederConfiguration feederConfiguration)
        {
            _apiService = apiService;
            _feederConfiguration = feederConfiguration;
        }
        
        public Task AnimalApproachingAsync(AnimalApproachingArgs animalApproachingArgs, CancellationToken cancellationToken = default)
        {
            if (!Animals.ContainsKey(animalApproachingArgs.AnimalId))
            {
                Animals.Add(animalApproachingArgs.AnimalId, 0);
            }
            return Task.CompletedTask;
        }

        public async Task AnimalDepartAsync(AnimalDepartArgs animalDepartArgs, CancellationToken cancellationToken = default)
        {
            if (Animals.ContainsKey(animalDepartArgs.Id))
            {
                var feeder = await _apiService.GetFeederDtoAsync();
                if (feeder is null) 
                {
                    await _apiService.CreateFeederAsync(_feederConfiguration.Latitude, _feederConfiguration.Longitude, cancellationToken);

                    feeder = await _apiService.GetFeederDtoAsync();

                    if (feeder is null)
                    {
                        throw new ArgumentException("Seems like you not configure application. Please run \"configure\".");
                    }
                }
                await _apiService.CreateAnimalFeederAsync(new AnimalFeederCreationDto
                {
                    FeederId = feeder.Id,
                    AnimalId = animalDepartArgs.Id,
                    AmountOfFood = Animals[animalDepartArgs.Id],
                });

                Animals.Remove(animalDepartArgs.Id);
            }
        }

        public Task AllAnimalDepartAsync(CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task>();
            foreach (var keyValue in Animals)
            {
                tasks.Add(AnimalDepartAsync(new AnimalDepartArgs
                {
                    Id = keyValue.Key,
                }));
            }

            return Task.WhenAll(tasks);
        }

        public Task AnimalEatsAsync(AnimalEatsArgs animalEatsArgs, CancellationToken cancellationToken = default)
        {
            if (animalEatsArgs.AmountOfFood > _feederConfiguration.AmountOfFood)
            {
                throw new ArgumentException($"Feeder have no so many food");
            }

            if (Animals.ContainsKey(animalEatsArgs.Id))
            {
                Animals[animalEatsArgs.Id] += animalEatsArgs.AmountOfFood;
            }
            else
            {
                Animals.Add(animalEatsArgs.Id, animalEatsArgs.AmountOfFood);
            }

            _feederConfiguration.AmountOfFood -= animalEatsArgs.AmountOfFood;

            return Task.CompletedTask;
        }
    }
}
