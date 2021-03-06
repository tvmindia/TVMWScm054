﻿using System;
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
        [Required(ErrorMessage = "Must be a date")]
        public string RefDate { get; set; }
        [Display(Name = "Amount (₹)")]
        public float Amount { get; set; }
        [Display(Name = "Mode Of Payment")]
        [Required(ErrorMessage = "Please Select Mode Of Payment")]
        public string PaymentMode { get; set; }
        [Display(Name = "Payment Ref.No.")]
        public string PaymentRefNo { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "From Date")]
        public string fromDate { get; set; }
        [Display(Name = "To Date")]
        public string toDate { get; set; }
        [Display(Name = "Show All Entries")]
        public bool showAllYN { get; set; }
        public string HiddenRefNo { get; set; }
        public List<SelectListItem> IncomeTypeList { get; set; }
        public List<SelectListItem> PaymentModeList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public string IncomeTypeDescription { get; set; }
        public String RefDateFormatted { get; set; }
    }
}