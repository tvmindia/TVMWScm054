using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class OtherIncomeViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        [Required(ErrorMessage = "Please Select Income Type")]
        [Display(Name = "Income Type")]
        public string IncomeTypeCode { get; set; }
        [Display(Name = "Reference No.")]
        public string RefNo { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime RefDate { get; set; }
        [Display(Name = "Amount (₹)")]
        public float Amount { get; set; }
        [Display(Name = "Mode Of Payment")]
        [Required(ErrorMessage = "Please Select Mode Of Payment")]
        public string PaymentMode { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "From Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? fromDate { get; set; }
        [Display(Name = "To Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? toDate { get; set; }
        [Display(Name = "Show All Entries")]
        public bool showAllYN { get; set; }
        public string HiddenRefNo { get; set; }
        public List<SelectListItem> IncomeTypeList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public string IncomeTypeDescription { get; set; }
        public String RefDateFormatted { get; set; }
    }
}