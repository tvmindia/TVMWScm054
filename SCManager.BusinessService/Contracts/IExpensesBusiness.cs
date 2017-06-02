using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.BusinessService.Contracts
{
    public interface IExpensesBusiness
    {

        List<ExpenseType> GetAllExpenseTypes(UA UA);
        object InsertUpdateExpenses(Expenses ExpensesObj);
        List<Expenses> GetAllExpenses(UA UA,string FromDate, string ToDate);
        Expenses GetExpensesByID(UA UA,string ID);
        Expenses GetOutStandingPayment(UA UA);
        string DeleteExpenses(string ID, UA ua);
    }
}
