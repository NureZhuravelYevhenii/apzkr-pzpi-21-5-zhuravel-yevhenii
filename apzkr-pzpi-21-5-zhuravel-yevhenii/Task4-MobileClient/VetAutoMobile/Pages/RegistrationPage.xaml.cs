using System.Resources;
using VetAutoMobile.Abstractions.Pages;
using VetAutoMobile.Entities.Guards;
using VetAutoMobile.Resources;
using VetAutoMobile.ViewModels.PageViewModels;

namespace VetAutoMobile.Pages;

public partial class RegistrationPage : ContentPage, IEntityWithGuards
{
	public RegistrationPage(RegistrationViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        var resourceManager = new ResourceManager(typeof(MobileResources));

        Title = resourceManager.GetString("Registration");
    }

    public IEnumerable<Guard> Guards => [Guard.OnlyIfLogout];
}