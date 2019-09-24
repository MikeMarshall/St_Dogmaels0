using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace St_Dogmaels.Models
{
    [XmlRoot("Walk")]
    public class Walk
    {

        [XmlElement("Name")]
        public string Name
        {
            get;
            set;
        }
        [XmlElement("Distance")]
        public string Distance { get; set; }
        [XmlElement("Time")]
        public string Time { get; set; }
        [XmlElement("Description")]
        public string Description { get; set; }
        [XmlElement("Start")]
        public string Start { get; set; }
        [XmlElement("Grade")]
        public string Grade { get; set; }
        [XmlArray("Steps")]
        [XmlArrayItem("Step")]
        public List<Step> Steps { get; set; }

    }

    public class Step
    {

        [XmlElement("Latitude")]
        public double Latitude { get; set; }
        [XmlElement("Longitude")]
        public double Longitude { get; set; }
        [XmlElement("Title")]
        public string Title { get; set; }
        [XmlElement("Subtitle")]
        public string Subtitle { get; set; }
    }
}
