using Rent_a_Car.Models.Concerets;
using Rent_a_Car.DataAccess.Conceretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rent_a_Car.Commons.Conceretes.Helpers;
using Rent_a_Car.Commons.Conceretes.Loggers;

namespace Rent_a_Car.Conceretes
{
    public class VehicleLogic : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public bool InsertVehicle(Vehicle entity)
        {
            try
            {
                bool isSuccess;
                using (var repo = new VehicleRepository())
                {
                    isSuccess = repo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:VehicleLogic::InsertVehicle::Error occured.", ex);
            }
        }

        public IList<Vehicle> SelectAllVehicles()
        {
            IList<Vehicle> vehicles = new List<Vehicle>();
            try
            {
                using(var repo = new VehicleRepository())
                {
                    vehicles = repo.SelectAll();
                }
                return vehicles;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:VehicleLogic::SelectAllVehicles::Error occured.", ex);
            }
        }

        public Vehicle SelectVehicleById(int id)
        {
            Vehicle vehicle = new Vehicle();
            try
            {
                using(var repo = new VehicleRepository())
                {
                    vehicle = repo.SelectById(id);
                    if(vehicle == null)
                        throw new NullReferenceException("Vehicle doesn't exists!");
                }
                return vehicle;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:VehicleLogic::SelectVehicleById::Error occured.", ex);
            }
        }

        public bool DeleteVehicleById(int id)
        {
            try
            {
                bool isSuccess;
                using (var repo = new VehicleRepository())
                {
                    isSuccess = repo.DeleteById(id);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:VehicleLogic::DeleteVehicleById::Error occured.", ex);
            }
        }


    }
}
