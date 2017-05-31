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
    public class TCRBillEntryController : Controller
    {
        #region Constructor_Injection


        ITCRBillEntryBusiness _iTCRBillEntryBusiness;
        IEmployeesBusiness _iEmployeesBusiness;

        public TCRBillEntryController(ITCRBillEntryBusiness iTCRBillEntryBusiness, IEmployeesBusiness iEmployeesBusiness)
        {
            _iTCRBillEntryBusiness = iTCRBillEntryBusiness;
            _iEmployeesBusiness = iEmployeesBusiness;

        }
        #endregion Constructor_Injection
        // GET: TCRBillEntry
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            TCRBillEntryViewModel tCRBillEntryViewModel = null;
            try
            {
                tCRBillEntryViewModel = new TCRBillEntryViewModel();
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
                tCRBillEntryViewModel.TechniciansList = selectListItem;

                //------------------ UOM Dropdown Bind--------------------//
                selectListItem = new List<SelectListItem>();
                //Categories Drop down bind
                List<TCRBillEntryViewModel> jobNoList = Mapper.Map<List<TCRBillEntry>, List<TCRBillEntryViewModel>>(_iTCRBillEntryBusiness.GetAllJobNo(ua));
                jobNoList = jobNoList == null ? null : jobNoList.OrderBy(attset => attset.JobNo).ToList();
                foreach (TCRBillEntryViewModel tcrvm in jobNoList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = tcrvm.JobNo,
                        Value = tcrvm.JobNo,
                        Selected = false
                    });
                }
                tCRBillEntryViewModel.JobNoList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(tCRBillEntryViewModel);
        }
        Const c = new Const();
        #region InsertUpdateTCRBillEntry
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateTCRBillEntry(TCRBillEntryViewModel TCRBillEntryViewModelObj)
        {
            string result = "";

            try
            {
                //if (ModelState.IsValid)
                //{
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(TCRBillEntryViewModelObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    TCRBillEntryViewModelObj.TCRBillEntryDetail = JsonConvert.DeserializeObject<List<TCRBillEntryDetailViewModel>>(ReadableFormat);
                    TCRBillEntryViewModel r = Mapper.Map<TCRBillEntry, TCRBillEntryViewModel>(_iTCRBillEntryBusiness.InsertUpdate(Mapper.Map<TCRBillEntryViewModel, TCRBillEntry>(TCRBillEntryViewModelObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
                //}
                //else
                //{
                //    List<string> modelErrors = new List<string>();
                //    foreach (var modelState in ModelState.Values)
                //    {
                //        foreach (var modelError in modelState.Errors)
                //        {
                //            modelErrors.Add(modelError.ErrorMessage);
                //        }
                //    }
                //    return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
                //    //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
                //}

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
           // return result;
        }
        #endregion InsertUpdateTCRBillEntry

        #region GetAllTCRBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetAllTCRBillEntry()
        {
            UA ua = new UA();
            List<TCRBillEntryViewModel> ItemList = Mapper.Map<List<TCRBillEntry>, List<TCRBillEntryViewModel>>(_iTCRBillEntryBusiness.GetAllTCRBillEntry(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #endregion GetAllTCRBillEntry

        #region DeleteTCRBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteTCRBillEntry(TCRBillEntryViewModel tcrObj)
        {

            try
            {
                if (tcrObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _iTCRBillEntryBusiness.DeleteTCRBillEntry(tcrObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }
        #endregion DeleteTCRBillEntry

        #region DeleteTCRBillDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteTCRBillDetail(TCRBillEntryDetailViewModel tcrDObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = tcrDObj.ID.GetValueOrDefault();
                Guid HeaderID = tcrDObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _iTCRBillEntryBusiness.DeleteTCRBillDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteTCRBillDetail

        #region GetTCRBillHeaderByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetTCRBillHeaderByID(TCRBillEntryViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                TCRBillEntryViewModel Frm8 = Mapper.Map<TCRBillEntry, TCRBillEntryViewModel>(_iTCRBillEntryBusiness.GetTCRBillHeaderByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Frm8 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetTCRBillHeaderByID

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



                    break;
                case "Edit":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New TCR Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save TCR Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete TCR Bill";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for new TCR Entry";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save TCR Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete TCR";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new TCR Entry";
                    ToolboxViewModelObj.deletebtn.Event = "";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

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