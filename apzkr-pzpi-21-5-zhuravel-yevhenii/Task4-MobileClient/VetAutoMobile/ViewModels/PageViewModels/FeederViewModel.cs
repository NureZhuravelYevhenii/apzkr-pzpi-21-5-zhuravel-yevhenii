using AutoMapper;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.Feeders;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public class FeederViewModel : EntityBaseViewModel<Feeder, FeederId, FeederCreation, FeederUpdate>
    {
        public FeederViewModel(IEntityApiService<Feeder, FeederId, FeederCreation, FeederUpdate> apiService, IMapper mapper) : base(apiService, mapper)
        {
        }
    }
}
