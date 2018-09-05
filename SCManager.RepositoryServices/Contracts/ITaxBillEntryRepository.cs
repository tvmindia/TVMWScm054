using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface ITaxBillEntryRepository
    {
        List<TaxBillEntry> GetAllTaxBillEntry(UA UA);
        TaxBillEntry GetTaxBillHeaderByID(Guid ID, UA ua);
        List<TaxBillEntryDetail> GetTaxBillDetail(Guid ID, UA UA);
        TaxBillEntry UpdateTaxBillEntry(TaxBillEntry taxBillEntry, UA UA);
        List<TaxBillEntry> GetAllFranchiseeDetail(UA UA);
        List<TaxBillEntry> GetCustomerInfoFromTaxBill(string taxBillIDs);
        TaxBillEntry MergeTaxBill(TaxBillEntry taxBillEntry, UA UA);
        List<Employees> GetTechnicianListForMergeTaxBill(string taxBillIDs);
    }
}
