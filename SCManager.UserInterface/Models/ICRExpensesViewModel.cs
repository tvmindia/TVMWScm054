using System;
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

        [Display(Name = "Payment Ref. No.")]
        public string RefNo { get; set; }

        [Required(ErrorMessage = "Must be date")]
        [Display(Name = "Date")]
        public string RefDate { get; set; }

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
        public string fromDate { get; set; }
        [Display(Name = "To Date")]
        public string toDate { get; set; }
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