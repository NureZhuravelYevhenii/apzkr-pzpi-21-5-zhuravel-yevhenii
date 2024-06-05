using AutoMapper;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.AnimalCenters;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public class AnimalCenterViewModel : EntityBaseViewModel<AnimalCenter, AnimalCenterId, AnimalCenterCreation, AnimalCenterUpdate>
    {
        public AnimalCenterViewModel(IEntityApiService<AnimalCenter, AnimalCenterId, AnimalCenterCreation, AnimalCenterUpdate> apiService, IMapper mapper) : base(apiService, mapper)
        {
        }
    }
}
