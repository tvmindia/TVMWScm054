using SCManager.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Controllers
{
    public class ExpensesController : Controller
    {
        private IExpensesBusiness _expensesBusiness;
        public ExpensesController(IExpensesBusiness expensesBusiness)
        {
            _expensesBusiness = expensesBusiness;
        }
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }
    }
}