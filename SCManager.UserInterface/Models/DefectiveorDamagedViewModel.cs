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
        public Guid ID { get; set; }
        public string SCCode { get; set; }
        public string Type { get; set; }
        public string Technician { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public string OpenDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public DateTime? ReturnDate { get; set; }
        public Guid EmpID { get; set; }
        [Display(Name = "Reference No.")]
        public string RefNo { get; set; }
        public bool ReturnStatusYN { get; set; }
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        public Guid ItemID { get; set; }
        public string Description { get; set; }
        [Display(Name = "Quantity")]
        public string Qty { get; set; }
        public string Remarks { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public List<SelectListItem> ItemCodeList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}