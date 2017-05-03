using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;

namespace SCManager.RepositoryServices.Contracts
{
    public interface ILocalPurchaseRepository
    {
        List<LocalPurchase> GetAllLocalPurchase(UA UA);
        LocalPurchase InsertLocalPurchase(LocalPurchase LP, UA UA);
        LocalPurchase UpdateLocalPurchase(LocalPurchase LP, UA UA);
        LocalPurchase GetLocalPurchaseHeader(Guid ID, UA UA);
        List<LocalPurchaseDetail> GetLocalPurchaseDetail(Guid ID, UA UA);
        bool DeleteLocalPurchase(Guid ID, UA UA);
        bool DeleteLocalPurchaseDetail(Guid ID, Guid HeaderID, UA UA);
    }
}
