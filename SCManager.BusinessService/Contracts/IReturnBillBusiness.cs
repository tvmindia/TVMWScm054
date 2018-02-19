using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
   public interface IReturnBillBusiness
    {
        List<ReturnBill> GetAllReturnBill(UA UA);
        List<ReturnBill> GetAllFranchiseeDetail(UA UA);
        List<ReturnBillDetail> GetMaterialsFromDefectiveDamaged(string TicketNo, string SCCode);
        ReturnBill InsertUpdate(ReturnBill rtb, UA UA);
        bool DeleteReturnBill(Guid ID, UA UA);
        bool DeleteReturnBillDetail(Guid ID, Guid HeaderID, UA UA);
        ReturnBill GetReturnBill(Guid ID, UA UA);
        List<ReturnBill> GetAllTicketNo(UA UA);
        string ReturnDefectiveDamaged(string HeaderID, UA ua,string TicketNo);        
    }
}
