using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using St_Dogmaels.Views;
using SQLite;
using St_Dogmaels.DataAccess;
using St_Dogmaels.Models;
using SkiaSharp;
using St_Dogmaels.ViewModels;
using System.Threading.Tasks;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace St_Dogmaels
{
    

    public partial class App : Application
    {
        public static bool LayerHouses = true;
        public static bool LayerPois = true;
        public static bool LayerSteps = true;
        public static bool LayerFacilities = true;
        public static bool LayerLandscape = true;
        public static bool LayerGardens = true;

        public static bool RebuildDB = false;

        public static List<House> houses = new List<House>();
        public static HousesViewModel housesViewModel;
        public static double ScreenWidth;
        public static double ScreenHeight;
        public static SQLiteConnection mapDB;
        public static SQLiteConnection housesDB;
        public static int ZoomLevel = 17;
        public static float ZoomMinium = 1.0f;
        public static float ZoomMaximum = 3.0f;
        public static float ZoomScale = ZoomMinium;
        public static double Adjust = 0;
        public static Position CurrentCentre = new Position(52.0810427, -4.6806994); // St Dogmaels centre
        public static Position PreviousCentre = new Position(52.0810427, -4.6806994); // St Dogmaels centre 52.0818715, -4.6803483
        public static double LatDegreesPerSquare = 0.0067520141602 / 4 ;  // for zoom 15 zoom 16 /2  zoom 17 /4
        public static double LonDegreesPerSquare = 0.010986328125 / 4; // for zoom 15 zoom 16 /2  zoom 17 /4
        public static double CurrentLatDegreesPerPixel = LatDegreesPerSquare / 256.0;
        public static double CurrentLonDegreesPerPixel = LonDegreesPerSquare / 256.0;
        public static SKPoint Pressed;
        public static SKPoint Pressed1;
        public static int ReleasedCount = 0;
        public static List<Walk> Walks = new List<Walk>();

        public static readonly BindableProperty PlacesProperty =
             BindableProperty.Create("Places", typeof(IEnumerable<Place>), typeof(HousesViewModel), null);
        public  IEnumerable<Place> Places
        {
            get { return (IEnumerable<Place>)GetValue(PlacesProperty); }
            set { SetValue(PlacesProperty, value); }
        }

        private string GetDatabasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public App()
        {
            InitializeComponent();

            TilesDB tiledb = new TilesDB();


            housesViewModel = new HousesViewModel();

            MainPage = new MainPage();

            // sort out houses database
            string sqliteFilename = "houses.db";
            string dbPath = Path.Combine(GetDatabasePath(), sqliteFilename);
            if (App.RebuildDB)
                File.Delete(dbPath);
            // Check if your DB has already been created.
            if (!File.Exists(dbPath))
            {
                
                Task t = housesViewModel.GetHousesAsync();
            }
            else
            {
                housesDB = new SQLiteConnection(dbPath);
                String query = "SELECT * FROM House";
                houses = App.housesDB.Query<House>(query);
            }
        }

           
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

       

    }
}
