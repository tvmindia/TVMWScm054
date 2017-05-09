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
    }
}
