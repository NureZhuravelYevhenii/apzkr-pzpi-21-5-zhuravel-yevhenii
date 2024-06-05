using System.Resources;
using VetAutoMobile.Abstractions.Pages;
using VetAutoMobile.Entities.Guards;
using VetAutoMobile.Resources;
using VetAutoMobile.ViewModels.PageViewModels;

namespace VetAutoMobile.Pages;

public partial class AnimalTypesPage : ContentPage, IEntityWithGuards
{
	public AnimalTypesPage(AnimalTypeViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        var resourceManager = new ResourceManager(typeof(MobileResources));

        Title = resourceManager.GetString("AnimalTypes");
    }

    public IEnumerable<Guard> Guards => [Guard.LoginRequired];
}