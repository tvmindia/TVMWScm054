﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class CreditNotesViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please enter CreditNote No.")]
        [Display(Name = "Credit Note No.")]
        public string CreditNoteNo { get; set; }
        [Required(ErrorMessage = "Please Select Date")]
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Amount (₹)")]
        public float Amount { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "From Date")]
        public string fromDate { get; set; }
        [Display(Name = "To Date")]
        public string toDate { get; set; }
        [Display(Name = "Show All Credit Notes")]
        public bool showAllYN { get; set; }
        public string HiddenCreditNoteNo { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public String DateFormatted { get; set; }
    }
}