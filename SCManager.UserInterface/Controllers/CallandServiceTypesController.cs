﻿using AutoMapper;
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
    public class CallandServiceTypesController : Controller
    {
        #region Constructor_Injection

        ICallandServiceTypesBusiness _iCallandServiceTypesBusiness;

        public CallandServiceTypesController(ICallandServiceTypesBusiness iCallandServiceTypesBusiness)
        {
            _iCallandServiceTypesBusiness = iCallandServiceTypesBusiness;

        }
        #endregion Constructor_Injection
        // GET: CallandServiceTypes
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult Index()
        {
            return View();
        }
      
        #region GetServiceTypes
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetServiceTypes()
        {
            try
            {
                UA ua = new UA();
                CallandServiceTypesViewModel _serviceType = Mapper.Map<ServiceTypes, CallandServiceTypesViewModel>(_iCallandServiceTypesBusiness.GetServiceTypes(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = _serviceType });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
         

        }
        #endregion GetServiceTypes
        #region UpdateCallandServiceTypes
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string UpdateServiceTypes(CallandServiceTypesViewModel callandServiceTypesViewModel)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    UA ua = new UA();
                    callandServiceTypesViewModel.logDetails = new LogDetailsViewModel();
                    //Getting UA
                    callandServiceTypesViewModel.logDetails.CreatedBy = ua.UserName;
                    callandServiceTypesViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    callandServiceTypesViewModel.logDetails.UpdatedBy = callandServiceTypesViewModel.logDetails.CreatedBy;
                    callandServiceTypesViewModel.logDetails.UpdatedDate = callandServiceTypesViewModel.logDetails.CreatedDate;
                    callandServiceTypesViewModel.SCCode = ua.SCCode;
                    result = _iCallandServiceTypesBusiness.UpdateServiceTypesAndCommission(Mapper.Map<CallandServiceTypesViewModel,ServiceTypes>(callandServiceTypesViewModel));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
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
            }

        }
        #endregion UpdateCallandServiceTypes
        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
              
                case "Update":
                    
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save Call/Service Type";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                   
                    break;
             
              
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}