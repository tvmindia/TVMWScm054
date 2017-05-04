using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCManager.DataAccessObject.DTO;
namespace SCManager.BusinessService.Contracts
{
    public interface ILocalPurchaseBusiness
    {
        List<LocalPurchase> GetAllLocalPurchase(UA UA);
        LocalPurchase InsertUpdate(LocalPurchase LP, UA UA);
        bool DeleteLocalPurchaseDetail(Guid ID, Guid HeaderID, UA UA);
        bool DeleteLocalPurchase(Guid ID, UA UA);
        LocalPurchase GetLocalPurchase(Guid ID, UA ua);

    }
}
