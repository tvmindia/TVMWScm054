using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
   public interface IOtherIncomeRepository
    {
        List<OtherIncome> GetAllOtherIncome(UA UA, string showAllYN);
        object InsertOtherIncome(OtherIncome otherIncomeObj);
        object UpdateOtherIncome(OtherIncome otherIncomeObj);
        List<OtherIncome> GetAllIncomeType();
        List<OtherIncome> GetOtherIncomeByID(UA UA, string ID);
        string DeleteOtherIncome(string ID, UA ua);
        List<OtherIncome> GetOtherIncomeBetweenDates(UA UA, string fromDate, string toDate);
    }
}
