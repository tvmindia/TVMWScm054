using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class Form8ViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }

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
        public DateTime? ChallanDate { get; set; }

        [Display(Name = "PO No")]
        [MaxLength(20)]
        public string PONo { get; set; }

        [Display(Name = "PO Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? PODate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "VAT %")]
        [Range(0, 9999999999999999.99)]
        public decimal VATAmount { get; set; }

        [Display(Name = "Total")]
        [Range(0, 9999999999999999.99)]
        public decimal TotalItemsValue { get; set; }

        [Display(Name = "Discount")]
        [Range(0, 9999999999999999.99)]
        public decimal Discount { get; set; }

        [Display(Name = "Sub Total")]
        [Range(0, 9999999999999999.99)]
        public decimal Subtotal { get; set; }

        [Display(Name = "Grand Total")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }

        public LogDetailsViewModel logDetails { get; set; }

        public decimal Total { get; set; }

        public String ChallanDateFormatted { get; set; }
        public String PODateFormatted { get; set; }
        public String InvoiceDateFormatted { get; set; }

        
    }
}