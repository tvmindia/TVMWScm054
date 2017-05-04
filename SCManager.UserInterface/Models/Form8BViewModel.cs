using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class Form8BViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Invoice No is missing")]
        [Display(Name = "Invoice No")]
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Invoice Date is missing")]
        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? InvoiceDate { get; set; }

        [Display(Name = "Sale Order No")]
        [MaxLength(20)]
        public string SaleOrderNo { get; set; }

        [Display(Name = "Challan No")]
        [MaxLength(20)]
        public string ChallanNo { get; set; }

        [Display(Name = "Challan Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? ChallanDate { get; set; }

        [Display(Name = "PO No")]
        [MaxLength(20)]
        public string PONo { get; set; }

        [Display(Name = "PO Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? PODate { get; set; }

        [Display(Name = "SPU No")]
        [MaxLength(20)]
        public string SPUNo { get; set; }

        [Display(Name = "Cust. Delivery Address")]
        [DataType(DataType.MultilineText)]
        public string CustomerDelvAddrs { get; set; }

        [Display(Name = "Cust. Billing Address")]
        [DataType(DataType.MultilineText)]
        public string CustomerBillAddrs { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "VAT %")]
        [Range(0, 9999999999999999.99)]
        public decimal? VATAmount { get; set; }

        [Display(Name = "Total")]
        [Range(0, 9999999999999999.99)]
        public decimal? TotalItemsValue { get; set; }

        [Display(Name = "Vat Expense (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? VATExpense { get; set; }

        [Display(Name = "Sub Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }

        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }

        public LogDetailsViewModel logDetails { get; set; }

        public decimal Total { get; set; }

        public String ChallanDateFormatted { get; set; }
        public String PODateFormatted { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public String DetailJSON { get; set; }
        public List<Form8BDetailViewModel> Form8BDetail { get; set; }
    }

    public class Form8BDetailViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        public Guid? HeaderID { get; set; }
        public Guid? MaterialID { get; set; }
        public int? SlNo { get; set; }
        public string Material { get; set; }
        public int? Quantity { get; set; }
        public string UOM { get; set; }
        public decimal? Rate { get; set; }
        public decimal? BasicAmount { get; set; }
        public decimal? TradeDiscount { get; set; }
        public decimal? NetAmount { get; set; }

    }
}