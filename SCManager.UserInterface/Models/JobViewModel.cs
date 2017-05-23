using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCManager.UserInterface.Models
{
    public class JobViewModel
    {
        public Guid ID { get; set; }
        public string Source { get; set; }

        [Display(Name = "Service Date")]
        public string ServiceDate { get; set; }
        [Display(Name = "Repeat Job No")]
        public string RepeatJobNo { get; set; }
        [Display(Name = "Technician Name")]
        public Guid? EmpID { get; set; }
        [Required(ErrorMessage = "Please enter job no")]
        [Display(Name = "Job No")]
        [StringLength(50)]
        public string JobNo { get; set; }

        [Required(ErrorMessage = "Please enter customer name")]
        [Display(Name = "Customer Name")]
        [StringLength(250)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter customer location")]
        [Display(Name = "Customer Location")]
        [StringLength(250)]
        public string CustomerLocation { get; set; }

        [Required(ErrorMessage = "Please enter service type")]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
        public List<SelectListItem> ServiceTypes { get; set; }

        [Required(ErrorMessage = "Please enter call type")]
        [Display(Name = "Call Type")]
        public string CallType { get; set; }
        public List<SelectListItem> CallTypes { get; set; }

      
        [Display(Name = "Model No")]
        [StringLength(50)]
        public string ModelNo { get; set; }

       
        [Display(Name = "Serial No")]
        [StringLength(50)]
        public string SerialNo { get; set; }

       
        [Display(Name = "ICR No")]
        [StringLength(50)]
        public string ICRNo { get; set; }

      
        [Display(Name = "Technician Remark")]
        [StringLength(50)]
        public string TechnicianRemark { get; set; }

        public LogDetailsViewModel logDetails { get; set; }
    }
}