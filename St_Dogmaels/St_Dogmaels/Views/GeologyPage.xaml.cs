using St_Dogmaels.ViewModels;
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
    public partial class GeologyPage : ContentPage
    {
        public GeologyPage()
        {
            InitializeComponent();
            this.BindingContext = new GeologyViewModel();
        }

        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (wvGeology.CanGoBack)
            {
                wvGeology.GoBack();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        void OnForwardButtonClicked(object sender, EventArgs e)
        {
            if (wvGeology.CanGoForward)
            {
                wvGeology.GoForward();
            }
        }
    }
}