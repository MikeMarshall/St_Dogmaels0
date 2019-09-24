using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels.Models
{


    public class Place
    {
        public Place() { }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        [JsonIgnore]
        public string Id { get { return PartitionKey + " " + RowKey; } set { string[] ss = value.Split(); PartitionKey = ss[0]; RowKey = ss[1]; } }
        public static string Row(string id) { return (id + " 0").Split()[1]; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Postcode { get; set; }
        public string Tags { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Zoom { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Text { get; set; }
        public string Year { get; set; }
        public DateTime Updated { get; set; }
        public string User { get; set; }
        public string DeleteOK { get; set; }
        public string UpdateTrail { get; set; }

        [JsonIgnore]
        public double Lat { get { double.TryParse(Latitude, out double lat); return lat; } set => Latitude = value.ToString("F6"); }
        [JsonIgnore]
        public double Lon { get { double.TryParse(Longitude, out double lon); return lon; } set => Longitude = value.ToString("F6"); }
        [JsonIgnore]
        public string[] Pics { get { return Pic2 == null ? new string[0] : Pic2.Split(';'); } }
        [JsonIgnore]
        public string FirstPic { get { return !String.IsNullOrWhiteSpace(Pic1) && Pic1[0] != '!' ? Pic1 : Pics.Length > 0 ? Pics[0] : ""; } }

        public bool HasTag(string tag) { return Tags.Contains(tag); }
        public bool InRectangle(double n, double w, double s, double e) { return Lat > s && Lat < n && Lon > w && Lon < e; }
    }
}
