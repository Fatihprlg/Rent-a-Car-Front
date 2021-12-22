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
    }
}