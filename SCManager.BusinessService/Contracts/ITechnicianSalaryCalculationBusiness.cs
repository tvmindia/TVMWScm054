using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface ITechnicianSalaryCalculationBusiness
    {
        List<TechnicianSalary> GetTechniciansCalculatedSalary(string SCCode, string Month, string Year);
        List<TechnicianSalaryJobBreakUp> GetTechnicianJobCommissionBreakUp(string SCCode, Guid EmpID, Int16? Month, Int16? Year);
        List<TechnicianSalaryTCRBreakUp> GetTechnicianTCRCommissionBreakUp(string SCCode, Guid EmpID, Int16? Month, Int16? Year);
        List<TechnicianSalaryAMCBreakUp> GetTechnicianAMCCommissionBreakUp(string SCCode, Guid EmpID, Int16? Month, Int16? Year);
        List<TechnicianSalaryAdvanceBreakUp> GetTechnicianSalaryAdvanceBreakUp(string SCCode, Guid EmpID, Int16? Month, Int16? Year);
    }
}
