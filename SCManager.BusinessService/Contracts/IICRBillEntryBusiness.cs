using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public  interface IICRBillEntryBusiness
    {
        List<ICRBillEntry> GetAllICRBillEntry(UA UA);
        ICRBillEntry GetICRBillHeaderByID(Guid ID, UA ua);
        ICRBillEntry InsertUpdate(ICRBillEntry iCRBillEntry, UA UA);
        bool DeleteICRBillDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteICRBillEntry(Guid ID, UA UA);
    }
}
