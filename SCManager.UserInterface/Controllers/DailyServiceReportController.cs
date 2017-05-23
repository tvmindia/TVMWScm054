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
    public class DailyServiceReportController : Controller
    {
        IDailyServiceBusiness _dailyServiceBusiness;
        public DailyServiceReportController(IDailyServiceBusiness dailyServiceBusiness)
        {
            _dailyServiceBusiness = dailyServiceBusiness;
        }
        // GET: DailyServiceReport
        public ActionResult Index()
        {
            return View();
        }
        #region InsertUpdateJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateJob(JobViewModel jobViewModel)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                try
                {
                    UA ua = new UA();
                    jobViewModel.logDetails = new LogDetailsViewModel();
                    jobViewModel.logDetails.CreatedBy = ua.UserName;
                    jobViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    jobViewModel.logDetails.UpdatedBy= ua.UserName;
                    jobViewModel.logDetails.UpdatedDate= ua.CurrentDatetime();
                    result = _dailyServiceBusiness.InsertJob(Mapper.Map<JobViewModel, TechnicianJob>(jobViewModel));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            //Model state errror
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
        #endregion InsertUpdateJob

        public ActionResult TechnicianJobForm(string source)
        {

            //Service type drow down bind here
            JobViewModel jobVM = new JobViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            UA ua = new UA();
            List<ServiceTypeViewModel> serviceTypeVM = Mapper.Map<List<ServiceType>, List<ServiceTypeViewModel>>(_dailyServiceBusiness.GetAllServiceTypes(ua));
            if(serviceTypeVM!=null)
            {
                foreach (ServiceTypeViewModel serviceTypevm in serviceTypeVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = serviceTypevm.Description,
                        Value = serviceTypevm.Code,
                        Selected = false
                    });
                }
            }
            jobVM.ServiceTypes = selectListItem;
            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<CallTypeViewModel> calltypeListVM = Mapper.Map<List<CallTypes>, List<CallTypeViewModel>>(_dailyServiceBusiness.GetCallTypes(ua));
            if(calltypeListVM!=null)
            {
                foreach (CallTypeViewModel callTypevm in calltypeListVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = callTypevm.Code,
                        Value = callTypevm.Code,
                        Selected = false
                    });
                }
            }
           
            jobVM.CallTypes = selectListItem;

            jobVM.Source = source;
            return View("TechnicianJobForm", jobVM);
        }

        [HttpGet]
        public string GetJobNumbersForDropDown()
        {
            try
            {
                UA ua = new UA();
                List<JobViewModel> jobListVM = Mapper.Map<List<Job>, List<JobViewModel>>(_dailyServiceBusiness.GetAllJobNumbers(ua.SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = jobListVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        public string GetAllTechnicianForServiceTypeDropDown()
        {
            try
            {
                UA ua = new UA();
                List<EmployeesViewModel> EmpListVM = Mapper.Map<List<Employees>, List<EmployeesViewModel>>(_dailyServiceBusiness.GetTechniciansForRepeatedJob(ua.SCCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = EmpListVM });
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
                case "Add":
                    ToolboxViewModelObj.addbtn.Visible = true;
                    ToolboxViewModelObj.addbtn.Text = "Add Job";
                    ToolboxViewModelObj.addbtn.Title = "Add Job";
                    ToolboxViewModelObj.addbtn.Event = "AddTechnicanJob();";
                    break;
                case "Save":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "SaveTechnicanJob();";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion
    }
}