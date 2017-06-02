﻿using SCManager.BusinessService.Contracts;
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
        private ICommonBusiness _commonBusiness;
        public ExpensesBusiness(IExpensesRepository expensesRepository, ICommonBusiness commonBusiness)
        {
            _expensesRepository = expensesRepository;
            _commonBusiness = commonBusiness;
        }

      

        public List<Expenses> GetAllExpenses(UA UA, string FromDate, string ToDate)
        {
            List<Expenses> expenseList = null;
            expenseList = _expensesRepository.GetAllExpenses(UA,FromDate,ToDate);
            if (expenseList != null)
            {
                foreach (Expenses EX in expenseList)
                {
                    SCManagerSettings settings = new SCManagerSettings();

                    if (EX.RefDate != null)
                        EX.DateFormatted = EX.RefDate.ToString(settings.dateformat);
                }

            }
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
        public string DeleteExpenses(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _expensesRepository.DeleteExpenses(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public Expenses GetOutStandingPayment(UA UA)
        {
            Expenses expenseObj = null;
            expenseObj = _expensesRepository.GetOutStandingPayment(UA);
            expenseObj.OutStandingPaymentFormatted = _commonBusiness.ConvertCurrency(expenseObj.OutStandingPayment, 2);
            return expenseObj;
        }
    }
}