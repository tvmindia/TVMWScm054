using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using UserInterface.Models;

namespace SCManager.UserInterface.Models
{
    public class ReturnBillViewModel
    {
        public PDFTools PDFToolsObj { get; set; }
        public string SCCode { get; set; }
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Ticket No is missing")]
        [Display(Name = "Ticket No")]
        [MaxLength(20)]
        public string TicketNo { get; set; }

        [Required(ErrorMessage = "Invoice No is missing")]
        [Display(Name = "Invoice No")]
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Invoice Date is missing")]
        [Display(Name = "Invoice Date")]
        public string InvoiceDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
       
        [Display(Name = "Net Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }

        [Display(Name = "Total Tax Amount (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? TotalTaxAmount { get; set; }

        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }

        public LogDetailsViewModel logDetails { get; set; }

        [Display(Name = "Total Amount (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal TotalValue { get; set; }

        public String InvoiceDateFormatted { get; set; }
        public String DetailJSON { get; set; }
        public List<ReturnBillDetailViewModel> ReturnBillDetail { get; set; }

        [Required(ErrorMessage = "Customer Name is Missing")]
        [Display(Name = "Customer Name")]
        [MaxLength(20)]
        public string CustomerName { get; set; }        

        public List<SelectListItem> TicketNoList { get; set; }

        [Display(Name = "Customer Address")]
        [DataType(DataType.MultilineText)]
        public string CustomerAddress { get; set; }

        [Display(Name = "Phone No")]
        [StringLength(50, MinimumLength = 5)]
        public string CustomerPhoneNo { get; set; }

        [Display(Name = "Email")]
        [MaxLength(20)]
        public string CustomerEmail { get; set; }

        [Display(Name = "GSTIN")]
        [MaxLength(20)]
        public string CustomerGstIn { get; set; }

        [Display(Name = "PAN No")]
        [MaxLength(20)]
        public string CustomerPanNo { get; set; }

        [Display(Name = "Place Of Supply")]
        [MaxLength(20)]
        public string PlaceOfSupply { get; set; }

        public bool? ReturnStatusYN { get; set; }

        [Display(Name = "Shipping Address")]
        [DataType(DataType.MultilineText)]
        public string ShippingAddress { get; set; }

        [Display(Name = "GSTIN")]
        [MaxLength(20)]
        public string ShippingGstIn { get; set; }

        [Display(Name = "PAN No")]
        [MaxLength(20)]
        public string ShippingPanNo { get; set; }

        [Display(Name = "Customer Name")]
        [MaxLength(20)]
        public string ShippingCustomerName { get; set; }

        [Display(Name = "Phone No")]
        [StringLength(50, MinimumLength = 5)]
        public string ShippingCustomerPhoneNo { get; set; }

        [Display(Name = "Email")]
        [MaxLength(20)]
        public string ShippingCustomerEmail { get; set; }

        public string ServiceCenterCode { get; set; }
        public string ServiceCenterDescription { get; set; }
        public string ServiceCenterAddress { get; set; }
        public string ServiceCenterEmail { get; set; }
        public string ServiceCenterContactNo { get; set; }
        public string ServiceCenterGstIn { get; set; }
        public string ServiceCenterPanNo { get; set; }
        public string ServiceCenterPlace { get; set; }

    }

    public class ReturnBillDetailViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
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
        public decimal? TotalTaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? NetAmount { get; set; }
        public string TicketNo { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalValue { get; set; }
        public bool? ReturnStatusYN { get; set; }
    }
}