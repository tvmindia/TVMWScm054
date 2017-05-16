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
    public class ReportController : Controller
    {
        IReportBusiness _reportBusiness;
        public ReportController(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }
        // GET: OfficeStockReport
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetItemsSummary(string fromdate=null,string todate=null)
        {
            UA ua = new UA();
            List<StockSummaryViewModel> ItemList = Mapper.Map<List<Item>, List<StockSummaryViewModel>>(_reportBusiness.GetItemsSummary(ua, fromdate, todate));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });
        }

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    ToolboxViewModelObj.savebtn.Visible = true;
                    ToolboxViewModelObj.savebtn.Text = "Print";
                    ToolboxViewModelObj.savebtn.Title = "Print";
                    ToolboxViewModelObj.savebtn.Event = "UnderConstruction();";

                    break;
               
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion
    }
}