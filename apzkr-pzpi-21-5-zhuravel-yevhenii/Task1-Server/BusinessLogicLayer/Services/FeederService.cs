using AutoMapper;
using BusinessLogicLayer.Abstractions;
using BusinessLogicLayer.Entities.Feeders;
using BusinessLogicLayer.Services.BaseServices;
using Core.Localizations;
using DataAccessLayer.Abstractions;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Services
{
    public class FeederService : BaseCrudService<Feeder, FeederIdDto, FeederDto, FeederCreationDto, FeederUpdateDto, KeyAttribute>, IFeederService
    {
        public FeederService(IMapper mapper, IUnitOfWork unitOfWork, IStringLocalizer<Resource> stringLocalizer) : base(mapper, unitOfWork, stringLocalizer)
        {
        }
    }
}
