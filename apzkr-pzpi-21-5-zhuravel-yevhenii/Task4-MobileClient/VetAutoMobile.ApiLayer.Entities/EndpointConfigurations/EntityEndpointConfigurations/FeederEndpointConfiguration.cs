using VetAutoMobile.Entities.Feeders;

namespace VetAutoMobile.ApiLayer.Entities.EndpointConfigurations.EntityEndpointConfigurations
{
    public class FeederEndpointConfiguration : BaseEntityEndpointConfiguration<Feeder>
    {
        public string CreateAnimalFeeder => $"Animals/create-animal-feeder";
        public string GetByCoordinates => $"Feeders/by-coordinates";
    }
}
