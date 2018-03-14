using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class EmployeesBusiness : IEmployeesBusiness
    {
        private IEmployeesRepository _iEmployeesRepository;
        public EmployeesBusiness(IEmployeesRepository iEmployeesRepository)
        {
            _iEmployeesRepository = iEmployeesRepository;
        }

        public List<Employees> GetAllEmployees(UA UA,string filter)
        {
            List<Employees> employeesList = null;
            employeesList = _iEmployeesRepository.GetAllEmployees(UA,filter);
            return employeesList;
        }
        public List<Employees> GetEmployeeByID(UA UA, string ID)
        {
            List<Employees> employeesList = null;
            employeesList = _iEmployeesRepository.GetEmployeeByID(UA,ID);
            return employeesList;
        }
        public List<Employees> GetAllTechnicians(UA UA)
        {
            List<Employees> employeesList = null;
            employeesList = _iEmployeesRepository.GetAllTechnicians(UA);
            return employeesList;
        }
        public object InsertUpdateEmployee(Employees employeesObj)
        {
            object result = null;
            try
            {
                if(employeesObj.ID==Guid.Empty)
                {
                    result = _iEmployeesRepository.InsertEmployee(employeesObj);
                }
                else
                {
                    result= _iEmployeesRepository.UpdateEmployee(employeesObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string DeleteEmployee(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iEmployeesRepository.DeleteEmployee(ID,ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}