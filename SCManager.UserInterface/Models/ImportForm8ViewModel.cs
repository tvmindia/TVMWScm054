using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class ImportForm8ViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string Remarks { get; set; }
        public decimal VATAmount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public String ChallanDateFormatted { get; set; }
        public String PODateFormatted { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public CommonViewModel CommonObj { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public decimal? Rate { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? SGSTAmount { get; set; }
        ///public decimal? TotalTaxAmount { get; set; }
        public decimal? NetAmount { get; set; }
    }
}