using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace St_Dogmaels.Models
{
    //CREATE TABLE "House" (	"ID"	TEXT,	"Lat"	FLOAT,	"Lon"	FLOAT,	"Postcode"	TEXT,	"Pic1"	TEXT,	"Pic2"	TEXT,	"FirstPic"	TEXT,	
    //"Title"	TEXT,	"Subtitle"	TEXT,	"Tags"	TEXT,	"Text"	TEXT,	"Year"	TEXT,	"Zoom"	TEXT

    [Table("House")]
    public class House
    {
        
        [Column("ID")]
        public string ID { get; set; }
        [Column("Lat")]
        public double Lat { get; set; }
        [Column("Lon")]
        public double Lon{ get; set; }
        [Column("Postcode")]
        public string Postcode { get; set; }
        [Column("Pic1")]
        public string Pic1 { get; set; }
        [Column("Pic2")]
        public string Pic2 { get; set; }
        [Column("FirstPic")]
        public string FirstPic { get; set; }     
        [Column("Title")]
        public string Title { get; set; }
        [Column("Subtitle")]
        public string Subtitle { get; set; }
        [Column("Text")]
        public string Text { get; set; }
        [Column("Tags")]
        public string Tags { get; set; }
        [Column("Year")]
        public string Year { get; set; }
        [Column("Zoom")]
        public string Zoom { get; set; }
    }
}
