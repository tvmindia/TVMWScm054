using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class TaxBillEntry
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
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
        public string PaymentRefNo { get; set; }
        public Guid? jobNoID { get; set; }
        public LogDetails logDetails { get; set; }
        public string DetailXML { get; set; }
        public List<TaxBillEntryDetail> TaxBillEntryDetail { get; set; }
        public Guid TcrID { get; set; }
        public int IsActive { get; set; }  
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTAmount { get; set; }  
        public decimal? TotalTaxAmount { get; set; }
        public decimal? CgstPercentage { get; set; }
        public decimal? SgstPercentage { get; set; }



        public string ServiceCenterCode { get; set; }
        public string ServiceCenterDescription { get; set; }
        public string ServiceCenterAddress { get; set; }
        public string ServiceCenterEmail { get; set; }
        public string ServiceCenterContactNo { get; set; }
        public string ServiceCenterGstIn { get; set; }
        public string ServiceCenterPanNo { get; set; }
        public string ServiceCenterPlace { get; set; }

    }

    public class TaxBillEntryDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public int? ItemNo { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public decimal? ReferralRate { get; set; }
        public decimal? Rate { get; set; }
        public string UOM { get; set; }
       
        public string Description { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? CgstPercentage { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SgstPercentage { get; set; }        
        public decimal? SGSTAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? TotalTaxAmount { get; set; }
       
    }

}