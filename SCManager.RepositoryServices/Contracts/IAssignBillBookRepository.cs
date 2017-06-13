using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
        string BillBookRangeValidation(string seriesStart, string seriesEnd,string BillNo, string BillBookType, UA UA);
        DataSet GetMissingSerials(string seriesStart, string seriesEnd, string BillBookType, UA UA);
        string BillBookNumberValidation(UA UA, string BillNo, string billBookType, string empID);
    }
}
