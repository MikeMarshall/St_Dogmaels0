using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels.Models
{
    public enum MenuItemType
    {
        Home,
        Map,
        Walks,
        Poi,
        Facilities,
        Geology,
        Abbey,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
