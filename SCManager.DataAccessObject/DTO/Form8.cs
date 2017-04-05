using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class Form8
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string ChallanNo { get; set; }

        public DateTime ChallanDate { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string Remarks { get; set; }

        public decimal VATAmount { get; set; }
        public decimal TotalItemsValue { get; set; }
        public decimal Discount { get; set; }
        public LogDetails logDetails { get; set; }

        public decimal Total { get; set; }
    }

    public class Form8Details
    {
    }
}