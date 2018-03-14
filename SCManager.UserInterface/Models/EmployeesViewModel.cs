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
        [RegularExpression(@"^((?!All Technician).)*$", ErrorMessage = "Entered name is not valid.")]
        [Display(Name = "Name")]
        [StringLength(250)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Select Type")]
        [Display(Name = "Type")]
        [StringLength(50)]
        public string Type { get; set; }

        //[RegularExpression(@"^\(?([0-9])\)?[-. ]?([0-9])[-. ]?([0-9])$", ErrorMessage = "Entered phone format is not valid.")]
        [Display(Name = "Mobile No")]
        [StringLength(50, MinimumLength = 5)]
        public string MobileNo { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public LogDetailsViewModel logDetails { get; set; }

        [Display(Name = "Is Active(Y/N)")]
        public bool IsActive { get; set; }
        public string Filter { get; set; }
    }
}