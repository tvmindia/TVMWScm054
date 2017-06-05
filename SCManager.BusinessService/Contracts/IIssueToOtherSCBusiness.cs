using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IIssueToOtherSCBusiness
    {
        bool DeleteIssueToOtherSC(Guid ID, UA UA);
        bool DeleteIssueToOtherSCDetail(Guid ID, Guid HeaderID, UA UA);
        IssueToOtherSC InsertUpdate(IssueToOtherSC issueToOtherSC, UA UA);
        List<IssueToOtherSC> GetAllIssueToOtherSC(UA UA);
        IssueToOtherSC GetIssueToOtherSCByID(Guid ID, UA ua);
    }
}
