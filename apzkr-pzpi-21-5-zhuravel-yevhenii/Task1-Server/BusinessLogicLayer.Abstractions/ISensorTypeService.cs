using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.SensorTypes;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Abstractions
{
    public interface ISensorTypeService : ICrudService<SensorType, SensorTypeIdDto, SensorTypeDto, SensorTypeCreationDto, SensorTypeUpdateDto>
    {
    }
}
