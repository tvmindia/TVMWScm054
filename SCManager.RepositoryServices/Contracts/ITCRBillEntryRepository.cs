using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface ITCRBillEntryRepository
    {
        TCRBillEntry InsertTCRBillEntry(TCRBillEntry tCRBillEntry, UA UA);
        TCRBillEntry UpdateTCRBillEntry(TCRBillEntry tCRBillEntry, UA UA);
        List<TCRBillEntryDetail> GetTCRBillDetail(Guid ID, UA UA);
        List<TCRBillEntry> GetAllJobNo(UA UA);
        List<TCRBillEntry> GetAllTCRBillEntry(UA UA);
        TCRBillEntry GetTCRBillHeaderByID(Guid ID, UA ua);
        bool DeleteTCRBillDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteTCRBillEntry(Guid ID, UA UA);
    }
}
