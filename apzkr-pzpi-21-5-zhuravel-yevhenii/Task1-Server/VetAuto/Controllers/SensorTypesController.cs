using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.SensorTypes;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/sensor-types")]
    [ApiController]
    public class SensorTypesController : BaseController<SensorType, SensorTypeDto, SensorTypeIdDto, SensorTypeCreationDto, SensorTypeUpdateDto, BaseFilter>
    {
        public SensorTypesController(ICrudService<SensorType, SensorTypeIdDto, SensorTypeDto, SensorTypeCreationDto, SensorTypeUpdateDto> crudService) : base(crudService)
        {
        }
    }
}
