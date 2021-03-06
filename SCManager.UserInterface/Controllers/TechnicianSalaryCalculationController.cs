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

        #region GetTechniciansCalculatedSalary
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetTechniciansCalculatedSalary(string Month,string Year)
        {
           try
                {
                UA ua = new UA();
                List<TechnicianSalaryViewModel> tSVMList = Mapper.Map<List<TechnicianSalary>,List<TechnicianSalaryViewModel>>(_technicianSalaryCalculationBusiness.GetTechniciansCalculatedSalary(ua.SCCode,Month,Year));
                decimal totlcommiss = tSVMList == null ? 0 : tSVMList.Select(T => T.TotalCommission).Sum();
                decimal totlAdvance = tSVMList == null ? 0 : tSVMList.Select(T => T.SalaryAdvance).Sum();
                decimal totalpaysum = tSVMList == null ? 0 : tSVMList.Select(T => T.TotalPayable).Sum();
                string totalcommisswithrupee=_commonBusiness.ConvertCurrency(totlcommiss);
                string totalAdvancewithrupee= _commonBusiness.ConvertCurrency(totlAdvance);
                string totalpayablewithrupee=_commonBusiness.ConvertCurrency(totalpaysum);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = tSVMList,Record= new { TotalCommission= totalcommisswithrupee,TotalAdvanceSalary= totalAdvancewithrupee,TotalPayable= totalpayablewithrupee } });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
        }
        #endregion GetTechniciansCalculatedSalary
        #region  GetAllBreakUpSalaryByTechnician

        /// <summary>
        /// Get All Break Up Salary By Technician
        /// </summary>
        /// <param name="ActionType"></param>
        /// <returns>details of different salary break ups combined</returns>
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole)]
        public string GetAllBreakUpSalaryByTechnician(string SCCode, string EmpID, string Month, string Year)
        {
            try
            {
                UA ua = new UA();
               
                    List<TechnicianSalaryJobBreakUpViewModel> tSjBVMList = Mapper.Map<List<TechnicianSalaryJobBreakUp>, List<TechnicianSalaryJobBreakUpViewModel>>(_technicianSalaryCalculationBusiness.GetTechnicianJobCommissionBreakUp(ua.SCCode, Guid.Parse(EmpID), Int16.Parse(Month), Int16.Parse(Year)));
                    decimal totaljobsum = tSjBVMList == null ? 0 : tSjBVMList.Select(T => T.Total).Sum();
                    string totaljobsumwithrupee = _commonBusiness.ConvertCurrency(totaljobsum);
               
                
                    List<TechnicianSalaryTCRBreakUpViewModel> tSTCRVMList = Mapper.Map<List<TechnicianSalaryTCRBreakUp>, List<TechnicianSalaryTCRBreakUpViewModel>>(_technicianSalaryCalculationBusiness.GetTechnicianTCRCommissionBreakUp(ua.SCCode, Guid.Parse(EmpID), Int16.Parse(Month), Int16.Parse(Year)));
                    decimal totalTCRsum = tSTCRVMList == null ? 0 : tSTCRVMList.Select(T => T.Total).Sum();
                    string totalTCRsumwithrupee = _commonBusiness.ConvertCurrency(totalTCRsum);

                List<TechnicianSalaryAMCBreakUpViewModel> tSAMCVMList = Mapper.Map<List<TechnicianSalaryAMCBreakUp>, List<TechnicianSalaryAMCBreakUpViewModel>>(_technicianSalaryCalculationBusiness.GetTechnicianAMCCommissionBreakUp(ua.SCCode, Guid.Parse(EmpID), Int16.Parse(Month), Int16.Parse(Year)));
                decimal totalAMCsum = tSAMCVMList == null ? 0 : tSAMCVMList.Select(T => T.AMCCommission).Sum();
                string totalAMCsumwithrupee = _commonBusiness.ConvertCurrency(totalAMCsum);

                List<TechnicianSalaryAdvanceBreakUpViewModel> tSAVMList = Mapper.Map<List<TechnicianSalaryAdvanceBreakUp>, List<TechnicianSalaryAdvanceBreakUpViewModel>>(_technicianSalaryCalculationBusiness.GetTechnicianSalaryAdvanceBreakUp(ua.SCCode, Guid.Parse(EmpID), Int16.Parse(Month), Int16.Parse(Year)));
                decimal totalAdvancesum = tSAVMList == null ? 0 : tSAVMList.Select(T => T.Advance).Sum();
                string totalAdvancesumwithrupee = _commonBusiness.ConvertCurrency(totalAdvancesum);
                decimal totalcommission = totaljobsum + totalTCRsum + totalAMCsum;
                string totalcommissonwithrupee = _commonBusiness.ConvertCurrency(totalcommission);
                decimal netpayable = totalcommission - totalAdvancesum;
                string totalpayablewithrupee= _commonBusiness.ConvertCurrency(netpayable);
                return JsonConvert.SerializeObject(new { Result = "OK", JobRecords = tSjBVMList,JobRecord= totaljobsumwithrupee,TCRRecords= tSTCRVMList, TCRRecord = totalTCRsumwithrupee,AMCRecords= tSAMCVMList, AMCRecord= totalAMCsumwithrupee,SARecords= tSAVMList, SARecord= totalAdvancesumwithrupee,TotalComm= totalcommissonwithrupee, NetPayable= totalpayablewithrupee });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllBreakUpSalaryByTechnician
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