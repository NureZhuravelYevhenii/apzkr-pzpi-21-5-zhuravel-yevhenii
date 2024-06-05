using VetAutoMobile.Abstractions.Pages;
using VetAutoMobile.Entities.Guards;
using VetAutoMobile.ViewModels.AppViewModels;

namespace VetAutoMobile
{
    public partial class AppShell : Shell
    {
        public AppShell(IEnumerable<IEntityWithGuards> pages, AppShellViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;

            Items.Clear();

            foreach (var page in pages)
            {
                HandleGuards(page);
            }
        }

        private void HandleGuards(IEntityWithGuards page)
        {
            var pageAsContentPage = page as ContentPage;
            if (pageAsContentPage is null)
            {
                return;
            }

            var flyoutItem = new FlyoutItem();

            flyoutItem.Items.Add(pageAsContentPage);
            flyoutItem.Title = pageAsContentPage.Title;

            if (page.Guards.Contains(Guard.LoginRequired))
            {
                flyoutItem.SetBinding(FlyoutItem.IsVisibleProperty, nameof(AppShellViewModel.IsLoggedIn));
            }
            if (page.Guards.Contains(Guard.OnlyIfLogout))
            {
                flyoutItem.SetBinding(FlyoutItem.IsVisibleProperty, nameof(AppShellViewModel.IsLoggedOut));
            }

            Items.Add(flyoutItem);
        }
    }
}
