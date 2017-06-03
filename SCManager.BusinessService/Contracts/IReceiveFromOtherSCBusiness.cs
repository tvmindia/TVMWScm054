using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IReceiveFromOtherSCBusiness
    {
        bool DeleteOtherSCReceipt(Guid ID, UA UA);
        bool DeleteOtherScReceiptDetail(Guid ID, Guid HeaderID, UA UA);
        ReceiveFromOtherSC InsertUpdate(ReceiveFromOtherSC receiveFromOtherSC, UA UA);
        List<ReceiveFromOtherSC> GetAllOtherSCReceipt(UA UA);
        ReceiveFromOtherSC GetOtherSCReceiptByID(Guid ID, UA ua);
    }
}
