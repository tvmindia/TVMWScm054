using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class OfficeBillEntryViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please Enter Bill No")]
        [Display(Name = "Bill No")]
        public string BillNo { get; set; }
        [Required(ErrorMessage = "Please select Bill date")]
        [Display(Name = "Bill Date")]
        public string BillDate { get; set; }
        public string BillDateFormatted { get; set; }
        [Required(ErrorMessage = "Please Enter Customer Name")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Customer Contact")]
        public string CustomerContactNo { get; set; }
        [Display(Name = "Customer Location")]
        public string CustomerLocation { get; set; }
        [Required(ErrorMessage = "Please select mode of payment")]
        [Display(Name = "Mode Of Payment")]
        public string PaymentMode { get; set; }
        [Display(Name = "Payment Ref.No.")]
        public string PaymentRefNo { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name = "VAT %")]
        public decimal? VATAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Total { get; set; }
        [Required(ErrorMessage = "Grand total should have value")]
        [Display(Name = "Grand Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? GrandTotal { get; set; }
        [Display(Name = "Sub Total (₹)")]
        [Range(0, 9999999999999999.99)]
        public decimal? Subtotal { get; set; }
        public String DetailJSON { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public List<SelectListItem> PaymentModeList { get; set; }
        public List<OfficeBillEntryDetailViewModel> OfficeBillEntryDetail { get; set; }
        public OfficeBillEntryDetailViewModel OfficeBillEntryDetailObj { get; set; }
       
    }
    public class OfficeBillEntryDetailViewModel
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