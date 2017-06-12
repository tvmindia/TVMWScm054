using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IIssueToTechnicianRepository
    {
        List<IssueToTechnician> GetIssueSheets(string ID, string transferDate, UA UA);
        List<IssueToTechnician> InsertIssueToTechnician(IssueToTechnician issueToTechnician, Guid? empID, string issueDate, UA UA);
        List<IssueToTechnician> GetAllIssueToTechnician(string empID, string fromDate, string toDate, UA UA);
        string DeleteIssueToTechnician(string ID, UA ua);
        List<IssueToTechnician> UpdateIssueToTechnician(IssueToTechnician issueToTechnician, Guid? empID, string issueDate, UA UA);
    }
}
