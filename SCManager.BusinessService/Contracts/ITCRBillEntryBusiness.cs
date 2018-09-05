using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface ITCRBillEntryBusiness
    {
        TCRBillEntry InsertUpdate(TCRBillEntry tCRBillEntry, UA UA);
        List<TCRBillEntry> GetAllJobNo(UA UA);
        List<TCRBillEntry> GetAllTCRBillEntry(UA UA);
        TCRBillEntry GetTCRBillHeaderByID(Guid ID, UA ua);
        bool DeleteTCRBillDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteTCRBillEntry(Guid ID, UA UA);
        List<TCRBillEntry> GetAllTCRBillEntryForExport(UA UA);
        
    }
}
