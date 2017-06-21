using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SCManager.UserInterface.Models;
using SCManager.DataAccessObject.DTO;
using SCManager.BusinessService.Contracts;
using SCManager.UserInterface.CustomAttributes;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class LocalPurchaseController : Controller
    {
        // GET: LocalPurchase
        #region Constructor_Injection

        ILocalPurchaseBusiness _localPurchaseBusiness;
        Const c = new Const();

        public LocalPurchaseController(ILocalPurchaseBusiness localPurchaseBusiness)
        {
            _localPurchaseBusiness = localPurchaseBusiness;

        }
        #endregion Constructor_Injection
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            return View();
        }

        #region GetAllLocalPurchase
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllLocalPurchase(LocalPurchaseViewModel dataObj)
        {
            try
            {
                UA ua = new UA();
                List<LocalPurchaseViewModel> LocalPurchaseList = Mapper.Map<List<LocalPurchase>, List<LocalPurchaseViewModel>>(_localPurchaseBusiness.GetAllLocalPurchase(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = LocalPurchaseList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetAllLocalPurchase

        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateLocalPurchase(LocalPurchaseViewModel LocalPurchaseObj)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(LocalPurchaseObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    LocalPurchaseObj.LocalPurchaseDetail = JsonConvert.DeserializeObject<List<LocalPurchaseDetailViewModel>>(ReadableFormat);
                    LocalPurchaseViewModel r = Mapper.Map<LocalPurchase, LocalPurchaseViewModel>(_localPurchaseBusiness.InsertUpdate(Mapper.Map<LocalPurchaseViewModel, LocalPurchase>(LocalPurchaseObj), ua));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
                }

            }
            catch (Exception ex)
            {

                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            return result;
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteLocalPurchase(LocalPurchaseViewModel LocalPurchaseObj)
        {

            try
            {
                if (LocalPurchaseObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _localPurchaseBusiness.DeleteLocalPurchase(LocalPurchaseObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteLocalPurchaseDetail(LocalPurchaseDetailViewModel LocalPurchaseObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = LocalPurchaseObj.ID.GetValueOrDefault();
                Guid HeaderID = LocalPurchaseObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _localPurchaseBusiness.DeleteLocalPurchaseDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetLocalPurchase(LocalPurchaseViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                LocalPurchaseViewModel Frm8 = Mapper.Map<LocalPurchase, LocalPurchaseViewModel>(_localPurchaseBusiness.GetLocalPurchase(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Frm8 });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }


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
                    ToolboxViewModelObj.addbtn.Title = "Add New Bill";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "resetCurrent();";

                    break;
                case "Add":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";


                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Disable = true;
                    ToolboxViewModelObj.addbtn.DisableReason = "";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Bill";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Bill";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new item";
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