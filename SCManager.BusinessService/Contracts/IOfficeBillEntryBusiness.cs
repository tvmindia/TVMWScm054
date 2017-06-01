using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IOfficeBillEntryBusiness
    {
        List<OfficeBillEntry> GetAllOfficeBillEntry(UA UA);
        OfficeBillEntry InsertUpdate(OfficeBillEntry officeBillEntry, UA UA);
        OfficeBillEntry GetOfficeBillHeaderByID(Guid ID, UA ua);
        bool DeleteOfficeBillEntry(Guid ID, UA UA);
        bool DeleteOfficeBillDetail(Guid ID, Guid HeaderID, UA UA);
    }
}
