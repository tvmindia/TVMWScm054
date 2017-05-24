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
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            UA ua = new UA();
            JobViewModel jobVM = new JobViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<EmployeesViewModel> EmpVM = Mapper.Map<List<Employees>, List<EmployeesViewModel>>(_dailyServiceBusiness.GetAllTechnicians(ua));
            if (EmpVM != null)
            {
                foreach (EmployeesViewModel emp in EmpVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = emp.Name,
                        Value = emp.ID.ToString(),
                        Selected = false
                    });
                }
            }
            jobVM.Employees = selectListItem;

            ViewBag.Servdate = DateTime.Now.ToString("yyyy-MM-dd");
            return View(jobVM);
        }
        #region InsertUpdateJob
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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
                    jobViewModel.SCCode = ua.SCCode;
                    jobViewModel.logDetails = new LogDetailsViewModel();
                    jobViewModel.logDetails.CreatedBy = ua.UserName;
                    jobViewModel.logDetails.CreatedDate = ua.CurrentDatetime();
                    jobViewModel.logDetails.UpdatedBy= ua.UserName;
                    jobViewModel.logDetails.UpdatedDate= ua.CurrentDatetime();
                    jobViewModel.CallStatusCode = "CLSD";
                    switch(jobViewModel.ID!=null)
                    {
                        case true:
                            result = _dailyServiceBusiness.UpdateJob(Mapper.Map<JobViewModel, TechnicianJob>(jobViewModel));
                            break;
                        default:
                            result = _dailyServiceBusiness.InsertJob(Mapper.Map<JobViewModel, TechnicianJob>(jobViewModel));
                            break;
                    }
                   
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

        [HttpPost]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string DeleteTechnicianJob(JobViewModel job)
        {
            object result = null;
            try
                {
                    UA ua = new UA();
                    job.SCCode = ua.SCCode;
                    result = _dailyServiceBusiness.DeleteJob(Mapper.Map<JobViewModel,Job>(job));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = result });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetDailyJobByID(string ID)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    UA ua = new UA();
                    JobViewModel jobVM = Mapper.Map<Job, JobViewModel>(_dailyServiceBusiness.GetDailyJobByID(ua.SCCode, ID));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = jobVM });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID is not valid" });
            }
        }


        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole,RoleContants.ManagerRole)]
        public string GetAllServiceReports(string ID,string ServieDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(ID) && (!string.IsNullOrEmpty(ServieDate)))
                {
                    UA ua = new UA();
                    List<JobViewModel> jobList = Mapper.Map<List<Job>, List<JobViewModel>>(_dailyServiceBusiness.GetJobs(ua.SCCode,Guid.Parse(ID),ServieDate));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = jobList });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID or Date is Empty!" });
                }
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
           
            
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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
                    ToolboxViewModelObj.addbtn.Text = "Add";
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