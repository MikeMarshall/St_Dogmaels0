using St_Dogmaels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));  //ItemsPage()
                        break;
                    case (int)MenuItemType.Map:
                        MenuPages.Add(id, new NavigationPage(new MapPage()));
                        break;
                    case (int)MenuItemType.Poi:
                        MenuPages.Add(id, new NavigationPage(new PoiPage()));
                        break;
                    case (int)MenuItemType.Facilities:
                        MenuPages.Add(id, new NavigationPage(new FacilitiesPage()));
                        break;
                    case (int)MenuItemType.Walks:
                        MenuPages.Add(id, new NavigationPage(new WalksPage()));
                        break;
                    case (int)MenuItemType.Geology:
                        MenuPages.Add(id, new NavigationPage(new GeologyPage()));
                        break;
                    case (int)MenuItemType.Abbey:
                        MenuPages.Add(id, new NavigationPage(new AbbeyPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}