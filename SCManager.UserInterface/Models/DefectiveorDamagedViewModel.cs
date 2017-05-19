using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class DefectiveorDamagedViewModel
    {
        public Guid? ID { get; set; }
        public Guid? ReturnID { get; set; }
        public string SCCode { get; set; }
        [Required(ErrorMessage = "Please select type")]
        public string Type { get; set; }
      
        public string HiddenType { get; set; }
      
        public string Technician { get; set; }
        [Required(ErrorMessage = "Please select open date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [Display(Name = "Open Date")]
        public string OpenDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? ReturnDate { get; set; }
        [Required(ErrorMessage = "Please select Technician")]
        public Guid? EmpID { get; set; }
     
        public Guid? HiddenEmpID { get; set; }
        [Display(Name = "Reference No.")]
        public string RefNo { get; set; }
        [Display(Name = "SPU No.")]
        public string SPUNo { get; set; }
        [Display(Name = "Ticket No.")]
        public string TicketNo { get; set; }
        public bool? ReturnStatusYN { get; set; }
        [Required(ErrorMessage = "Please select item code")]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        public Guid? ItemID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select quantity")]
        [Display(Name = "Quantity")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Quantity must be a number")]
        public string Qty { get; set; }
        public string HiddenQty { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public List<SelectListItem> ItemCodeList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public String OpenDateFormatted { get; set; }
    }
}