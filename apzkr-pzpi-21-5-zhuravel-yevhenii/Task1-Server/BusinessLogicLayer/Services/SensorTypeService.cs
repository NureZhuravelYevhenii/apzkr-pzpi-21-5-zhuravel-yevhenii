using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.SensorTypes;
using BusinessLogicLayer.Services.BaseServices;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services
{
    public class SensorTypeService : BaseCrudService<SensorType, SensorTypeIdDto, SensorTypeDto, SensorTypeCreationDto, SensorTypeUpdateDto, KeyAttribute>, ISensorTypeService
    {
        public SensorTypeService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizer<Resource> stringLocalizer) : base(mapper, unitOfWork, stringLocalizer)
        {
        }
    }
}
