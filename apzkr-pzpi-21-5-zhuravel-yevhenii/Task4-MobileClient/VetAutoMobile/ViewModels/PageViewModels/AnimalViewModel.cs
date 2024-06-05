using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Styles;
using Mapsui.Tiling;
using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.Animals;
using VetAutoMobile.Entities.AnimalTypes;
using Color = Mapsui.Styles.Color;
using MapControl = Mapsui.UI.Maui.MapControl;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public partial class AnimalViewModel : EntityBaseViewModel<Animal, AnimalId, AnimalCreation, AnimalUpdate>
    {
        private readonly IEntityApiService<AnimalType, AnimalTypeId, AnimalTypeCreation, AnimalTypeUpdate> _animalTypeApiService;
        private readonly IAnimalApiService _animalApiService;

        [ObservableProperty]
        private ObservableCollection<Guid> _animalTypes = new ObservableCollection<Guid>();
        [ObservableProperty]
        private MapControl _mapControl = new MapControl();
        [ObservableProperty]
        private int _currentAnimalTypeIndex;
        [ObservableProperty]
        private bool _doesAnimalHaveFeedingPlaces = false;

        public AnimalViewModel(
            IEntityApiService<Animal, AnimalId, AnimalCreation, AnimalUpdate> apiService,
            IEntityApiService<AnimalType, AnimalTypeId, AnimalTypeCreation, AnimalTypeUpdate> animalTypeApiService,
            IAnimalApiService animalApiService,
            IMapper mapper) : base(apiService, mapper)
        {
            _animalTypeApiService = animalTypeApiService;
            _animalApiService = animalApiService;
        }

        protected override async Task LoadAsync()
        {
            AnimalTypes = new ObservableCollection<Guid>((await _animalTypeApiService.GetAllAsync()).Select(at => at.Id));

            await base.LoadAsync();
        }

        protected override async Task GetCurrentEntityAsync(SelectedItemChangedEventArgs? args)
        {
            await base.GetCurrentEntityAsync(args);

            if (CurrentEntity is null)
            {
                return;
            }

            CurrentAnimalTypeIndex = AnimalTypes.IndexOf(CurrentEntity.TypeId);

            await CreateMap();
        }

        private async Task CreateMap()
        {
            var animalFeedingPlaces = await _animalApiService.GetFeedingPlacesAsync(CurrentEntity!.Id);

            if (animalFeedingPlaces.Count() == 0)
            {
                DoesAnimalHaveFeedingPlaces = false;
                return;
            }
            DoesAnimalHaveFeedingPlaces = true;
            MapControl = new MapControl();

            var feedingPlacesList = animalFeedingPlaces.ToList();

            feedingPlacesList.Sort(new AnimalFeedingPlaceComparer());

            var coordinates = feedingPlacesList.Select(x => new Coordinate(x.Coordinates.First(), x.Coordinates.Skip(1).First()));

            MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MapControl.Map.Layers.Add(new MemoryLayer("path")
            {
                Features = [
                    new GeometryFeature{
                        Geometry = new LineString(coordinates.ToArray()),
                        Styles = [ new VectorStyle { Line = new Pen(Color.Violet, 6) } ]
                    }
                ]
            });

            var firstFeedingPlace = feedingPlacesList.First();
            MapControl.Map.Navigator.CenterOnAndZoomTo(
                new MPoint(firstFeedingPlace.Coordinates.First(), firstFeedingPlace.Coordinates.Skip(1).First()),
                MapControl.Map.Navigator.Resolutions[14]);
            OnPropertyChanged(nameof(MapControl));
        }

        private class AnimalFeedingPlaceComparer : IComparer<AnimalFeedingPlace>
        {
            public int Compare(AnimalFeedingPlace? x, AnimalFeedingPlace? y)
            {
                if (x is null && y is null)
                {
                    return 0;
                }
                if (x is null)
                {
                    return -1;
                }
                if (y is null)
                {
                    return 1;
                }

                return x.FeedingDate.CompareTo(y.FeedingDate);
            }
        }
    }
}
