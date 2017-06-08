using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class CommonViewModel
    {

       public List<SelectListItem> PaymentModelist = new List<SelectListItem>()        {
          new SelectListItem {Value="CASH", Text="CASH"},
          new SelectListItem {Value="CHEQUE", Text="CHEQUE"},
          new SelectListItem{Value="ONLINE",  Text="ONLINE"},
        
        };
    }

    public class LogDetailsViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
    public class SalesGraphViewModel
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}