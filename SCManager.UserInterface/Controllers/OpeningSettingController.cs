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
    public class OpeningSettingController : Controller
    {
        #region Constructor_Injection

        IOpeningSettingBusiness _openingSettingBusiness;

        public OpeningSettingController(IOpeningSettingBusiness openingSettingBusiness)
        {
            _openingSettingBusiness = openingSettingBusiness;

        }
        #endregion Constructor_Injection

        // GET: OpeningSetting
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult Index()
        {
            OpeningSettingViewModel o = new OpeningSettingViewModel();
            o.ID = Guid.Empty;
            o.BankFormatted = "0.00";
            o.CashFormatted = "0.00";
            o.WithEffectDateFormatted = "NIL";

            return View(o);
        }

        Const c = new Const();

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string InsertUpdateOpeningSetting(OpeningSettingViewModel OpeningSettingObj)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    UA ua = new UA();
                    object ResultFromJS = JsonConvert.DeserializeObject(OpeningSettingObj.DetailJSON);
                    string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    OpeningSettingObj.OpeningDetails = JsonConvert.DeserializeObject<List<OpeningDetailViewModel>>(ReadableFormat);
                    OpeningSettingViewModel r = Mapper.Map<OpeningSetting, OpeningSettingViewModel>(_openingSettingBusiness.InsertUpdate(Mapper.Map<OpeningSettingViewModel, OpeningSetting>(OpeningSettingObj), ua));
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
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string DeleteOpeningDetail(OpeningDetailViewModel ODobj)
        {

            try
            {
                UA ua = new UA();
                Guid ID = ODobj.ID.GetValueOrDefault();               
                if (ID == null )
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = c.DeleteFailure });
                }
                else
                {
                    _openingSettingBusiness.DeleteOpeningSettingDetail(ID, ua);
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
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetOpenings(OpeningSetting dataObj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                OpeningSettingViewModel opening = Mapper.Map<OpeningSetting, OpeningSettingViewModel>(_openingSettingBusiness.GetOpeningSetting(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = opening });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Edit";
                    ToolboxViewModelObj.addbtn.Title = "Edit opening";
                    ToolboxViewModelObj.addbtn.Event = "$('#AddTab').trigger('click');";



                    break;
                case "Save":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to View";
                    ToolboxViewModelObj.backbtn.Event = "$('#ListTab').trigger('click');";

                    
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Opening";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    

                    ToolboxViewModelObj.resetbtn.Visible = true;
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "BindOpening();";

                     
                

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}