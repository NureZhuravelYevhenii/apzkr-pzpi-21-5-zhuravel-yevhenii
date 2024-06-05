using VetAutoIoT.Entities.EventArguments;

namespace VetAutoIoT.Abstractions
{
    public interface IFeederService
    {
        Task AnimalApproachingAsync(AnimalApproachingArgs animalApproachingArgs, CancellationToken cancellationToken = default);

        Task AnimalDepartAsync(AnimalDepartArgs animalDepartArgs, CancellationToken cancellationToken = default);

        Task AnimalEatsAsync(AnimalEatsArgs animalEatsArgs, CancellationToken cancellationToken = default);
        
        Task AllAnimalDepartAsync(CancellationToken cancellationToken = default);
    }
}
