using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Sensors;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface ISensorService : ICrudService<Sensor, SensorIdDto, SensorDto, SensorCreationDto, SensorUpdateDto>
    {
    }
}
