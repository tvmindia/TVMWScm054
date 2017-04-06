using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using SCManager.BusinessService.Contracts;
using AutoMapper;

namespace SCManager.UserInterface.Controllers
{
    public class TechnicianController : Controller
    {
        //
        // GET: /Technician/
        private ITechnicianBusiness _technicianBusiness;
        public TechnicianController(ITechnicianBusiness technicianBusiness)
        {
            _technicianBusiness = technicianBusiness;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult _TechnicianSummary()
        {
            UA ua = new UA();
         
          

            List<TechnicianSummary> TechnicianSummarylist = _technicianBusiness.GetTechnicianSummary(ua);
            TechnicianViewModel TcnObj = new TechnicianViewModel();
            TcnObj.TechnicianSummaryViewModel = Mapper.Map<List<TechnicianSummary>, List<TechnicianSummaryViewModel>>(TechnicianSummarylist);
            return View(TcnObj);


        }

    }
}
