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
