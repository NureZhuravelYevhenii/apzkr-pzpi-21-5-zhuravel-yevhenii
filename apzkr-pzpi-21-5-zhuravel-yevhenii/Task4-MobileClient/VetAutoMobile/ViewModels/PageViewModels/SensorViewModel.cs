using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.Entities.Animals;
using VetAutoMobile.Entities.Sensors;
using VetAutoMobile.Entities.SensorTypes;

namespace VetAutoMobile.ViewModels.PageViewModels
{
    public partial class SensorViewModel : EntityBaseViewModel<Sensor, SensorId, SensorCreation, SensorUpdate>
    {
        private readonly IEntityApiService<Animal, AnimalId, AnimalCreation, AnimalUpdate> _animalApiService;
        private readonly IEntityApiService<SensorType, SensorTypeId, SensorTypeCreation, SensorTypeUpdate> _sensorTypeApiService;

        [ObservableProperty]
        private ObservableCollection<Guid> _animalIds = new ObservableCollection<Guid>();
        [ObservableProperty]
        private int _currentAnimalIdIndex;
        [ObservableProperty]
        private ObservableCollection<Guid> _sensorTypeIds = new ObservableCollection<Guid>();
        [ObservableProperty]
        private int _currentSensorTypeIdIndex;

        public SensorViewModel(
            IEntityApiService<Sensor, SensorId, SensorCreation, SensorUpdate> apiService,
            IEntityApiService<Animal, AnimalId, AnimalCreation, AnimalUpdate> animalApiService,
            IEntityApiService<SensorType, SensorTypeId, SensorTypeCreation, SensorTypeUpdate> sensorTypeApiService,
            IMapper mapper) : base(apiService, mapper)
        {
            _animalApiService = animalApiService;
            _sensorTypeApiService = sensorTypeApiService;
        }

        protected override async Task LoadAsync()
        {
            AnimalIds = new ObservableCollection<Guid>((await _animalApiService.GetAllAsync()).Select(a => a.Id));
            SensorTypeIds = new ObservableCollection<Guid>((await _sensorTypeApiService.GetAllAsync()).Select(st => st.Id));

            await base.LoadAsync();
        }

        protected override async Task GetCurrentEntityAsync(SelectedItemChangedEventArgs? args)
        {
            await base.GetCurrentEntityAsync(args);

            CurrentAnimalIdIndex = AnimalIds.IndexOf(CurrentEntity!.AnimalId);
            CurrentSensorTypeIdIndex = SensorTypeIds.IndexOf(CurrentEntity!.AnimalId);
        }
    }
}
