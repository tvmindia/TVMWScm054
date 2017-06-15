using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class AssignBillBookViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Required(ErrorMessage = "Please Enter Book No")]
        [Display(Name ="Book No")]
        public string BillNo { get; set; }
        [Required(ErrorMessage ="Please Enter Series Start")]
        [Display(Name ="Series Start")]
        public string SeriesStart { get; set; }
        [Required(ErrorMessage ="Please Enter Series End")]
        [Display(Name ="Series End")]
        public string SeriesEnd { get; set; }
        [Display(Name ="Last Used")]
        public string LastUsed { get; set; }
        [Display(Name ="Technician")]
        [Required(ErrorMessage ="Please Select Technician")]
        public Guid? EmpID { get; set; }
        public string Status { get; set; }
        [Display(Name ="Bill Book Type")]
        [Required(ErrorMessage = "Please Select Bill Book Type")]
        public string BillBookType { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public string Technician { get; set; }
        public string MissingSerials { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}