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
        public Guid? Repeat_EmpID { get; set; }
        public string EmpSelector { get; set; }
        public string CallStatusCode { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public Guid ID { get; set; }
        public string Source { get; set; }

        [Display(Name = "Service Date")]
        public string ServiceDate { get; set; }
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
       
        public string ModelNo { get; set; }

       
        [Display(Name = "Serial No")]
      
        public string SerialNo { get; set; }

       
        [Display(Name = "ICR No")]
       
        public string ICRNo { get; set; }

      
        [Display(Name = "Technician Remark")]
    
        public string TechnicianRemark { get; set; }

        public LogDetailsViewModel logDetails { get; set; }
    }
}