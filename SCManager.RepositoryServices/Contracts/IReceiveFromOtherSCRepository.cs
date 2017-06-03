using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IReceiveFromOtherSCRepository
    {
        List<ReceiveFromOtherSC> GetAllOtherSCReceipt(UA UA);
        ReceiveFromOtherSC InsertOtherSCReceipt(ReceiveFromOtherSC receiveFromOtherSC, UA UA);
        ReceiveFromOtherSC UpdateOtherSCReceipt(ReceiveFromOtherSC receiveFromOtherSC, UA UA);
        List<ReceiveFromOtherScDetail> GetOtherScReceiptDetail(Guid ID, UA UA);
        ReceiveFromOtherSC GetOtherSCReceiptByID(Guid ID, UA ua);
        bool DeleteOtherScReceiptDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteOtherSCReceipt(Guid ID, UA UA);
    }
}
