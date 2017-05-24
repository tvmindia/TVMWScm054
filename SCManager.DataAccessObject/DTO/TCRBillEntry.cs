using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class TCRBillEntry
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string BillNo { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillDateFormatted { get; set; }
        public Guid? EmpID { get; set; }
        public string JobNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerLocation { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? ServiceChargeComm { get; set; }
        public decimal? SCCommAmount { get; set; }
        public decimal? SpecialComm { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? Subtotal { get; set; }
        public string Technician { get; set; }
        public Guid? jobNoID { get; set; }
        public LogDetails logDetails { get; set; }
        public string DetailXML { get; set; }
        public List<TCRBillEntryDetail> TCRBillEntryDetail { get; set; }
    }
    public class TCRBillEntryDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public int? ItemNo { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public string UOM { get; set; }
        public decimal? NetAmount { get; set; }
    }
}