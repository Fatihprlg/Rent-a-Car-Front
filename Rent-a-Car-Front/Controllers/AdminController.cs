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
            if (Logged)
            {
                IDictionary<int, int> rentCounts = new Dictionary<int, int>();
               
                using(var logic = new RentLogic())
                {
                    rentCounts = logic.GetRentCountsByMonths();
                }
                return View(rentCounts);
            }
            else
            {
                return View("Login");
            }
        }
        public ActionResult Reservations()
        {
            if (Logged)
            {
                IList<RentInfo> rentInfos = new List<RentInfo>();
                IList<Rent> rents = new List<Rent>();
                IList<Vehicle> vehicles = SelectAllVehicles();
                using (var logic = new RentLogic())
                {
                    rents = logic.SelecAllRents();
                    foreach(var r in rents)
                    {
                        Vehicle v = vehicles.Where(x => x.ID == r.AracID).FirstOrDefault();
                        Customer c = SelectCustomerByID(r.MusteriID);
                        RentInfo temp = new RentInfo()
                        {
                            RentID = r.IslemID,
                            VehiclePlateNum = v.Plaka,
                            VehicleModel = v.Model,
                            Picture = v.Resim,
                            CustomerEmail = c.Email,
                            StartDate = r.KiralamaBaslangici,
                            EndDate = r.KiralamaBitisi,
                            TotalPrice = r.AlinanUcret,
                            CustomerAge = c.Yas,
                            LicenceAge = c.EhliyetYasi,
                            CustomerFullName = c.Isim + " " + c.Soyisim
                        };
                        if (r.Durum == null) temp.State = "Bekliyor";
                        else if (r.Durum == true) temp.State = "Onaylandı";
                        else temp.State = "Reddedildi";
                        rentInfos.Add(temp);
                    }

                }
                return View(rentInfos);
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult AcceptRequest(int id)
        {
            try
            {
                bool isSuccess;
                using (var logic = new RentLogic())
                {
                    Rent rent = new Rent()
                    {
                        IslemID = id,
                        Durum = true
                    };
                    isSuccess = logic.UpdateRent(rent);
                }
                return RedirectToAction("Reservations");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult RejectRequest(int id)
        {
            try
            {
                using (var logic = new RentLogic())
                {
                    Rent rent = new Rent()
                    {
                        IslemID = id,
                        Durum = true
                    };
                    logic.UpdateRent(rent);
                }
                return RedirectToAction("Reservations");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult Vehicles()
        {
            return View();
        }
         public ActionResult Users()
        {
            if (Logged)
            {
                try
                {
                    IList<Employee> employees;
                    using (var logic = new EmployeeLogic())
                    {
                        employees = logic.SelectAllEmployees();
                    }
                    return View(employees);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return View("Login");
            }
        }
        public ActionResult DeleteUser(int id)
        {
            try
            {
                bool isSuccess;
                using (var logic = new EmployeeLogic())
                {
                    isSuccess = logic.DeleteEmployeeById(id);
                }
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult InsertUser()
        {
            try
            {
                bool isSuccess;
                using (var logic = new EmployeeLogic())
                {
                    Employee temp = new Employee()
                    {
                        Isim = Request.Form["isim"],
                        Soyisim = Request.Form["soyisim"],
                        Telefon = Request.Form["telefon"],
                        Email = Request.Form["mail"],
                        Sifre = Request.Form["sifre"],
                        TC = Request.Form["tc"]
                    };
                    isSuccess = logic.InsertEmployee(temp);
                }
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult SaveUserChanges()
        {
            try
            {
                bool isSuccess;
                using (var logic = new EmployeeLogic())
                {
                    Employee temp = new Employee()
                    {
                        ID = int.Parse(Request.Form["ID"]),
                        Isim = Request.Form["isim"],
                        Soyisim = Request.Form["soyisim"],
                        Telefon = Request.Form["telefon"],
                        Email = Request.Form["mail"],
                        Sifre = Request.Form["sifre"],
                        TC = Request.Form["tc"]
                    };
                    isSuccess = logic.UpdateEmployee(temp);
                }
                return RedirectToAction("Users");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult FilterUsers()
        {
            if (Logged)
            {
                try
                {
                    var id = Request.Form["id"];
                    var isim = Request.Form["isim"];
                    IList<Employee> employees = new List<Employee>();
                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                    {
                        using (var logic = new EmployeeLogic())
                        {
                            employees.Add(logic.SelectEmployeeById(int.Parse(id)));
                        }
                    }
                    if(!string.IsNullOrEmpty(isim) && !string.IsNullOrWhiteSpace(isim))
                    {
                        using (var logic = new EmployeeLogic())
                        {
                            var temp = logic.SelectAllEmployees().Where(x => x.Isim == isim);
                            foreach (var a in temp)
                            {
                                employees.Add(a);
                            }
                        }
                    }
                    return View("Users",employees);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult EditVehicle()
        {
            return View();
        }
    }
}
