using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent_a_Car_Front.Models
{
    public class VehicleFilter
    {
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public string PickUpDate { get; set; }
        public string DropOffDate { get; set; }
        public string PickUpTime { get; set; }
    }
}