using St_Dogmaels.ViewModels;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using St_Dogmaels.Models;

namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HouseDetail : ContentPage
    {
        private ViewModels.HouseDetailViewModel ViewModel => BindingContext as ViewModels.HouseDetailViewModel;

        public HouseDetail()
        {
            InitializeComponent();
        }

        public HouseDetail(string id)
        {
            InitializeComponent();
            House house = App.houses.Where(m => m.ID == id).First();
            
            BindingContext = new ViewModels.HouseDetailViewModel(house);
            webView.Navigating += WebView_Navigating;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Description")
            {
                webView.Source = new HtmlWebViewSource() { BaseUrl = "/", Html = ViewModel.Description };
            }
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var s = e.Url;
            if (s.StartsWith("place:")) // Tag for internal links to places. See HouseDetailViewModel
            {
                e.Cancel = true;
                ViewModel.MoveToPlaceAsync("p1 " + s.Substring(s.IndexOf('=') + 1));
            }
        }


        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }


    }
}