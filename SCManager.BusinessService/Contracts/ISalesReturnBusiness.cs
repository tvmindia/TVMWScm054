using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface ISalesReturnBusiness
    {
        List<SalesReturn> GetAllSalesReturn(UA UA);
        string SalesReturnValidation(string itemID, UA ua);
        object InsertUpdateSalesReturn(SalesReturn salesReturnObj);
        List<SalesReturn> GetSalesReturnByID(UA UA, string ID);
        string DeleteSalesReturn(string ID, UA ua);
        string ReturnSalesToCompany(string ID, UA ua);
    }
}
