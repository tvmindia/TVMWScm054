using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCManager.DataAccessObject.DTO;
using SCManager.UserInterface.Models;
using SCManager.BusinessService.Contracts;
using AutoMapper;
using SCManager.UserInterface.CustomAttributes;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class TechnicianController : Controller
    {
        //
        // GET: /Technician/
        private ITechnicianBusiness _technicianBusiness;
        public TechnicianController(ITechnicianBusiness technicianBusiness)
        {
            _technicianBusiness = technicianBusiness;
        }

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRoles(RoleContants.SuperAdminRole, RoleContants.AdministratorRole, RoleContants.ManagerRole)]
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
