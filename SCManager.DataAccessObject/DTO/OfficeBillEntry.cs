using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class OfficeBillEntry
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerLocation { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public string BillDateFormatted { get; set; }
        public LogDetails logDetails { get; set; }
        public string DetailXML { get; set; }
        public List<OfficeBillEntryDetail> OfficeBillEntryDetail { get; set; }
    }
    public class OfficeBillEntryDetail
    {
        public int? SlNo { get; set; }
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public string Material { get; set; }
        public Guid? MaterialID { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public decimal? NetAmount { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
    }
}