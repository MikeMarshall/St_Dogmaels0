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
    public partial class FacilitiesPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public Command TapCommand
        {
            get
            {

                return new Command(val => {
                    DisplayAlert("Alert", val.ToString(), "OK");
                });
            }
        }

        public FacilitiesPage()
        {
            InitializeComponent();
            listView.ItemsSource = Facilities.facilities;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                //if (result)
                await RootPage.NavigateFromMenu(0);//await this.Navigation.PopToRootAsync();
                ;
            });
            return true;
        }

        private void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.ItemIndex)
            {
                case 0:
                    Device.OpenUri(new Uri("https://www.argovilla.co.uk/"));
                    break;
                case 1:
                    Device.OpenUri(new Uri("http://wwww.orielmilgi.co.uk"));
                    break;
                case 2:
                    Device.OpenUri(new Uri("https://bethsaida.wales/"));
                    break;
                case 3:
                    Device.OpenUri(new Uri("http://www.stdogmaelsabbey.org.uk"));
                    break;
                case 4:
                    Device.OpenUri(new Uri("https://www.premier-stores.co.uk/our-stores/siop-y-pentre-0"));
                    break;
                case 5:
                    Device.OpenUri(new Uri("https://www.postoffice.co.uk/branchfinder2/2516136/st-dogmaels"));
                    break;
            }
        }
    }
}