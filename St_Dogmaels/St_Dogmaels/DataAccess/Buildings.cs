using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLite;
using Acr.UserDialogs;
using Xamarin.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;


namespace St_Dogmaels
{
    public class Buildings
    {
        private SQLiteConnection database;
        private static object collisionLock = new object();

        public ObservableCollection<Building> buildings { get; set; }

        public Buildings()
        {
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            database.CreateTable<Building>();

            this.buildings = new ObservableCollection<Building>(database.Table<Building>());

            //if the table is empty initialize collection
            if (!database.Table<Building>().Any())
            {
                UserDialogs.Instance.ShowLoading("Downloading...");
                AddBuildings();
                UserDialogs.Instance.HideLoading();
            }
        }

        public void AddBuildings()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Buildings)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("St_Dogmaels.Data.buildings.txt");
            StreamReader sr = new StreamReader(stream);
            string line = sr.ReadLine(); // ignore header line
                                         // id	longitude	latitude	housename	housenumber	street	city	postcode	name	name_cy	is_in	place	alt_name	open_hours	phone	shop	url
            while ((line = sr.ReadLine()) != null)
            {
                string[] fields = line.Split('\t');
                double.TryParse(fields[1], out double lon);
                double.TryParse(fields[2], out double lat);
                //this.Buildings.Add(new Building
                //{
                //    Id = Int32.Parse(fields[0]),
                //    Longitude = lon, // - 4.681431
                //    Latitude = lat, // 52.081905
                //    Housename = fields[3], //"HouseName...",
                //    Housenumber = fields[4], //"1...",
                //    Street = fields[5], //"Street...",
                //    City = fields[6], //"City...",
                //    Postcode = fields[7], //"Postcode...",
                //    Name = fields[8], //"Name...",
                //    Name_cy = fields[9], //"Name_cy...",
                //    Is_in = fields[10], //"Is_in...",
                //    Place = fields[11], //"Place...",
                //    Alt_name = fields[12], //"Alt_name...",
                //    Open_hours = fields[13], //"Open_hours...",
                //    Phone = fields[14], //"Phone...",
                //    Shop = fields[15], //"Shop...",
                //    Url = fields[16] //"Url..."
                //});
                SaveBuilding(new Building
                {
                    Id = Int32.Parse(fields[0]),
                    Longitude = lon, // - 4.681431
                    Latitude = lat, // 52.081905
                    Housename = fields[3], //"HouseName...",
                    Housenumber = fields[4], //"1...",
                    Street = fields[5], //"Street...",
                    City = fields[6], //"City...",
                    Postcode = fields[7], //"Postcode...",
                    Name = fields[8], //"Name...",
                    Name_cy = fields[9], //"Name_cy...",
                    Is_in = fields[10], //"Is_in...",
                    Place = fields[11], //"Place...",
                    Alt_name = fields[12], //"Alt_name...",
                    Open_hours = fields[13], //"Open_hours...",
                    Phone = fields[14], //"Phone...",
                    Shop = fields[15], //"Shop...",
                    Url = fields[16] //"Url..."
                });
            }
        }

        //use LINQ to query and filter data
        public IEnumerable<Building> GetFilteredBuildings(string city)
        {
            //use lock to avoid database collisions
            lock (collisionLock)
            {

                var query = from building in database.Table<Building>()
                            where building.City.StartsWith(city) || building.Housename.StartsWith(city) || building.Postcode == city || building.Name.StartsWith(city)
                            select building;
                return query.AsEnumerable();

            }
        }



        // use SQL queries against data
        public IEnumerable<Building> GetFilteredBuildings()
        {
            lock (collisionLock)
            {
                return database.Query<Building>("SELECT * FROM Item WHERE city = 'St Dogmaels'").AsEnumerable();
            }
        }


        public Building GetBuilding(int id)
        {
            return database.Table<Building>().FirstOrDefault(Building => Building.Id == id);
        }

        public int SaveBuilding(Building BuildingInstance)
        {
            lock (collisionLock)
            {
                //if (BuildingInstance.Id != 0)
                //{
                //database.Update(BuildingInstance);
                //return BuildingInstance.Id;
                //}
                //else
                //{
                try
                {
                    database.Insert(BuildingInstance);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Source + ": " + e.Message + "\n" + e.StackTrace.ToString(), "ERROR");

                }
                return BuildingInstance.Id;
                //}
            }
        }

        public void SaveAllBuildings()
        {
            lock (collisionLock)
            {
                foreach (var BuildingInstance in this.buildings)
                {
                    //if (BuildingInstance.Id != 0)
                    //{
                    //    database.Update(BuildingInstance);
                    //
                    //}
                    //else
                    // {
                    try
                    {
                        database.Insert(BuildingInstance);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Source + ": " + e.Message + "\n" + e.StackTrace.ToString(), "ERROR");

                    }

                    //}
                }
            }
        }

        public int DeleteBuilding(Building BuildingInstance)
        {
            var id = BuildingInstance.Id;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    try
                    {
                        database.Delete<Building>(id);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Source + ": " + e.Message + "\n" + e.StackTrace.ToString(), "ERROR");

                    }
                }
            }
            this.buildings.Remove(BuildingInstance);
            return id;
        }

        public void DeleteAllBuildings()
        {
            lock (collisionLock)
            {
                database.DropTable<Building>();
                database.CreateTable<Building>();

            }

            this.buildings = null;
            this.buildings = new ObservableCollection<Building>(database.Table<Building>());

        }
    }

}




