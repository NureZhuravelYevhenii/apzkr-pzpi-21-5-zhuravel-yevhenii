using AutoMapper;
using BusinessLogicLayer.Entities.Sensors;
using BusinessLogicLayer.Services.BaseServices;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Services
{
    public class SensorService : BaseCrudService<Sensor, SensorIdDto, SensorDto, SensorCreationDto, SensorUpdateDto, KeyAttribute>
    {
        public SensorService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizer<Resource> stringLocalizer) : base(mapper, unitOfWork, stringLocalizer)
        {
        }
    }
}
