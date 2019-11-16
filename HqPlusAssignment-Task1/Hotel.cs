using System;
using System.Collections.Generic;
using System.Text;

namespace HqPlusAssignment_Task1
{
    public class Hotel
    {
        public string BookingPageUrl { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int? Classification { get; set; }
        public float? ReviewPoints { get; set; }
        public int? NumberOfReviews { get; set; }
        public List<string> RoomCategories { get; set; }
        public List<Hotel> AlternativeHotels { get; set; }
    }
}
