using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IEmployeesBusiness
    {
        List<Employees> GetAllEmployees(UA UA);
        List<Employees> GetEmployeeByID(UA UA, string ID);
        object InsertUpdateEmployee(Employees employeesObj);
        string DeleteEmployee(string ID, UA ua);
        List<Employees> GetAllTechnicians(UA UA);
    }
}
