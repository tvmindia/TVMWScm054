using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface ITaxBillEntryBusiness
    {       
        List<TaxBillEntry> GetAllTaxBillEntry(UA UA);
        TaxBillEntry GetTaxBillHeaderByID(Guid ID, UA ua);
        TaxBillEntry UpdateTaxBill(TaxBillEntry taxBillEntry, UA UA);
        TaxBillEntry GetTaxBill(Guid ID, UA UA);
        List<TaxBillEntry> GetAllFranchiseeDetail(UA UA);
        List<TaxBillEntry> GetCustomerInfoFromTaxBill(string taxBillIDs);
        TaxBillEntry MergeTaxBill(TaxBillEntry taxBillEntry, UA UA);
        List<Employees> GetTechnicianListForMergeTaxBill(string taxBillIDs);
    }
}
