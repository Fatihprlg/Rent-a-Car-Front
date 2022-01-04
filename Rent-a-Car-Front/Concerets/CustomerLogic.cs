using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rent_a_Car.Models.Concerets;
using Rent_a_Car.DataAccess.Conceretes;
using Rent_a_Car.Commons.Conceretes.Helpers;
using Rent_a_Car.Commons.Conceretes.Loggers;

namespace Rent_a_Car.BusinessLogic.Concerets
{
    public class CustomerLogic : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        
        public bool InsertCustomer(Customer entity)
        {
            try
            {
                bool isSuccess;
                using (var repo = new CustomerRepository())
                {
                    isSuccess = repo.Insert(entity);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:CustomerLogic::InsertCustomer::Error occured.", ex);
            }
        }
        public Customer SelectCustomerById(int customerId)
        {
            try
            {
                Customer responseEntitiy;
                using (var repo = new CustomerRepository())
                {
                    responseEntitiy = repo.SelectById(customerId);
                    if (responseEntitiy == null)
                        throw new NullReferenceException("Customer doesnt exists!");
                }
                return responseEntitiy;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("BusinessLogic:CustomerLogic::SelectCustomerById::Error occured.", ex);
            }
        }
    }
}
