using SCManager.BusinessService.Contracts;
using SCManager.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCManager.DataAccessObject.DTO;

namespace SCManager.BusinessService.Services
{
    public class ICRExpensesBusiness: IICRExpensesBusiness
    {

        private IICRExpensesRepository _iicrexpensesRepository;
        public ICRExpensesBusiness(IICRExpensesRepository icrexpensesRepository)
        {
            _iicrexpensesRepository = icrexpensesRepository;
        }

        public string DeleteICRExpenses(string ID, UA ua)
        {
            string status = null;
            try
            {
                status = _iicrexpensesRepository.DeleteICRExpenses(ID, ua);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;

        }

        public List<ICRExpenses> GetAllICRExpenses(UA UA, string FromDate, string ToDate, bool showAllYN)
        {
            List<ICRExpenses> expenseList = null;
            expenseList = _iicrexpensesRepository.GetAllICRExpenses(UA, FromDate, ToDate, showAllYN);
            if (expenseList != null)
            {
                foreach (ICRExpenses EX in expenseList)
                {
                    SCManagerSettings settings = new SCManagerSettings();
                    if (EX.RefDate != null)
                        EX.DateFormatted = EX.RefDate.ToString(settings.dateformat);
                }
            }
            return expenseList;

        }

        public ICRExpenses GetICRExpensesByID(UA UA, string ID)
        {
            ICRExpenses expenseObj = null;
            expenseObj = _iicrexpensesRepository.GetICRExpensesByID(UA, ID);
            return expenseObj;
        }

        public object InsertUpdateICRExpenses(ICRExpenses ExpensesObj)
        {
            object result = null;
            try
            {
                if (ExpensesObj.ID == Guid.Empty)
                {
                    result = _iicrexpensesRepository.InsertICRExpenses(ExpensesObj);
                }
                else
                {
                    result = _iicrexpensesRepository.UpdateICRExpenses(ExpensesObj);
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