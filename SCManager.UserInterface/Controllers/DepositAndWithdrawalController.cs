using SCManager.BusinessService.Contracts;
using SCManager.UserInterface.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    [CustomAuthenticationFilter]
    public class DepositAndWithdrawalController : Controller
    {
        private IDepositAndWithdrawalBusiness _depositAndWithdrawalBusiness;
        public DepositAndWithdrawalController(IDepositAndWithdrawalBusiness depositAndWithdrawalBusiness)
        {
            _depositAndWithdrawalBusiness = depositAndWithdrawalBusiness;
        }
        // GET: DepositAndWithdrawal
        public ActionResult Index()
        {
            return View();
        }



    }
}