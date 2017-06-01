using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IOfficeBillEntryRepository
    {
        List<OfficeBillEntry> GetAllOfficeBillHeader(UA UA);
        List<OfficeBillEntryDetail> GetOfficeBillDetail(Guid? ID, UA UA);
        OfficeBillEntry InsertOfficeBillEntry(OfficeBillEntry officeBillEntry, UA UA);
        OfficeBillEntry UpdateOfficeBillEntry(OfficeBillEntry officeBillEntry, UA UA);
        OfficeBillEntry GetOfficeBillHeaderByID(Guid ID, UA ua);
    }
}
