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

            ViewBag.Servdate = DateTime.Now.ToString("dd-MMM-yyyy");
         
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
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetDailyJobByJobNo(string JobNo)
        {
            if (!string.IsNullOrEmpty(JobNo))
            {
                try
                {
                    UA ua = new UA();
                    
                    JobViewModel jobVM = Mapper.Map<Job, JobViewModel>(_dailyServiceBusiness.GetDailyJobByJobNo(ua.SCCode, JobNo));
                    return JsonConvert.SerializeObject(new { Result = "OK", Record = jobVM });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "JobNo is not valid" });
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


            selectListItem = null;
            selectListItem = new List<SelectListItem>();
            List<JobCallTypesViewModel> jobcalltypeListVM = Mapper.Map<List<JobCallTypes>, List<JobCallTypesViewModel>>(_dailyServiceBusiness.GetJobCallTypes(ua.SCCode));
            if(jobcalltypeListVM != null)
            {
                foreach (JobCallTypesViewModel jobcallTypevm in jobcalltypeListVM)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = jobcallTypevm.Description,
                        Value = jobcallTypevm.Code,
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

        //GetTechnicianByJobNo
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetTechnicianByJobNo(string JobNo)
        {
            try
            {
                UA ua = new UA();
                List<EmployeesViewModel> EmpListVM = Mapper.Map<List<Employees>, List<EmployeesViewModel>>(_dailyServiceBusiness.GetTechniciansForRepeatedJob(ua.SCCode,JobNo));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = EmpListVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetServiceRegistrySummary(string ServiceDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(ServiceDate))
                {
                    UA ua = new UA();
                    List<ServiceRegistrySummaryViewModel> serviceList = Mapper.Map<List<ServiceRegistrySummary>, List<ServiceRegistrySummaryViewModel>>(_dailyServiceBusiness.GetServiceRegistrySummary(ua.SCCode, ServiceDate));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = serviceList });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ServieDate is Empty!" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetServicefilterbyDays(string Isdefault)
        {
            try
            {
                if (!string.IsNullOrEmpty(Isdefault))
                {
                    UA ua = new UA();
                    List<JobViewModel> jobList = Mapper.Map<List<Job>, List<JobViewModel>>(_dailyServiceBusiness.GetServicefilterbyDays(ua.SCCode,ua.GetCurrentDateTime().ToString(),Isdefault));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = jobList });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID or Date is Empty!" });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetServiceRegisterSummaryFilter(string Isdefault)
        {
            try
            {
                if (!string.IsNullOrEmpty(Isdefault))
                {
                    UA ua = new UA();
                    List<ServiceRegistrySummaryViewModel> serviceList = Mapper.Map<List<ServiceRegistrySummary>, List<ServiceRegistrySummaryViewModel>>(_dailyServiceBusiness.GetServiceRegisterSummaryFilter(ua.SCCode, ua.GetCurrentDateTime().ToString(), Isdefault));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = serviceList });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = "ID or Date is Empty!" });
                }
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

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToDoc();";
                    break;
                case "Back":
                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";

                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Export";
                    ToolboxViewModelObj.PrintBtn.Title = "Export";
                    ToolboxViewModelObj.PrintBtn.Event = "ExportData();";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion
    }
}