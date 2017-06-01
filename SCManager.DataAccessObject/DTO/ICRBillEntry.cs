using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class ICRBillEntry
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public string ICRNo { get; set; }
        public DateTime? ICRDate { get; set; }
        public Guid? EmpID { get; set; }
        public string JobNo { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactNo { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public string CustomerLocation { get; set; }
        public DateTime? AMCValidFromDate { get; set; }
        public DateTime? AMCValidToDate { get; set; }
        public string Remarks { get; set; }
        public string PaymentMode { get; set; }
        public string Technician { get; set; }
        public decimal? STAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TotalServiceTaxAmt { get; set; }
        public Guid? jobNoID { get; set; }
        public LogDetails logDetails { get; set; }
        public string DetailXML { get; set; }
        public String ICRDateFormatted { get; set; }
        public List<ICRBillEntryDetail> ICRBillEntryDetail { get; set; }
    }
    public class ICRBillEntryDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public int? ItemNo { get; set; }
       // public string ServiceTypeCode { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public int? SlNo { get; set; }
        public string UOM { get; set; }
      //  public Guid? MaterialID { get; set; }
        public string Material { get; set; }
        public decimal? NetAmount { get; set; }
        public string Description { get; set; }
    }
}