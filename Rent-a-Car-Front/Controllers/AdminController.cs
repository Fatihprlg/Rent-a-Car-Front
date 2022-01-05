using Rent_a_Car.Models.Concerets;
using Rent_a_Car.BusinessLogic.Concerets;
using Rent_a_Car.Conceretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Rent_a_Car_Front.Models;

namespace Rent_a_Car_Front.Controllers
{
    public class AdminController : Controller
    {
        public static bool Logged = false;
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
            if (Logged)
            {
                IList<Vehicle> vehicles = SelectAllVehicles();
                return View(vehicles);
            }
            else
            {
                return View("Login");
            }
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

        public ActionResult EditVehicle(int ID)
        {
            if (Logged)
            {
                return View();
            }
            else
            {
                return View("Login");
            }
        }
        public ActionResult SaveVehicleChanges(int ID)
        {
            try
            {
                Vehicle vehicle = new Vehicle
                {
                    ID = ID,
                    Resim = Request.Form["resim"].ToString(),
                    Plaka = Request.Form["plaka"].ToString(),
                    Marka = Request.Form["marka"].ToString(),
                    Model = Request.Form["model"].ToString(),
                    GunlukKMSinir = Convert.ToInt32(Request.Form["gunlukKM"]),
                    Kilometre = Convert.ToInt32(Request.Form["km"]),
                    YakitTuru = Request.Form["yakit"],
                    Aciklama = Request.Form["aciklama"],
                    KoltukSayisi = Convert.ToByte(Request.Form["koltuk"]),
                    BagajHacmi = Convert.ToInt32(Request.Form["bagaj"]),
                    GunlukFiyat = Convert.ToDouble(Request.Form["fiyat"]),
                    SanzimanTuru = Request.Form["vites"]
                };
                using (var logic = new VehicleLogic())
                {
                    logic.UpdateVehicle(vehicle);
                }
                return RedirectToAction("EditVehicle");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult NewVehicle()
        {
            return View();
        }
        public ActionResult AddVehicle() 
        {
            try
            {
                Vehicle vehicle = new Vehicle
                {
                    Resim = Request.Form["resim"].ToString(),
                    Plaka = Request.Form["plaka"].ToString(),
                    Marka = Request.Form["marka"].ToString(),
                    Model = Request.Form["model"].ToString(),
                    GunlukKMSinir = Convert.ToInt32(Request.Form["gunlukKM"]),
                    Kilometre = Convert.ToInt32(Request.Form["km"]),
                    YakitTuru = Request.Form["yakit"],
                    Aciklama = Request.Form["aciklama"],
                    KoltukSayisi = Convert.ToByte(Request.Form["koltuk"]),
                    BagajHacmi = Convert.ToInt32(Request.Form["bagaj"]),
                    GunlukFiyat = Convert.ToDouble(Request.Form["fiyat"]),
                    SanzimanTuru = Request.Form["vites"]
                };
                using (var logic = new VehicleLogic())
                {
                    logic.InsertVehicle(vehicle);
                }
                return RedirectToAction("Vehicles");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult Login()
        {
            try
            {
                bool response = false;
                if (Request.Form["admin-radio"] != null)
                {
                    if (Request.Form["admin-radio"].ToString().Equals("admin"))
                    {

                        using (var logic = new AdminLogic())
                        {
                            response = logic.LoginControl(Request.Form["email"].ToString(), Request.Form["pwd"].ToString());
                        }
                        Logged = response;
                    }
                    else if (Request.Form["admin-radio"].ToString().Equals("employee"))
                    {
                        using (var logic = new EmployeeLogic())
                        {
                            response = logic.LoginControl(Request.Form["email"].ToString(), Request.Form["pwd"].ToString());
                        }
                        Logged = response;
                    }
                    if (Logged) return RedirectToAction("AdminPanel");
                    else return RedirectToAction("Login", "Home");
                }
                else
                {
                    Response.Write("<script>alert('Giriş tipi belirleyiniz')</script>");
                    return RedirectToAction("Login", "Home"); 
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult FilterVehicles()
        {
            try
            {
                var vehicles = SelectAllVehicles();
                
                string marka = Request.Form["marka"];
                string plaka = Request.Form["plaka"];
                if (!string.IsNullOrEmpty(marka) && !string.IsNullOrWhiteSpace(marka))
                    vehicles = vehicles.Where(v => v.Marka.ToUpper() == marka.ToUpper()).ToList();
                if (!string.IsNullOrEmpty(plaka) && !string.IsNullOrWhiteSpace(plaka))
                    vehicles = vehicles.Where(v => v.Plaka.ToUpper() == plaka.ToUpper()).ToList();
                if ((!string.IsNullOrEmpty(marka) && !string.IsNullOrWhiteSpace(marka)) && (!string.IsNullOrEmpty(plaka) && !string.IsNullOrWhiteSpace(plaka)))
                    vehicles = vehicles.Where(v => (v.Marka.ToUpper() == marka.ToUpper()) && (v.Plaka.ToUpper() == plaka.ToUpper()) ).ToList();

                return View("Vehicles", vehicles);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private IList<Vehicle> SelectAllVehicles()
        {
            try
            {
                IList<Vehicle> response = new List<Vehicle>();
                using(var logic = new VehicleLogic())
                {
                    response = logic.SelectAllVehicles();
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private Customer SelectCustomerByID(int ID)
        {
            try
            {
                Customer response = new Customer();
                using (var logic = new CustomerLogic())
                {
                    response = logic.SelectCustomerById(ID);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }

}