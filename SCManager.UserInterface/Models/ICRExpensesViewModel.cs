﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class ICRExpensesViewModel
    {
        public string SCCode { get; set; }
        [Display(Name = "Expense ID")]
        public Guid ID { get; set; }

        [Display(Name = "Entry No.")]
        public string EntryNo { get; set; } 

        [Display(Name = "Reference No.")]
        public string RefNo { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [Display(Name = "Date")]
        public DateTime RefDate { get; set; }

        [Display(Name = "Mode of Payment")]
        [Required(ErrorMessage = "Please Select Mode of Payment")]
        public string PaymentMode { get; set; }

        [Display(Name = "Amount (₹)")]
        [Range(0.00, 1000000000.00, ErrorMessage = "Please enter a numeric")]
        public decimal Amount { get; set; }

        [Display(Name = " ")]
        public decimal OutStandingICRPayment { get; set; }
        public string OutStandingPaymentFormatted { get; set; }



        [Display(Name = "Note")]
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
        [Display(Name = "Show All Expenses")]
        public bool showAllYN { get; set; }

        public string EmpName { get; set; }
        public string DateFormatted { get; set; }
        public string ExpenseType { get; set; }

        public LogDetailsViewModel logDetails { get; set; }
     //   public List<SelectListItem> TechniciansList { get; set; }
        public List<SelectListItem> PaymentModeList { get; set; }
        public List<SelectListItem> ExpenseTypeList { get; set; }


    }
}