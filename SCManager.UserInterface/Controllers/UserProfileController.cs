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



        #region BuildMember
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public ActionResult BuildMember()
        {
            UserViewModel  userViewModel = new UserViewModel();
            UA ua = new UA();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
          
            List<ServiceCenterViewModel> ServiceCenterListVM = Mapper.Map<List<ServiceCenter>, List<ServiceCenterViewModel>>(_authenticationBusiness.GetAllServiceCenters());
            if(ServiceCenterListVM!=null)
            {
                foreach (ServiceCenterViewModel scvm in ServiceCenterListVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = scvm.Description,
                        Value = scvm.Code,
                        Selected = false
                    });
                }
            }

            userViewModel.SCList = selectListItem;
            selectListItem = null;
            userViewModel.RoleVMList = Mapper.Map<List<Role>, List<RoleViewModel>>(_authenticationBusiness.GetAllRolesByServicecenter(ua.SCCode)); 
            return View(userViewModel);
        }
        #endregion BuildMember

        #region GetAllUsers
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public string GetAllUsers()
        {
            try
            {
                List<UserViewModel> UserViewModelList = Mapper.Map<List<User>, List<UserViewModel>>(_authenticationBusiness.GetAllUsersInSystem());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = UserViewModelList });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllUsers
        #region GetUserDetailsByUser
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public string GetUserDetailsByUser(string UserID,string SCCode)
        {
            try
            {
                UserViewModel UserViewModel = Mapper.Map<User,UserViewModel>(_authenticationBusiness.GetUserDetailsByUser(Guid.Parse(UserID), SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = UserViewModel });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetUserDetailsByUser

        #region GetAllRolesBySC
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public string GetAllRolesBySC(string SCCode)
        {
            try
            {

                List<RoleViewModel> rolesVMLisit = Mapper.Map<List<Role>, List<RoleViewModel>>(_authenticationBusiness.GetAllRolesByServicecenter(SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rolesVMLisit });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllRolesBySC



        #region InsertUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public string InsertUpdateUser(UserViewModel userViewModel)
        {
            object result = null;
           
            if (ModelState.IsValid)
            {

                try
                {
                    UA ua = new UA();
                    userViewModel.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    userViewModel.logDetails.CreatedBy = ua.UserName;
                    userViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    userViewModel.logDetails.UpdatedBy = userViewModel.logDetails.CreatedBy;
                    userViewModel.logDetails.UpdatedDate = userViewModel.logDetails.CreatedDate;
                  
                    if (userViewModel.ID == null)
                    {
                       result = _authenticationBusiness.InserUser(Mapper.Map<UserViewModel, User>(userViewModel));
                       return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = result });
                    }
                    else
                    {
                        result = _authenticationBusiness.UpdateUser(Mapper.Map<UserViewModel, User>(userViewModel));
                       
                        return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = result });
                    }

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
        #endregion InsertUser


        #region DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole)]
        public string DeleteUser(UserViewModel userViewModel)
        {
            object result = null;
         
           

                try
                {
               
                        result = _authenticationBusiness.DeleteUser(Mapper.Map<UserViewModel, User>(userViewModel));
                        return JsonConvert.SerializeObject(new { Result = "OK", Message = result });
                   

                }
                catch (Exception ex)
                {

                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });

                }
            
           

        }
        #endregion DeleteUser

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
               
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    break;
                case "Add":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add";
                    ToolboxViewModelObj.addbtn.Event = "Add();";
                    break;

                case "Edit":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "Save();";
                    ToolboxViewModelObj.deletebtn.Visible = true;
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "Delete();";
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
    }
}