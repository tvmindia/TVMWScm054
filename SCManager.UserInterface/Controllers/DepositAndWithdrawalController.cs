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
    public class DepositAndWithdrawalController : Controller
    {
        private IDepositAndWithdrawalBusiness _depositAndWithdrawalBusiness;
        public DepositAndWithdrawalController(IDepositAndWithdrawalBusiness depositAndWithdrawalBusiness)
        {
            _depositAndWithdrawalBusiness = depositAndWithdrawalBusiness;
        }
        // GET: DepositAndWithdrawal
        public ActionResult Index()
        {
            UA ua = new UA();
            DepositAndWithdrawalViewModel depositAndWithdrawalViewModel = new DepositAndWithdrawalViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = "Deposit",
                Value = "DEPST",
                Selected = false
            });
            selectListItem.Add(new SelectListItem
                    {
                        Text = "Withdrawal",
                        Value = "WITDR",
                        Selected = false
                    });
            depositAndWithdrawalViewModel.TransactionTypeList = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = "Cash",
                Value = "Cash",
                Selected = false
            });
            selectListItem.Add(new SelectListItem
            {
                Text = "Cheque",
                Value = "Cheque",
                Selected = false
            });
            depositAndWithdrawalViewModel.DepositModeList = selectListItem;

            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem
            {
                Text = "Cleared",
                Value = "Cleared",
                Selected = false
            });
            selectListItem.Add(new SelectListItem
            {
                Text = "Not Cleared",
                Value = "NotCleared",
                Selected = false
            });
            depositAndWithdrawalViewModel.ChequeStatusList = selectListItem;


            DateTime dt = ua.CurrentDatetime();
            ViewBag.fromdate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
            ViewBag.todate = dt.ToString("dd-MMM-yyyy");

            return View(depositAndWithdrawalViewModel);
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllDepositAndWithdrawal()
        {
            try
            {
                UA ua = new UA();
                List<DepositAndWithdrawalViewModel> DepowithList = Mapper.Map<List<DepositAndWithdrawal>, List<DepositAndWithdrawalViewModel>>(_depositAndWithdrawalBusiness.GetAllDepositAndWithdrawal(ua.SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = DepowithList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetDepositAndWithdrawalEntryByID(string ID)
        {
            try
            {
                if(!string.IsNullOrEmpty(ID))
                {
                    UA ua = new UA();
                    DepositAndWithdrawalViewModel Depowith = Mapper.Map<DepositAndWithdrawal, DepositAndWithdrawalViewModel>(_depositAndWithdrawalBusiness.GetDepositAndWithdrawalEntryByID(ua.SCCode,Guid.Parse(ID)));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = Depowith });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is Empty" });
                }
               
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

     

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllDepositAndWithdrawalBetweenDates(string Fromdate,string Todate)
        {
            try
            {
                UA ua = new UA();
                List<DepositAndWithdrawalViewModel> DepowithList = Mapper.Map<List<DepositAndWithdrawal>, List<DepositAndWithdrawalViewModel>>(_depositAndWithdrawalBusiness.GetAllDepositAndWithdrawalBetweenDates(ua.SCCode,Fromdate,Todate));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = DepowithList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region InsertUpdateDepositAndWithdrawal
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateDepositAndWithdrawal(DepositAndWithdrawalViewModel depositAndWithdrawalViewModel)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    UA ua = new UA();
                    depositAndWithdrawalViewModel.SCCode = ua.SCCode;
                    depositAndWithdrawalViewModel.logDetails = new LogDetailsViewModel();
                    depositAndWithdrawalViewModel.logDetails.CreatedBy = ua.UserName;
                    depositAndWithdrawalViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    depositAndWithdrawalViewModel.logDetails.UpdatedBy = ua.UserName;
                    depositAndWithdrawalViewModel.logDetails.UpdatedDate = ua.CurrentDatetime();
                 
                    switch (depositAndWithdrawalViewModel.ID != null)
                    {
                        case true:
                            result = _depositAndWithdrawalBusiness.UpdateDepositAndWithdrawal(Mapper.Map<DepositAndWithdrawalViewModel, DepositAndWithdrawal>(depositAndWithdrawalViewModel));
                            break;
                        default:
                            result = _depositAndWithdrawalBusiness.InsertDepositAndWithdrawal(Mapper.Map<DepositAndWithdrawalViewModel, DepositAndWithdrawal>(depositAndWithdrawalViewModel));
                            break;
                    }


                    return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            //Model state errror
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
            }
        }
        #endregion InsertUpdateDepositAndWithdrawal


        #region DeleteDepositAndWithdrawal
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteDepositAndWithdrawal(DepositAndWithdrawalViewModel depositAndWithdrawalViewModel)
        {
               object result = null;
               try
                {
                    UA ua = new UA();
                    depositAndWithdrawalViewModel.SCCode = ua.SCCode;
                depositAndWithdrawalViewModel.logDetails = new LogDetailsViewModel();
                    depositAndWithdrawalViewModel.logDetails.UpdatedBy = ua.UserName;
                    result = _depositAndWithdrawalBusiness.DeleteDepositAndWithdrawal(Mapper.Map<DepositAndWithdrawalViewModel, DepositAndWithdrawal>(depositAndWithdrawalViewModel));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
         }
        #endregion DeleteDepositAndWithdrawal

        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "Add":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add";
                    ToolboxViewModelObj.addbtn.Event = "AddDepositandwithdrawal();";
                    break;
                case "EditSave":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddDepositandwithdrawal();";
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Entry";
                    ToolboxViewModelObj.savebtn.Event = "SaveDepositandwithdrawal();";
                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Entry";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteDepositandwithdrawal();";
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    break;
                case "Save":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Entry";
                    ToolboxViewModelObj.savebtn.Event = "SaveDepositandwithdrawal();";
                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Entry";
                    ToolboxViewModelObj.deletebtn.DisableReason = "Not applicable for new Entries";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteDepositandwithdrawal();";
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.DisableReason = "Not applicable for new Entries";
                    ToolboxViewModelObj.addbtn.Event = "AddDepositandwithdrawal();";
                    break;

                case "AfterSave":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Entry";
                    ToolboxViewModelObj.savebtn.Event = "SaveDepositandwithdrawal();";
                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Entry";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteDepositandwithdrawal();";
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "AddDepositandwithdrawal();";
                    break;
                

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion

    }
}