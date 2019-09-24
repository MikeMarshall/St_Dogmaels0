using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Data.SqlTypes;
using St_Dogmaels.Models;
using System.Reflection;
using Acr.UserDialogs;
using System.Diagnostics;

namespace St_Dogmaels.DataAccess
{
    public class HousesDB
    {
        IEnumerable<Place> Places = null;

        public HousesDB(IEnumerable<Place> places)
        {

            Places = places;

            App.housesDB = GetConnection();


        }

        private SQLiteConnection GetConnection()
        {
            string sqliteFilename = "houses.db";
            string dbPath = Path.Combine(GetDatabasePath(), sqliteFilename);
                       
            CreateDB(dbPath);
        

            SQLiteConnection db = new SQLiteConnection(dbPath);

            return db;
        }

        private void CreateDB(String dbPath)
        {
            //SQLiteConnection db = new SQLiteConnection(dbPath);

            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            SQLiteConnectionWithLock db = new SQLiteConnectionWithLock(connStr); 

            db.CreateTable<House>();

            //UserDialogs.Instance.ShowLoading("Downloading houses...");
            LoadHouses(db);
            //UserDialogs.Instance.HideLoading();
        }

        private string GetDatabasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        
        private void LoadHouses(SQLiteConnection db)
        {
            //var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            //String[] files = assembly.GetManifestResourceNames();

            

           
            foreach (Place place in Places)
            {
                try
                {
                    House house = new House();
                    house.ID = place.Id;
                    house.Lat = place.Lat;
                    house.Lon = place.Lon;
                    house.Pic1 = place.Pic1;
                    house.Pic2 = place.Pic2;
                    house.FirstPic = place.FirstPic;
                    house.Postcode = place.Postcode;
                    house.Title = place.Title;
                    house.Subtitle = place.Subtitle;
                    house.Text = place.Text;
                    house.Year = place.Year;
                    house.Zoom = place.Zoom;
                    house.Tags = place.Tags;
                    db.Insert(house);

                    App.houses.Add(house);
                  



                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);

                }
            }

            //SQLiteCommand command = db.CreateCommand("CREATE VIEW tiles AS   SELECT map.zoom_level as zoom_level,    map.tile_column as tile_column,    map.tile_row as tile_row,    images.tile_data as tile_data   FROM map JOIN images ON map.tile_id = images.tile_id");
            //command.ExecuteNonQuery();


            //SQLiteCommand command1 = db.CreateCommand("CREATE VIEW maptiles AS   SELECT map.zoom_level as zoom_level,    map.tile_column as tile_column,    map.tile_row as tile_row, map.tile_lat as tile_lat, map.tile_lon as tile_lon,    images.tile_data as tile_data   FROM map JOIN images ON map.tile_id = images.tile_id");
            //command1.ExecuteNonQuery();


        }







    }
}
