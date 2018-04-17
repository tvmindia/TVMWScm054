using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class ReturnBill
    {

        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string TicketNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public LogDetails logDetails { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public List<ReturnBillDetail> ReturnBillDetail { get; set; }
        public string DetailXML { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGstIn { get; set; }
        public string CustomerPanNo { get; set; }
        public string PlaceofSupply { get; set; }
        public string MCSerialNo { get; set; }
        public bool? ReturnStatusYN { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingGstIn { get; set; }
        public string ShippingPanNo { get; set; }
        public string ShippingCustomerName { get; set; }
        public string ShippingCustomerPhoneNo { get; set; }
        public string ShippingCustomerEmail { get; set; }

        public string ServiceCenterCode { get; set; }
        public string ServiceCenterDescription { get; set; }
        public string ServiceCenterAddress { get; set; }
        public string ServiceCenterEmail { get; set; }
        public string ServiceCenterContactNo { get; set; }
        public string ServiceCenterGstIn { get; set; }
        public string ServiceCenterPanNo { get; set; }
        public string ServiceCenterPlace { get; set; }

        //public string ServiceCenterCode { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyContactNo { get; set; }
        public string CompanyGstIn { get; set; }
        public string CompanyPanNo { get; set; }
        public string CompanyPlace { get; set; }
    }

    public  class ReturnBillDetail
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }       
        public string UOM { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Rate { get; set; }       
        public decimal? TradeDiscount { get; set; }
        public decimal? CGSTPercentage { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTPercentage { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal GrandTotal { get; set; }       
        public string TicketNo { get; set; }
        public string CustomerName { get; set; }
        public bool? ReturnStatusYN { get; set; }
    }
}