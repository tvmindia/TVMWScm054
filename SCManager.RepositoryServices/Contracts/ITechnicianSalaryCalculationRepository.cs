using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;
namespace SCManager.RepositoryServices.Contracts
{
    public interface ITechnicianSalaryCalculationRepository
    {
        List<TechnicianSalary> GetTechniciansCalculatedSalary(string SCCode,Int16? Month,Int16? Year);
        List<TechnicianSalaryJobBreakUp> GetTechnicianJobCommissionBreakUp(string SCCode,Guid EmpID, Int16? Month, Int16? Year);
    }
}
