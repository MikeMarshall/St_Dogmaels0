using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using St_Dogmaels.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;


namespace St_Dogmaels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoiPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }


        public ObservableCollection<Poi> Pois { get; set; }




        public PoiPage()
        {

            InitializeComponent();



            try
            {
                Pois = new ObservableCollection<Poi>();





                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(PoiPage)).Assembly;
                string[] names = assembly.GetManifestResourceNames();
                for (int i = 0; i < names.Count(); i++)
                {
                    string[] fields = names[i].Split('.');
                    if (fields[1] == "Pois")
                    {
                        Stream stream = assembly.GetManifestResourceStream(names[i]);
                        XmlSerializer serializer = new XmlSerializer(typeof(Poi));
                        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
                        serializer.UnknownElement += new XmlElementEventHandler(serializer_UnknownElement);
                        serializer.UnreferencedObject += new UnreferencedObjectEventHandler(serializer_UnknownObject);
                        Poi poi = (Poi)serializer.Deserialize(stream);
                        Pois.Add(poi);
                    }
                }




                carousel.ItemsSource = Pois;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Messages:" + ex.Message);
                Debug.WriteLine("Details:" + ex.ToString());
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
    }
}