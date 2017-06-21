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
    public class ReceiveFromOtherSCController : Controller
    {
        #region Constructor_Injection


        IReceiveFromOtherSCBusiness _iReceiveFromOtherSCBusiness;
        
        public ReceiveFromOtherSCController(IReceiveFromOtherSCBusiness iReceiveFromOtherSCBusiness)
        {
            _iReceiveFromOtherSCBusiness = iReceiveFromOtherSCBusiness;
          
        }
        #endregion Constructor_Injection
        // GET: ReceiveFromOtherSC
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            return View();
        }

        Const c = new Const();
        #region InsertUpdateReceiveFromOtherSC
        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string InsertUpdateReceiveFromOtherSC(ReceiveFromOtherSCViewModel ReceiveFromOtherSCViewModelObj)
        {
         
            try
            {
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                object ResultFromJS = JsonConvert.DeserializeObject(ReceiveFromOtherSCViewModelObj.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                ReceiveFromOtherSCViewModelObj.ReceiveFromOtherSCDetail = JsonConvert.DeserializeObject<List<ReceiveFromOtherSCDetailViewModel>>(ReadableFormat);
                    ReceiveFromOtherSCViewModel r = Mapper.Map<ReceiveFromOtherSC, ReceiveFromOtherSCViewModel>(_iReceiveFromOtherSCBusiness.InsertUpdate(Mapper.Map<ReceiveFromOtherSCViewModel, ReceiveFromOtherSC>(ReceiveFromOtherSCViewModelObj), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
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
            catch (Exception ex)
            {

                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            // return result;
        }
        #endregion InsertUpdateReceiveFromOtherSC

        #region GetAllOtherSCReceipt
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllOtherSCReceipt()
        {
            UA ua = new UA();
            List<ReceiveFromOtherSCViewModel> ItemList = Mapper.Map<List<ReceiveFromOtherSC>, List<ReceiveFromOtherSCViewModel>>(_iReceiveFromOtherSCBusiness.GetAllOtherSCReceipt(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
        #endregion GetAllOtherSCReceipt

        #region DeleteOtherSCReceipt
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteOtherSCReceipt(ReceiveFromOtherSCViewModel receiveFromOtherSCObj)
        {

            try
            {
                if (receiveFromOtherSCObj.ID.GetValueOrDefault() == Guid.Empty)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.NoItems });
                }

                UA ua = new UA();
                _iReceiveFromOtherSCBusiness.DeleteOtherSCReceipt(receiveFromOtherSCObj.ID.GetValueOrDefault(), ua);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });

            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherSCReceipt

        #region DeleteOtherScReceiptDetail
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteOtherScReceiptDetail(ReceiveFromOtherSCDetailViewModel receiveFromOtherSCDetailObj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = receiveFromOtherSCDetailObj.ID.GetValueOrDefault();
                Guid HeaderID = receiveFromOtherSCDetailObj.HeaderID.GetValueOrDefault();
                if (ID == null || HeaderID == null)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _iReceiveFromOtherSCBusiness.DeleteOtherScReceiptDetail(ID, HeaderID, ua);
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = c.DeleteSuccess });
                }


            }
            catch (Exception ex)
            {
                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }


        }
        #endregion DeleteOtherScReceiptDetail

        #region GetOtherSCReceiptByID
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetOtherSCReceiptByID(ReceiveFromOtherSCViewModel dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                ReceiveFromOtherSCViewModel receiveFromOtherSC = Mapper.Map<ReceiveFromOtherSC, ReceiveFromOtherSCViewModel>(_iReceiveFromOtherSCBusiness.GetOtherSCReceiptByID(dataObj.ID.GetValueOrDefault(), ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = receiveFromOtherSC });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetOtherSCReceiptByID

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
                    ToolboxViewModelObj.addbtn.Title = "Add New Receipt From Other SC";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Receipt From Other SC";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Receipt From Other SC";
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
                    ToolboxViewModelObj.addbtn.DisableReason = "N/A for new Receipt From Other SC";
                    ToolboxViewModelObj.addbtn.Text = "New";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "";

                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Receipt From Other SC";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete Receipt From Other SC";
                    ToolboxViewModelObj.deletebtn.DisableReason = "N/A for new Receipt From Other SC";
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