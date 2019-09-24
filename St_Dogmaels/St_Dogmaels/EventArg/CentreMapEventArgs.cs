using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels.EventArg
{
    public class CentreMapEventArgs : EventArgs
    {
        public CentreMapEventArgs(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
