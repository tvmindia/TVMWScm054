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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? OpenDate { get; set; }
        [Display(Name = "Reference No.")]
        public string RefNo { get; set; }
        public Guid? ItemID { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please enter Quantity")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Quantity must be a number")]
        public int? Qty { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public string ReturnStatusYN { get; set; }
        public DateTime? ReturnDate { get; set; }
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