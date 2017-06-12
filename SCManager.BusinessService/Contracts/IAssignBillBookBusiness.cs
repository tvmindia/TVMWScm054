using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
        string BillBookRangeValidation(string seriesStart, string seriesEnd,string BillNo, string BillBookType, UA UA);
        DataSet GetMissingSerials(string seriesStart, string seriesEnd, string BillBookType, UA UA);
        string BillBookNumberValidation(UA UA, string ID, string billBookType);
    }
}
