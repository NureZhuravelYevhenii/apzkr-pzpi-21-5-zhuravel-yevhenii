using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Feeders;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface IFeederService : ICrudService<Feeder, FeederIdDto, FeederDto, FeederCreationDto, FeederUpdateDto>
    {
    }
}
