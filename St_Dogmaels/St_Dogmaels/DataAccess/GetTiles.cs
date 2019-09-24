using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace St_Dogmaels.DataAccess
{
    public class GetTiles
    {

    }

    //public TileSet GetTileSet(int zoom_level, column, row)
    //{
    //    //select tile_column, tile_row, tile_data from tiles where (zoom_level = 17) and  (tile_column between 63827 and 63835) and (tile_row between 43241 and 43251)

    //    byte[] iconBytes = null;
    //    using (var dbConnection = new SQLiteConnection(DataSource))
    //    {
    //        dbConnection.Open();
    //        using (var transaction = dbConnection.BeginTransaction())
    //        {
    //            using (var command = new SQLiteCommand(dbConnection))
    //            {
    //                command.CommandText = "SELECT icon FROM my_table";

    //                using (var reader = command.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        if (reader["icon"] != null && !Convert.IsDBNull(reader["icon"]))
    //                        {
    //                            iconBytes = (byte[])reader["icon"];
    //                        }
    //                    }
    //                }
    //            }
    //            transaction.Commit();
    //        }
    //    }
    //}
}
