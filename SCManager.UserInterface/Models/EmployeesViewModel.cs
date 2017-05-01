using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCManager.UserInterface.Models
{
    public class EmployeesViewModel
    {
        public string SCCode { get; set; }
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        [Display(Name = "Name")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Select Type")]
        [Display(Name = "Type")]
        [StringLength(50)]
        public string Type { get; set; }

        [Display(Name = "Mobile No")]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public LogDetailsViewModel logDetails { get; set; }
    }
}