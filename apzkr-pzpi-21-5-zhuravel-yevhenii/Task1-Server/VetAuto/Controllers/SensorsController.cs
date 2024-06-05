using BusinessLogicLayer.Abstractions.BaseInterfaces;
using BusinessLogicLayer.Entities.Sensors;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetAuto.Models;

namespace VetAuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : BaseController<Sensor, SensorDto, SensorIdDto, SensorCreationDto, SensorUpdateDto, BaseFilter>
    {
        public SensorsController(ICrudService<Sensor, SensorIdDto, SensorDto, SensorCreationDto, SensorUpdateDto> crudService) : base(crudService)
        {
        }
    }
}
