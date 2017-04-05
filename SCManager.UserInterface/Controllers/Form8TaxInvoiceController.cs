using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using SCManager.UserInterface.Models;
using SCManager.DataAccessObject.DTO;
using SCManager.BusinessService.Contracts;

namespace SCManager.UserInterface.Controllers
{
    public class Form8TaxInvoiceController : Controller
    {
        //
        // GET: /Form8TaxInvoice/
         #region Constructor_Injection

        IForm8TaxInvoiceBusiness _form8TaxInvoiceBusiness;

        public Form8TaxInvoiceController(IForm8TaxInvoiceBusiness form8TaxInvoiceBusiness)
        {
            _form8TaxInvoiceBusiness = form8TaxInvoiceBusiness;           
            
        }
        #endregion Constructor_Injection

        public ActionResult Index()
        {
            return View();
        }


        #region GetAllForm8
        [HttpGet]
        public string GetAllForm8(Form8ViewModel dataObj)
        {
            try
            {
                UA ua=new UA();
                List<Form8ViewModel> Form8List = Mapper.Map<List<Form8>, List<Form8ViewModel>>(_form8TaxInvoiceBusiness.GetAllForm8(ua));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = Form8List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion  GetAllForm8


    }
}
