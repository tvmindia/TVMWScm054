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
    public class TechnicianSalaryCalculationController : Controller
    {
        ITechnicianSalaryCalculationBusiness _technicianSalaryCalculationBusiness;
        ICommonBusiness _commonBusiness;
        public TechnicianSalaryCalculationController(ITechnicianSalaryCalculationBusiness technicianSalaryCalculationBusiness, ICommonBusiness commonBusiness)
        {
            _technicianSalaryCalculationBusiness = technicianSalaryCalculationBusiness;
            _commonBusiness = commonBusiness;
        }
        // GET: TechnicianSalaryCalculation
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public ActionResult Index()
        {
            TechnicianSalaryViewModel _technicianSalaryViewModel = new TechnicianSalaryViewModel();
            try
            {

            
            UA ua = new UA();
           
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            string[] months =   {"","January","February","March","April","May","June","July","August","September","October","November","December"};
            for (int i = 1; i < 13; i++)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = months[i],
                    Value = i.ToString(),
                    Selected = false
                });
            }
        
            _technicianSalaryViewModel.MonthList = selectListItem;
            selectListItem = null;
            selectListItem= new List<SelectListItem>();
            DateTime dt = ua.CurrentDatetime();
            Int16 prevyears = Int16.Parse(dt.AddYears(-5).Year.ToString());
            Int16 comyears= Int16.Parse(dt.AddYears(5).Year.ToString());
            for(Int16 j = prevyears;j<= comyears;j++)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = j.ToString(),
                    Value = j.ToString(),
                    Selected = false
                });
            }
           
            _technicianSalaryViewModel.YearList = selectListItem;
                ViewBag.month = dt.AddDays(-30).Month;
                ViewBag.year = dt.Year;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
           
            return View(_technicianSalaryViewModel);
        }


        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetTechniciansCalculatedSalary(string Month,string Year)
        {
           try
                {
                UA ua = new UA();
               
                List<TechnicianSalaryViewModel> tSVMList = Mapper.Map<List<TechnicianSalary>,List<TechnicianSalaryViewModel>>(_technicianSalaryCalculationBusiness.GetTechniciansCalculatedSalary(ua.SCCode,Month,Year));
                decimal totalpaysum = tSVMList == null ? 0 : tSVMList.Select(T => T.TotalPayable).Sum();
                string totalpayablewithrupee=_commonBusiness.ConvertCurrency(totalpaysum);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = tSVMList,Record= totalpayablewithrupee });
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

                case "Calculate":

                    ToolboxViewModelObj.calculateBtn.Visible = true;
                    ToolboxViewModelObj.calculateBtn.Text = "Calc";
                    ToolboxViewModelObj.calculateBtn.Title = "Calculate";
                    ToolboxViewModelObj.calculateBtn.Event = "SalaryCalculate();";


                    break;


                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion

    }
}