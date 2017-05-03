using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCManager.UserInterface.Models
{
    public class LocalPurchaseViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Bill No is missing")]
        [Display(Name = "Bill No")]
        [MaxLength(20)]
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "Bill Date is missing")]
        [Display(Name = "Bill Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Vendor")]
        [MaxLength(50)]
        public string VendorName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "VAT %")]
        [Range(0, 9999999999999999.99)]
        public decimal VATAmount { get; set; }

        [Display(Name = "Sub Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal GrandTotal { get; set; }

        public String InvoiceDateFormatted { get; set; }
        public List<LocalPurchaseDetailViewModel> LocalPurchaseDetail { get; set; }
        public string DetailXML { get; set; }
        public String DetailJSON { get; set; }
    }
    public class LocalPurchaseDetailViewModel
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