using Rent_a_Car.Conceretes;
using Rent_a_Car.BusinessLogic.Concerets;
using Rent_a_Car.Models.Concerets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rent_a_Car_Front.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FastRent()
        {

            var pick = Convert.ToDateTime(Request.Form["pickUpDate"]).ToString("yyyy-MM-dd");
            var drop = Convert.ToDateTime(Request.Form["dropOffDate"]).ToString("yyyy-MM-dd");
            ViewBag.PickUpDate = pick;
            ViewBag.DropOffDate = drop;
            IList<Vehicle> vehicles = FilterVehicle(Convert.ToDateTime(pick), Convert.ToDateTime(drop));
            return View("Vehicles", vehicles);
        }
        public ActionResult VehicleFilter()
        {
            var pick = Convert.ToDateTime(Request.Form["pickUpDate"]);
            var drop = Convert.ToDateTime(Request.Form["dropOffDate"]);
            try
            {
                IList<Vehicle> vehicles = FilterVehicle(pick, drop);
                ViewBag.PickUpDate = pick.ToString("yyyy-MM-dd");
                ViewBag.DropOffDate = drop.ToString("yyyy-MM-dd");
                return View("Vehicles", vehicles);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Vehicles()
        {
            try
            {
                IList<Vehicle> vehicles = SelectAllVehicles();
                return View(vehicles);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult VehicleDetail(int ID)
        {
            try
            {
                Vehicle vehicle = SelectVehicleByID(ID);
                return View(vehicle);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult BookVehicle(int ID)
        {
            Vehicle vehicle = SelectVehicleByID(ID);
            ViewBag.amount = vehicle.GunlukFiyat;
            ViewBag.vehicleID = ID;
            return View();
        }

        public ActionResult BookRequest()
        {
            try
            {
                var pick = Convert.ToDateTime(Request.Form["pickUpDate"]);
                var drop = Convert.ToDateTime(Request.Form["dropOffDate"]);
                string TCKN = Request.Form["identity"].ToString();
                Customer customer;
                Vehicle vehicle = new Vehicle();
                bool avaliable = true;
                using (var vehicleLogic = new VehicleLogic())
                {
                    vehicle = SelectVehicleByID(int.Parse(Request.Form["vehicleID"]));
                }
                Rent rent = new Rent()
                {
                    AracID = int.Parse(Request.Form["vehicleID"]),
                    KiralamaBaslangici = pick,
                    KiralamaBitisi = drop,
                    BaslangicKM = vehicle.Kilometre,
                    AlinanUcret = Convert.ToInt32(drop.Subtract(pick).TotalDays) * vehicle.GunlukFiyat
                };
                using (var rentLogic = new RentLogic())
                {
                    var rents = rentLogic.SelecAllRents().Where(r => r.AracID == vehicle.ID);
                    if (rents.Count() != 0)
                    {
                        foreach (var r in rents)
                        {
                            if ((r.KiralamaBaslangici >= pick && r.KiralamaBitisi >= pick) || !(r.KiralamaBitisi >= drop && r.KiralamaBaslangici <= drop) || !(r.KiralamaBaslangici >= pick && r.KiralamaBitisi <= drop))
                            {
                                avaliable = false;
                                break;
                            }
                        }
                    }
                }
                if (!avaliable)
                {
                    Response.Write("<script>alert('Araç girilen tarihlerde rezervasyona uygun değil!')</script>");
                    return RedirectToAction("BookVehicle",new { ID = vehicle.ID });
                }
                else
                {
                    using (var customerLogic = new CustomerLogic())
                    {
                        customer = customerLogic.SelectCustomerByTCKN(TCKN);
                        if (customer == null)
                        {
                            customer = new Customer()
                            {
                                TC = TCKN,
                                Isim = Request.Form["name"].ToString(),
                                Soyisim = Request.Form["surname"].ToString(),
                                Telefon = Request.Form["phone"].ToString(),
                                Adres = Request.Form["adress"].ToString(),
                                Email = Request.Form["mail"].ToString(),
                                EhliyetYasi = byte.Parse(Request.Form["licenceAge"]),
                                Yas = Convert.ToByte(Math.Floor(DateTime.Now.Subtract(Convert.ToDateTime(Request.Form["bornDate"])).TotalDays / 365))
                            };
                        }
                    }
                    bool isSuccess = Book(customer, rent);
                    string response;
                    if (isSuccess) response = "başarılı bir şekilde gönderildi.";
                    else response = "gönderilirken bir hata oluştu, lütfen bilgileri kontrol edip tekrar deneyiniz.";
                    Response.Write("<script>alert('Rezervasyon isteği " + response + "')</script>");
                    return RedirectToAction("BookVehicle", new { ID = vehicle.ID });
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        private bool Book(Customer customer, Rent rent)
        {
            try
            {
                bool isSuccess;

                using (var customerLogic = new CustomerLogic())
                {
                    isSuccess = customerLogic.InsertCustomer(customer);
                    if (isSuccess)
                    {
                        rent.MusteriID = customerLogic.SelectCustomerByTCKN(customer.TC).ID;
                    }
                }
                if (isSuccess)
                    using (var logic = new RentLogic())
                    {
                        isSuccess = logic.InsertRent(rent);
                    }
                return isSuccess;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private Vehicle SelectVehicleByID(int id)
        {
            Vehicle response = null;
            try
            {
                using (var logic = new VehicleLogic())
                {
                    response = logic.SelectVehicleById(id);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IList<Vehicle> FilterVehicle(DateTime pickDate, DateTime dropDate)
        {
            IList<Vehicle> response = new List<Vehicle>();
            try
            {
                using (var logic = new VehicleLogic())
                {
                    response = logic.FilterVehiclesByDate(pickDate, dropDate);
                }
                return response;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private IList<Vehicle> SelectAllVehicles()
        {
            IList<Vehicle> response = new List<Vehicle>();
            try
            {
                using (var logic = new VehicleLogic())
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
    }
}