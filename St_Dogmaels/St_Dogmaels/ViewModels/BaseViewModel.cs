using Xamarin.Forms;
using St_Dogmaels.Models;
using St_Dogmaels.Services;

namespace St_Dogmaels.ViewModels
{
    // About Xamarin.Forms.BindableObject:
    // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/xaml/bindable-properties

    public class BaseViewModel : BindableObject
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? new MockDataStore();

        public static readonly BindableProperty IsBusyProperty =
             BindableProperty.Create("IsBusy", typeof(bool), typeof(BaseViewModel), false);
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
             BindableProperty.Create("Title", typeof(string), typeof(BaseViewModel), "Untitled");
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
