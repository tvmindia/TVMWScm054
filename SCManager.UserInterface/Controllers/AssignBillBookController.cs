using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class AssignBillBookController : Controller
    {
        Const c = new Const();
        #region Constructor_Injection
       
        IEmployeesBusiness _iEmployeesBusiness;
        IAssignBillBookBusiness _iAssignBillBookBusiness;


        public AssignBillBookController( IEmployeesBusiness iEmployeesBusiness, IAssignBillBookBusiness iAssignBillBookBusiness)
        {          
            _iEmployeesBusiness = iEmployeesBusiness;
            _iAssignBillBookBusiness = iAssignBillBookBusiness;
         
        }
        #endregion Constructor_Injection
        // GET: AssignBillBook
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            AssignBillBookViewModel assignBillBookViewModel = null;
            try
            {
                assignBillBookViewModel = new AssignBillBookViewModel();
                UA ua = new UA();
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
                assignBillBookViewModel.TechniciansList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(assignBillBookViewModel);
        }


        #region GetAllBillBook
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllBillBook()
        {
            UA ua = new UA();
            List<AssignBillBookViewModel> CreditNotesList = Mapper.Map<List<AssignBillBook>, List<AssignBillBookViewModel>>(_iAssignBillBookBusiness.GetAllBillBook(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = CreditNotesList });

        }
        #endregion GetAllBillBook

        #region GeBillBookByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GeBillBookByID(string ID)
        {
            UA ua = new UA();
            List<AssignBillBookViewModel> BillBookList = Mapper.Map<List<AssignBillBook>, List<AssignBillBookViewModel>>(_iAssignBillBookBusiness.GeBillBookByID(ua, ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = BillBookList });

        }
        #endregion GeBillBookByID

        #region BillBookNumberValidation
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string BillBookNumberValidation(string BillNo, string billBookType,string empID)
        {
            UA ua = new UA();
            object result = _iAssignBillBookBusiness.BillBookNumberValidation(ua, BillNo,billBookType,empID);
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

        }
        #endregion BillBookNumberValidation

        #region GetMissingSerials
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetMissingSerials(string seriesStart, string seriesEnd, string BillBookType)
        {
            UA ua = new UA();
            DataSet ds =(_iAssignBillBookBusiness.GetMissingSerials(seriesStart,seriesEnd,BillBookType,ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ds });

        }
        #endregion GetMissingSerials

        #region DeleteBillBook
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteBillBook(string ID, string BillBookType)
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
                        status = _iAssignBillBookBusiness.DeleteBillBook(ID,BillBookType, ua);
                    }
                    switch (status)
                    {
                        case "0":
                            msg = c.DeleteFailure;
                            break;
                        case "1":
                            msg = c.DeleteSuccess;
                            break;
                        case "2":
                            msg = c.FKviolation;
                            break;
                    }
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = status, Message = msg });
                }
                catch (Exception ex)
                {
                    ConstMessage cm = c.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion DeleteBillBook

        #region BillBookRangeValidation
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string BillBookRangeValidation(string seriesStart, string seriesEnd,string BillNo, string BillBookType)
        {
            string status = null;
            string msg = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    
                        status = _iAssignBillBookBusiness.BillBookRangeValidation(seriesStart,seriesEnd, BillNo,BillBookType, ua);
                    
                    switch (status)
                    {
                        case "0":
                            msg = c.SeriesStartDuplication;
                            break;
                        case "1":
                            msg = c.SeriesEndDuplication;
                            break;
                        case "2":
                            msg = c.SeriesStartAndEndDuplication;
                            break;
                        case "3":
                            msg = c.SeriesStartConflict;
                            break;
                        case "4":
                            msg = c.SeriesEndConflict;
                            break;
                        case "5":
                            msg = c.SeriesStartAndEndConflict;
                            break;
                        case "6":
                            msg = "6";
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
        #endregion BillBookRangeValidation

        #region InsertUpdateBillBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateBillBook(AssignBillBookViewModel assignBillBookViewModelObj)
        {
            object result = null;
            
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    assignBillBookViewModelObj.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    assignBillBookViewModelObj.logDetails.CreatedBy = ua.UserName;
                    assignBillBookViewModelObj.logDetails.CreatedDate = ua.CurrentDatetime();
                    assignBillBookViewModelObj.logDetails.UpdatedBy = assignBillBookViewModelObj.logDetails.CreatedBy;
                    assignBillBookViewModelObj.logDetails.UpdatedDate = assignBillBookViewModelObj.logDetails.CreatedDate;
                    assignBillBookViewModelObj.SCCode = ua.SCCode;
                    //Getting UA

                    result = _iAssignBillBookBusiness.InsertUpdateBillBook(Mapper.Map<AssignBillBookViewModel, AssignBillBook>(assignBillBookViewModelObj));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {


                    ConstMessage cm = c.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });


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
        #endregion InsertUpdateBillBook

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
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "ExportData();";

                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill Book";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill Book";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill Book";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for new Bill Book";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill Book";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill Book";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new Bill Book";
                    ToolboxViewModelObj.deletebtn.Event = "";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;
                case "Closed":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill Book";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";


                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill Book";
                    ToolboxViewModelObj.savebtn.Event = "save();";
                  

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "Reset();";

                    break;
                case "AddSub":

                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}