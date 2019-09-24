using St_Dogmaels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using St_Dogmaels.DataAccess;
using Xamarin.Forms;


namespace St_Dogmaels.ViewModels
{
    public class HousesViewModel : BaseViewModel
    {
        public static readonly BindableProperty LocationProperty =
             BindableProperty.Create("Location", typeof(Position), typeof(HousesViewModel), new Position(52.06, -4.75));
        public Position Location
        {
            get { return (Position)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }


        public static readonly BindableProperty PlacesProperty =
             BindableProperty.Create("Places", typeof(IEnumerable<Place>), typeof(HousesViewModel), null);
        public IEnumerable<Place> Places
        {
            get { return (IEnumerable<Place>)GetValue(PlacesProperty); }
            set { SetValue(PlacesProperty, value); }
        }

        public static readonly BindableProperty SelectedPlaceProperty =
             BindableProperty.Create("SelectedPlace", typeof(Place), typeof(HousesViewModel), null);
        public Place SelectedPlace
        {
            get { return (Place)GetValue(SelectedPlaceProperty); }
            set { SetValue(SelectedPlaceProperty, value); }
        }

        //public static readonly BindableProperty MapSpanProperty =
        //     BindableProperty.Create("MapSpan", typeof(MapSpan), typeof(HousesViewModel),
        //         MapSpan.FromCenterAndRadius(new Position(52.06, -4.75), Distance.FromMeters(50)));
        //public MapSpan MapSpan
        //{
        //    get { return (MapSpan)GetValue(MapSpanProperty); }
        //    set { SetValue(MapSpanProperty, value); }
        //}


        public HousesViewModel()
        {
            Title = "Map";
            //StartLocationListener();
        }

        private async void StartLocationListener()
        {
            var locator = Plugin.Geolocator.CrossGeolocator.Current;

            if (locator.IsListening)
                return;

            locator.PositionChanged += (sender, e) => {
                this.Location = new Position(e.Position.Latitude, e.Position.Longitude);
                LocationUpdate();
            };

            await locator.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);

        }

        public void SelectionUpdate(Place place)
        {
            SelectedPlace = place;
            var distance = Math.Sqrt(DistanceSquared(Location, place));
            SetMap(distance);
        }

        public void LocationUpdate()
        {
            SelectedPlace = NearestPlace(Location, out var distance);
            SetMap(distance);
        }

        private void SetMap(double distance)
        {
            //MapSpan = MapSpan.FromCenterAndRadius(Location, Distance.FromMeters(Math.Max(distance * 12, 50)));
        }

        /// <summary>
        /// Find the nearest place to a given position, within a fixed search radius.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="distance">to place from current position, or search radius if none found</param>
        /// <returns></returns>
        private Place NearestPlace(Position position, out double distance)
        {
            const double limitSquared = 250000; // Half a Km
            Place chosen = null;
            double chosenDistanceSquared = limitSquared;
            if (Places != null)
            {
                foreach (var house in Places)
                {
                    var distanceSquared = DistanceSquared(position, house);
                    if (distanceSquared > limitSquared) continue;
                    if (distanceSquared < chosenDistanceSquared)
                    {
                        chosen = house;
                        chosenDistanceSquared = distanceSquared;
                    }
                }
            }
            distance = Math.Sqrt(chosenDistanceSquared);
            return chosen;
        }

        const double latMeters = 15000; // Pembs. 17900
        const double longMeters = 6840;
        private double DistanceSquared(Position p1, Place house)
        {
            var dlat = (p1.Latitude - house.Lat) * latMeters;
            var dlong = (p1.Longitude - house.Lon) * longMeters;
            return dlat * dlat + dlong * dlong;
        }

        public async Task GetHousesAsync()
        {
            var store = Services.AzureDataStore.Store;
            Places = await store.GetItemsAsync();
            HousesDB housesDB = new HousesDB(Places);
        }

    }
}