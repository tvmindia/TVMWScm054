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
        private ICommonBusiness _commonBusiness;
        private ITechnicianSalaryCalculationBusiness _technicianSalaryCalculationBusiness;
        public ExpensesBusiness(IExpensesRepository expensesRepository, ICommonBusiness commonBusiness, ITechnicianSalaryCalculationBusiness technicianSalaryCalculationBusiness)
        {
            _expensesRepository = expensesRepository;
            _commonBusiness = commonBusiness;
            _technicianSalaryCalculationBusiness = technicianSalaryCalculationBusiness;
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
                        EX.DateFormatted = EX.RefDate;//.ToString(settings.dateformat);
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

        public TechnicianSalary GetTechnicianSalaryByTechnician(UA ua, Guid ID,string date)
        {
            List<TechnicianSalary> technicianSalaryList = null;
            try
            {
                string m;
                string y;
                if (string.IsNullOrEmpty(date))
                {
                    DateTime dt = ua.CurrentDatetime();
                    m = dt.AddMonths(-1).Month.ToString();
                    y = dt.AddMonths(-1).Year.ToString();
                }
                else
                {
                    m = DateTime.Parse(date).AddMonths(-1).Month.ToString();
                    y= DateTime.Parse(date).AddMonths(-1).Year.ToString();
                }
                technicianSalaryList = _technicianSalaryCalculationBusiness.GetTechniciansCalculatedSalary(ua.SCCode, m,y);
                technicianSalaryList=technicianSalaryList != null && technicianSalaryList.Count > 0 ? technicianSalaryList.Where(t => t.EmpID == ID).ToList():null;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return technicianSalaryList != null && technicianSalaryList.Count > 0 ? technicianSalaryList[0] : null;
        }
    }
}