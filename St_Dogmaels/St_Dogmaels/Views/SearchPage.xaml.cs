using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using St_Dogmaels.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using St_Dogmaels.Views;
using St_Dogmaels.EventArg;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public Buildings bda;
        IEnumerable<Building> buildingList;
        private List<TitleSubtitle> buildings = new List<TitleSubtitle>();
        private List<BuildingLocation> buildingLocations = new List<BuildingLocation>();

        public SearchPage()
        {
            InitializeComponent();

            bda = new Buildings();
        }


        private void OnSearchBarButtonPressed(object sender, EventArgs e)
        {
            string filter = searchBar.Text.Trim();
            buildingList = bda.GetFilteredBuildings(filter);

            buildings.Clear();
            buildingLocations.Clear();
            lvSearch.ItemsSource = null;


            foreach (Building building in buildingList)
            {
                //Debug.WriteLine("HouseName:" + building.Housename + " HouseNmunber:" + building.Housenumber + " Street:" + building.Street + " Name:" + building.Name);
                // Combine Name & HouseName & HouseNumber
                string start = "";
                string name1 = building.Name.Trim();
                string name2 = building.Housename.Trim();
                string number = building.Housenumber.Trim();
                if (name1.Trim().Length > 0)
                    start = name1.Trim();
                if (name2.Length > 0)
                {
                    if (start.Length > 0)
                        start = start + ", " + name2;
                    else
                        start = name2.Trim();
                }
                if (number.Length > 0)
                {
                    if (start.Length > 0)
                        start = start + ", " + number.Trim();
                    else
                        start = number.Trim();
                }

                if (start.Length > 0)
                {
                    start = start + ", ";
                    buildings.Add(new TitleSubtitle { Title = start + building.Street, Subtitle = building.City + " " + building.Postcode });
                    buildingLocations.Add(new BuildingLocation { Longitude = building.Longitude, Latitude = building.Latitude });
                }
            }

            if (buildings.Count == 0)
            {
                DisplayAlert("Building Search", "No matching items found", "OK");
                return;
            }

            lvSearch.ItemsSource = buildings;
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            { }
        }

        async void BuildingSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItemIndex > -1)
            {
       
                await Navigation.PopModalAsync();

                CentreMapEventArgs centreMapEventArgs = new CentreMapEventArgs(buildingLocations[e.SelectedItemIndex].Latitude, buildingLocations[e.SelectedItemIndex].Longitude);
                centreMapEventArgs.Title = buildings[e.SelectedItemIndex].Title;
                centreMapEventArgs.Subtitle = buildings[e.SelectedItemIndex].Subtitle;


                OnCentreMap(centreMapEventArgs);


            }
        }

        public event EventHandler<CentreMapEventArgs> CentreMap;

        protected virtual void OnCentreMap(CentreMapEventArgs e)
        {
            EventHandler<CentreMapEventArgs> handler = CentreMap;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //private void SetLocation(double lat, double lon)
        //{
            //var viewModel = new MapViewModel(Navigation);
            //viewModel.UserLocation = new Position(lat, lon);
            //viewModel.CenterLocation = viewModel.UserLocation;
            //viewModel.UpdateViewPortAction(viewModel.CenterLocation, null, null, true, null);
            ////SetCurrentLocationMarker();
            //for (int i = 0; i < viewModel.Annotations.Count; i++)
            //{
            //    if (viewModel.Annotations[i].Title == "Current Location")
            //    {
            //        viewModel.Annotations.RemoveAt(i);
            //        break;
            //    }
            //}
            //viewModel.Annotations.Add(new PointAnnotation
            //{


            //    Coordinate = viewModel.CenterLocation,
            //    Icon = "centre",
            //    Title = "Current Location",
            //    SubTitle = ""
            //});


            //((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(App.MapPage());

            //App.SearchLatitude = lat;
            //App.SearchLongitude = lon;
            //App.MapPage.Focus();



        //}

    }





    public class TitleSubtitle
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

    }

    public class BuildingLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}