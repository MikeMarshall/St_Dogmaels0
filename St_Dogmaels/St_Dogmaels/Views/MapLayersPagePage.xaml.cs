using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapLayersPage : ContentPage
    {
        public MapLayersPage()
        {
            InitializeComponent();
            HousesSwitch.IsToggled = App.LayerHouses;
            GardensSwitch.IsToggled = App.LayerGardens;
            LandscapeSwitch.IsToggled = App.LayerLandscape;
            PoisSwitch.IsToggled = App.LayerPois;
            WalksSwitch.IsToggled = App.LayerSteps;
            FacilitiesSwitch.IsToggled = App.LayerFacilities;
        }


        public void Houses_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerHouses = e.Value;
        }
        public void Gardens_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerGardens = e.Value;
        }
        public void Landscape_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerLandscape = e.Value;
        }
        public void Pois_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerPois = e.Value;
        }
        public void Walks_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerSteps = e.Value;
        }
        public void Facilities_Switch_Toggled(object sender, ToggledEventArgs e)
        {
            App.LayerFacilities = e.Value;
        }
        

        private async void Close_Button(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}