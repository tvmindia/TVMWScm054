using SCManager.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCManager.RepositoryServices.Contracts
{
    public interface IExpensesRepository
    {
        List<ExpenseType> GetAllExpenseTypes(UA UA);
        object InsertExpenses(Expenses ExpensesObj);
        object UpdateExpenses(Expenses ExpensesObj);
        List<Expenses> GetAllExpenses(UA UA, string FromDate, string ToDate, bool showAllYN);
        Expenses GetExpensesByID(UA UA, string ID);
        string DeleteExpenses(string ID, UA ua);
        Expenses GetOutStandingPayment(UA UA);

    }
}
