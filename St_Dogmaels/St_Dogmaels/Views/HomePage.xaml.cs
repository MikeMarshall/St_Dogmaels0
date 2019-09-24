using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using St_Dogmaels.Models;
using St_Dogmaels.Views;
using St_Dogmaels.ViewModels;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public HomePage()
        {

            InitializeComponent();


        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        async void MapButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Map);
        }



        async void PoiButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Poi);
        }

        async void FacilitiesButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Facilities);
        }

        async void GeologyButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Geology);
        }

        async void WalksButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Walks);
        }

        async void AbbeyButtonClicked(object sender, EventArgs e)
        {
            await RootPage.NavigateFromMenu((int)MenuItemType.Abbey);
        }

        
    }
}