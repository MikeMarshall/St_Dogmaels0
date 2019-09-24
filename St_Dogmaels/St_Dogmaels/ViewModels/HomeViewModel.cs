using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace St_Dogmaels.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public static readonly BindableProperty RegionProperty =
             BindableProperty.Create("Region", typeof(string), typeof(HousesViewModel), null);
        public string Region
        {
            get { return (string)GetValue(RegionProperty); }
            set { SetValue(RegionProperty, value); }
        }

        public HomeViewModel()
        {
            Title = "Home";
            Task t = CheckGeo();
        }

        private async Task CheckGeo()
        {
            Region = "stdogmaels";
            //try
            //{
            //    var position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
            //    if (position != null)
            //    {
            //        if (position.Latitude > 52.04 && position.Longitude > -4.8 && position.Latitude < 52.0826 && position.Longitude < -4.705)
            //            Region = "moylgrove";
            //        else if (position.Latitude > 52.052826 && position.Longitude > -4.77 && position.Latitude < 52.1 && position.Longitude < -4.773087)
            //            Region = "stdogmaels";
            //        else if (position.Latitude > 52.027519 && position.Longitude > -4.700646 && position.Latitude < 52.1 && position.Longitude < -4.7)
            //            Region = "cardigan";
            //        else
            //            Region = "";
            //    }
            //}
            //catch (Exception) { }
        }
    }
}
