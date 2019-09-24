using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace St_Dogmaels.Droid
{
    [Activity(Label = "St_Dogmaels", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            Forms.SetFlags("CollectionView_Experimental");

            // https://jamesmontemagno.github.io/GeolocatorPlugin/GettingStarted.html
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Acr.UserDialogs.UserDialogs.Init(() => this);


            var width = Resources.DisplayMetrics.WidthPixels;
            var height = Resources.DisplayMetrics.HeightPixels;
            var density = Resources.DisplayMetrics.Density;

            App.ScreenWidth = (width - 0.5f) / density;
            App.ScreenHeight = (height - 0.5f) / density;

            System.Diagnostics.Debug.WriteLine("DEBUG h:" + height.ToString() + " w:" + width.ToString());

            LoadApplication(new App());
        }

        /// <summary>
        /// For Geolocator. See https://jamesmontemagno.github.io/GeolocatorPlugin/GettingStarted.html
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}