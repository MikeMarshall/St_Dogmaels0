using System;
using System.Collections.Generic;
using System.Text;
using St_Dogmaels.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using System.Windows.Input;
using System.Diagnostics;

namespace St_Dogmaels.ViewModels
{
    public class FacilitiesViewModel : BaseViewModel
    {

        private ICommand _tapCommand;
        public ICommand TapCommand =>
        _tapCommand ?? (_tapCommand = new Command<string>(OpenUrl));

        public FacilitiesViewModel()
        {
            Title = "Facilities";
        }

        void OpenUrl(string url)
        {
            Device.OpenUri(new Uri(url));
        }

        //private void TapCommand(object obj)
        //{
        //    var labelText = (obj as Label).Text;
        //}
    }
}
