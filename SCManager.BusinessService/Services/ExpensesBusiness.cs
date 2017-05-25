using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class ExpensesBusiness: IExpensesBusiness
    {
        private IExpensesRepository _expensesRepository;
        public ExpensesBusiness(IExpensesRepository expensesRepository)
        {
            _expensesRepository = expensesRepository;
        }

        public List<Expenses> GetAllExpenses(UA UA, string FromDate, string ToDate, bool showAllYN)
        {
            List<Expenses> expenseList = null;
            expenseList = _expensesRepository.GetAllExpenses(UA,FromDate,ToDate,showAllYN);
            return expenseList;
        }

        public List<ExpenseType> GetAllExpenseTypes(UA UA)
        {
            List<ExpenseType> expenseTypeList = null;
            expenseTypeList = _expensesRepository.GetAllExpenseTypes(UA);
            return expenseTypeList;
        }

        public Expenses GetExpensesByID(UA UA, string ID)
        {
          
            Expenses expenseObj = null;
            expenseObj = _expensesRepository.GetExpensesByID(UA,ID);
            return expenseObj;
        }

        public object InsertUpdateExpenses(Expenses ExpensesObj)
        {
            object result = null;
            try
            {
                if (ExpensesObj.ID == Guid.Empty)
                {
                    result = _expensesRepository.InsertExpenses(ExpensesObj);
                }
                else
                {
                    result = _expensesRepository.UpdateExpenses(ExpensesObj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


    }
}