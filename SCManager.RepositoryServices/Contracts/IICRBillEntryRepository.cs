using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IICRBillEntryRepository
    {
        List<ICRBillEntry> GetAllICRBillEntry(UA UA);
        ICRBillEntry InsertICRBillEntry(ICRBillEntry iCRBillEntry, UA UA);
        ICRBillEntry UpdateICRBillEntry(ICRBillEntry iCRBillEntry, UA UA);
        List<ICRBillEntryDetail> GetICRBillDetail(Guid? ID, UA UA);
        ICRBillEntry GetICRBillHeaderByID(Guid ID, UA ua);
        bool DeleteICRBillDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteICRBillEntry(Guid ID, UA UA);
        List<ICRBillEntry> GetAllICRBillEntryForExport(UA UA);
    }
}
