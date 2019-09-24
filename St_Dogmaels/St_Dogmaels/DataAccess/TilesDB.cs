using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Data.SqlTypes;
using St_Dogmaels.Models;
using System.Reflection;
using Acr.UserDialogs;

namespace St_Dogmaels.DataAccess
{
    public class TilesDB
    {
       

        public TilesDB()
        {

           
            
            App.mapDB = GetConnection();

            
        }

        private SQLiteConnection GetConnection()
        {
            string sqliteFilename = "map.db";
            string dbPath = Path.Combine(GetDatabasePath(), sqliteFilename);
            if (App.RebuildDB)
                File.Delete(dbPath);
            // Check if your DB has already been created.
            if (!File.Exists(dbPath))
            {
                //using (BinaryReader br = new BinaryReader(Android.App.Application.Context.Assets.Open(dbName)))
                //{
                //    using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                //    {
                //        byte[] buffer = new byte[2048];
                //        int len = 0;
                //        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                //        {
                //            bw.Write(buffer, 0, len);
                //        }
                //    }
                //}
                CreateDB(dbPath);
            }

            SQLiteConnection db = new SQLiteConnection(dbPath);

            return db;
        }

        private void CreateDB(String dbPath)
        {
            //SQLiteConnection db = new SQLiteConnection(dbPath);
           
            SQLiteConnectionString connStr = new SQLiteConnectionString(dbPath, true);
            SQLiteConnectionWithLock db = new SQLiteConnectionWithLock(connStr); //, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.NoMutex);

            db.CreateTable<Images>();
            db.CreateTable<Map>();
            UserDialogs.Instance.ShowLoading("Downloading map...");
            FindTiles(db);
            UserDialogs.Instance.HideLoading();
        }

        private string GetDatabasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        private Position TileToWorldPos(int tile_x, int tile_y, int zoom)
        {
            Position p = new Position();

            double x = Convert.ToDouble(tile_x);
            double y = Convert.ToDouble(tile_y);

            double n = Math.PI - ((2.0 * Math.PI * y) / Math.Pow(2.0, zoom));

            p.Longitude = (float)((x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            p.Latitude = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));
            

            return p;
        }

        private void FindTiles(SQLiteConnection db)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            String[] files = assembly.GetManifestResourceNames();

           
            
            // Perform the required action on each file here.
            // Modify this block to perform your required task.
            foreach (string file in files)
            {
                try
                {
                    if (file.StartsWith("St_Dogmaels.Tiles."))
                    { 
                        Images images = new Images();
                        int pos1 = file.IndexOf("1");
                        int pos2 = file.IndexOf(".png");
                        String strZ = file.Substring(pos1, 2);
                        String strXY = file.Substring(pos1 + 3, pos2 - pos1 - 3);
                        String[] strX_Y = strXY.Split('_');
                        String strX = strX_Y[0];
                        String strY = strX_Y[1];
                        int x = Convert.ToInt32(strX);
                        int y = Convert.ToInt32(strY);
                        int z = Convert.ToInt32(strZ);
                        images.Tile_id = strZ + @"/" + strX + @"/" + strY;

                    
                        var stream = assembly.GetManifestResourceStream(file);
                    
                        var buffer = new byte[24 * 1024];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            images.Tile_data = ms.ToArray();

                        }

                        db.Insert(images);

                        Map map = new Map
                        {
                            Zoom_level = z,
                            Tile_column = x,
                            Tile_row = y,
                            Tile_id = images.Tile_id
                        };
                        Position position = TileToWorldPos(x, y, z);
                        map.Tile_lat = position.Latitude;
                        map.Tile_lon = position.Longitude;
                        db.Insert(map);

                    }

                }
                catch (System.IO.FileNotFoundException e)
                {

                    Console.WriteLine(e.Message);

                }
            }

            SQLiteCommand command = db.CreateCommand("CREATE VIEW tiles AS   SELECT map.zoom_level as zoom_level,    map.tile_column as tile_column,    map.tile_row as tile_row,    images.tile_data as tile_data   FROM map JOIN images ON map.tile_id = images.tile_id");
            command.ExecuteNonQuery();


            SQLiteCommand command1 = db.CreateCommand("CREATE VIEW maptiles AS   SELECT map.zoom_level as zoom_level,    map.tile_column as tile_column,    map.tile_row as tile_row, map.tile_lat as tile_lat, map.tile_lon as tile_lon,    images.tile_data as tile_data   FROM map JOIN images ON map.tile_id = images.tile_id");
            command1.ExecuteNonQuery();


        }


        

       


    }
}
