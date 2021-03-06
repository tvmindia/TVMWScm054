﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class IssueToTechnicianViewModel
    {
        public string SCCode { get; set; }
        public Guid? ID { get; set; }
        [Display(Name = "Technician")]
        public Guid? EmpID { get; set; }
        [Display(Name = "Technician")]
        [Required(ErrorMessage = "Please select Technician")]
        public Guid? HiddenEmpID { get; set; }
        [Required(ErrorMessage = "Please Select Transfer Date")]
        [Display(Name = "Transfer Date")]
        public string IssueDate { get; set; }
        public Guid? MaterialID { get; set; }
        public string Material { get; set; }
        public int SlNo { get; set; }
        public string UOM { get; set; }
        public int? Qty { get; set; }
        public List<SelectListItem> TechniciansList { get; set; }
        public List<SelectListItem> TechniciansListItems { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
        public string Technician { get; set; }
        [Display(Name = "From Date")]
        public string fromDate { get; set; }
        [Display(Name = "To Date")]
        public string toDate { get; set; }
        public String DetailJSON { get; set; }
        public string empName { get; set; }
        public String DateFormatted { get; set; }
        public string Description { get; set; }
    }
}