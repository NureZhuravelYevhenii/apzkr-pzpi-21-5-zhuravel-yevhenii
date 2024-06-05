using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using VetAutoMobile.ApiLayer.Abstractions;
using VetAutoMobile.ViewModels.BaseViewModels;

namespace VetAutoMobile.ViewModels
{
    public partial class EntityBaseViewModel<T, TId, TCreation, TUpdate> : BaseViewModel
        where T : class
        where TCreation : new()
    {
        protected readonly IEntityApiService<T, TId, TCreation, TUpdate> _apiService;
        protected readonly IMapper _mapper;
        [ObservableProperty]
        private T? _currentEntity;
        [ObservableProperty]
        private TCreation? _newEntity = new TCreation();
        [ObservableProperty]
        private bool _isCreating = false;
        [ObservableProperty]
        private IEnumerable<T> _entities = new List<T>();

        public ICommand Loaded { get; set; }
        public ICommand GetEntities { get; set; }
        public ICommand GetEntity { get; set; }
        public ICommand Update { get; set; }
        public ICommand Create { get; set; }
        public ICommand ToCreate { get; set; }
        public ICommand BackToList { get; set; }

        public bool IsCurrentEntitySet => !IsCreating && CurrentEntity is not null;
        public bool IsCurrentEntityNotSet => !IsCreating && CurrentEntity is null;

        public EntityBaseViewModel(IEntityApiService<T, TId, TCreation, TUpdate> apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;

            Loaded = new AsyncRelayCommand(LoadAsync);
            GetEntities = new AsyncRelayCommand(GetEntitiesAsync);
            GetEntity = new AsyncRelayCommand<SelectedItemChangedEventArgs>(GetCurrentEntityAsync);
            Update = new AsyncRelayCommand(UpdateAsync);
            Create = new AsyncRelayCommand(CreateAsync);
            ToCreate = new RelayCommand(ToCreatePage);
            BackToList = new AsyncRelayCommand(BackToListAsync);
        }

        partial void OnCurrentEntityChanged(T? oldValue, T? newValue)
        {
            OnPropertyChanged(nameof(IsCurrentEntitySet));
            OnPropertyChanged(nameof(IsCurrentEntityNotSet));
        }

        partial void OnIsCreatingChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(nameof(IsCurrentEntitySet));
            OnPropertyChanged(nameof(IsCurrentEntityNotSet));
        }

        private async Task GetEntitiesAsync()
        {
            Entities = await _apiService.GetAllAsync(); 
        }

        protected virtual async Task GetCurrentEntityAsync(SelectedItemChangedEventArgs? args)
        {
            var entity = args?.SelectedItem as T;

            if (entity is null)
            {
                return;
            }

            CurrentEntity = await _apiService.GetByIdAsync(_mapper.Map<TId>(entity));
        }

        protected virtual Task LoadAsync()
        {
            return GetEntitiesAsync();
        }

        protected virtual Task UpdateAsync()
        {
            return _apiService.UpdateAsync(_mapper.Map<TUpdate>(CurrentEntity));
        }

        protected virtual async Task CreateAsync()
        {
            await _apiService.CreateAsync(NewEntity!); 
            await GetEntitiesAsync();
            IsCreating = false;
        }

        private void ToCreatePage()
        {
            NewEntity = new TCreation();
            IsCreating = true;
        }

        private Task BackToListAsync()
        {
            CurrentEntity = null;
            return GetEntitiesAsync();
        }
    }
}
