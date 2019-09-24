using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Extensions;
using St_Dogmaels.Models;

namespace St_Dogmaels.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HousePopup : PopupPage
	{
        private string ID = null;
        //private House house;
		public HousePopup ( string id, string title, string subtitle)
		{
			InitializeComponent();
            ID = id;
            lblID.Text = id;
            lblTitle.Text = title.Replace("&#39;", "'");
            lblSubtitle.Text = subtitle;
            if (id == "W" || (id == "S"))
                DetailButton.IsVisible = false;
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            IsVisible = false;
            if (ID != "W" && ID != "S")
            {
                NavigationPage navigationPage = new NavigationPage(new HouseDetail(ID));
                await Navigation.PushModalAsync(navigationPage);
            }
           
            
           

            await Navigation.PopPopupAsync();
        }
    }
}