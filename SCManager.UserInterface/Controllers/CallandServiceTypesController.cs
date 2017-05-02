using AutoMapper;
using Newtonsoft.Json;
using SCManager.BusinessService.Contracts;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
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
        public ActionResult Index()
        {
            return View();
        }
        #region GetCallTypes
        [HttpGet]
        public string GetCallTypes()
        {
            UA ua = new UA();
            List<CallandServiceTypesViewModel> callTypeList = Mapper.Map<List<CallTypes>, List<CallandServiceTypesViewModel>>(_iCallandServiceTypesBusiness.GetCallTypes(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Record = callTypeList });

        }
        #endregion GetCallTypes
        #region GetServiceTypes
        [HttpGet]
        public string GetServiceTypes()
        {
            UA ua = new UA();
            List<CallandServiceTypesViewModel> ServiceTypeList = Mapper.Map<List<ServiceTypes>, List<CallandServiceTypesViewModel>>(_iCallandServiceTypesBusiness.GetServiceTypes(ua));
            return JsonConvert.SerializeObject(new { Result = "OK", Record =ServiceTypeList });

        }
        #endregion GetServiceTypes
        #region UpdateCallandServiceTypes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string UpdateCallandServiceTypes(CallandServiceTypesViewModel callandServiceTypesViewModel)
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


                    result = _iCallandServiceTypesBusiness.UpdateCallandServiceTypes(Mapper.Map<CallandServiceTypesViewModel, CallTypes>(callandServiceTypesViewModel),Mapper.Map<CallandServiceTypesViewModel,ServiceTypes>(callandServiceTypesViewModel));

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
                //return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "Please Check the values" });
            }

        }
        #endregion UpdateCallandServiceTypes

        #region ButtonStyling
        [HttpGet]
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