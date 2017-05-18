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
using SCManager.UserInterface.CustomAttributes;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class ReportController : Controller
    {
        IReportBusiness _reportBusiness;
        public ReportController(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }
        // GET: OfficeStockReport
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            UA ua = new UA();
            List<SystemReportViewModel> SysReportVM = Mapper.Map<List<SystemReport>,List<SystemReportViewModel>>(_reportBusiness.GetAllSysReports(ua));
            return View(SysReportVM);
        }
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult StockSummary()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult StockLedger()
        {
            UA ua = new UA();
            DateTime dt = ua.CurrentDatetime();
            ViewBag.fromdate = dt.AddDays(-30).ToString("yyyy-MM-dd");
            ViewBag.todate = dt.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult TechnicianStock()
        {
            return View();
        }
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllTechnicianStock(string fromdate = null, string todate = null)
        {
            UA ua = new UA();
            List<TechnicianStockViewModel> techinicianList = Mapper.Map<List<TechnicianStock>, List<TechnicianStockViewModel>>(_reportBusiness.GetTechniciansStockSummary(ua, fromdate, todate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = techinicianList });
        }



        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetAllStockLedger(string fromdate = null, string todate = null)
        {

            UA ua = new UA();
            List<StockLedgerViewModel> stockList = Mapper.Map<List<StockLedger>, List<StockLedgerViewModel>>(_reportBusiness.GetStockLedger(ua, fromdate, todate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = stockList });
        }
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public string GetItemsSummary(string fromdate=null,string todate=null)
        {
            UA ua = new UA();
            List<StockSummaryViewModel> ItemList = Mapper.Map<List<Item>, List<StockSummaryViewModel>>(_reportBusiness.GetItemsSummary(ua, fromdate, todate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });
        }

        #region ButtonStyling
        [HttpGet]
        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.PrintBtn.Visible = true;
                    ToolboxViewModelObj.PrintBtn.Text = "Print";
                    ToolboxViewModelObj.PrintBtn.Title = "Print";
                    ToolboxViewModelObj.PrintBtn.Event = "PrintTableToDoc();";

                    ToolboxViewModelObj.backbtn.Visible = true;
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to reports";
                    ToolboxViewModelObj.backbtn.Event = "goBack();";
                    break;
             

                 
               
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion
    }
}