using AutoMapper;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.AnimalTypes;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public class AnimalTypeViewModel : EntityBaseViewModel<AnimalType, AnimalTypeId, AnimalTypeCreation, AnimalTypeUpdate>
    {
        public AnimalTypeViewModel(IEntityApiService<AnimalType, AnimalTypeId, AnimalTypeCreation, AnimalTypeUpdate> apiService, IMapper mapper) : base(apiService, mapper)
        {
        }
    }
}
