using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IIssueToTechnicianBusiness
    {
        List<IssueToTechnician> InsertUpdateIssueToTechnician(List<IssueToTechnician> issueToTechnician,Guid? empID,DateTime? issueDate, UA UA);
        List<IssueToTechnician> GetAllIssueToTechnician(string empID, string fromDate, string toDate, UA UA);
        List<IssueToTechnician> GetIssueSheets(string ID, string transferDate, UA UA);
        string DeleteIssueToTechnician(string ID, UA ua);
    }
}
