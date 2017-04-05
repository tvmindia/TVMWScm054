using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class Form8ViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string ChallanNo { get; set; }

        public DateTime? ChallanDate { get; set; }
        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string Remarks { get; set; }

        public decimal VATAmount { get; set; }
        public decimal TotalItemsValue { get; set; }
        public decimal Discount { get; set; }
        public LogDetailsViewModel logDetails { get; set; }

        public decimal Total { get; set; }
        
    }
}