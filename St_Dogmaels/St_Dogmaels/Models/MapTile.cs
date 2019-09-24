using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace St_Dogmaels.Models
{
   
    [Table("MapTile")]
    public class MapTile
    {
        [Column("Zoom_level")]
        public int Zoom_level { get; set; }
        [Column("Tile_column")]
        public int Tile_column { get; set; }
        [Column("Tile_row")]
        public int Tile_row { get; set; }
        [Column("Tile_id")]
        public String Tile_id { get; set; }
        [Column("Tile_lat")]
        public double Tile_lat { get; set; }
        [Column("Tile_lon")]
        public double Tile_lon { get; set; }
        [Column("Tile_data")]
        public byte[] Tile_data { get; set; }
    }
    
}
