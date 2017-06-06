using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IAssignBillBookRepository
    {
        List<AssignBillBook> GetAllBillBook(UA UA);
        object InsertBillBook(AssignBillBook assignBillBook);
        List<AssignBillBook> GeBillBookByID(UA UA, string ID);
        object UpdateBillBook(AssignBillBook assignBillBook);
        string DeleteBillBook(string ID, UA ua);
    }
}
