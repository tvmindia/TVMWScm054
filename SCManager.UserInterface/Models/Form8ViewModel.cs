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
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Invoice No is missing")]
        [Display(Name = "Invoice No")]
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Invoice Date is missing")]
        [Display(Name = "Invoice Date")]
        public string InvoiceDate { get; set; }

        [Display(Name = "Sale Order No")]
        [MaxLength(20)]
        public string SaleOrderNo { get; set; }

        [Display(Name = "Challan No")]
        [MaxLength(20)]
        public string ChallanNo { get; set; }

        [Display(Name = "Challan Date")]
        public string ChallanDate { get; set; }

        [Display(Name = "PO No")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Please Enter PO No.")]
        public string PONo { get; set; }

        [Display(Name = "PO Date")]
        public string PODate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "VAT %")]
        [Range(0, 9999999999999999.99)]
        public decimal? VATAmount { get; set; }

        [Display(Name = "Total")]
        [Range(0, 9999999999999999.99)]
        public decimal? TotalItemsValue { get; set; }

        [Display(Name = "Discount (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Discount { get; set; }

        [Display(Name = "Net Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }

        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }

        public LogDetailsViewModel logDetails { get; set; }

        [Display(Name = "Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal Total { get; set; }

        public String ChallanDateFormatted { get; set; }
        public String PODateFormatted { get; set; }
        public String InvoiceDateFormatted { get; set; }
        public String DetailJSON { get; set; }
        public List<Form8DetailViewModel> Form8Detail { get; set; }

    }

    public class Form8DetailViewModel
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
        public decimal? NetAmount { get; set; }

    }
}