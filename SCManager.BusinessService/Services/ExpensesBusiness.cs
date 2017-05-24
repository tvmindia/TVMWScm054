using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.BusinessService.Services
{
    public class ExpensesBusiness: IExpensesBusiness
    {
        private IExpensesRepository _expensesRepository;
        public ExpensesBusiness(IExpensesRepository expensesRepository)
        {
            _expensesRepository = expensesRepository;
        }
    }
}