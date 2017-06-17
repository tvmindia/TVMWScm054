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
    public class OfficeBillEntryController : Controller
    {
        #region Constructor_Injection


        IOfficeBillEntryBusiness _iOfficeBillEntryBusiness;       
      
        public OfficeBillEntryController(IOfficeBillEntryBusiness iOfficeBillEntryBusiness)
        {
            _iOfficeBillEntryBusiness = iOfficeBillEntryBusiness;          
           
        }
        #endregion Constructor_Injection

        // GET: OfficeBillEntry
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            OfficeBillEntryViewModel OfficeBillEntryViewModel = null;
            try
            {
                OfficeBillEntryViewModel = new OfficeBillEntryViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>(); 

                selectListItem = null;
                selectListItem = new List<SelectListItem>();
                CommonViewModel CVM = new CommonViewModel();
                selectListItem = CVM.PaymentModelist;
                OfficeBillEntryViewModel.PaymentModeList = selectListItem; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(OfficeBillEntryViewModel);
         
        }

        Const c = new Const();
        #region InsertUpdateOfficeBillEntry
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateOfficeBillEntry(OfficeBillEntryViewModel OfficeBillEntryViewModelObj)
        {
            //string result = "";

            try
            {
               
                UA ua = new UA();
                object ResultFromJS = JsonConvert.DeserializeObject(OfficeBillEntryViewModelObj.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                OfficeBillEntryViewModelObj.OfficeBillEntryDetail = JsonConvert.DeserializeObject<List<OfficeBillEntryDetailViewModel>>(ReadableFormat);
                OfficeBillEntryViewModel r = Mapper.Map<OfficeBillEntry, OfficeBillEntryViewModel>(_iOfficeBillEntryBusiness.InsertUpdate(Mapper.Map<OfficeBillEntryViewModel, OfficeBillEntry>(OfficeBillEntryViewModelObj), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
               

            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
            // return result;
        }
        #endregion InsertUpdateOfficeBillEntry

        #region GetOfficeBillHeaderByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetOfficeBillHeaderByID(OfficeBillEntryViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                OfficeBillEntryViewModel offc = Mapper.Map<OfficeBillEntry, OfficeBillEntryViewModel>(_iOfficeBillEntryBusiness.GetOfficeBillHeaderByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = offc });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetOfficeBillHeaderByID

        #region GetAllOfficeBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllOfficeBillEntry()
        {
            UA ua = new UA();
            List<OfficeBillEntryViewModel> ItemList = Mapper.Map<List<OfficeBillEntry>, List<OfficeBillEntryViewModel>>(_iOfficeBillEntryBusiness.GetAllOfficeBillEntry(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #endregion GetAllOfficeBillEntry

        #region DeleteOfficeBillEntry
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteOfficeBillEntry(OfficeBillEntryViewModel offcObj)
        {

            try
            {
                if (offcObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _iOfficeBillEntryBusiness.DeleteOfficeBillEntry(offcObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }


        }
        #endregion DeleteOfficeBillEntry

        #region DeleteOfficeBillDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteOfficeBillDetail(OfficeBillEntryDetailViewModel offcDObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = offcDObj.ID.GetValueOrDefault();
                Guid HeaderID = offcDObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _iOfficeBillEntryBusiness.DeleteOfficeBillDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOfficeBillDetail

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
                    ToolboxViewModelObj.addbtn.Title = "Add New Office Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Office Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Office Bill";
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
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for new Office Entry";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Office Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Office Entry";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new Office Entry";
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