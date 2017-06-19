using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ICRExpensesController : Controller
    {

        Const constObj = new Const();
        private IICRExpensesBusiness _ICRexpensesBusiness;

        public ICRExpensesController(IICRExpensesBusiness ICRexpensesBusiness)
        {
            _ICRexpensesBusiness = ICRexpensesBusiness;
          
        }


        // GET: ICRExpenses
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            UA ua = new UA();
            DateTime dt = ua.CurrentDatetime();
            ViewBag.fromdate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");
            // return View();ICROT
            ICRExpensesViewModel ICRexpenseViewModel = null;
            try
            {
                ICRexpenseViewModel = new ICRExpensesViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();   
                selectListItem = null; 
                CommonViewModel CVM = new CommonViewModel();
                selectListItem = CVM.PaymentModelist;
                ICRexpenseViewModel.PaymentModeList = selectListItem; 

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ICRexpenseViewModel);
        }

        #region InsertUpdateICRExpenses
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateICRExpenses(ICRExpensesViewModel expensesViewModelObj)
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

                    result = _ICRexpensesBusiness.InsertUpdateICRExpenses(Mapper.Map<ICRExpensesViewModel, ICRExpenses>(expensesViewModelObj));
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
        #endregion InsertUpdateICRExpenses

        #region GetAllICRExpenses
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllICRExpenses(string FromDate, string ToDate)
        {
            UA ua = new UA();
            List<ICRExpensesViewModel> ICRExpensesList = Mapper.Map<List<ICRExpenses>, List<ICRExpensesViewModel>>(_ICRexpensesBusiness.GetAllICRExpenses(ua, FromDate, ToDate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ICRExpensesList });

        }
        #endregion GetAllICRExpenses

        #region GetICRExpensesByID

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetICRExpensesByID(string ID)
        {
            UA ua = new UA();
            ICRExpensesViewModel ICRExpensesList = Mapper.Map<ICRExpenses, ICRExpensesViewModel>(_ICRexpensesBusiness.GetICRExpensesByID(ua, ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ICRExpensesList });
        }
        #endregion GetAllICRExpensesByID

        #region DeleteICRExpenses
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteICRExpenses(string ID)
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
                         status = _ICRexpensesBusiness.DeleteICRExpenses(ID, ua);
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
        #endregion DeleteICRExpenses

        #region GetOutStandingICRPayment
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetOutStandingICRPayment()
        {
            UA ua = new UA();
            ICRExpensesViewModel CreditNotesList = Mapper.Map<ICRExpenses, ICRExpensesViewModel>(_ICRexpensesBusiness.GetOutStandingICRPayment(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetOutStandingPayment

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
                    ToolboxViewModelObj.deletebtn.Title = "Delete ICR Payments";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new ICR Payments";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.DisableReason = "Not applicable for new ICR Payments";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save ICR Payments";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete ICR Payments";
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