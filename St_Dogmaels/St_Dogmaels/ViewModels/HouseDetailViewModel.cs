using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using St_Dogmaels.Models;
using St_Dogmaels.Views;
using Xamarin.Forms;

namespace St_Dogmaels.ViewModels
{
    class HouseDetailViewModel : BaseViewModel
    {

        private HousesViewModel housesViewModel;
        public static readonly BindableProperty PlaceProperty =
            BindableProperty.Create("Place", typeof(House), typeof(HouseDetailViewModel), null);
        public House Place
        {
            get { return (House)GetValue(PlaceProperty); }
            set
            {
                SetValue(PlaceProperty, value);
                Title = value != null ? value.Title.Replace("&#39;","'") : " - ";
                // Tag 'place:' for links to places intercepted in HouseDetail.xaml.cs::WebView_Navigating
                Description = "<!DOCTYPE><html><body>" + value.Text.Replace("./?", "place://") + "</body></html>";
            }
        }

        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create("Description", typeof(string), typeof(HouseDetailViewModel), "-");
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public HouseDetailViewModel(House house)
        {
            //NavigationPage.SetHasNavigationBar(this, true);
            Place = house;
            housesViewModel = App.housesViewModel;
        }


        /// <summary>
        /// User clicked a link to another place.
        /// Close the current Place page and behave as if the user has tapped the target place on the map.
        /// </summary>
        /// <param name="placeId"></param>
        public async void MoveToPlaceAsync(string placeId)
        {
            try
            {
                //this.Place = await Services.AzureDataStore.Store.GetItemAsync(placeId);
                //housesViewModel.SelectionUpdate(Place);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
