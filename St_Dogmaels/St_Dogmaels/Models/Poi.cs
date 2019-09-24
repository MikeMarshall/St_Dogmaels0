using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace St_Dogmaels.Models
{
    [XmlRoot("Poi")]
    public class Poi
    {

        [XmlElement("Title")]
        public string Title
        {
            get;
            set;
        }

        [XmlElement("Image")]
        public string Image { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }


    }
}
