using AutoMapper;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.SensorTypes;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public class SensorTypeViewModel : EntityBaseViewModel<SensorType, SensorTypeId, SensorTypeCreation, SensorTypeUpdate>
    {
        public SensorTypeViewModel(IEntityApiService<SensorType, SensorTypeId, SensorTypeCreation, SensorTypeUpdate> apiService, IMapper mapper) : base(apiService, mapper)
        {
        }
    }
}
