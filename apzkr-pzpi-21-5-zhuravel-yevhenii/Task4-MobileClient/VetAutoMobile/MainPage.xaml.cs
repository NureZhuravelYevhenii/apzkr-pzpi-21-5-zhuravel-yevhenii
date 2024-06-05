using Mapsui.Tiling;
using Mapsui.UI.Maui;

namespace VetAutoMobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var mapControl = new MapControl();
            mapControl.Map?.Layers.Add(OpenStreetMap.CreateTileLayer());
            MapControl.Content = mapControl;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
