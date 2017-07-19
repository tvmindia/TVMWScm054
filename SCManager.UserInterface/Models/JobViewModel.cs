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
        public string SCCode { get; set; }
        [Display(Name = "Technician Name")]
        public Guid? Repeat_EmpID { get; set; }
        public EmployeesViewModel Employee { get; set; }
        public string RepeatEmpName { get; set; }
        public string RepeatJobNo { get; set; }
        public string CallStatusDescription { get; set; }
        public string ServiceTypeDescription { get; set; }
        public string JobCallTypeDescription { get; set; }
        public string EmpSelector { get; set; }
        public string CallStatusCode { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public Guid? ID { get; set; }
        public string Source { get; set; }

        [Display(Name = "Service Date")]
        [DataType(DataType.Date, ErrorMessage = "Must be a Date")]
        public string ServiceDate { get; set; }

        public string ServiceDateformatted { get; set; }

        [Display(Name = "Repeat Job No")]
        public string Repeat_JobNo { get; set; }
        [Display(Name = "Technician Name")]
        public Guid? TechEmpID { get; set; }
      
        [Display(Name = "Job No")]
      
        public string JobNo { get; set; }

      
        [Display(Name = "Customer Name")]
       
        public string CustomerName { get; set; }

       
        [Display(Name = "Customer Location")]
   
        public string CustomerLocation { get; set; }

    
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; }
        public List<SelectListItem> ServiceTypes { get; set; }

     
        [Display(Name = "Call Type")]
        public string CallType { get; set; }
        public List<SelectListItem> CallTypes { get; set; }

        
        [Display(Name = "Model No")]
        [MaxLength(50)]
        public string ModelNo { get; set; }

       
        [Display(Name = "Serial No")]
        [MaxLength(50)]
        public string SerialNo { get; set; }

       
        [Display(Name = "ICR No")]
       
        public string ICRNo { get; set; }

        [Display(Name = "Mobile No")]

        public string MobileNumber { get; set; }

        [Display(Name = "Technician Remark")]
    
        public string TechnicianRemark { get; set; }

        [Display(Name = "Special Commission(₹)")]
        [Range(0.00, 1000000000.00, ErrorMessage = "Please enter a numeric")]
        public decimal? SCCommAmount { get; set; }

        public LogDetailsViewModel logDetails { get; set; }
    }

    public class JobCallTypesViewModel
    {
        public string SCCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}