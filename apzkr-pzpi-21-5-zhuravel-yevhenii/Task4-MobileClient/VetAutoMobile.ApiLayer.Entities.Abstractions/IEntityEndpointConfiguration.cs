namespace VetAutoMobile.ApiLayer.Entities.Abstractions
{
    public interface IEntityEndpointConfiguration
    {
        string GetByIdEndpoint { get; }
        string GetAllEndpoint { get; }
        string CreateEndpoint { get; }
        string DeleteEndpoint { get; }
        string UpdateEndpoint { get; }
    }
}
