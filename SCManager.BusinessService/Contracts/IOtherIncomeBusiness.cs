using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IOtherIncomeBusiness
    {
        List<OtherIncome> GetAllOtherIncome(UA UA, string showAllYN);
        List<OtherIncome> GetAllIncomeType();
        object InsertUpdateOtherIncome(OtherIncome otherIncomeObj);
        List<OtherIncome> GetOtherIncomeByID(UA UA, string ID);
        List<OtherIncome> GetOtherIncomeBetweenDates(UA UA, string fromDate, string toDate);
        string DeleteOtherIncome(string ID, UA ua);
    }
}
