using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class DepositAndWithdrawalViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please enter Transaction Type")]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
        public List<SelectListItem> TransactionTypeList { get; set; }
        public string TransactionDescription { get; set; }
       
        [Display(Name = "Reference No")]
        [StringLength(20)]
        public string RefNo { get; set; }
        [Display(Name = "Reference Date")]
        [Required(ErrorMessage = "Please enter Reference date")]
        public string RefDate { get; set; }
        [Required(ErrorMessage = "Please enter Amount")]
        [Range(0.00, 1000000000.00, ErrorMessage = "Please enter a numeric")]
        [Display(Name = "Amount")]

        public decimal? Amount { get; set; }
        [Display(Name = "Note")]
        public string Description { get; set; }
        [Display(Name = "Deposit Mode")]
        public string DepositMode { get; set; }
        public List<SelectListItem> DepositModeList { get; set; }
        [Display(Name = "Cheque Status")]
        public string ChequeStatus { get; set; }
        public List<SelectListItem> ChequeStatusList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
}
}