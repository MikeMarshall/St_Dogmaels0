using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;

namespace St_Dogmaels.Models
{
    public enum MarkerType
    {
        Step,
        Poi,
        Facility,
        House,
        Landscape,
        Gardens,
        Search
        
    }
    public class Marker
    {
        public MarkerType Type { get; set; }
        public string Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Position Position  { get; set; }
        public SKPoint Point  { get; set; }

       
    }
}
