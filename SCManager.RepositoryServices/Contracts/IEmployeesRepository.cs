using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IEmployeesRepository
    {
        List<Employees> GetAllEmployees(UA UA);
        List<Employees> GetEmployeeByID(UA UA, string ID);
        object InsertEmployee(Employees employeesObj);
        object UpdateEmployee(Employees employeesObj);
        string DeleteEmployee(string ID, UA ua);
    }
}
