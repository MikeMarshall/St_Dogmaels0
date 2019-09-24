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
	public partial class WebViewPage : ContentPage
	{
		public WebViewPage ()
		{
			InitializeComponent ();
            
            
            MyWebView.Source = new HtmlWebViewSource
            {
                Html = @"<html><body>An html string</body></html>"
            };
        }
	}
}