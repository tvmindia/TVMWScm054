using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IICRExpensesBusiness
    {     
        object InsertUpdateICRExpenses(ICRExpenses ExpensesObj);
        List<ICRExpenses> GetAllICRExpenses(UA UA, string FromDate, string ToDate);
        ICRExpenses GetICRExpensesByID(UA UA, string ID);       
        string DeleteICRExpenses(string ID, UA ua);
        ICRExpenses GetOutStandingICRPayment(UA UA);
    }
}
