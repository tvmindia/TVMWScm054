using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class SalesReturnViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please Select Date")]
        public string OpenDate { get; set; }
        [Display(Name = "Reference No.")]
        [Required(ErrorMessage ="Please Enter Reference No.")]
        public string RefNo { get; set; }
        public Guid? ItemID { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please Enter Quantity")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Quantity must be a number")]
        public string Qty { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public string ReturnStatusYN { get; set; }
        public string ReturnDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Select Item Code")]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        public string HiddenQty { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public String OpenDateFormatted { get; set; }
    }
}