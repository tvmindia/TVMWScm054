using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
  public interface IAssignBillBookBusiness
    {
        List<AssignBillBook> GetAllBillBook(UA UA);
        List<AssignBillBook> GeBillBookByID(UA UA, string ID);
        string DeleteBillBook(string ID, UA ua);
        object InsertUpdateBillBook(AssignBillBook assignBillBookObj);
    }
}
