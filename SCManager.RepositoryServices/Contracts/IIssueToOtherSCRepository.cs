using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IIssueToOtherSCRepository
    {
        List<IssueToOtherSC> GetAllIssueToOtherSC(UA UA);
        IssueToOtherSC InsertIssueToOtherSC(IssueToOtherSC issueToOtherSC, UA UA);
        IssueToOtherSC UpdateIssueToOtherSC(IssueToOtherSC issueToOtherSC, UA UA);
        List<IssueToOtherScDetail> GetIssueToOtherScDetail(Guid ID, UA UA);
        IssueToOtherSC GetIssueToOtherSCByID(Guid ID, UA ua);
        bool DeleteIssueToOtherSCDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteIssueToOtherSC(Guid ID, UA UA);
    }
}
