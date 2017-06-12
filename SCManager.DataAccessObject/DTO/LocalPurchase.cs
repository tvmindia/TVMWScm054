using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class LocalPurchase
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string VendorName { get; set; }      
        public string Remarks { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Subtotal { get; set; }       
        public LogDetails logDetails { get; set; }
        public decimal GrandTotal { get; set; }
 
        public String InvoiceDateFormatted { get; set; }
        public List<LocalPurchaseDetail> LocalPurchaseDetail { get; set; }
        public string DetailXML { get; set; }
    }
    public class LocalPurchaseDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? Rate { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? NetAmount { get; set; }
    }
}