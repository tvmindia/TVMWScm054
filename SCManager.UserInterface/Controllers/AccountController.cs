using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.CustomAttributes;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SCManager.UserInterface.Controllers
{
    public class AccountController : Controller
    {
        private IAuthenticationBusiness _authenticationBusiness;
        public AccountController(IAuthenticationBusiness authenticationBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }




        #region Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginvm)
        {
                 UserViewModel uservm = null;
           
                if (!ModelState.IsValid)
                {
                    return View(loginvm);
                }
                    uservm = Mapper.Map<User, UserViewModel>(_authenticationBusiness.CheckUserCredentials(Mapper.Map<LoginViewModel, User>(loginvm)));
            if (uservm != null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, uservm.UserName, DateTime.Now, DateTime.Now.AddHours(24), true, uservm.RoleList);
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                //session setting
                UA ua = new UA();
                ua.UserName = uservm.UserName;
                ua.UserID = uservm.ID;
                ua.SCCode = uservm.serviceCenter.Code;
                Session.Add("TvmValid", ua);
                return RedirectToLocal();
                
            }
            else
            {
                loginvm.IsFailure = true; 
                return View("Index",loginvm);
            }
           
        }
        #endregion Login

        #region EmailValidation
        [HttpGet]
        public string EmailValidation(string EmailID)
        {
            object result = null;

            try
            {
               // UA ua = new UA();
                result = _authenticationBusiness.EmailValidation(EmailID);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });


            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });

            }



        }
        #endregion EmailValidation

        #region VerificationCodeEmit
        [HttpGet]
        public string VerificationCodeEmit(string email)
        {
            UserViewModel userObj=new UserViewModel();
            userObj.Email = email;
            object result = null;

            try
            {
                // UA ua = new UA();
                result = _authenticationBusiness.VerificationCodeEmit(Mapper.Map<UserViewModel, User>(userObj)); ;
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });


            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });

            }



        }
        #endregion VerificationCodeEmit

        #region VerifyCode
        [HttpGet]
        public string VerifyCode(string email,string verificationCode)
        {
            UserViewModel userObj = new UserViewModel();
            userObj.Email = email;
            userObj.VerificationCode = verificationCode;
            object result = null;

            try
            {
                // UA ua = new UA();
                result = _authenticationBusiness.VerifyCode(Mapper.Map<UserViewModel, User>(userObj)); 
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });


            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });

            }



        }
        #endregion VerifyCode

        #region UpdatePassword
        [HttpGet]
        public string UpdatePassword(string ID, string password)
        {
            UserViewModel userObj = new UserViewModel();
            userObj.ID = Guid.Parse(ID);
            userObj.Password = password;
            object result = null;

            try
            {
                // UA ua = new UA();
                result = _authenticationBusiness.ResetPassword(Mapper.Map<UserViewModel, User>(userObj));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });


            }
            catch (Exception ex)
            {

                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });

            }



        }
        #endregion UpdatePassword


        private ActionResult RedirectToLocal()
        {
         return RedirectToAction("Index", "DashBoard");
        }
        private ActionResult RedirectToLogin()
        {
            return RedirectToAction("Index", "Account");
        }


        #region Logout
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Remove("TvmValid");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           return RedirectToLogin();
        }
        #endregion Logout

        [HttpGet]
        public ActionResult NotAuthorized()
        {
            return View();
        }

        [HttpGet]
        public string AreyouAlive()
        {
           string result = "";
            try
            {
              UA uaObj = null;
              if ((System.Web.HttpContext.Current.Session != null)&&(System.Web.HttpContext.Current.Session["TvmValid"] != null))
              {
                   uaObj = (UA)System.Web.HttpContext.Current.Session["TvmValid"];
                   result = "alive";
              }
              else
              {
                result = "dead";
              }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }

            return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
        }

     
        

    }
}