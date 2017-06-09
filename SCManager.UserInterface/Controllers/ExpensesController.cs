using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ExpensesController : Controller
    {
        Const constObj = new Const();
        private IExpensesBusiness _expensesBusiness;
        IEmployeesBusiness _iEmployeesBusiness;
        ICommonBusiness _commonBusiness;

        public ExpensesController(IExpensesBusiness expensesBusiness, IEmployeesBusiness iEmployeesBusiness, ICommonBusiness commonBusiness)
        {
            _expensesBusiness = expensesBusiness;
            _iEmployeesBusiness = iEmployeesBusiness;
            _commonBusiness = commonBusiness;
        }
        // GET: Expenses
        public ActionResult Index()
        {
            UA ua = new UA();
            DateTime dt = ua.CurrentDatetime();
            ViewBag.fromdate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            // return View();
            ExpensesViewModel expenseViewModel = null;
            try
            {
                expenseViewModel = new ExpensesViewModel(); 
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                //Technician Drop down bind
                List<EmployeesViewModel> TechniciansList = Mapper.Map<List<Employees>, List<EmployeesViewModel>>(_iEmployeesBusiness.GetAllTechnicians(ua));
                TechniciansList = TechniciansList == null ? null : TechniciansList.OrderBy(attset => attset.Name).ToList();
                foreach (EmployeesViewModel clvm in TechniciansList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = clvm.Name,
                        Value = clvm.ID.ToString(),
                        Selected = false
                    });
                }
                expenseViewModel.TechniciansList = selectListItem;
                

                List<ExpenseTypeViewModel> ExpenseTypelist = Mapper.Map<List<ExpenseType>, List<ExpenseTypeViewModel>>(_expensesBusiness.GetAllExpenseTypes(ua));
                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                foreach (ExpenseTypeViewModel vm in ExpenseTypelist)
                {
                    if (vm.Code!="ICROT")
                    selectListItem.Add(new SelectListItem
                    {
                        Text = vm.Description,
                        Value = vm.Code
                    });
                }
                expenseViewModel.ExpenseTypeList = selectListItem;

                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                CommonViewModel CVM = new CommonViewModel();
                selectListItem = CVM.PaymentModelist;
                expenseViewModel.PaymentModeList = selectListItem;



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(expenseViewModel);
        }

        #region InsertUpdateExpenses
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateExpenses(ExpensesViewModel expensesViewModelObj)
        {
            object result = null;
            if (expensesViewModelObj.ID != Guid.Empty)
            {
                ModelState.Remove("CreditNoteNo");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    UA ua = new UA();
                    expensesViewModelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    expensesViewModelObj.logDetails.CreatedBy = ua.UserName;
                    expensesViewModelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    expensesViewModelObj.logDetails.UpdatedBy = expensesViewModelObj.logDetails.CreatedBy;
                    expensesViewModelObj.logDetails.UpdatedDate = expensesViewModelObj.logDetails.CreatedDate;
                    expensesViewModelObj.SCCode = ua.SCCode;

                    result = _expensesBusiness.InsertUpdateExpenses(Mapper.Map<ExpensesViewModel, Expenses>(expensesViewModelObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                  
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
                //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion InsertUpdateExpenses

        #region GetAllExpenses
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllExpenses(string FromDate,string ToDate)
        {
            UA ua = new UA();
            List<ExpensesViewModel> CreditNotesList = Mapper.Map<List<Expenses>, List<ExpensesViewModel>>(_expensesBusiness.GetAllExpenses(ua,FromDate,ToDate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetAllExpenses

        #region GetExpensesByID

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetExpensesByID(string ID)
        {
            UA ua = new UA();
            ExpensesViewModel CreditNotesList = Mapper.Map<Expenses,ExpensesViewModel>(_expensesBusiness.GetExpensesByID(ua,ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetAllExpenses

        #region GetOutStandingPayment
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetOutStandingPayment()
        {
            UA ua = new UA();
            ExpensesViewModel CreditNotesList = Mapper.Map<Expenses, ExpensesViewModel>(_expensesBusiness.GetOutStandingPayment(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetOutStandingPayment

        #region DeleteExpenses
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteExpenses(string ID)
        {
            string status = null;
            string msg = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    if (!string.IsNullOrEmpty(ID))
                    {
                        status = _expensesBusiness.DeleteExpenses(ID, ua);
                    }
                    switch (status)
                    {
                        case "0":
                            msg = constObj.DeleteFailure;
                            break;
                        case "1":
                            msg = constObj.DeleteSuccess;
                            break;
                        case "2":
                            msg = constObj.FKviolation;
                            break;
                    }
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = msg });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion DeleteExpenses

        #region GetTechnicianSalaryByTechnician
        /// <summary>
        /// Get Salary Summary of a Technicain  for the last month(last 30 days)
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>TechnicianSalaryViewModel</returns>
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetTechnicianSalaryByTechnician(string ID,string Date)
        {
            try
            {
            if(!string.IsNullOrEmpty(ID))
            {
                UA ua = new UA();

                TechnicianSalaryViewModel technicianSalaryVM = Mapper.Map<TechnicianSalary, TechnicianSalaryViewModel>(_expensesBusiness.GetTechnicianSalaryByTechnician(ua, Guid.Parse(ID),Date));
                if(technicianSalaryVM!=null)
                    {
                      
                 technicianSalaryVM.Period= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)technicianSalaryVM.Month)+"/"+ technicianSalaryVM.Year; 
                 technicianSalaryVM.TotalCommissionRupee = _commonBusiness.ConvertCurrency(technicianSalaryVM.TotalCommission);
                 technicianSalaryVM.AdvanceRupee= _commonBusiness.ConvertCurrency(technicianSalaryVM.SalaryAdvance);
                 technicianSalaryVM.PayableRupee = _commonBusiness.ConvertCurrency(technicianSalaryVM.TotalPayable);
                }
                return JsonConvert.SerializeObject(new { Result = "OK", Record = technicianSalaryVM });
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is Empty" });
            }
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }



        }
        #endregion GetTechnicianSalaryByTechnician

        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    break;

                case "Add":

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Expenses";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new Expenses";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.DisableReason = "Not applicable for new Expenses";
                    ToolboxViewModelObj.addbtn.Event = "Add();"; 

                    break;
                case "Edit": 
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Expenses";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Expenses";
                    ToolboxViewModelObj.deletebtn.Event = "Delete()";
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}