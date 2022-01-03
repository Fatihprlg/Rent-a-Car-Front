using Rent_a_Car.Commons.Conceretes.Helpers;
using Rent_a_Car.Commons.Conceretes.Loggers;
using Rent_a_Car.DataAccess.Conceretes;
using Rent_a_Car.Models.Concerets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rent_a_Car.Conceretes
{
    class EmployeeLogic : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        public Employee SelectEmployeeById(int id)
        {
            try
            {
                Employee responseEntitiy;
                using (var repo = new EmployeeRepository())
                {
                    responseEntitiy = repo.SelectById(id);
                    if (responseEntitiy == null)
                        throw new NullReferenceException("Employee doesn't exists!");
                }
                return responseEntitiy;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:EmployeeLogic::SelectEmployeeById::Error occured.", ex);
            }
        }

    }
}
