using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class DailyServiceReportViewModel
    {
        [Required(ErrorMessage = "Choose Technician")]
        [Display(Name = "Technician")]
        public int TechnicianID { get; set; }
        public List<SelectListItem> Technicians { get; set; }

        [Required(ErrorMessage = "Choose Service date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        [Display(Name = "Service Date")]
        public DateTime ServiceDate { get; set; }

    }
}