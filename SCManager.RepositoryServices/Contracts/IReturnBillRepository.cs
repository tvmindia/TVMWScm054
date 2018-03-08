using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
  public interface IReturnBillRepository
    {
        List<ReturnBill> GetAllReturnBill(UA UA);
        List<ReturnBill> GetAllFranchiseeDetail(UA UA);
        List<ReturnBill> GetSupplierDetail(UA UA);
        List<ReturnBillDetail> GetMaterialsFromDefectiveDamaged(string TicketNo, string SCCode);
        ReturnBill InsertReturnBill(ReturnBill rtb, UA UA);
        ReturnBill UpdateReturnBill(ReturnBill rtb, UA UA);
        bool DeleteReturnBill(Guid ID, UA UA);
        bool DeleteReturnBillDetail(Guid ID, Guid HeaderID, UA UA);
        ReturnBill GetReturnBillHeader(Guid ID, UA UA);
        List<ReturnBillDetail> GetReturnBillDetail(Guid ID, UA UA);
        List<ReturnBill> GetAllTicketNo(UA UA);
        string ReturnDefectiveDamaged(string ID, UA ua,string TicketNo);        

    }
}
