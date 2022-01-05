using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent_a_Car_Front.Models
{
    public class RentInfo
    {
        public int RentID { get; set; }
        public string Picture { get; set; }
        public string VehiclePlateNum { get; set; }
        public string VehicleModel { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFullName { get; set; }
        public string State { get; set; }
        public byte LicenceAge { get; set; }
        public byte CustomerAge { get; set; }
        public double TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}