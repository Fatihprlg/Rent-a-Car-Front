using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rent_a_Car_Front.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminPanel()
        {
            return View();
        }
        public ActionResult Reservations()
        {
            return View();
        }
        public ActionResult Vehicles()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult EditVehicle()
        {
            return View();
        }
    }
}