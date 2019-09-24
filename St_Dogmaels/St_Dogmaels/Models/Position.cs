using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels.Models
{
    public class Position
    {
        public Position() { }
        public Position(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
