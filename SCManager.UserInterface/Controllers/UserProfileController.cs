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
    public class UserProfileController : Controller
    {
        private IAuthenticationBusiness _authenticationBusiness;
        private Const constObj = new Const();
        public UserProfileController(IAuthenticationBusiness authenticationBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
        }
        // GET: UserProfile
        [AuthorizeRoles(RoleContants.ManagerRole, RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult Index()
        {
            return View();
        }
        #region UpdateUserProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.ManagerRole, RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string UpdateUserProfile(UserProfileViewModel userProfileViewModel)
        {
            object result = null;
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    userProfileViewModel.logDetails = new LogDetailsViewModel();
                    //Getting UA

                    userProfileViewModel.logDetails.UpdatedBy = ua.UserName;
                    userProfileViewModel.logDetails.UpdatedDate = ua.CurrentDatetime();
                    userProfileViewModel.SCCode = ua.SCCode;
                    userProfileViewModel.ID = ua.UserID;
                    result = _authenticationBusiness.UpdateUserProfile(Mapper.Map<UserProfileViewModel, UserProfile>(userProfileViewModel));
                    string message = "";
                    string rslt = "";
                    switch(result.ToString())
                    {
                        case "0":
                            message = constObj.UpdateFailure;
                            rslt = "ERROR";
                            break;
                        case "1":
                            message = constObj.UpdateSuccess;
                            rslt = "OK";
                            break;
                        case "2":
                            message = constObj.PasswordError;
                            rslt = "ERROR";
                            break;

                    }
                    return JsonConvert.SerializeObject(new { Result = rslt, Message = message });
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
        #endregion UpdateUserProfile


        [HttpGet]
        [AuthorizeRoles(RoleContants.ManagerRole,RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "Save":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "Save();";
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
    }
}