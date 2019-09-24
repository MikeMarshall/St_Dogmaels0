using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using St_Dogmaels.Models;
using System.Collections.ObjectModel;
using St_Dogmaels.Views;


namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalksPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }


        public IList<Walk> Walks { get; private set; }


        public WalksPage()
        {

            InitializeComponent();

            Walks = new List<Walk>();


            try
            {
                LoadWalks();


                BindingContext = this;

                lvWalks.ItemsSource = Walks;

            }
            catch (FormatException fe)
            {
                Console.WriteLine("Message:" + fe.Message + " Source:" + fe.Source + ":" + fe.StackTrace.ToString());
            }




        }

        private void LoadWalks()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(WalksPage)).Assembly;
            string[] names = assembly.GetManifestResourceNames();
            for (int i = 0; i < names.Count(); i++)
            {
                string[] fields = names[i].Split('.');
                if (fields[1] == "Walks")
                {
                    Stream stream = assembly.GetManifestResourceStream(names[i]);
                    XmlSerializer serializer = new XmlSerializer(typeof(Walk));
                    serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                    serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
                    serializer.UnknownElement += new XmlElementEventHandler(serializer_UnknownElement);
                    serializer.UnreferencedObject += new UnreferencedObjectEventHandler(serializer_UnknownObject);
                    Walk walk = (Walk)serializer.Deserialize(stream);
                    Walks.Add(walk);
                }
            }




        }

        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
        private void serializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            Console.WriteLine("Unknown Element:" + e.Element.Name);
        }
        private void serializer_UnknownObject(object sender, UnreferencedObjectEventArgs e)
        {
            Console.WriteLine("Unreference object:" + e.UnreferencedObject.ToString());
        }
        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }





        //public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        //{

        //    Walk walk = Walks[e.ItemIndex];
        //    MapPage mapPage = new MapPage(walk);
        //    //OnAddWalk(new AddWalkEventArgs(walk));
        //}




        //protected virtual void OnAddWalk(AddWalkEventArgs e)
        //{
        //    EventHandler<AddWalkEventArgs> handler = OnAddWalk(<AddWalkEventArgs>);

        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        public void OnListViewSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItemIndex > -1)
            {
                //App.walk = Walks[e.SelectedItemIndex];
                //((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new MapPage(walk));
                //try
                //{
                //    MessagingCenter.Send(this, "WALK", walk);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    Console.WriteLine(ex.Source);
                //    Console.WriteLine(ex.StackTrace.ToString());
                //}
                //lvWalks.SelectedItem = null;
                //await Navigation.PopModalAsync();

                //AddWalkEvent(new AddWalkEventArgs(Walks[e.SelectedItemIndex]));
                //App.Walk = Walks[e.SelectedItemIndex];
                //lvWalks.SelectedItem = null;
                //((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new MapPage());
                //(App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage(new MapPage());
            }
        }


        //private void CallMap(Walk walk)
        //{


        //    ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new MapPage(walk));
        //}

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                //if (result)
                var id = (int)(MenuItemType.Home);
                await RootPage.NavigateFromMenu(id);
                ;
            });
            return true;
        }

        async void AddToMapClicked(object sender, EventArgs e)
        {
            App.Walks.Add((Walk)lvWalks.SelectedItem);
            lvWalks.SelectedItem = null;
            //(App.Current.MainPage as MasterDetailPage).Detail = new NavigationPage(new MapPage());
            var id = (int)(MenuItemType.Map);
            await RootPage.NavigateFromMenu(id);
        }
    }





}