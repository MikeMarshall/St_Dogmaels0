using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace St_Dogmaels.Models
{
    //CREATE TABLE images (tile_id TEXT, tile_data BLOB)
    [Table("Images")]
    public class Images
    {
        [PrimaryKey, Column("Tile_id")]
        public String Tile_id { get; set; }
        [Column("Tile_data")]
        public byte[] Tile_data { get; set; }
    }
}
