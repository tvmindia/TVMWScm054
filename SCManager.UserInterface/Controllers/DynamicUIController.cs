using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using SCManager.BusinessService.Contracts;

namespace SCManager.UserInterface.Controllers
{
    public class DynamicUIController : Controller
    {
        //
        // GET: /DynamicUI/
        private IDynamicUIBusiness _dynamicUIBusiness;
        private ICommonBusiness _commonBusiness;
        private ISalesBusiness _salesBusiness;
        public DynamicUIController(IDynamicUIBusiness dynamicUIBusiness,ICommonBusiness commonBusiness, ISalesBusiness salesBusiness)
        {
            _dynamicUIBusiness = dynamicUIBusiness;
            _commonBusiness = commonBusiness;
            _salesBusiness = salesBusiness;
        }

        public ActionResult _MenuNavBar()
        {
            List<Menu> menulist = _dynamicUIBusiness.GetAllMenues();
            DynamicUIViewModel dUIObj = new DynamicUIViewModel();
            dUIObj.MenuViewModelList = Mapper.Map<List<Menu>, List<MenuViewModel>>(menulist);
            return View(dUIObj);
        }

        public ActionResult _ReorderAlertItems()
        {
            UA ua = new UA();
            List<ReorderAlert> ReorderAlertlist = _dynamicUIBusiness.GetReorderAlertITems(ua);
            DynamicUIViewModel dUIObj = new DynamicUIViewModel();
            dUIObj.ReorderAlertViewModelList = Mapper.Map<List<ReorderAlert>, List<ReorderAlertViewModel>>(ReorderAlertlist);
            return View(dUIObj);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _UnderConstruction()
        {            
            return View();
        }

        public ActionResult demoGrid()
        {
            return View();
        }




        [HttpGet]
        public string GetStockValueSummary(StockValueSummary obj)
        {
            try
            {
                //   System.Threading.Thread.Sleep(5000);
                UA ua = new UA();
                List<StockValueSummary> Result = _dynamicUIBusiness.GetStockValueSummary(ua);                
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Result });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #region GetWeeklySalesSummaryForChart
        [HttpGet]
        public string GetWeeklySalesDetails()
        {
            try
            {
                UA ua = new UA();
                List<SalesGraphViewModel> salesSummaryList = Mapper.Map<List<SalesGraph>, List<SalesGraphViewModel>>(_salesBusiness.GetWeeklySalesDetails(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = salesSummaryList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetWeeklySalesSummaryForChart

    }
}
