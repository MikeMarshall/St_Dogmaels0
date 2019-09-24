using System;
using System.Collections.Generic;
using System.Text;
using St_Dogmaels.Models;

namespace St_Dogmaels
{
    public static class Facilities
    {
        public static List<Facility> facilities = new List<Facility>
        {
            new Facility() { Name = "Argo Villa B&B", Detail = "A warm welcome from Gill & Antosh We are passionate walkers, who love the local environment and history. Argo Villa overlooks the River Teifi. It is ideal for hikers as it is just 100m from the Start of the Pembrokeshire Coast Path which links to our extensive local footpath network. Poppit Road, St Dogmaels Pembrokeshire. SA43 3LF +44 (0) 1239 613 031",  Image="fac1.png" },
            new Facility() { Name = "Oriel Milgi", Detail = "We believe it’s the details that make your stay special. With this in mind our 3 bedrooms have 100% pure French linen, beautifully soft and naturally hypoallergenic and the beds are dressed with traditional, antique Welsh wool blankets. All rooms feature contemporary wooden shutters and bespoke, rustic Welsh ash headboards.",  Image = "fac2.png" },
            new Facility() { Name = "Bethsaida B&B", Detail = "Welcome to Bethsaida B&B, a unique 4* graded accommodation in a redeveloped Baptist Chapel set in the beautiful and historic village of St Dogmaels, Pembrokeshire. Five beautifully furnished guest rooms, suitable for couples, families and solo travellers. Ideally located for the Coastal Paths of Pembrokeshire and Ceredigion, moments from Cardigan the home of the Eisteddfod as well as being surrounded by beautiful, award winning beaches. We actively support Welsh businesses and have taken every opportunity to source local goods and services to enhance your experience in this wonderful Welsh location. A perfect place to be active……..to relax……..to enjoy solitude…….or to be part of a group. We look forward to welcoming you soon.",  Image = "fac4.png" },
            new Facility() { Name = "Coach House", Detail = "Great cafe for tasy food and delicious coffee and other drinks. Art gallery and sales. Museum",  Image = "fac3.png" },         
            new Facility() { Name = "Premier Shop", Detail = "Great shop for wine, beer, bread and many other groceries. Newspapers and stationary.",  Image = "fac5.png" },
            new Facility() { Name = "Post Office", Detail = "Standard Post Office services plus various extra items. Birthday and othe cards.",  Image = "fac6.png" }
        };
    }
}
