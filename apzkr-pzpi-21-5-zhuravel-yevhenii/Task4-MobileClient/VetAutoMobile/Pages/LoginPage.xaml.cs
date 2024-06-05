using System.Resources;
using VetAutoMobile.Abstractions.Pages;
using VetAutoMobile.Entities.Guards;
using VetAutoMobile.Resources;
using VetAutoMobile.ViewModels.PageViewModels;

namespace VetAutoMobile.Pages;

public partial class LoginPage : ContentPage, IEntityWithGuards
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

		var resourceManager = new ResourceManager(typeof(MobileResources));

		Title = resourceManager.GetString("Login");
	}

    public IEnumerable<Guard> Guards => [Guard.OnlyIfLogout];
}